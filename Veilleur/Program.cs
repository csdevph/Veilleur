using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace Veilleur
{
    static class Program
    {
        /// <summary>
        /// Point d'entrée principal de l'application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            VerifInstance();
            Console.WriteLine("\n{0}_DEBUT =========================================================", DateTime.Now.ToLongTimeString());
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
            Console.WriteLine("FIN =========================================================");
        }

        static void VerifInstance()
        {
            Process currProcess = Process.GetCurrentProcess();
            Process[] processes = Process.GetProcessesByName(currProcess.ProcessName);
            if (processes.Length > 1)
            {
                Console.WriteLine("Une autre instance en cours d'exécution.");
                Environment.Exit(0);
            }
        }
    }
}
