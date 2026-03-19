import styles from './Header.module.css';

function Header() {
  return (
    <header className={styles.headerContainer}>
      
      <div className={styles.searchWrapper}>
        <div className={styles.searchIcon}>
          <svg fill="none" stroke="currentColor" viewBox="0 0 24 24">
            <path strokeLinecap="round" strokeLinejoin="round" strokeWidth="2" d="M21 21l-6-6m2-5a7 7 0 11-14 0 7 7 0 0114 0z"></path>
          </svg>
        </div>
        
        <input
          type="text"
          placeholder="Search"
          className={styles.searchInput}
        />
      </div>

      <div className={styles.actions}>
        <button className={styles.signInBtn}>Sign In</button>
        <button className={styles.registerBtn}>Register</button>
      </div>
      
    </header>
  );
}

export default Header;