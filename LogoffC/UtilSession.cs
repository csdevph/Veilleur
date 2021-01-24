using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LogoffC
{
    /// <summary>
    /// Support Windows
    /// </summary>
    internal static class UtilSession
    {
        readonly static RegistryKey rk = Registry.CurrentUser.CreateSubKey("Recreation", true);

        internal static int DureeMaxi { get => (int)rk.GetValue("Duree", 60); }
        internal static int DureePause { get => (int)rk.GetValue("Pause", 5); }
        internal static int DureePreavis { get => (int)rk.GetValue("Preavis", 10); }

        /// <summary>
        /// Définit la durée en fonction de la dernière session.
        /// </summary>
        /// <returns>Nombre de minutes</returns>
        internal static int DureeSession
        {
            get
            {
                DateTime jourAvant;
                int restePrecedent = (int)rk.GetValue("Reste", DureeMaxi);
                string chaineDate = (string)rk.GetValue("cejour");

                DateTime.TryParse(chaineDate, out jourAvant);

                switch ((DateTime.Today - jourAvant).TotalDays)
                {
                    case 0:
                        // Connecté aujourd'hui : 
                        // retourne la durée restante (ou du préavis au minimum)
                        return (restePrecedent < DureePreavis) ? DureePreavis : restePrecedent;
                    case 1:
                        // Connecté hier : 
                        // retourne la durée restante avec un bonus, sans dépasser la durée maxi
                        restePrecedent += DureeMaxi / 2;
                        return (restePrecedent < DureeMaxi) ? restePrecedent : DureeMaxi;
                    default:
                        // retourne la durée entière
                        return DureeMaxi;
                }
            }
        }

        internal static void SauveDate()
        {
            rk.SetValue("CeJour", DateTime.Today.ToShortDateString(), RegistryValueKind.String);
        }

        internal static void SauveDuree(int minutes)
        {
            rk.SetValue("Reste", minutes, RegistryValueKind.DWord);
        }
    }
}