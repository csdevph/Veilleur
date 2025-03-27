using System;
using System.Timers;

namespace Veilleur
{
    /// <summary>
    /// Session de l'utilisateur
    /// </summary>
    internal class Session
    {
        #region Atttributs
        internal static TimeSpan DureePause = new TimeSpan(0, UtilSession.DureePause, 0);
        internal static TimeSpan DureePreavisFin = new TimeSpan(0, UtilSession.DureePreavis, 0);

        private static Session instance;     // Singleton
        private EtatSession etat;
        private TimeSpan duree;
#if DEBUG
        const double timerInterval = 200;
#else
        const double timerInterval = 1000;
#endif
        private readonly Timer timer = new Timer { Interval = timerInterval, Enabled = true };
        internal event EventHandler ChangeEtat;
        internal event EventHandler TicTac;
        internal event EventHandler TicTacMinute;

        public DateTime HeureLimite;

        /// <summary>
        /// Durée de la sesssion. Elle s'écoule sur deux états : EnCours et EnPreavisFin
        /// </summary>
        public TimeSpan Duree { get => duree; }

        public EtatSession Etat
        {
            get { return etat; }
            set
            {
                if (etat == value) return;
                etat = value;
                if (etat is EtatFinal) timer.Stop();
                // on signale le changement d'état
                ChangeEtat?.Invoke(this, new EventArgs());
            }
        }
        #endregion

        #region Constructeurs
        // Constructeur privé, pas d'instanciation manuelle du Singleton 
        private Session()
        {
            duree = new TimeSpan(0, UtilSession.DureeSession, 0);
            HeureLimite = DateTime.Now + duree;
            timer.Elapsed += CompteARebours;
            timer.Elapsed += Timer_Elapsed;
            etat = new EtatZero(this);
        }
        #endregion

        #region Méthodes
        /// <summary>
        /// Retourne l'instance de la session (Singleton)
        /// </summary>
        public static Session Instance()
        {
            if (instance == null) instance = new Session();
            return instance;
        }

        private void CompteARebours(object sender, EventArgs e)
        {
            Etat.Duree -= TimeSpan.FromSeconds(1);
            if (etat is EnCours || etat is EnPreavisFin)
            {
                duree -= TimeSpan.FromSeconds(1);
            }
            if (Etat.Duree <= TimeSpan.Zero)
            {
                Etat = Etat.EtatSuivant();
            }
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            TicTac?.Invoke(this, new EventArgs());
            if (e.SignalTime.Second == 0)
            {
                TicTacMinute?.Invoke(this, new EventArgs());
            }
        }
        #endregion
    }
}
