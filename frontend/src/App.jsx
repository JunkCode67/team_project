import styles from './App.module.css';
import Sidebar from './components/Sidebar/Sidebar';
import Header from './components/Header/Header';
import CompetitionCard from './components/CompetitionCard/CompetitionCard';

function App() {
  return (
    <div className={styles.appContainer}>
      
      <Sidebar />

      <div className={styles.mainContent}>
        
        <Header />

        <main className={styles.pagePadding}>
          <h1 className={styles.heroTitle}>Competitions and Hackathons</h1>
          <p className={styles.heroDesc}>
            Grow your data science skills by competing in our exciting competitions.
          </p>

          <div>
            <h2 className={styles.sectionTitle}>
              <span>⚑</span> Getting Started
            </h2>
            <p className={styles.sectionDesc}>Competitions with approachable ML fundamentals.</p>
            
            <div className={styles.cardsGrid}>
              <CompetitionCard 
                image="https://storage.googleapis.com/kaggle-competitions/kaggle/3136/logos/header.png"
                title="Titanic - Machine Learning from Disaster"
                description="Start here! Predict survival on the Titanic and get familiar with ML basics."
                teams="12303"
              />
              <CompetitionCard 
                image="https://storage.googleapis.com/kaggle-competitions/kaggle/5407/logos/header.png"
                title="House Prices - Advanced Regression Techniques"
                description="Predict sales prices and practice feature engineering, RFs, and gradient boosting."
                teams="4162"
              />
              <CompetitionCard 
                image="https://storage.googleapis.com/kaggle-competitions/kaggle/34377/logos/header.png"
                title="Spaceship Titanic"
                description="Predict which passengers are transported to an alternate dimension."
                teams="2018"
              />
            </div>
          </div>
        </main>
        
      </div>
    </div>
  )
}

export default App;