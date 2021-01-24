using System;
using System.CodeDom;
using System.Drawing;
using System.Windows.Forms;

namespace LogoffC
{
    /// <summary>Occultation de l'écran</summary>
    public partial class Form1 : Form
    {
        readonly int EcranLarg = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Width;
        readonly int EcranHaut = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Height;
        readonly Session Sess = Session.Instance();

        public Form1()
        {
            InitializeComponent();
            Sess.TicTac += Minuteur;
            Sess.ChangeEtat += ChangeEtat;
        }

        private void ChangeEtat(object sender, EventArgs e)
        {
            Console.WriteLine($"{DateTime.Now:HH:mm:ss} > {Sess.Etat.GetType().Name} ================");

            this.Invoke(new MethodInvoker(
                () =>
                {
                    switch (Sess.Etat)
                    {
                        case EnCours _:
                            Ecran_EnCours();
                            break;
                        case EnPause _:
                            Ecran_EnPause();
                            break;
                        case EnPreavisFin _:
                            Ecran_EnPreavisFin();
                            break;
                        case EtatFinal _:
                            Ecran_EtatFinal();
                            break;
                        default:
                            break;
                    }
                }
            ));
        }

        private void Minuteur(object sender, EventArgs e)
        {
            Console.WriteLine($"TicTac   > {Sess.Etat.GetType().Name} : {Sess.Etat.Duree}");
            if (Sess.Etat is EnPause) return;

            label1.Invoke(new MethodInvoker
                (() => { label1.Text = InfoDuree(); })
            );
            if (notifyIcon1 != null) notifyIcon1.Text = InfoDuree();
        }

        private void BougeEcran(object sender, EventArgs e)
        {
            this.Invoke(new MethodInvoker(
                () =>
                {
                    this.Left += 10;
                    if (this.Right > EcranLarg) this.Left = 5;
                }
            ));
        }

        private void Ecran_EnCours()
        {
            //this.Opacity = .7;
            this.Hide();
        }

        private void Ecran_EnPause()
        {
            //this.Opacity = 1;
            this.Show();
        }

        private void Ecran_EnPreavisFin()
        {
            //this.Opacity = 1;
            button1.Enabled = false;
            this.SetDesktopLocation(10, 0);
            this.BackColor = Color.Orange;
            this.Height = 50;
            this.Width = 900;
            this.Show();

            Sess.TicTac += BougeEcran;

            notifyIcon1.Icon = null;
            notifyIcon1 = null;
            //GC.Collect();
        }

        private void Ecran_EtatFinal()
        {
            Sess.TicTac -= BougeEcran;

            this.BackColor = Color.Orange;
            this.SetDesktopLocation(50, 1);
            this.Height = EcranHaut - 30 - 200;
            this.Width = (int)(EcranLarg * 0.95);
            this.Show();
            //this.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Height = (int)(EcranHaut * 0.95);
            this.Height = (int)(EcranHaut * 0.35);
            this.Width = (int)(EcranLarg * 0.95);

            if (this.Bottom > EcranHaut) this.Top = 5;
            if (this.Right > EcranLarg) this.Left = 5;

            label1.Text = InfoDuree();
            notifyIcon1.Text = InfoDuree();

            UtilSession.SauveDate();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            WinExit.ExitWindowsEx(WinExit.EWX_LOGOFF | WinExit.EWX_FORCE, 0);
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            Sess.Etat = new EnCours(Sess);
        }

        private void NotifyIcon1_Click(object sender, EventArgs e)
        {
            if (!(Sess.Etat is EnCours)) return;

            if (DateTime.Now > Sess.HeureLimite)
            {
                this.notifyIcon1.Click -= NotifyIcon1_Click;
                MessageBox.Show("Il est temps d'arrêter l'ordinateur...", "Session trop longue",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                Sess.Etat = new EnPause(Sess);
            }
        }

        private String InfoDuree()
        {
            return "Durée de la session = " + Sess.Duree;
        }
    }
}
