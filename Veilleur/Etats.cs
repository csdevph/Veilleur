using System;

namespace Veilleur
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

    /// <summary>
    /// L'utilisateur peut suspendre sa session.
    /// La durée de la pause est limitée.
    /// </summary>
    internal class EnPause : EtatSession
    {
        public EnPause(Session s) : base(s, Session.DureePause) { }
        internal override EtatSession EtatSuivant() => new EnCours(sess);
    }

    /// <summary>
    /// Etat principal.
    ///[ Durée = durée de la session - durée préavis ]
    /// </summary>
    internal class EnCours : EtatSession
    {
        public EnCours(Session s) : base(s, s.Duree - Session.DureePreavisFin) { }
        internal override EtatSession EtatSuivant() => new EnPreavisFin(sess);
    }

    /// <summary>
    /// Préavis avant la fin de la session.
    /// </summary>
    internal class EnPreavisFin : EtatSession
    {
        public EnPreavisFin(Session s) : base(s, Session.DureePreavisFin) { }
        internal override EtatSession EtatSuivant() => new EtatFinal(sess);
    }
}
