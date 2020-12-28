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
        private TimeSpan dureeEtat;

        protected Session sess; // Contexte

        internal TimeSpan DureeEtat { get => dureeEtat; set => dureeEtat = value; }

        internal EtatSession(Session s, TimeSpan d)
        {
            sess = s;
            dureeEtat = d;
        }

        internal abstract EtatSession EtatSuivant();
    }
}
