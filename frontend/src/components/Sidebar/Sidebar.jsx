import styles from './Sidebar.module.css';

function Sidebar() {
  return (
    <div className={styles.sidebar}>
      
      <div className={styles.logoContainer}>
        <div className={styles.logoCircle}></div>
        <span className={styles.logoText}>KaggleClone</span>
      </div>

      <button className={styles.createBtn}>
        <span style={{ fontSize: '20px', lineHeight: 1 }}>+</span>
        <span>Create</span>
      </button>

      <nav className={styles.navMenu}>
        <a href="#" className={styles.navItem}>🏠 Home</a>
        {/* Обрати внимание, как добавляются два класса: */}
        <a href="#" className={`${styles.navItem} ${styles.active}`}>🏆 Competitions</a>
        <a href="#" className={styles.navItem}>📊 Datasets</a>
        <a href="#" className={styles.navItem}>💻 Code</a>
      </nav>
      
    </div>
  );
}

export default Sidebar;