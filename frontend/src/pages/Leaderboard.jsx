import { useState } from 'react';
import styles from './Leaderboard.module.css';
import appStyles from '../App.module.css';

// Временные данные (потом это прилетит по API от бэкендеров)
const MOCK_TEAMS = [
  {
    id: 1,
    rank: 1,
    name: "Code Crusaders",
    total: 385,
    scores: { backend: 95, frontend: 98, database: 92, functionality: 100 },
    comment: "Шикарная архитектура! Фронтенд выглядит очень современно, а бэкенд выдержал все нагрузочные тесты."
  },
  {
    id: 2,
    rank: 2,
    name: "Null Pointers",
    total: 340,
    scores: { backend: 85, frontend: 70, database: 90, functionality: 95 },
    comment: "Хорошая логика, но интерфейс немного недоработан. База данных спроектирована грамотно."
  },
  {
    id: 3,
    rank: 3,
    name: "Runtime Terrors",
    total: 290,
    scores: { backend: 60, frontend: 80, database: 70, functionality: 80 },
    comment: "Проект работает, но есть серьезные баги в маршрутизации на бэкенде. Дизайн приятный."
  }
];

function Leaderboard() {
  // Эта переменная хранит ID команды, которую мы сейчас развернули (null = ничего не открыто)
  const [expandedTeamId, setExpandedTeamId] = useState(null);

  // Функция для клика: если кликнули на открытую - закрываем, иначе открываем новую
  const toggleRow = (id) => {
    if (expandedTeamId === id) {
      setExpandedTeamId(null);
    } else {
      setExpandedTeamId(id);
    }
  };

  return (
    <main className={appStyles.pagePadding}>
      <h1 className={appStyles.heroTitle}>🏆 Leaderboard</h1>
      <p className={appStyles.heroDesc}>
        Final standings and detailed jury evaluations for the current hackathon.
      </p>

      <div className={styles.boardContainer}>
        {/* Шапка таблицы */}
        <div className={styles.headerRow}>
          <div>Rank</div>
          <div>Team Name</div>
          <div style={{ textAlign: 'right' }}>Total Score</div>
        </div>

        {/* Перебираем команды и рисуем строки */}
        {MOCK_TEAMS.map((team) => (
          <div key={team.id}>
            
            {/* Сама строка команды */}
            <div 
              className={styles.teamRow} 
              onClick={() => toggleRow(team.id)}
            >
              <div className={styles.rank}>
                {team.rank === 1 ? '🥇 1' : team.rank === 2 ? '🥈 2' : team.rank === 3 ? '🥉 3' : team.rank}
              </div>
              <div className={styles.teamName}>{team.name}</div>
              <div className={styles.totalScore}>{team.total}</div>
            </div>

            {/* Детализация (показывается только если ID совпадает) */}
            {expandedTeamId === team.id && (
              <div className={styles.detailsPanel}>
                
                <div className={styles.scoresGrid}>
                  <div className={styles.scoreCard}>
                    <div className={styles.scoreLabel}>Backend</div>
                    <div className={styles.scoreValue}>{team.scores.backend}</div>
                  </div>
                  <div className={styles.scoreCard}>
                    <div className={styles.scoreLabel}>Frontend</div>
                    <div className={styles.scoreValue}>{team.scores.frontend}</div>
                  </div>
                  <div className={styles.scoreCard}>
                    <div className={styles.scoreLabel}>Database</div>
                    <div className={styles.scoreValue}>{team.scores.database}</div>
                  </div>
                  <div className={styles.scoreCard}>
                    <div className={styles.scoreLabel}>Functionality</div>
                    <div className={styles.scoreValue}>{team.scores.functionality}</div>
                  </div>
                </div>

                <div className={styles.juryComment}>
                  <span>💬 Jury Comment:</span>
                  {team.comment}
                </div>

              </div>
            )}
            
          </div>
        ))}
      </div>
    </main>
  );
}

export default Leaderboard;