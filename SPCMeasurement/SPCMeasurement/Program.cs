using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SPCMeasurement
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
            if (args.Count() == 0)
            {
                Application.Run(new frmLogin(""));
            } else
            {
                string param = args[0];
                string[] arrayparam = param.Split(',');

                string userID = arrayparam[1];
                string FactoryCode = arrayparam[2];
                string ProcessGroup = arrayparam[3];
                string LineGroup = arrayparam[4];
                string ProcessCode = arrayparam[5];
                string LineCode = arrayparam[6];
                string ItemType = arrayparam[7];
                string ItemCheck = arrayparam[8];
                string ShiftCode = arrayparam[9];
                string SeqNo = arrayparam[10];
                string ProdDate = arrayparam[11];
                ProdDate = ProdDate.Replace("%20", "");
                Application.Run(new frmMeasurement(userID, null, FactoryCode, ProcessGroup, LineGroup, ProcessCode, LineCode, ItemType, ItemCheck, ShiftCode, SeqNo, ProdDate));
            }
        }
    }
}
