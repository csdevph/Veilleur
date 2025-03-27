using System;

namespace Veilleur
{
    /// <summary>
    /// Template pour les différents états d'une session
    /// </summary>
    abstract class EtatSession
    {
        protected Session session; // Contexte

        internal TimeSpan Duree { get; set; }

        internal EtatSession(Session s, TimeSpan d)
        {
            session = s;
            Duree = d;
        }

        internal abstract EtatSession EtatSuivant();
    }
}
