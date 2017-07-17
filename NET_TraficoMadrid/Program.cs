using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NET_TraficoMadrid
{
    static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1());
            string strDirectoryPathIn = @"C:\UNIVERSIDAD\00_TFM\TFM_TraficoMadrid\TFM_WSTraficoMadrid\TFM_WSTraficoMadrid\bin\Debug" + @"\";
            string strDirectoryPathOut = @"C:\UNIVERSIDAD\00_TFM\TFM_TraficoMadrid\CSV\";
            Utilidades.TFM_GetArriveStopEnd(strDirectoryPathOut);

           
        }
    }
}
