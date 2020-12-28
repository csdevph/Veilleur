namespace LogoffC
{
    internal class EtatZero : EtatSession
    {
        public EtatZero(Session s) : base(s, 0) { }
        internal override EtatSession EtatSuivant() => new enPause(sess, 7);
    }

    internal class enPause : EtatSession
    {
        public enPause(Session s, int m) : base(s, m) { }
        internal override EtatSession EtatSuivant() => new enCours(sess, 12);
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