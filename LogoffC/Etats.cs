using System;

namespace LogoffC
{
    internal class EtatZero : EtatSession
    {
        public EtatZero(Session s) : base(s, TimeSpan.Zero) { }
        internal override EtatSession EtatSuivant() => new EnPause(sess);
    }

    internal class EtatFinal : EtatSession
    {
        public EtatFinal(Session s) : base(s, TimeSpan.Zero) { }
        internal override EtatSession EtatSuivant() => this;
    }

    internal class EnPause : EtatSession
    {
        public EnPause(Session s) : base(s, new TimeSpan(0, Session.pauseMaxi, 0)) { }
        internal override EtatSession EtatSuivant() => new EnCours(sess);
    }

    internal class EnCours : EtatSession
    {
        public EnCours(Session s) : base(s, s.Duree) { }
        internal override EtatSession EtatSuivant() => new EnPreavis(sess);
    }

    internal class EnPreavis : EtatSession
    {
        public EnPreavis(Session s) : base(s, new TimeSpan(0, Session.preavisFin, 0)) { }
        internal override EtatSession EtatSuivant() => new EnSursis(sess);
    }

    internal class EnSursis : EtatSession
    {
        public EnSursis(Session s) : base(s, new TimeSpan(0, 0, 5)) { }
        internal override EtatSession EtatSuivant() => new EtatFinal(sess);
    }
}