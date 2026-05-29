import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import styles from './Login.module.css';

function Login() {
  // Стейт для пошти, пароля та помилок
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [error, setError] = useState('');

  // Ініціалізуємо хук для перенаправлення
  const navigate = useNavigate();

  // Справжня функція для класичного входу
  const handleStandardLogin = async (e) => {
    e.preventDefault(); // Щоб сторінка не перезавантажувалась
    setError(''); // Очищаємо попередні помилки

    try {
      const response = await fetch('http://localhost:8080/api/Auth/login', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ email, password })
      });

      if (response.ok) {
        const data = await response.json();
        // Зберігаємо токен у пам'ять браузера
        localStorage.setItem('token', data.token); 
        // Перекидаємо користувача на сторінку змагань
        navigate('/'); 
      } else {
        setError('Неправильний email або пароль');
      }
    } catch (err) {
      setError('Помилка з\'єднання з сервером');
    }
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

        {/* --- ВИВІД ПОМИЛКИ --- */}
        {error && (
          <div style={{ color: 'red', marginBottom: '15px', textAlign: 'center', fontSize: '14px' }}>
            {error}
          </div>
        )}

        {/* --- ФОРМА ДЛЯ EMAIL ТА ПАРОЛЯ --- */}
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

        {/* Розділювач */}
        <div className={styles.divider}>OR</div>

        {/* --- КНОПКИ СОЦМЕРЕЖ --- */}
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