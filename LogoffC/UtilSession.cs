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
        /// <summary>
        /// Définit la durée en fonction de la dernière session.
        /// </summary>
        /// <returns>Nombre de minutes</returns>
        internal static int MinutesDisponibles()
        {
            DateTime jourAvant;
            int dureeMaxi = (int)rk.GetValue("Duree", 60);
            int restePrecedent = (int)rk.GetValue("Reste", dureeMaxi);
            string chaineDate = (string)rk.GetValue("cejour");

            DateTime.TryParse(chaineDate, out jourAvant);

            switch ((DateTime.Today - jourAvant).TotalDays)
            {
                case 0:
                    // Connecté aujourd'hui : 
                    // retourne la durée restante ou 5 minutes au minimum
                    return (restePrecedent < 5) ? 5 : restePrecedent;
                case 1:
                    // Connecté hier : 
                    // retourne la durée restante avec un bonus
                    restePrecedent += dureeMaxi / 2;
                    return (restePrecedent < dureeMaxi) ? restePrecedent : dureeMaxi;
                default:
                    // retourne la durée entière
                    return dureeMaxi;
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