using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Veilleur
{
    /// <summary>
    /// Template pour les différents états d'une session
    /// </summary>
    abstract class EtatSession
    {
        protected Session sess; // Contexte

        internal TimeSpan Duree { get; set; }

        internal EtatSession(Session s, TimeSpan d)
        {
            sess = s;
            Duree = d;
        }

        internal abstract EtatSession EtatSuivant();
    }
}
