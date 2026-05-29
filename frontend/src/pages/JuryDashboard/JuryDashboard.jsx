import { useState, useEffect } from 'react';
import styles from './JuryDashboard.module.css';
import appStyles from '../../App.module.css';

function JuryDashboard() {
  // Стейт для списку робіт з бекенду
  const [assignedProjects, setAssignedProjects] = useState([]);
  const [loading, setLoading] = useState(true);
  
  // Стейт для обраного проекту та повідомлень
  const [selectedProject, setSelectedProject] = useState(null);
  const [message, setMessage] = useState({ text: '', type: '' });

  // Стани для повзунків оцінок
  const [scores, setScores] = useState({ backend: 0, frontend: 0, database: 0, functionality: 0 });
  const [comment, setComment] = useState('');

  // Завантажуємо роботи при відкритті сторінки
  useEffect(() => {
    const fetchSubmissions = async () => {
      const token = localStorage.getItem('token');
      if (!token) {
        setMessage({ text: 'Будь ласка, увійдіть у систему.', type: 'error' });
        setLoading(false);
        return;
      }

      try {
        // Отримуємо список робіт (можливо, тобі знадобиться змінити на /api/Submissions/round/{roundId})
        const response = await fetch('http://localhost:8080/api/Submissions', {
          headers: { 'Authorization': `Bearer ${token}` }
        });

        if (response.ok) {
          const data = await response.json();
          setAssignedProjects(data);
        } else {
          setMessage({ text: 'Помилка завантаження списку робіт.', type: 'error' });
        }
      } catch (error) {
        setMessage({ text: 'Помилка з\'єднання з сервером.', type: 'error' });
      } finally {
        setLoading(false);
      }
    };

    fetchSubmissions();
  }, []);

  // Оновлення конкретного повзунка
  const handleScoreChange = (category, value) => {
    setScores(prev => ({ ...prev, [category]: value }));
  };

  // Відправка оцінки на бекенд
  const handleSubmit = async (e) => {
    e.preventDefault();
    setMessage({ text: '', type: '' });

    const token = localStorage.getItem('token');
    
    // Вираховуємо середній бал з 4 критеріїв
    const averageScore = Math.round(
      (parseInt(scores.backend) + parseInt(scores.frontend) + parseInt(scores.database) + parseInt(scores.functionality)) / 4
    );

    // Формуємо об'єкт для EvaluateDto
    const evaluationData = {
      submissionId: selectedProject.id,
      score: averageScore, // Або передавай об'єкт з усіма 4 оцінками, якщо твій бекенд це підтримує
      comment: comment
    };

    try {
      const response = await fetch('http://localhost:5058/api/Evaluations/evaluate', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${token}`
        },
        body: JSON.stringify(evaluationData)
      });

      if (response.ok) {
        setMessage({ text: `✅ Оцінку для ${selectedProject.teamName || 'команди'} збережено!`, type: 'success' });
        setSelectedProject(null); // Повертаємось до списку
        
        // Очищаємо форму для наступних оцінок
        setScores({ backend: 0, frontend: 0, database: 0, functionality: 0 });
        setComment('');
        
        // Тут в ідеалі треба перезапросити список робіт, щоб оновити їх статус на "Evaluated"
      } else {
        setMessage({ text: '❌ Помилка збереження оцінки.', type: 'error' });
      }
    } catch (error) {
      setMessage({ text: '❌ Сервер не відповідає.', type: 'error' });
    }
  };

  return (
    <main className={appStyles.pagePadding}>
      <h1 className={appStyles.heroTitle}>⚖️ Jury Dashboard</h1>
      <p className={appStyles.heroDesc}>
        {selectedProject 
          ? `Evaluating: ${selectedProject.teamName || 'Project ' + selectedProject.id}` 
          : 'Please evaluate the projects assigned to you.'}
      </p>

      {/* Вивід повідомлень */}
      {message.text && (
        <div style={{ 
          padding: '15px', marginBottom: '20px', borderRadius: '8px', 
          backgroundColor: message.type === 'error' ? '#fce8e6' : '#e6f4ea',
          color: message.type === 'error' ? '#d93025' : '#137333'
        }}>
          {message.text}
        </div>
      )}

      {/* ЯКЩО ПРОЕКТ НЕ ОБРАНО -> ПОКАЗУЄМО СПИСОК */}
      {!selectedProject ? (
        loading ? (
          <p>Завантаження робіт...</p>
        ) : assignedProjects.length === 0 ? (
          <p>Наразі немає робіт для оцінювання.</p>
        ) : (
          <div className={styles.projectsGrid}>
            {assignedProjects.map(proj => (
              <div 
                key={proj.id} 
                className={styles.projectCard}
                onClick={() => setSelectedProject(proj)}
                style={{ cursor: 'pointer' }}
              >
                {/* Використовуй правильні поля з твого DTO, наприклад proj.teamName */}
                <div className={styles.teamName}>{proj.teamName || `Submission #${proj.id}`}</div>
                <div className={proj.isEvaluated ? styles.statusDone : styles.statusPending}>
                  {proj.isEvaluated ? '✅ Evaluated' : '⏳ Pending'}
                </div>
              </div>
            ))}
          </div>
        )
      ) : (
        /* ЯКЩО ПРОЕКТ ОБРАНО -> ПОКАЗУЄМО ФОРМУ ОЦІНКИ */
        <div className={styles.evaluationView}>
          <button className={styles.backBtn} onClick={() => setSelectedProject(null)}>
            ← Back to assigned projects
          </button>

          <div className={styles.linksBox}>
            {/* Підставляй лінки, якщо вони є в твоєму SubmissionDto */}
            <a href={selectedProject.githubUrl || "#"} target="_blank" rel="noreferrer" className={`${styles.linkBtn} ${styles.githubLink}`}>
              View GitHub Repo
            </a>
            <a href={selectedProject.demoUrl || "#"} target="_blank" rel="noreferrer" className={`${styles.linkBtn} ${styles.youtubeLink}`}>
              Watch Demo Video
            </a>
          </div>

          <form onSubmit={handleSubmit}>
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