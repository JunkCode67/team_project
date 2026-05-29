import { useState, useEffect } from 'react';
import styles from './Leaderboard.module.css';
import appStyles from '../../App.module.css';

function Leaderboard() {
  const [teams, setTeams] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');
  
  // Змінна для збереження ID розгорнутої команди
  const [expandedTeamId, setExpandedTeamId] = useState(null);

  // Завантажуємо таблицю лідерів при відкритті сторінки
  useEffect(() => {
    const fetchLeaderboard = async () => {
      const token = localStorage.getItem('token');
      if (!token) {
        setError('Будь ласка, увійдіть у систему, щоб побачити результати.');
        setLoading(false);
        return;
      }

      try {
        // Уточни точний роут у своєму бекенді. Можливо це /api/Submissions/leaderboard
        const response = await fetch('http://localhost:5058/api/Evaluations/leaderboard', {
          headers: {
            'Authorization': `Bearer ${token}`
          }
        });

        if (response.ok) {
          const data = await response.json();
          
          // Сортуємо команди за загальним балом (від найбільшого до найменшого)
          // Заміни "totalScore" на те поле, яке реально повертає твій DTO
          const sortedData = data.sort((a, b) => (b.totalScore || 0) - (a.totalScore || 0));
          
          setTeams(sortedData);
        } else {
          setError('Не вдалося завантажити таблицю лідерів.');
        }
      } catch (err) {
        setError('Помилка з\'єднання з сервером.');
      } finally {
        setLoading(false);
      }
    };

    fetchLeaderboard();
  }, []);

  const toggleRow = (id) => {
    setExpandedTeamId(expandedTeamId === id ? null : id);
  };

  return (
    <main className={appStyles.pagePadding}>
      <h1 className={appStyles.heroTitle}>🏆 Leaderboard</h1>
      <p className={appStyles.heroDesc}>
        Final standings and detailed jury evaluations for the current hackathon.
      </p>

      {/* Повідомлення про помилки */}
      {error && (
        <div style={{ color: '#d93025', padding: '15px', marginBottom: '20px', borderRadius: '8px', backgroundColor: '#fce8e6' }}>
          {error}
        </div>
      )}

      <div className={styles.boardContainer}>
        {/* Шапка таблиці */}
        <div className={styles.headerRow}>
          <div>Rank</div>
          <div>Team Name</div>
          <div style={{ textAlign: 'right' }}>Total Score</div>
        </div>

        {/* Стан завантаження */}
        {loading ? (
          <div style={{ padding: '20px', textAlign: 'center' }}>Підраховуємо бали...</div>
        ) : !error && teams.length === 0 ? (
          <div style={{ padding: '20px', textAlign: 'center' }}>Поки що немає оцінених робіт.</div>
        ) : (
          /* Перебираємо команди з бази */
          teams.map((team, index) => {
            // Автоматично вираховуємо місце (index + 1)
            const rank = index + 1;

            return (
              <div key={team.id}>
                
                {/* Сама строка команди */}
                <div 
                  className={styles.teamRow} 
                  onClick={() => toggleRow(team.id)}
                >
                  <div className={styles.rank}>
                    {rank === 1 ? '🥇 1' : rank === 2 ? '🥈 2' : rank === 3 ? '🥉 3' : rank}
                  </div>
                  {/* Перевір, як називається поле з іменем команди в твоєму DTO (напр. teamName) */}
                  <div className={styles.teamName}>{team.teamName || `Team #${team.id}`}</div>
                  <div className={styles.totalScore}>{team.totalScore || 0}</div>
                </div>

                {/* Деталізація (показується тільки якщо ID збігається) */}
                {expandedTeamId === team.id && (
                  <div className={styles.detailsPanel}>
                    
                    {/* Якщо твій бекенд повертає детальні оцінки (scores), виводь їх. 
                        Якщо він повертає тільки одну загальну оцінку, цей блок можна прибрати або змінити */}
                    {team.scores && (
                      <div className={styles.scoresGrid}>
                        <div className={styles.scoreCard}>
                          <div className={styles.scoreLabel}>Backend</div>
                          <div className={styles.scoreValue}>{team.scores.backend || 0}</div>
                        </div>
                        <div className={styles.scoreCard}>
                          <div className={styles.scoreLabel}>Frontend</div>
                          <div className={styles.scoreValue}>{team.scores.frontend || 0}</div>
                        </div>
                        <div className={styles.scoreCard}>
                          <div className={styles.scoreLabel}>Database</div>
                          <div className={styles.scoreValue}>{team.scores.database || 0}</div>
                        </div>
                        <div className={styles.scoreCard}>
                          <div className={styles.scoreLabel}>Functionality</div>
                          <div className={styles.scoreValue}>{team.scores.functionality || 0}</div>
                        </div>
                      </div>
                    )}

                    <div className={styles.juryComment}>
                      <span>💬 Jury Comment:</span>
                      {team.juryComment || team.comment || "Журі ще не залишило коментар."}
                    </div>

                  </div>
                )}
                
              </div>
            );
          })
        )}
      </div>
    </main>
  );
}

export default Leaderboard;