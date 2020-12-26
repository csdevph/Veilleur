using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LogoffC
{
    /// <summary>
    /// sauvegrade
    /// </summary>
    /// 
    internal class UtilSession
    {
        RegistryKey rk = Registry.CurrentUser.CreateSubKey("Recreation", true);
        private TimeSpan duree;
        public DateTime HeureLimite;

        private void Session(int minutes = 60)
        {
            duree = new TimeSpan(0, minutes, 0);

            DateTime CeJour;
            int intDuree = (int)rk.GetValue("Duree", 60);
            int intReste = (int)rk.GetValue("Reste", intDuree);
            string strCeJour = (string)rk.GetValue("cejour");

            DateTime.TryParse(strCeJour, out CeJour);

            switch ((DateTime.Today - CeJour).TotalDays)
            {
                case 0: //Aujourd'hui
                    if (intReste < 2) intReste = 2;
                    break;
                case 1: //Hier
                    intReste += intDuree / 2;
                    intReste = Math.Min(intReste, intDuree);
                    break;
                default:
                    intReste = intDuree;
                    break;
            }

            duree = new TimeSpan(0, intReste, 0);
            HeureLimite = DateTime.Now + duree;

            SauveDate();
            SauveDuree();
        }
        void SauveDate()
        {
            rk.SetValue("CeJour", DateTime.Today.ToShortDateString(), RegistryValueKind.String);
        }

        void SauveDuree()
        {
            rk.SetValue("Reste", NbMinutes(), RegistryValueKind.DWord);
        }
        public int NbMinutes()
        {
            return (int)duree.TotalMinutes;

        }

    }
}