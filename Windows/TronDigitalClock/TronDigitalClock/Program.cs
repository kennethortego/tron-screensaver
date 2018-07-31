﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TronDigitalClock
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);            

            if (args.Length > 0)
            {
                string firstArgument = args[0].ToLower().Trim();
                string secondArgument = null;



                // Handle cases where arguments are separated by colon.
                // Examples: /c:1234567 or /P:1234567
                if (firstArgument == "/arcade")
                {
                    TronDigitalClockForm.isarcde = true;

                    string startprogram = Convert.ToString(ConfigurationManager.AppSettings["StartProgram"]);

                    if (!string.IsNullOrWhiteSpace(startprogram))
                    {
                        ProcessStartInfo psi = new ProcessStartInfo(startprogram);
                        psi.CreateNoWindow = true;
                        psi.UseShellExecute = false;

                        Process p = Process.Start(psi);
                        p.WaitForExit();
                        p.Close();
                    }

                    ShowScreenSaver();
                    Application.Run();
                    return;
                }
                else if (firstArgument.Length > 2)
                {
                    secondArgument = firstArgument.Substring(3).Trim();
                    firstArgument = firstArgument.Substring(0, 2);
                }
                else if (args.Length > 1)
                    secondArgument = args[1];

                if (firstArgument == "/c")           // Configuration mode
                {
                    // TODO
                }
                else if (firstArgument == "/p")      // Preview mode
                {
                    if (secondArgument == null)
                    {
                        MessageBox.Show("Sorry, but the expected window handle was not provided.",
                            "ScreenSaver", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }

                    IntPtr previewWndHandle = new IntPtr(long.Parse(secondArgument));
                    Application.Run(new TronDigitalClockForm(previewWndHandle));

                }
                else if (firstArgument == "/s")      // Full-screen mode
                {
                    ShowScreenSaver();
                    Application.Run();
                }
                else if (firstArgument == "/d")           // Debug mode, run from studio         
                {
                    bool micro = secondArgument == "micro" ? true : false;

                    ShowScreenSaver(micro);
                    Application.Run();

                }
                else    // Undefined argument
                {
                    MessageBox.Show("Sorry, but the command line argument \"" + firstArgument +
                        "\" is not valid.", "Tron Clock",
                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            else    // No arguments - treat like /c
            {
                // TODO
            }
        }

        static void ShowScreenSaver(bool micro = false)
        {
            foreach (Screen screen in Screen.AllScreens)
            {
                Rectangle bounds = micro ? new Rectangle(300, 300, 800, 450) : screen.Bounds;
                TronDigitalClockForm screensaver = new TronDigitalClockForm(bounds);
                screensaver.Show();
            }
        }

    }
}
