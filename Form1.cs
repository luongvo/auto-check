using Ini;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Net.Mail;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Example
{
    public partial class Form1 : Form
    {
        //=======================================//
        private int hwndCheckoutButton = 0x00030A10;
        private int checkOutTimeHour = 19;
        private int checkOutTimeMinute = 29;
        private int checkOutTimeSecond = 0;
        //=======================================//
        private bool sent = false;

        private const int WM_CLOSE = 16;
        private const int BN_CLICKED = 245;

        [DllImport("User32.dll")]
        public static extern Int32 FindWindow(String lpClassName, String lpWindowName);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int SendMessage(int hWnd, int msg, int wParam, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindowEx(IntPtr parentHandle, IntPtr childAfter, string className, string windowTitle);

        public Form1()
        {
            InitializeComponent();
            this.Hide();
            this.Visible = false;

            IniFile ini = new IniFile(Application.StartupPath + "\\test.ini");
            hwndCheckoutButton = Convert.ToInt32(ini.IniReadValue("test", "hwndCheckoutButton"), 16);
            checkOutTimeHour = Convert.ToInt32(ini.IniReadValue("test", "checkOutTimeHour"));
            checkOutTimeMinute = Convert.ToInt32(ini.IniReadValue("test", "checkOutTimeMinute"));
            checkOutTimeSecond = Convert.ToInt32(ini.IniReadValue("test", "checkOutTimeSecond"));

            Debug.WriteLine(hwndCheckoutButton);
            Debug.WriteLine(checkOutTimeHour);
            Debug.WriteLine(checkOutTimeMinute);
            Debug.WriteLine(checkOutTimeSecond);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            timerCheckTime.Enabled = true;
        }

        private void doCheckOut()
        {
            SendMessage((int)hwndCheckoutButton, BN_CLICKED, 0, IntPtr.Zero);

            timer1.Enabled = true;
        }

        private static void ClickButtonLabeled_YES()
        {
            int hwnd = 0;
            IntPtr hwndChild = IntPtr.Zero;
            hwnd = FindWindow(null, "Confirm");
            if (hwnd != 0)
            {
                hwndChild = FindWindowEx((IntPtr)hwnd, IntPtr.Zero, "Button", "Yes");
                SendMessage((int)hwndChild, BN_CLICKED, 0, IntPtr.Zero);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            ClickButtonLabeled_YES();

            if (!sent)
            {
                sent = true;
                send();
            }

            timer2.Enabled = true;
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            Process.Start("shutdown", "/s /t 0");

            //MessageBox.Show("Shuting down PC...");
        }

        private void timerCheckTime_Tick(object sender, EventArgs e)
        {
            DateTime dateTime = DateTime.Now;
            if (dateTime.Hour == checkOutTimeHour && dateTime.Minute == checkOutTimeMinute && dateTime.Second == checkOutTimeSecond)
            {
                doCheckOut();
            }
        }

        private void send()
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                mail.From = new MailAddress("vodailuong@gmail.com");
                mail.To.Add("vodailuong@gmail.com");
                mail.Subject = "Check";
                mail.Body = "Done";

                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential("vodailuong@gmail.com", "nadvcoyqhwktchpt");
                SmtpServer.EnableSsl = true;

                SmtpServer.Send(mail);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
