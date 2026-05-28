import { useState } from 'react';
import styles from './JuryDashboard.module.css';
import appStyles from '../../App.module.css';

// Тестовые данные (придут с бэка)
const ASSIGNED_PROJECTS = [
  { id: 1, team: "Code Crusaders", status: "pending", github: "https://github.com", youtube: "https://youtube.com" },
  { id: 2, team: "Null Pointers", status: "evaluated", github: "https://github.com", youtube: "https://youtube.com" },
  { id: 3, team: "Runtime Terrors", status: "pending", github: "https://github.com", youtube: "https://youtube.com" }
];

function JuryDashboard() {
  // Какую команду сейчас оцениваем (null = показываем список)
  const [selectedProject, setSelectedProject] = useState(null);

  // Состояния для ползунков оценок
  const [scores, setScores] = useState({ backend: 0, frontend: 0, database: 0, functionality: 0 });
  const [comment, setComment] = useState('');

  // Обновление конкретного ползунка
  const handleScoreChange = (category, value) => {
    setScores(prev => ({ ...prev, [category]: value }));
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    alert(`Оценка для ${selectedProject.team} сохранена!`);
    setSelectedProject(null); // Возвращаемся к списку после сохранения
  };

  return (
    <main className={appStyles.pagePadding}>
      <h1 className={appStyles.heroTitle}>⚖️ Jury Dashboard</h1>
      <p className={appStyles.heroDesc}>
        {selectedProject 
          ? `Evaluating: ${selectedProject.team}` 
          : 'Please evaluate the projects assigned to you.'}
      </p>

      {/* ЕСЛИ ПРОЕКТ НЕ ВЫБРАН -> ПОКАЗЫВАЕМ СПИСОК КАРТОЧЕК */}
      {!selectedProject ? (
        <div className={styles.projectsGrid}>
          {ASSIGNED_PROJECTS.map(proj => (
            <div 
              key={proj.id} 
              className={styles.projectCard}
              onClick={() => setSelectedProject(proj)}
            >
              <div className={styles.teamName}>{proj.team}</div>
              <div className={proj.status === 'evaluated' ? styles.statusDone : styles.statusPending}>
                {proj.status === 'evaluated' ? '✅ Evaluated' : '⏳ Pending'}
              </div>
            </div>
          ))}
        </div>
      ) : (
        /* ЕСЛИ ПРОЕКТ ВЫБРАН -> ПОКАЗЫВАЕМ ФОРМУ ОЦЕНКИ */
        <div className={styles.evaluationView}>
          <button className={styles.backBtn} onClick={() => setSelectedProject(null)}>
            ← Back to assigned projects
          </button>

          {/* Клибельные ссылки на работы */}
          <div className={styles.linksBox}>
            <a href={selectedProject.github} target="_blank" className={`${styles.linkBtn} ${styles.githubLink}`}>
              View GitHub Repo
            </a>
            <a href={selectedProject.youtube} target="_blank" className={`${styles.linkBtn} ${styles.youtubeLink}`}>
              Watch Demo Video
            </a>
          </div>

          <form onSubmit={handleSubmit}>
            {/* Рендерим 4 ползунка */}
            {['backend', 'frontend', 'database', 'functionality'].map((category) => (
              <div key={category} className={styles.sliderGroup}>
                <div className={styles.sliderHeader}>
                  <span style={{textTransform: 'capitalize'}}>{category}</span>
                  <span className={styles.sliderValue}>{scores[category]} / 100</span>
                </div>
                <input 
                  type="range" 
                  min="0" max="100" 
                  value={scores[category]}
                  onChange={(e) => handleScoreChange(category, e.target.value)}
                  className={styles.slider}
                />
              </div>
            ))}

            {/* Комментарий Жюри */}
            <div style={{ marginBottom: '24px' }}>
              <label style={{ display: 'block', fontWeight: 'bold', marginBottom: '8px' }}>Jury Comment</label>
              <textarea 
                style={{ width: '100%', padding: '12px', borderRadius: '8px', border: '1px solid #d1d5db', minHeight: '100px' }}
                placeholder="Оставьте подробный фидбек для команды..."
                value={comment}
                onChange={(e) => setComment(e.target.value)}
                required
              />
            </div>

            <button type="submit" style={{ backgroundColor: '#111827', color: 'white', padding: '12px 24px', borderRadius: '8px', fontWeight: 'bold', cursor: 'pointer', border: 'none' }}>
              Save Evaluation
            </button>
          </form>
        </div>
      )}
    </main>
  );
}

export default JuryDashboard;