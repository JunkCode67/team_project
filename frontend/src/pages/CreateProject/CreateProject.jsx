import styles from './CreateProject.module.css';
import appStyles from '../../App.module.css'; 

function CreateProject() {
  return (
    <main className={appStyles.pagePadding}>
      <h1 className={appStyles.heroTitle}>🚀 Create New Project</h1>
      <p className={appStyles.heroDesc}>
        Зарегистрируйте идею вашей команды. Вы сможете редактировать описание позже.
      </p>

      <div className={styles.formContainer}>
        <form onSubmit={(e) => { e.preventDefault(); alert('Проект создан!'); }}>
          
          <div className={styles.inputGroup}>
            <label className={styles.label}>Project Name *</label>
            <input type="text" className={styles.input} placeholder="Например: AI Analytics Dashboard" required />
          </div>

          <div className={styles.inputGroup}>
            <label className={styles.label}>Select Track (Категория) *</label>
            <select className={styles.select} required>
              <option value="">Выберите направление хакатона...</option>
              <option value="fintech">FinTech & Banking</option>
              <option value="edtech">Education Technology</option>
              <option value="gamedev">GameDev</option>
            </select>
          </div>

          <div className={styles.inputGroup}>
            <label className={styles.label}>Brief Description</label>
            <textarea className={styles.textarea} placeholder="Кратко опишите, какую проблему решает ваш проект..."></textarea>
          </div>

          <button type="submit" className={styles.submitBtn}>
            Register Project
          </button>
        </form>
      </div>
    </main>
  );
}

export default CreateProject;