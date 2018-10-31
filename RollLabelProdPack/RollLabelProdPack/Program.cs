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
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //valid input values are pack, production or manual
            if (args.Length > 0 && args[0] != null && args[0] == "pack")
            {
                Application.Run(new FrmPackPrint());
            }
            else if (args.Length > 0 && args[0] != null && args[0] == "mix")
            {
                Application.Run(new FrmMix());
            }
            else if (args.Length > 0 && args[0] != null && args[0] == "manual")
            {
                Application.Run(new FrmCreateRollLabels());
            }
            else
            {
                Application.Run(new FrmMain());
            }
           
        }
    }
}
