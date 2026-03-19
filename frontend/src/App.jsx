import { BrowserRouter, Routes, Route } from 'react-router-dom';
import styles from './App.module.css';
import Sidebar from './components/Sidebar/Sidebar';
import Header from './components/Header/Header';
import Competitions from './pages/Competitions';
import Leaderboard from './pages/Leaderboard';
import SubmitWork from './pages/SubmitWork';

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
          </Routes>
          
        </div>
      </div>
    </BrowserRouter>
  );
}

export default App;