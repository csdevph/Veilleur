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
        public EnPause(Session s) : base(s, Session.DureePause) { }
        internal override EtatSession EtatSuivant() => new EnCours(sess);
    }

    internal class EnCours : EtatSession
    {
        public EnCours(Session s) : base(s, s.Duree) { }
        internal override EtatSession EtatSuivant() => new EnPreavisFin(sess);
    }

    internal class EnPreavisFin : EtatSession
    {
        public EnPreavisFin(Session s) : base(s, Session.DureePreavisFin) { }
        internal override EtatSession EtatSuivant() => new EtatFinal(sess);
    }
}
