﻿using Durak.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DurakGame
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
#if !DEBUG
            try
            {
#endif
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                frmSplashScreen splash = new frmSplashScreen();
                splash.ShowDialog();

                Application.Run(new frmDurakMain());

#if !DEBUG
            } catch (Exception e)
            {
                Logger.Write("A FATAL EXCEPTION HAS OCCURED");
                Logger.Write(e);

                MessageBox.Show("A fatal error has occured:\n" + e.Message + "\nProgram will now close", "Fatal Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
#endif
        }                
    }
}
