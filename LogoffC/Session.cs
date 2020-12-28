using Microsoft.Win32;
using System;
using System.Timers;

namespace LogoffC
{
    /// <summary>
    /// Session de l'utilisateur
    /// </summary>
    internal class Session
    {
        #region Atttributs

        private static Session instance = null;     // Singleton
        private EtatSession etat;
        private TimeSpan duree;

        private readonly Timer timer = new Timer { Interval = 1000, Enabled = true };
        internal event EventHandler ChangeEtat;
        internal event EventHandler TicTac;
        internal event EventHandler TicTacMinute;

        internal const int preavisFin = 2;    // 5 minutes
        internal const int dureeSursis = 3;    // 15 minutes
        internal const int pauseMaxi = 1;    // 3 minutes

        public DateTime HeureLimite;

        public string DureeLisible
        {
            get => duree.ToString("h' h 'mm' min 'ss' s'");
        }

        public EtatSession Etat
        {
            get { return etat; }
            set
            {
                if (etat != value)
                {
                    etat = value;
                    // on signale le changement d'état
                    if (ChangeEtat != null) ChangeEtat(this, new EventArgs());
                }
            }
        }

        public TimeSpan Duree { get => duree; set => duree = value; }

        #endregion

        #region Constructeurs
        // Constructeur privé, pas d'instanciation manuelle du Singleton 
        private Session(int minutes = 60)
        {
            duree = new TimeSpan(0, minutes, 0);
            HeureLimite = DateTime.Now + duree;
            timer.Elapsed += CompteARebours;
            timer.Elapsed += timer_Elapsed;
            etat = new EtatZero(this);
        }
        #endregion

        #region Méthodes
        // Accesseur retournant le Singleton
        public static Session Instance()
        {
            if (instance == null) instance = new Session();
            return instance;
        }

        private void CompteARebours(object sender, EventArgs e)
        {
            Etat.DureeEtat -= TimeSpan.FromSeconds(1);
            if (Etat.DureeEtat < TimeSpan.Zero)
            {
                Etat = Etat.EtatSuivant();
            }
        }

        private void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (TicTac != null) TicTac(this, new EventArgs());
            if (e.SignalTime.Second == 0)
            {
                if (TicTacMinute != null) TicTacMinute(this, new EventArgs());
            }
        }
        #endregion
    }
}
