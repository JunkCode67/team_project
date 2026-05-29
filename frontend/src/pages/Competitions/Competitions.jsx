import { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import styles from '../../App.module.css';
import FilterTabs from '../../components/FilterTabs/FilterTabs';
import CompetitionCard from '../../components/CompetitionCard/CompetitionCard';

function Competitions() {
  return (
    <main className={styles.pagePadding}>
      <h1 className={styles.heroTitle}>Competitions and Hackathons</h1>
      <p className={styles.heroDesc}>
        Grow your data science skills by competing in our exciting competitions.
      </p>

      <FilterTabs />

      <div>
        <h2 className={styles.sectionTitle}>
          <span>⚑</span> Getting Started
        </h2>
        <p className={styles.sectionDesc}>Competitions with approachable ML fundamentals.</p>
        
        {/* Блок повідомлення про помилку або прохання авторизації */}
        {error && (
          <div style={{ color: '#d93025', padding: '15px', marginBottom: '20px', border: '1px solid #fce8e6', borderRadius: '8px', backgroundColor: '#fce8e6' }}>
            {error} <br/>
            <Link to="/login" style={{ fontWeight: 'bold', color: '#1a73e8', textDecoration: 'none' }}>
              Перейти на сторінку входу ➔
            </Link>
          </div>
        )}

        <div className={styles.cardsGrid}>
          {/* Стан завантаження */}
          {loading ? (
            <p>Завантаження турнірів...</p>
          ) : (
            /* Якщо помилок немає, але турнірів теж немає */
            !error && tournaments.length === 0 ? (
              <p>Наразі немає доступних змагань.</p>
            ) : (
              /* Рендеримо реальні картки з бази даних */
              tournaments.map((t) => (
                <CompetitionCard 
                  key={t.id}
                  image={`https://picsum.photos/seed/${t.id}/400/200`} 
                  title={t.title}
                  description={t.description || "Опис турніру відсутній."}
                  teams={t.teamsCount?.toString() || "0"} 
                />
              ))
            )
          )}
        </div>
      </div>
    </main>
  );
}

export default Competitions;