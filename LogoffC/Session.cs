﻿using Microsoft.Win32;
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
        internal static TimeSpan DureePause = new TimeSpan(0, 0, 10);
        internal static TimeSpan DureePreavisFin = new TimeSpan(0, 0, 50);

        private static Session instance;     // Singleton
        private EtatSession etat;
        private TimeSpan duree;

        private readonly Timer timer = new Timer { Interval = 1000, Enabled = true };
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
                if (ChangeEtat != null) ChangeEtat(this, new EventArgs());
            }
        }
        #endregion

        #region Constructeurs
        // Constructeur privé, pas d'instanciation manuelle du Singleton 
        private Session(int minutes)
        {
            duree = new TimeSpan(0, minutes, 0);
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
        /// <param name="minutes">Durée de la session en minutes</param>
        public static Session Instance(int minutes = 60)
        {
            if (instance == null) instance = new Session(minutes);
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
            if (TicTac != null) TicTac(this, new EventArgs());
            if (e.SignalTime.Second == 0)
            {
                if (TicTacMinute != null) TicTacMinute(this, new EventArgs());
            }
        }
        #endregion
    }
}
