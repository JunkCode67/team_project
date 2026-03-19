import { BrowserRouter, Routes, Route } from 'react-router-dom';
import styles from './App.module.css';
import Sidebar from './components/Sidebar/Sidebar';
import Header from './components/Header/Header';
import Competitions from './pages/Competitions/Competitions';
import Leaderboard from './pages/Leaderboard/Leaderboard';
import SubmitWork from './pages/SubmitWork/SubmitWork';
import JuryDashboard from './pages/JuryDashboard/JuryDashboard';
import AdminPanel  from './pages/AdminPanel/AdminPanel';
import Login from './pages/Login/Login';

function App() {
  return (
    <BrowserRouter>
      <div className={styles.appContainer}>
        
        <Sidebar />

        <div className={styles.mainContent}>
          
          <Header />
          
          <Routes>
            {/* Если путь "/", показываем страницу соревнований */}
            <Route path="/" element={<Competitions />} />
            {/* Если путь "/leaderboard", показываем таблицу лидеров */}
            <Route path="/leaderboard" element={<Leaderboard />} />
            <Route path="/submit" element={<SubmitWork />} />
            <Route path="/jury" element={<JuryDashboard />} />
            <Route path="/admin" element={<AdminPanel />} />
            <Route path="/login" element={<Login />} />
          </Routes>
          
        </div>
      </div>
    </BrowserRouter>
  );
}

export default App;