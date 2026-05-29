import { useState } from 'react';
import styles from './AdminPanel.module.css';
import appStyles from '../../App.module.css'; 

function AdminPanel() {
  // Стейт для форми створення турніру
  const [title, setTitle] = useState('');
  const [description, setDescription] = useState('');
  const [maxTeams, setMaxTeams] = useState(50);
  const [startDate, setStartDate] = useState('');
  const [registrationStart, setRegistrationStart] = useState('');
  const [registrationEnd, setRegistrationEnd] = useState('');
  
  // Стейт для сповіщень
  const [message, setMessage] = useState({ text: '', type: '' });

  // Функція відправки даних на бекенд
  const handleSaveSettings = async (e) => {
    e.preventDefault();
    setMessage({ text: '', type: '' });

    const token = localStorage.getItem('token');
    if (!token) {
      setMessage({ text: 'Помилка: Ви не авторизовані. Увійдіть у систему.', type: 'error' });
      return;
    }

    // Формуємо об'єкт так, як очікує CreateTournamentDto
    const newTournament = {
      title,
      description,
      maxTeams: parseInt(maxTeams),
      // Перетворюємо локальний час у формат ISO (UTC), який любить .NET
      startDate: new Date(startDate).toISOString(),
      registrationStart: new Date(registrationStart).toISOString(),
      registrationEnd: new Date(registrationEnd).toISOString(),
    };

    try {
      const response = await fetch('http://localhost:5058/api/Tournaments', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${token}`
        },
        body: JSON.stringify(newTournament)
      });

      if (response.ok) {
        setMessage({ text: '✅ Хакатон успішно створено!', type: 'success' });
        // Очищаємо форму (за бажанням)
        setTitle('');
        setDescription('');
      } else {
        setMessage({ text: '❌ Помилка створення. Перевірте правильність дат.', type: 'error' });
      }
    } catch (error) {
      setMessage({ text: '❌ Помилка з\'єднання з сервером.', type: 'error' });
    }
  };

  const handleDistribute = () => {
    alert('Магия запущена! 🪄 Работы случайным образом распределены между членами жюри.');
  };

  return (
    <main className={appStyles.pagePadding}>
      <h1 className={appStyles.heroTitle}>⚙️ Admin Panel</h1>
      <p className={appStyles.heroDesc}>
        Управление настройками хакатона и распределением работ для жюри.
      </p>

      {/* Вивід повідомлення про успіх або помилку */}
      {message.text && (
        <div style={{ 
          padding: '15px', 
          marginBottom: '20px', 
          borderRadius: '8px', 
          backgroundColor: message.type === 'error' ? '#fce8e6' : '#e6f4ea',
          color: message.type === 'error' ? '#d93025' : '#137333',
          border: `1px solid ${message.type === 'error' ? '#fad2cf' : '#ceead6'}`
        }}>
          {message.text}
        </div>
      )}

      {/* БЛОК 1: НАСТРОЙКИ ХАКАТОНА */}
      <section className={styles.adminSection}>
        <h2 className={styles.sectionTitle}>Hackathon Settings</h2>
        
        <form className={styles.formGrid} onSubmit={handleSaveSettings}>
          
          <div className={`${styles.inputGroup} ${styles.fullWidth}`}>
            <label className={styles.label}>Название турнира</label>
            <input 
              type="text"
              className={styles.input} 
              placeholder="Например: AI Spring Hackathon"
              value={title}
              onChange={(e) => setTitle(e.target.value)}
              required
            />
          </div>

          <div className={`${styles.inputGroup} ${styles.fullWidth}`}>
            <label className={styles.label}>Описание турнира</label>
            <textarea 
              className={`${styles.input} ${styles.textarea}`} 
              placeholder="Введите правила, формат и описание хакатона..."
              value={description}
              onChange={(e) => setDescription(e.target.value)}
              required
            />
          </div>

          <div className={styles.inputGroup}>
            <label className={styles.label}>Лимит команд</label>
            <input 
              type="number" 
              className={styles.input} 
              value={maxTeams}
              onChange={(e) => setMaxTeams(e.target.value)}
              required
            />
          </div>

          <div className={styles.inputGroup}>
            <label className={styles.label}>Старт турнира</label>
            <input 
              type="datetime-local" 
              className={styles.input} 
              value={startDate}
              onChange={(e) => setStartDate(e.target.value)}
              required
            />
          </div>

          <div className={styles.inputGroup}>
            <label className={styles.label}>Старт регистрации</label>
            <input 
              type="datetime-local" 
              className={styles.input} 
              value={registrationStart}
              onChange={(e) => setRegistrationStart(e.target.value)}
              required
            />
          </div>

          <div className={styles.inputGroup}>
            <label className={styles.label}>Конец регистрации</label>
            <input 
              type="datetime-local" 
              className={styles.input} 
              value={registrationEnd}
              onChange={(e) => setRegistrationEnd(e.target.value)}
              required
            />
          </div>

          <div className={styles.fullWidth} style={{ marginTop: '16px' }}>
            <button type="submit" className={styles.saveBtn}>Create Tournament</button>
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
        </div>

        <button className={styles.distributeBtn} onClick={handleDistribute}>
          <span>🪄</span> Distribute Randomly
        </button>
      </section>

    </main>
  );
}

export default AdminPanel;