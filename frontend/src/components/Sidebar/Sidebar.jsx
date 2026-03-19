import { Link } from 'react-router-dom';
import styles from './Sidebar.module.css';

function Sidebar() {
  return (
    <div className={styles.sidebar}>
      
      <div className={styles.logoContainer}>
        <div className={styles.logoCircle}></div>
        <span className={styles.logoText}>HackPlatform</span>
      </div>

      <button className={styles.createBtn}>
        <span style={{ fontSize: '20px', lineHeight: 1 }}>+</span>
        <span>New Project</span>
      </button>

      <nav className={styles.navMenu}>
        
        {/* --- ОСНОВНОЕ МЕНЮ --- */}
        <div style={{ fontSize: '12px', fontWeight: 'bold', color: '#9ca3af', marginBottom: '8px', marginTop: '16px', padding: '0 12px', textTransform: 'uppercase', letterSpacing: '0.05em' }}>
          Main
        </div>
        
        <Link to="/" className={styles.navItem}>🏠 Home</Link>
        {/* Временно вешаем active на соревнования */}
        <Link to="/" className={`${styles.navItem} ${styles.active}`}>🏆 Competitions</Link>
        <Link to="/leaderboard" className={styles.navItem}>📊 Leaderboard</Link>
        
        {/* --- МЕНЮ УЧАСТНИКА --- */}
        <div style={{ fontSize: '12px', fontWeight: 'bold', color: '#9ca3af', marginBottom: '8px', marginTop: '24px', padding: '0 12px', textTransform: 'uppercase', letterSpacing: '0.05em' }}>
          Participant
        </div>
        <a href="#" className={styles.navItem}>👥 My Team</a>
        <Link to="/submit" className={styles.navItem}>📤 Submit Work</Link>

        {/* --- МЕНЮ ЖЮРИ / АДМИНА --- */}
        <div style={{ fontSize: '12px', fontWeight: 'bold', color: '#9ca3af', marginBottom: '8px', marginTop: '24px', padding: '0 12px', textTransform: 'uppercase', letterSpacing: '0.05em' }}>
          Management
        </div>
        <a href="#" className={styles.navItem}>⚖️ Jury Dashboard</a>
        <a href="#" className={styles.navItem}>⚙️ Admin Panel</a>
      </nav>
      
    </div>
  );
}

export default Sidebar;