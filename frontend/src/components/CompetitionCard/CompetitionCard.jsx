import styles from './CompetitionCard.module.css';

function CompetitionCard({ image, title, description, teams }) {
  return (
    <div className={styles.card}>
      <img src={image} alt={title} className={styles.image} />

      <div className={styles.content}>
        <p className={styles.tag}>🏆 COMPETITION</p>
        <h3 className={styles.title}>{title}</h3>
        <p className={styles.description}>{description}</p>
        
        <div className={styles.footer}>
          {teams} Teams
        </div>
      </div>
    </div>
  );
}

export default CompetitionCard;