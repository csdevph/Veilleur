using System;
using System.Drawing;
using System.Windows.Forms;

namespace Veilleur
{
    /// <summary>Occultation de l'écran</summary>
    public partial class Form1 : Form
    {
        readonly int EcranLarg = (int)(Screen.PrimaryScreen.WorkingArea.Width * .95);
        readonly int EcranHaut = (int)(Screen.PrimaryScreen.WorkingArea.Height * .95);
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
                    if (DateTime.Now.Second % 10 != 0) return;
                    this.Left += 50;
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
            this.Focus();
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

            //notifyIcon1 = null;
            notifyIcon1.Dispose();
        }

        private void Ecran_EtatFinal()
        {
            Sess.TicTac -= BougeEcran;
            Sess.TicTac -= Minuteur;
            label1.Text = "Fin de la session. Il faut se déconnecter maintenant !";
            this.BackColor = Color.Orange;
            this.SetDesktopLocation(50, 1);
            this.Height = EcranHaut;
            this.Width = EcranLarg;
            this.CenterToScreen();
            //this.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Height = EcranHaut;
            this.Width = EcranLarg;
            this.CenterToScreen();

            button1.Left = this.Width / 2 - button1.Width / 2;

            label1.Width = EcranLarg - 100;
            label1.Left = this.Width / 2 - label1.Width / 2;
            label1.Text = InfoDuree();
            notifyIcon1.Text = InfoDuree();

            UtilSession.SauveDate();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.TaskManagerClosing)
            {
                DialogResult res = MessageBox.Show("End task", "Alert", MessageBoxButtons.OK);
            }
            e.Cancel = true;
            return;
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
