import { useNavigate } from 'react-router-dom';
import styles from './Profile.module.css';
import appStyles from '../../App.module.css';

function Profile({ userRole, setUserRole }) {
  const navigate = useNavigate();

  // Функция выхода переехала сюда из Хедера
  const handleLogout = () => {
    setUserRole('guest');
    navigate('/login');
  };

  // Настраиваем отображение в зависимости от роли
  const getRoleDetails = () => {
    if (userRole === 'admin') {
      return { name: 'Administrator', letter: 'A', email: 'admin@admin.com', css: styles.roleAdmin };
    }
    if (userRole === 'jury') {
      return { name: 'Jury Member', letter: 'J', email: 'jury@jury.com', css: styles.roleJury };
    }
    // По умолчанию - Участник
    return { name: 'Participant', letter: 'P', email: 'user@example.com', css: styles.roleParticipant };
  };

  const details = getRoleDetails();

  return (
    <main className={appStyles.pagePadding}>
      <h1 className={appStyles.heroTitle}>👤 My Profile</h1>
      <p className={appStyles.heroDesc}>
        Manage your account settings and view your current role on the platform.
      </p>

      <div className={styles.profileCard}>
        
        <div className={styles.avatarBig}>{details.letter}</div>
        <div className={`${styles.roleBadge} ${details.css}`}>
          {details.name}
        </div>

        <div className={styles.infoGroup}>
          <div className={styles.label}>Account Role</div>
          <div className={styles.value}>{details.name}</div>
        </div>

        <div className={styles.infoGroup}>
          <div className={styles.label}>Email Address</div>
          <div className={styles.value}>{details.email}</div>
        </div>

        <button className={styles.logoutBtn} onClick={handleLogout}>
          Log Out
        </button>
        
      </div>
    </main>
  );
}

export default Profile;