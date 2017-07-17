using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WS_TraficoMadrid
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

            string strDirectoryPathInAEMET = @"http://www.aemet.es/xml/municipios/localidad_28079.xml";
            string strDirectoryPathIn = @"C:\UNIVERSIDAD\00_TFM\TFM_TraficoMadrid\TFM_WSTraficoMadrid\TFM_WSTraficoMadrid\bin\Debug" + @"\";
            string strDirectoryPathOut = @"C:\UNIVERSIDAD\00_TFM\TFM_TraficoMadrid\CSV\";
            //string strUsuario = "Introduce tu usuario";
            //string strPassword = "Introduce tu password";
            string strFechaWS = DateTime.Now.ToShortDateString();
            string[] stridBus = { "51", "115", "28", "148", "59", "39", "162" };
            int iContidBus = stridBus.Length;

             // ------ SERVICE BUS ------------------
            // Recupera la relación general de líneas con su descripción y subgrupo al que pertenencen. 

            ServiceBUS.BUS_GetListLines_WS(strDirectoryPathIn, strDirectoryPathOut, strUsuario, strPassword, strFechaWS);
            // Recupera todos los identificadores de parada, junto con su coordenada UTM, nombre y la relación de líneas/sentido que pasan por cada uno de ellos.
            ServiceBUS.BUS_GetNodesLines_WS(strDirectoryPathIn, strDirectoryPathOut, strUsuario, strPassword);

            //--------------------------------    PARA TODAS LAS LINEAS  --------------------------------
            ////Obtenemos todas las lineas del BUS
            //List<ServiceBUS> lstLines = ServiceBUS.BUS_lstLines(strDirectoryPathIn, strDirectoryPathOut, strUsuario, strPassword, strFechaWS);

            ////Recorremos todas las lineas, una a una
            //foreach (ServiceBUS itemLines in lstLines)
            //{
            //    //Se obtiene el itinerario de una línea (o varias líneas separadas por el carácter pipe(|)), con los vértices para construir las rectas del recorrido y las coordenadas UTM de los ejes viales y los códigos de parada.
            //    ServiceBUS.BUS_GetRouteLines_WS(strDirectoryPathIn, strDirectoryPathOut, strUsuario, strPassword, strFechaWS, itemLines.stridServiceBus);
            //    // Obtiene los datos de estimación de llegadas del autobús a una parada determinada
            //    List<ServiceBUS> lstNodeRouteLines = ServiceBUS.BUS_lstNodeRouteLines(strDirectoryPathIn, strDirectoryPathOut, strUsuario, strPassword, strFechaWS, itemLines.stridServiceBus);

            //    //Recorremos todos las paradas, una a una
            //    foreach (ServiceBUS itemNodeRouteLines in lstNodeRouteLines)
            //    {
            //        ServiceGEO.GEO_getArriveStop_WS(strDirectoryPathIn, strDirectoryPathOut, strUsuario, strPassword, itemNodeRouteLines.stridServiceBus);
            //    }
            //}
            //---------------------------------------------------------------------------------------------

            //Recorremos el array buscando todas las lineas que debemos seguir
            for (int i = 0; i < iContidBus; i++)
            {
                //Se obtiene el itinerario de una línea (o varias líneas separadas por el carácter pipe(|)), con los vértices para construir las rectas del recorrido y las coordenadas UTM de los ejes viales y los códigos de parada.
                ServiceBUS.BUS_GetRouteLines_WS(strDirectoryPathIn, strDirectoryPathOut, strUsuario, strPassword, strFechaWS, stridBus[i]);
                // Obtiene los datos de estimación de llegadas del autobús a una parada determinada
                List<ServiceBUS> lstNodeRouteLines = ServiceBUS.BUS_lstNodeRouteLines(strDirectoryPathIn, strDirectoryPathOut, strUsuario, strPassword, strFechaWS, stridBus[i]);

                foreach (ServiceBUS itemNodeRouteLines in lstNodeRouteLines)
                {
                    ServiceGEO.GEO_getArriveStop_WS(strDirectoryPathIn, strDirectoryPathOut, strUsuario, strPassword, itemNodeRouteLines.stridServiceBus);
                }
            }

            // Obtiene la previsión de precipitaciones para hoy, mañana y pasado mañana.
            ServiceAEMET.AEMET_getPrevisionTiempo(strDirectoryPathInAEMET, strDirectoryPathOut);
        }
    }
}
