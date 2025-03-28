using Microsoft.Win32;
using System;

namespace Veilleur
{
    /// <summary>
    /// Support Windows
    /// </summary>
    internal static class UtilSession
    {
        readonly static RegistryKey rk = Registry.CurrentUser.CreateSubKey("Recreation", true);

        internal static int DureeMaxi { get => (int)rk.GetValue("DureeMaxi", 150); }
        internal static int DureePause { get => (int)rk.GetValue("DureePause", 5); }
        internal static int DureePreavis { get => (int)rk.GetValue("DureePreavis", 10); }

        /// <summary>
        /// Définit la durée en fonction de la dernière session.
        /// </summary>
        /// <returns>Nombre de minutes</returns>
        internal static int DureeSession
        {
            get
            {
                string chaineDate = (string)rk.GetValue("SessionDate");
                DateTime.TryParse(chaineDate, out DateTime jourAvant);

                if ((DateTime.Now - jourAvant).TotalHours < 12)
                {
                    // Connecté aujourd'hui : retourne la durée restante ou du préavis
                    int restePrecedent = (int)rk.GetValue("SessionReste", DureeMaxi);
                    return (restePrecedent < DureePreavis) ? DureePreavis : restePrecedent;
                }
                else
                {
                    return DureeMaxi;
                }
            }
        }

        internal static void SauveDate()
        {
            rk.SetValue("SessionDate", DateTime.Now.ToString(), RegistryValueKind.String);
        }

        internal static void SauveDuree(int minutes)
        {
            rk.SetValue("SessionReste", minutes, RegistryValueKind.DWord);
        }
    }
}