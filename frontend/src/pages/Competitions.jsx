import styles from '../App.module.css';
import FilterTabs from '../components/FilterTabs/FilterTabs';
import CompetitionCard from '../components/CompetitionCard/CompetitionCard';

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
        
        <div className={styles.cardsGrid}>
          <CompetitionCard 
            image="https://picsum.photos/seed/kaggle1/400/200"
            title="Titanic - Machine Learning from Disaster"
            description="Start here! Predict survival on the Titanic and get familiar with ML basics."
            teams="12303"
          />
          <CompetitionCard 
            image="https://picsum.photos/seed/kaggle2/400/200"
            title="House Prices - Advanced Regression Techniques"
            description="Predict sales prices and practice feature engineering, RFs, and gradient boosting."
            teams="4162"
          />
          <CompetitionCard 
            image="https://picsum.photos/seed/kaggle3/400/200"
            title="Spaceship Titanic"
            description="Predict which passengers are transported to an alternate dimension."
            teams="2018"
          />
        </div>
      </div>
    </main>
  );
}

export default Competitions;