import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import styles from './Login.module.css';

function Login({ setUserRole }) {
  const [isLogin, setIsLogin] = useState(true);
  const [confirmPassword, setConfirmPassword] = useState('');
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [error, setError] = useState('');
  const navigate = useNavigate();

  const handleSubmit = (e) => {
    e.preventDefault();
    
    if (isLogin) {
      // ИМИТАЦИЯ БЭКЕНДА: Проверяем почту и выдаем роль
      let assignedRole = 'participant'; 
      
      if (email === 'admin@admin.com') {
        assignedRole = 'admin';
      } else if (email === 'jury@jury.com') {
        assignedRole = 'jury';
      }

      setUserRole(assignedRole); // Сохраняем роль
      navigate('/'); // Перекидываем на главную

    } else {
      if (password !== confirmPassword) {
        alert('❌ Пароли не совпадают!');
        return;
      }
      setUserRole('participant');
      navigate('/');
    }
  };

  // Вот эти функции ты случайно удалил:


  // Справжня функція для класичного входу
  const handleStandardLogin = async (e) => {
    e.preventDefault(); // Щоб сторінка не перезавантажувалась
    setError(''); // Очищаємо попередні помилки

    try {
      const response = await fetch('http://localhost:5058/api/Auth/login', {
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

  const toggleMode = () => {
    setIsLogin(!isLogin);
    setEmail('');
    setPassword('');
    setConfirmPassword('');
  };

  return (
    <div className={styles.pageContainer}>
      <div className={styles.loginCard}>
        
        <div className={styles.logoCircle}></div>
        
        <h1 className={styles.title}>
          {isLogin ? 'Welcome back' : 'Create an account'}
        </h1>
        <p className={styles.subtitle}>
          {isLogin 
            ? 'Sign in to your account to continue' 
            : 'Join our hackathon platform today'}
        </p>

        <form className={styles.form} onSubmit={handleSubmit}>
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

          {!isLogin && (
            <input 
              type="password" 
              placeholder="Confirm Password" 
              className={styles.input}
              value={confirmPassword}
              onChange={(e) => setConfirmPassword(e.target.value)}
              required
            />
          )}

          <button type="submit" className={styles.submitBtn}>
            {isLogin ? 'Sign In' : 'Sign Up'}
          </button>
        </form>

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

        <div className={styles.toggleContainer}>
          {isLogin ? "Don't have an account?" : "Already have an account?"}
          <span className={styles.toggleLink} onClick={toggleMode}>
            {isLogin ? 'Sign up' : 'Sign in'}
          </span>
        </div>

      </div>
    </div>
  );
}

export default Login;