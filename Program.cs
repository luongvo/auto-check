using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Example
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Form1 form1 = new Form1();
            form1.Visible = false;
            Application.Run(form1);
            form1.Hide();
        }
    }
}
