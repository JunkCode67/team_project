import styles from './FilterTabs.module.css';

function FilterTabs() {
  return (
    <div className={styles.tabsContainer}>
      
      {/* Первая вкладка - Активная */}
      <div className={`${styles.tab} ${styles.active}`}>
        <div className={styles.tabHeader}>
          <span>All Competitions</span>
          <span>🗂️</span> {/* Вместо иконок пока юзаем эмодзи для простоты */}
        </div>
        <span className={styles.tabDesc}>Everything, past & present</span>
      </div>

      <div className={styles.tab}>
        <div className={styles.tabHeader}>
          <span>Featured</span>
          <span>⭐</span>
        </div>
        <span className={styles.tabDesc}>Premier challenges with prizes</span>
      </div>

      <div className={styles.tab}>
        <div className={styles.tabHeader}>
          <span>Hackathons</span>
          <span>🚀</span>
        </div>
        <span className={styles.tabDesc}>Open-ended explorations</span>
      </div>

      <div className={styles.tab}>
        <div className={styles.tabHeader}>
          <span>Getting Started</span>
          <span>⚑</span>
        </div>
        <span className={styles.tabDesc}>Approachable ML fundamentals</span>
      </div>

    </div>
  );
}

export default FilterTabs;