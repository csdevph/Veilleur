using System;

namespace LogoffC
{
    internal class EtatZero : EtatSession
    {
        public EtatZero(Session s) : base(s, TimeSpan.Zero) { }
        internal override EtatSession EtatSuivant() => new enPause(sess);
    }

    internal class EtatFinal : EtatSession
    {
        public EtatFinal(Session s) : base(s, TimeSpan.Zero) { }
        internal override EtatSession EtatSuivant() => this;
    }

    internal class enPause : EtatSession
    {
        public enPause(Session s) : base(s, new TimeSpan(0, 0, 7)) { }
        internal override EtatSession EtatSuivant() => new enCours(sess, sess.Duree);
    }

    internal class enCours : EtatSession
    {
        public enCours(Session s,TimeSpan d) : base(s, d) { }
        internal override EtatSession EtatSuivant() => new enPreavis(sess);
    }

    internal class enPreavis : EtatSession
    {
        public enPreavis(Session s) : base(s, new TimeSpan(0, 0, 8)) { }
        internal override EtatSession EtatSuivant() => new enSursis(sess);
    }

    internal class enSursis : EtatSession
    {
        public enSursis(Session s) : base(s, new TimeSpan(0, 0, 5)) { }
        internal override EtatSession EtatSuivant() => new EtatFinal(sess);
    }
}