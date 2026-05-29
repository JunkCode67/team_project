import { Link, useLocation } from 'react-router-dom';
import styles from './Sidebar.module.css';

function Sidebar({ userRole }) {
  // Получаем текущий путь (например, '/' или '/leaderboard')
  const location = useLocation();
  const path = location.pathname;

  return (
    <div className={styles.sidebar}>
      
      <div className={styles.logoContainer}>
        <div className={styles.logoCircle}></div>
        <span className={styles.logoText}>HackPlatform</span>
      </div>

      {/* Кнопка создания проекта ведет на /create */}
      {userRole === 'participant' && (
        <Link to="/create" style={{ textDecoration: 'none' }}>
          <button className={styles.createBtn}>
            <span style={{ fontSize: '20px', lineHeight: 1 }}>+</span>
            <span>New Project</span>
          </button>
        </Link>
      )}

      <nav className={styles.navMenu}>
        
        <div style={{ fontSize: '12px', fontWeight: 'bold', color: '#9ca3af', marginBottom: '8px', marginTop: '16px', padding: '0 12px', textTransform: 'uppercase', letterSpacing: '0.05em' }}>
          Main
        </div>
        
        <a href="#" className={styles.navItem}>🏠 Home</a>
        
        {/* Подсвечиваем, если мы на главной */}
        <Link to="/" className={`${styles.navItem} ${path === '/' ? styles.active : ''}`}>
          🏆 Competitions
        </Link>
        
        {/* Подсвечиваем, если мы на /leaderboard */}
        <Link to="/leaderboard" className={`${styles.navItem} ${path === '/leaderboard' ? styles.active : ''}`}>
          📊 Leaderboard
        </Link>
        
        {userRole === 'participant' && (
          <>
            <div style={{ fontSize: '12px', fontWeight: 'bold', color: '#9ca3af', marginBottom: '8px', marginTop: '24px', padding: '0 12px', textTransform: 'uppercase', letterSpacing: '0.05em' }}>
              Participant
            </div>
            <Link to="/team" className={`${styles.navItem} ${path === '/team' ? styles.active : ''}`}>👥 My Team</Link>
            <Link to="/submit" className={`${styles.navItem} ${path === '/submit' ? styles.active : ''}`}>📤 Submit Work</Link>
          </>
        )}

        {(userRole === 'jury' || userRole === 'admin') && (
          <>
            <div style={{ fontSize: '12px', fontWeight: 'bold', color: '#9ca3af', marginBottom: '8px', marginTop: '24px', padding: '0 12px', textTransform: 'uppercase', letterSpacing: '0.05em' }}>
              Management
            </div>
            <Link to="/jury" className={`${styles.navItem} ${path === '/jury' ? styles.active : ''}`}>⚖️ Jury Dashboard</Link>
            {userRole === 'admin' && (
              <Link to="/admin" className={`${styles.navItem} ${path === '/admin' ? styles.active : ''}`}>⚙️ Admin Panel</Link>
            )}
          </>
        )}

      </nav>
    </div>
  );
}

export default Sidebar;