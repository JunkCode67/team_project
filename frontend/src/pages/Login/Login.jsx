import { useState } from 'react';
import styles from './Login.module.css';

function Login() {
  // Состояния для хранения почты и пароля
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');

  // Функция для классического входа
  const handleStandardLogin = (e) => {
    e.preventDefault(); // Чтобы страница не перезагружалась
    console.log('Отправляем на бэкенд:', { email, password });
    alert(`Пытаемся войти под почтой: ${email}`);
    // Тут будет fetch/axios запрос к вашему API
  };

  const handleGoogleLogin = () => {
    console.log('Redirecting to Google Auth...');
  };

  const handleGithubLogin = () => {
    console.log('Redirecting to GitHub Auth...');
  };

  return (
    <div className={styles.pageContainer}>
      <div className={styles.loginCard}>
        
        <div className={styles.logoCircle}></div>
        <h1 className={styles.title}>Welcome back</h1>
        <p className={styles.subtitle}>Sign in to your account to continue</p>

        {/* --- ФОРМА ДЛЯ EMAIL И ПАРОЛЯ --- */}
        <form className={styles.form} onSubmit={handleStandardLogin}>
          <input 
            type="email" 
            placeholder="Email address" 
            className={styles.input}
            value={email}
            onChange={(e) => setEmail(e.target.value)}
            required
          />
          <input 
            type="password" 
            placeholder="Password" 
            className={styles.input}
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            required
          />
          <button type="submit" className={styles.submitBtn}>
            Sign In
          </button>
        </form>

        {/* Разделитель */}
        <div className={styles.divider}>OR</div>

        {/* --- КНОПКИ СОЦСЕТЕЙ --- */}
        <button className={`${styles.authBtn} ${styles.googleBtn}`} onClick={handleGoogleLogin}>
          <span style={{ fontSize: '20px' }}>🌐</span>
          Continue with Google
        </button>

        <button className={`${styles.authBtn} ${styles.githubBtn}`} onClick={handleGithubLogin}>
          <span style={{ fontSize: '20px' }}>🐙</span>
          Continue with GitHub
        </button>

      </div>
    </div>
  );
}

export default Login;