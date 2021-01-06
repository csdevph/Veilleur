﻿using System;
using System.CodeDom;
using System.Drawing;
using System.Windows.Forms;

namespace LogoffC
{
    /// <summary>Occultation de l'écran</summary>
    public partial class Form1 : Form
    {
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        static extern bool ExitWindowsEx(uint uFlags, uint dwReason);
        private const uint EWX_LOGOFF = 0;  //Log off the network
        private const uint EWX_FORCE = 4;   //Force any applications to quit

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
            switch (Sess.Etat)
            {
                case EnCours _:
                    Ecran_EnCours();
                    break;
                case EnPause _:
                    Ecran_EnPause();
                    break;
                case EnPreavis _:
                    Ecran_EnPreavis();
                    break;
                case EnSursis _:
                    Ecran_EnSursis();
                    break;
                default:
                    break;
            }
        }

        private void Minuteur(object sender, EventArgs e)
        {
            Console.WriteLine("TicTac : Etat {0} : {1}", Sess.Etat.GetType(), Sess.Etat.DureeEtat);
            label1.Invoke(new MethodInvoker
                (() => { label1.Text = Sess.Etat.GetType().Name + " Durée de la session = " + Sess.Etat.DureeEtat; })
            );
            if (notifyIcon1 != null) notifyIcon1.Text = Sess.Etat.GetType().Name + Sess.Etat.DureeEtat;
        }

        private void Ecran_EnCours()
        {
            this.Invoke(new MethodInvoker(
                () =>
                {
                    //this.Opacity = .7;
                    this.Hide();
                }
            ));
        }

        private void Ecran_EnPause()
        {
            this.Invoke(new MethodInvoker(
                () =>
                {
                    //this.Opacity = 1;
                    this.Show();
                }
            ));
        }

        private void Ecran_EnPreavis()
        {
            button1.Invoke(new MethodInvoker(
                () =>
                {
                    //this.Opacity = 1;
                    button1.Enabled = false;
                    this.SetDesktopLocation(10, 0);
                    this.BackColor = Color.Orange;
                    this.Height = 50;
                    this.Width = 900;
                    this.Show();
                }
            ));

            notifyIcon1.Icon = null;
            notifyIcon1 = null;
            //GC.Collect();
        }

        private void Ecran_EnSursis()
        {
            this.Invoke(new MethodInvoker(
                () =>
                {
                    this.BackColor = Color.Orange;
                    this.SetDesktopLocation(50, 1);
                    this.Height = EcranHaut - 30 - 200;
                    this.Width = (int)(EcranLarg * 0.95);
                    this.Show();
                }
            ));
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Height = (int)(EcranHaut * 0.95);
            this.Height = (int)(EcranHaut * 0.35);
            this.Width = (int)(EcranLarg * 0.95);

            if (this.Bottom > EcranHaut) this.Top = 5;
            if (this.Right > EcranLarg) this.Left = 5;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //  ExitWindowsEx(EWX_LOGOFF | EWX_FORCE, 0);
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
    }
}
