using System;
using System.Timers;

namespace LogoffC
{
    // Classe template ==========================================================================
    abstract class EtatSession
    {
        private TimeSpan dureeEtat;

        protected Session sess; // Contexte

        internal TimeSpan DureeEtat { get => dureeEtat; set => dureeEtat = value; }

        internal EtatSession(Session s, int minutes)
        {
            sess = s;
            DureeEtat = new TimeSpan(0, 0, minutes);
            sess.TicTac += TicTacSession;
        }

        internal abstract EtatSession EtatSuivant();

        private void TicTacSession(object sender, EventArgs e)
        {
            DureeEtat -= System.TimeSpan.FromSeconds(1);
            if (DureeEtat < TimeSpan.Zero)
            {
                sess.TicTac -= TicTacSession;     // Arrêt de la minuterie
                sess.Etat = EtatSuivant();
            }
        }
    }
    // Fin classe template EtatSession =======================================================================

    internal class enPause : EtatSession
    {
        public enPause(Session s, int m) : base(s, m) { }

        internal override EtatSession EtatSuivant() => new enCours(sess, 10);
    }

    internal class enCours : EtatSession
    {
        public enCours(Session s, int m) : base(s, m) { }

        internal override EtatSession EtatSuivant() => new enPreavis(sess, 8);
    }

    internal class enPreavis : EtatSession
    {
        public enPreavis(Session s, int m) : base(s, m) { }

        internal override EtatSession EtatSuivant() => new enSursis(sess, 5);
    }

    internal class enSursis : EtatSession
    {
        public enSursis(Session s, int m) : base(s, m) { }

        internal override EtatSession EtatSuivant() => this;
    }
}