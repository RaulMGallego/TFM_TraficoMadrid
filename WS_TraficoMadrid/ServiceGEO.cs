using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using WS_TraficoMadrid.WS_TraficoMadrid_ServiceBUS;
using WS_TraficoMadrid.WS_TraficoMadrid_ServiceGEO;

namespace WS_TraficoMadrid
{
    public class ServiceGEO
    {
        #region GEO_getArriveStop
        public static void GEO_getArriveStop_WS(string strDirectoryPathIn, string strDirectoryPathOut, string strUsuario, string strPassword, string stridStop)
        {
            string strNameFunction = "getArriveStop";
            //Creamos un nuevo servicio WS
            ServiceGEOSoapClient WS_ServiceGEO = new WS_TraficoMadrid_ServiceGEO.ServiceGEOSoapClient();

            //Llamamos a la funcion GetListLines que "Recupera la relación general de líneas con su descripción y subgrupo al que pertenencen."
            XmlNode xN = WS_ServiceGEO.getArriveStop(strUsuario, strPassword, stridStop, "", "");

            //Montamos el día de Hoy
            DateTime GetFechaHoy = DateTime.Now;
            string strGetFechaHoy = GetFechaHoy.ToShortDateString();
            string strGetFechaHoyFormat = strGetFechaHoy.Substring(6, 4) + strGetFechaHoy.Substring(3, 2) + strGetFechaHoy.Substring(0, 2) + "_";

            //Construimos el nombre de fichero de entrada y de salida
            string strFilePathIn = strDirectoryPathIn + strGetFechaHoyFormat + strNameFunction + ".xml";
            string strFilePathOut = strDirectoryPathOut + strGetFechaHoyFormat + strNameFunction + ".csv";

            //Guardamos los valores del WS en un fichero XML
            using (XmlWriter writer = XmlWriter.Create(strFilePathIn))
            {
                xN.WriteTo(writer);
            }

            //Llamamos a la funcion que nos convierte el XML en CSV
            GEO_getArriveStop_XMLToCSV(strFilePathIn, strFilePathOut);

            //Borramos el fichero temporal, puesto que ya no sirve para nada
            File.Delete(strFilePathIn);
        }

        public static void GEO_getArriveStop_XMLToCSV(string FilePathIn_CSVToXML, string FilePathOut_CSVToXML)
        {
            //This method converts an xml file into a .csv file

            XDocument xDocument = XDocument.Load(FilePathIn_CSVToXML);
            StringBuilder dataToBeWritten = new StringBuilder();

            var results = xDocument.Descendants("Arrive").Select(x => new
            {
                IdStop = (string)x.Element("IdStop"),
                idLine = (string)x.Element("idLine"),
                IsHead = (string)x.Element("IsHead"),
                Destination = (string)x.Element("Destination"),
                IdBus = (string)x.Element("IdBus"),
                TimeLeftBus = (string)x.Element("TimeLeftBus"),
                DistanceBus = (string)x.Element("DistanceBus"),
                PositionXBus = (string)x.Element("PositionXBus"),
                PositionYBus = (string)x.Element("PositionYBus"),
                PositionTypeBus = (string)x.Element("PositionTypeBus")
            }).ToList();

            for (int i = 0; i < results.Count; i++)
            {
                string tempIdStop = results[i].IdStop.Trim();
                string tempidLine = results[i].idLine.Trim();
                string tempIsHead = results[i].IsHead.Trim();
                string tempDestination = results[i].Destination.Trim();
                string tempIdBus = results[i].IdBus.Trim();
                string tempTimeLeftBus = results[i].TimeLeftBus.Trim();
                string tempDistanceBus = results[i].DistanceBus.Trim();
                string tempPositionXBus = results[i].PositionXBus.Trim();
                string tempPositionYBus = results[i].PositionYBus.Trim();
                string tempPositionTypeBus = results[i].PositionTypeBus.Trim();

                dataToBeWritten.Append(tempIdStop);
                dataToBeWritten.Append("|");
                dataToBeWritten.Append(tempidLine);
                dataToBeWritten.Append("|");
                dataToBeWritten.Append(tempIsHead);
                dataToBeWritten.Append("|");
                dataToBeWritten.Append(tempDestination);
                dataToBeWritten.Append("|");
                dataToBeWritten.Append(tempIdBus);
                dataToBeWritten.Append("|");
                dataToBeWritten.Append(tempTimeLeftBus);
                dataToBeWritten.Append("|");
                dataToBeWritten.Append(tempDistanceBus);
                dataToBeWritten.Append("|");
                dataToBeWritten.Append(tempPositionXBus);
                dataToBeWritten.Append("|");
                dataToBeWritten.Append(tempPositionYBus);
                dataToBeWritten.Append("|");
                dataToBeWritten.Append(tempPositionTypeBus);
                dataToBeWritten.Append("|");
                dataToBeWritten.Append(DateTime.Now);
                dataToBeWritten.Append(Environment.NewLine);
            }

            // Guardamos y añadimos el Fichero CSV en su sitio correspondiente
            File.AppendAllText(FilePathOut_CSVToXML, dataToBeWritten.ToString());
        }
        #endregion GEO_getArriveStop
    }
}
