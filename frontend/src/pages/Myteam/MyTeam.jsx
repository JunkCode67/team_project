import { useState } from 'react';
import styles from './MyTeam.module.css';
import appStyles from '../../App.module.css';

function MyTeam() {
  // Текущие участники (потом будут грузиться с бэка)
  const [members, setMembers] = useState([
    { id: 1, email: 'captain@example.com', isCaptain: true },
    { id: 2, email: 'backend_dev@example.com', isCaptain: false }
  ]);
  
  // Состояние инпута для инвайта
  const [inviteEmail, setInviteEmail] = useState('');

  // Функция добавления тиммейта
  const handleAddMember = (e) => {
    e.preventDefault();
    if (!inviteEmail) return;

    // Простая проверка, есть ли уже такой email
    if (members.find(m => m.email === inviteEmail)) {
      alert('Этот участник уже в команде!');
      return;
    }

    const newMember = {
      id: Date.now(), // Генерируем временный ID
      email: inviteEmail,
      isCaptain: false
    };

    setMembers([...members, newMember]);
    setInviteEmail(''); // Очищаем поле ввода
  };

  // Функция удаления тиммейта
  const handleRemoveMember = (idToRemove) => {
    // Оставляем только тех, чей ID не совпадает с удаляемым
    setMembers(members.filter(m => m.id !== idToRemove));
  };

  return (
    <main className={appStyles.pagePadding}>
      <h1 className={appStyles.heroTitle}>👥 My Team</h1>
      <p className={appStyles.heroDesc}>
        Управление составом вашей команды. Капитан может приглашать и удалять участников.
      </p>

      <div className={styles.teamContainer}>
        
        {/* Шапка команды */}
        <div className={styles.teamHeader}>
          <div className={styles.teamNameBox}>
            <h2>Code Crusaders</h2>
            <span className={styles.teamRole}>👑 Team Captain</span>
          </div>
          <div style={{ color: '#6b7280', fontWeight: '600' }}>
            {members.length} / 5 Members
          </div>
        </div>

        {/* Форма инвайта */}
        <div className={styles.inviteSection}>
          <h3 className={styles.inviteTitle}>Invite new members</h3>
          <form className={styles.inviteForm} onSubmit={handleAddMember}>
            <input 
              type="email" 
              className={styles.input} 
              placeholder="Enter teammate's email address..." 
              value={inviteEmail}
              onChange={(e) => setInviteEmail(e.target.value)}
              disabled={members.length >= 5} // Блокируем, если лимит
            />
            <button 
              type="submit" 
              className={styles.addBtn}
              disabled={members.length >= 5}
            >
              Add to team
            </button>
          </form>
          {members.length >= 5 && (
            <p style={{ color: '#ef4444', fontSize: '12px', marginTop: '8px' }}>
              Вы достигли лимита участников в команде (5/5).
            </p>
          )}
        </div>

        {/* Список команды */}
        <div className={styles.membersList}>
          {members.map((member) => (
            <div key={member.id} className={styles.memberCard}>
              <div className={styles.memberInfo}>
                <div className={styles.avatar}>
                  {/* Берем первую букву почты для аватарки */}
                  {member.email.charAt(0).toUpperCase()}
                </div>
                <div>
                  <div className={styles.memberEmail}>{member.email}</div>
                  <div style={{ fontSize: '12px', color: '#6b7280', marginTop: '2px' }}>
                    {member.isCaptain ? 'Captain' : 'Member'}
                  </div>
                </div>
              </div>
              
              {/* Кнопка удаления не показывается для капитана */}
              {!member.isCaptain && (
                <button 
                  className={styles.removeBtn} 
                  onClick={() => handleRemoveMember(member.id)}
                >
                  Remove
                </button>
              )}
            </div>
          ))}
        </div>

      </div>
    </main>
  );
}

export default MyTeam;