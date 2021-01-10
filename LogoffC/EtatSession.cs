using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogoffC
{
    // Classe template
    abstract class EtatSession
    {
        protected Session sess; // Contexte

        internal TimeSpan DureeEtat { get; set; }

        internal EtatSession(Session s, TimeSpan d)
        {
            sess = s;
            DureeEtat = d;
        }

        internal abstract EtatSession EtatSuivant();
    }
}
