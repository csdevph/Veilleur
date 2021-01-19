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

        internal static int MinutesDisponibles()
        {
            DateTime jourAvant;
            int dureeSession = (int)rk.GetValue("Duree", 60);
            int restePrecedent = (int)rk.GetValue("Reste", dureeSession);
            string chaineDate = (string)rk.GetValue("cejour");

            DateTime.TryParse(chaineDate, out jourAvant);

            switch ((DateTime.Today - jourAvant).TotalDays)
            {
                case 0: // Aujourd'hui
                    return (restePrecedent < 2) ? 2 : restePrecedent;
                case 1: // Hier
                    restePrecedent += dureeSession / 2;
                    return Math.Min(restePrecedent, dureeSession);
                default:
                    return dureeSession;
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