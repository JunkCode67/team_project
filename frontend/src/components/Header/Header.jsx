import { Link, useNavigate } from 'react-router-dom';
import styles from './Header.module.css';

function Header({ userRole }) {
  const navigate = useNavigate();

  return (
    <header className={styles.header}>
      <div className={styles.searchContainer}>
        <span className={styles.searchIcon}>🔍</span>
        <input 
          type="text" 
          placeholder="Search competitions, datasets..." 
          className={styles.searchInput}
        />
      </div>
      
      <div className={styles.authButtons}>
        {userRole !== 'guest' ? (
          // Теперь аватарка перекидывает на /profile
          <div 
            className={styles.avatar} 
            onClick={() => navigate('/profile')}
            title="Go to Profile"
          >
            {userRole === 'admin' ? 'A' : userRole === 'jury' ? 'J' : 'P'}
          </div>
        ) : (
          <Link to="/login" className={styles.signInBtn}>Sign In</Link>
        )}
      </div>
    </header>
  );
}

export default Header;