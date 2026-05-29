import { useState } from 'react';
import styles from './SubmitWork.module.css';
import appStyles from '../../App.module.css';

function SubmitWork() {
  // Состояния для хранения того, что юзер ввел в поля
  const [github, setGithub] = useState('');
  const [youtube, setYoutube] = useState('');
  const [description, setDescription] = useState('');
  
  // Состояние для вывода сообщений пользователю
  const [message, setMessage] = useState({ text: '', type: '' });
  
  // Имитация дедлайна (в будущем можно проверять по дате текущего турнира)
  const isDeadlinePassed = false; 

  // Функция отправки данных на бэкенд
  const handleSubmit = async (e) => {
    e.preventDefault();
    setMessage({ text: '', type: '' }); // Очищаем старые сообщения

    const token = localStorage.getItem('token');
    if (!token) {
      setMessage({ text: '⚠️ Ошибка: Вы не авторизованы. Пожалуйста, войдите в систему.', type: 'error' });
      return;
    }

    // Формируем объект данных. 
    // ВАЖНО: Убедись, что названия полей совпадают с твоим SubmissionDto в C#!
    const submissionData = {
      githubUrl: github,
      videoUrl: youtube, // или demoUrl (проверь в своем DTO)
      description: description,
      // roundId: 1 // Если твой бэкенд требует ID раунда или турнира, добавь его сюда
    };

    try {
      const response = await fetch('http://localhost:5058/api/Submissions', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${token}` // Прикрепляем JWT токен
        },
        body: JSON.stringify(submissionData)
      });

      if (response.ok) {
        setMessage({ text: '✅ Работа успешно отправлена! 🚀', type: 'success' });
        // Очищаем форму после успешной отправки
        setGithub('');
        setYoutube('');
        setDescription('');
      } else {
        // Если статус 400 (Bad Request) или 500
        setMessage({ text: '❌ Ошибка при отправке работы. Проверьте данные.', type: 'error' });
      }
    } catch (error) {
      setMessage({ text: '❌ Ошибка соединения с сервером.', type: 'error' });
    }
  };

  return (
    <main className={appStyles.pagePadding}>
      <h1 className={appStyles.heroTitle}>📤 Submit Work</h1>
      <p className={appStyles.heroDesc}>
        Отправьте финальный проект вашей команды. Убедитесь, что репозиторий открыт, а видео доступно по ссылке.
      </p>

      <div className={styles.formContainer}>
        {/* Вывод сообщения об успехе или ошибке API */}
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
            <div className={styles.statusMessage} style={{ color: '#d93025', marginTop: '10px' }}>
              Прием работ закрыт. Вы больше не можете изменить или отправить проект.
            </div>
          )}

        </form>
      </div>
    </main>
  );
}

export default SubmitWork;