import { useState } from 'react';
import styles from './SubmitWork.module.css';
import appStyles from '../App.module.css';

function SubmitWork() {
  // Состояния для хранения того, что юзер ввел в поля
  const [github, setGithub] = useState('');
  const [youtube, setYoutube] = useState('');
  const [description, setDescription] = useState('');
  
  // Имитация дедлайна (потом будете получать статус с бэкенда)
  const isDeadlinePassed = false; // Поменяй на true для теста блокировки!

  // Функция, которая сработает при нажатии на Submit
  const handleSubmit = (e) => {
    e.preventDefault(); // Останавливает стандартную перезагрузку страницы браузером
    alert('Работа успешно отправлена! 🚀');
    // Тут потом ваши бэкендеры попросят добавить fetch/axios запрос к их API
  };

  return (
    <main className={appStyles.pagePadding}>
      <h1 className={appStyles.heroTitle}>📤 Submit Work</h1>
      <p className={appStyles.heroDesc}>
        Отправьте финальный проект вашей команды. Убедитесь, что репозиторий открыт, а видео доступно по ссылке.
      </p>

      <div className={styles.formContainer}>
        <form onSubmit={handleSubmit}>
          
          <div className={styles.inputGroup}>
            <label className={styles.label}>GitHub Repository URL *</label>
            <input 
              type="url" 
              className={styles.input} 
              placeholder="https://github.com/username/repo"
              value={github}
              onChange={(e) => setGithub(e.target.value)}
              required
              disabled={isDeadlinePassed}
            />
          </div>

          <div className={styles.inputGroup}>
            <label className={styles.label}>YouTube Demo Video URL</label>
            <input 
              type="url" 
              className={styles.input} 
              placeholder="https://youtube.com/watch?v=..."
              value={youtube}
              onChange={(e) => setYoutube(e.target.value)}
              disabled={isDeadlinePassed}
            />
          </div>

          <div className={styles.inputGroup}>
            <label className={styles.label}>Project Description *</label>
            <textarea 
              className={styles.textarea} 
              placeholder="Кратко опишите архитектуру, стек технологий и основные фичи..."
              value={description}
              onChange={(e) => setDescription(e.target.value)}
              required
              disabled={isDeadlinePassed}
            ></textarea>
          </div>

          <button 
            type="submit" 
            className={styles.submitBtn}
            disabled={isDeadlinePassed}
          >
            {isDeadlinePassed ? 'Дедлайн прошел 🔒' : 'Submit Project'}
          </button>

          {/* Сообщение об ошибке появляется только если дедлайн прошел */}
          {isDeadlinePassed && (
            <div className={styles.statusMessage}>
              Прием работ закрыт. Вы больше не можете изменить или отправить проект.
            </div>
          )}

        </form>
      </div>
    </main>
  );
}

export default SubmitWork;