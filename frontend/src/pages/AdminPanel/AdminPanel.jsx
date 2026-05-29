import styles from './AdminPanel.module.css';
import appStyles from '../../App.module.css'; // Путь изменился из-за новой папки!

function AdminPanel() {
  
  const handleSaveSettings = (e) => {
    e.preventDefault();
    alert('Настройки хакатона успешно сохранены! ⚙️');
  };

  const handleDistribute = () => {
    // Тут будет запрос на бэкенд, который запустит алгоритм рандома
    alert('Магия запущена! 🪄 Работы случайным образом распределены между членами жюри.');
  };

  return (
    <main className={appStyles.pagePadding}>
      <h1 className={appStyles.heroTitle}>⚙️ Admin Panel</h1>
      <p className={appStyles.heroDesc}>
        Управление настройками хакатона и распределением работ для жюри.
      </p>

      {/* БЛОК 1: НАСТРОЙКИ ХАКАТОНА */}
      <section className={styles.adminSection}>
        <h2 className={styles.sectionTitle}>Hackathon Settings</h2>
        
        <form className={styles.formGrid} onSubmit={handleSaveSettings}>
          
          <div className={`${styles.inputGroup} ${styles.fullWidth}`}>
            <label className={styles.label}>Описание турнира</label>
            <textarea 
              className={`${styles.input} ${styles.textarea}`} 
              placeholder="Введите правила, формат и описание хакатона..."
            />
          </div>

          <div className={styles.inputGroup}>
            <label className={styles.label}>Лимит команд</label>
            <input type="number" className={styles.input} placeholder="Например: 50" />
          </div>

          <div className={styles.inputGroup}>
            <label className={styles.label}>Макс. участников в команде</label>
            <input type="number" className={styles.input} defaultValue={5} />
          </div>

          <div className={styles.inputGroup}>
            <label className={styles.label}>Старт регистрации</label>
            <input type="datetime-local" className={styles.input} />
          </div>

          <div className={styles.inputGroup}>
            <label className={styles.label}>Дедлайн сдачи работ</label>
            <input type="datetime-local" className={styles.input} />
          </div>

          <div className={styles.fullWidth} style={{ marginTop: '16px' }}>
            <button type="submit" className={styles.saveBtn}>Save Settings</button>
          </div>
        </form>
      </section>

      {/* БЛОК 2: РАСПРЕДЕЛЕНИЕ ЖЮРИ */}
      <section className={styles.adminSection}>
        <h2 className={styles.sectionTitle}>Jury Distribution</h2>
        <p style={{ color: '#4b5563', marginBottom: '24px' }}>
          Список сданных работ, ожидающих оценки. Нажмите кнопку, чтобы система случайным образом закрепила их за доступными членами жюри.
        </p>

        <div className={styles.worksList}>
          <div className={styles.workItem}>
            <span>1. Code Crusaders - "AI Analytics Dashboard"</span>
            <span style={{ color: '#d97706' }}>Not Assigned</span>
          </div>
          <div className={styles.workItem}>
            <span>2. Null Pointers - "FinTech App"</span>
            <span style={{ color: '#d97706' }}>Not Assigned</span>
          </div>
          <div className={styles.workItem}>
            <span>3. Runtime Terrors - "Smart City API"</span>
            <span style={{ color: '#d97706' }}>Not Assigned</span>
          </div>
        </div>

        <button className={styles.distributeBtn} onClick={handleDistribute}>
          <span>🪄</span> Distribute Randomly
        </button>
      </section>

    </main>
  );
}

export default AdminPanel;