using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RollLabelProdPack
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// <param name="args">Command-line arguments passed to the application.</param>
        [STAThread]
        static void Main(string[] args)
        {
            // Enable visual styles for the application
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Check the command-line arguments to determine the application behavior
            if (args.Length > 0 && args[0] != null)
            {
                // Execute specific functionality based on the provided argument

                //if (args[0] == "pack")
                //{
                //    // Run the "FrmPackPrint" form for "pack" argument
                //    Application.Run(new FrmPackPrint());
                //}
                //else if (args[0] == "tubpack")
                //{
                //    // Run the "FrmTubPackPrint" form for "tubpack" argument
                //    Application.Run(new FrmTubPackPrint());
                //}
                //else if (args[0] == "mix")
                //{
                //    // Run the "FrmMix" form for "mix" argument
                //    Application.Run(new FrmMix());
                //}
                //else if (args[0] == "mask")
                //{
                //    // Run the "FrmMask" form for "mask" argument
                //    Application.Run(new FrmMask());
                //}
                //else if (args[0] == "tub")
                //{
                //    // Run the "frmTub2" form for "tub" argument
                //    Application.Run(new frmTub2());
                //}
                //else if (args[0] == "copack")
                //{
                //    // Run the "frmCoPack" form for "copack" argument
                //    Application.Run(new frmCoPack());
                //}
                //else if (args[0] == "copackprint")
                //{
                //    // Run the "frmCoPackPrint" form for "copackprint" argument
                //    Application.Run(new FrmCoPackPrint());
                //}
            }
            else
            {
                // Run the default "FrmMain" form when no argument is provided
                //Application.Run(new FrmMain());
                Application.Run(new NonPGStartForm());
            }
        }
    }
}
