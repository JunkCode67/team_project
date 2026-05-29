import { useState } from 'react';
import { BrowserRouter, Routes, Route } from 'react-router-dom';
import styles from './App.module.css';
import Sidebar from './components/Sidebar/Sidebar';
import Header from './components/Header/Header';

import Competitions from './pages/Competitions/Competitions';
import Leaderboard from './pages/Leaderboard/Leaderboard';
import SubmitWork from './pages/SubmitWork/SubmitWork';
import JuryDashboard from './pages/JuryDashboard/JuryDashboard';
import AdminPanel from './pages/AdminPanel/AdminPanel';
import Login from './pages/Login/Login';
import MyTeam from './pages/MyTeam/MyTeam';
import CreateProject from './pages/CreateProject/CreateProject';
import Profile from './pages/Profile/Profile';

function App() {
  const [userRole, setUserRole] = useState('participant');

  return (
    <BrowserRouter>
      <div className={styles.appContainer}>
        
        {/* Передаем роль в боковое меню, чтобы оно знало, что скрывать */}
        <Sidebar userRole={userRole} />

        <div className={styles.mainContent}>
          
          {/* Передаем роль и функцию её смены в шапку (для нашего переключателя) */}
          <Header userRole={userRole} setUserRole={setUserRole} />
          
          <Routes>
            <Route path="/" element={<Competitions />} />
            <Route path="/leaderboard" element={<Leaderboard />} />
            <Route path="/submit" element={<SubmitWork />} />
            <Route path="/jury" element={<JuryDashboard />} />
            <Route path="/admin" element={<AdminPanel />} />
            <Route path="/team" element={<MyTeam />} />
            <Route path="/login" element={<Login setUserRole={setUserRole} />} />
            <Route path="/create" element={<CreateProject />} />
            <Route path="/profile" element={<Profile userRole={userRole} setUserRole={setUserRole} />} />
          </Routes>
          
        </div>
      </div>
    </BrowserRouter>
  );
}

export default App;