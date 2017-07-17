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
    public class ServiceBUS
    {
        #region PROPIEDADES
        private string _stridServiceBus;

        public string stridServiceBus
        {
            get { return _stridServiceBus; }
            set { _stridServiceBus = value; }
        }
        #endregion PROPIEDADES

        public ServiceBUS(string pstridServiceBus)
        {
            _stridServiceBus = pstridServiceBus;
        }

        #region BUS_GetListLines
        public static void BUS_GetListLines_WS(string strDirectoryPathIn, string strDirectoryPathOut, string strUsuario, string strPassword, string strFechaWS)
        {
            string strNameFunction = "GetListLines";

            //Creamos un nuevo servicio WS
            ServiceBusSoapClient WS_ServiceBUS = new WS_TraficoMadrid_ServiceBUS.ServiceBusSoapClient();

            //Llamamos a la funcion GetListLines que "Recupera la relación general de líneas con su descripción y subgrupo al que pertenencen."
            XmlNode xN = WS_ServiceBUS.GetListLines(strUsuario, strPassword, strFechaWS, "");

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
            BUS_GetListLines_XMLToCSV(strFilePathIn, strFilePathOut);

            //Borramos el fichero temporal, puesto que ya no sirve para nada
            File.Delete(strFilePathIn);
        }

        public static void BUS_GetListLines_XMLToCSV(string strFilePathIn_CSVToXML, string strFilePathOut_CSVToXML)
        {
            //This method converts an xml file into a .csv file

            XDocument xDocument = XDocument.Load(strFilePathIn_CSVToXML);
            StringBuilder dataToBeWritten = new StringBuilder();

            var results = xDocument.Descendants("REG").Select(x => new
            {
                GroupNumber = (string)x.Element("GroupNumber"),
                DateFirst = (string)x.Element("DateFirst"),
                DateEnd = (string)x.Element("DateEnd"),
                Line = (string)x.Element("Line"),
                Label = (string)x.Element("Label"),
                NameA = (string)x.Element("NameA"),
                NameB = (string)x.Element("NameB")
            }).ToList();

            for (int i = 0; i < results.Count; i++)
            {
                string tempGroupNumber = results[i].GroupNumber.Trim();
                string tempDateFirst = results[i].DateFirst.Trim();
                string tempDateEnd = results[i].DateEnd.Trim();
                string tempLine = results[i].Line.Trim();
                string tempLabel = results[i].Label.Trim();
                string tempNameA = results[i].NameA.Trim();
                string tempNameB = results[i].NameB.Trim();

                dataToBeWritten.Append(tempGroupNumber);
                dataToBeWritten.Append("|");
                dataToBeWritten.Append(tempDateFirst);
                dataToBeWritten.Append("|");
                dataToBeWritten.Append(tempDateEnd);
                dataToBeWritten.Append("|");
                dataToBeWritten.Append(tempLine);
                dataToBeWritten.Append("|");
                dataToBeWritten.Append(tempLabel);
                dataToBeWritten.Append("|");
                dataToBeWritten.Append(tempNameA);
                dataToBeWritten.Append("|");
                dataToBeWritten.Append(tempNameB);
                dataToBeWritten.Append("|");
                dataToBeWritten.Append(DateTime.Now);
                dataToBeWritten.Append(Environment.NewLine);
            }

            // Guardamos el Fichero CSV en su sitio correspondiente
            File.WriteAllText(strFilePathOut_CSVToXML, dataToBeWritten.ToString());
        }

        /// <summary>
        /// OBTENEMOS EL LISTADO DE TODAS LAS PARADAS DE UNA LINEA DETERMINADA
        /// </summary>
        /// <param name="strDirectoryPathIn"></param>
        /// <param name="strDirectoryPathOut"></param>
        /// <param name="strUsuario"></param>
        /// <param name="strPassword"></param>
        /// <param name="strFechaWS"></param>
        /// <param name="stridBus"></param>
        /// <returns></returns>

        public static List<ServiceBUS> BUS_lstLines(string strDirectoryPathIn, string strDirectoryPathOut, string strUsuario, string strPassword, string strFechaWS)
        {

            string strNameFunction = "GetRouteLines";
            //Creamos un nuevo servicio WS
            ServiceBusSoapClient WS_ServiceBUS = new WS_TraficoMadrid_ServiceBUS.ServiceBusSoapClient();

            //Llamamos a la funcion GetListLines que "Recupera la relación general de líneas con su descripción y subgrupo al que pertenencen."
            XmlNode xN = WS_ServiceBUS.GetListLines(strUsuario, strPassword, strFechaWS, "");

            //Construimos el nombre de fichero de entrada y de salida
            string strFilePathIn = strDirectoryPathIn + strNameFunction + ".xml";
            string strFilePathOut = strDirectoryPathOut + strNameFunction + ".csv";

            //Guardamos los valores del WS en un fichero XML
            using (XmlWriter writer = XmlWriter.Create(strFilePathIn))
            {
                xN.WriteTo(writer);
            }

            List<ServiceBUS> lstLines = new List<ServiceBUS>();

            XDocument xDocument = XDocument.Load(strFilePathIn);
            StringBuilder dataToBeWritten = new StringBuilder();

            var results = xDocument.Descendants("REG").Select(x => new
            {
                Line = (string)x.Element("Line"),
            }).ToList();

            for (int i = 0; i < results.Count; i++)
            {
                string tempLine = results[i].Line.Trim();
                lstLines.Add(new ServiceBUS(tempLine));
            }

            //Borramos el fichero temporal, puesto que ya no sirve para nada
            File.Delete(strFilePathIn);

            return lstLines;
        }
        #endregion BUS_GetListLines

        #region BUS_GetNodesLines
        public static void BUS_GetNodesLines_WS(string strDirectoryPathIn, string strDirectoryPathOut, string strUsuario, string strPassword)
        {
            string strNameFunction = "GetNodesLines";
            //Creamos un nuevo servicio WS
            ServiceBusSoapClient WS_ServiceBUS = new WS_TraficoMadrid_ServiceBUS.ServiceBusSoapClient();

            //Llamamos a la funcion GetListLines que "Recupera la relación general de líneas con su descripción y subgrupo al que pertenencen."
            XmlNode xN = WS_ServiceBUS.GetNodesLines(strUsuario, strPassword, "");

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
            BUS_GetNodesLines_XMLToCSV(strFilePathIn, strFilePathOut);

            //Borramos el fichero temporal, puesto que ya no sirve para nada
            File.Delete(strFilePathIn);
        }

        public static void BUS_GetNodesLines_XMLToCSV(string strFilePathIn_CSVToXML, string strFilePathOut_CSVToXML)
        {
            //This method converts an xml file into a .csv file

            XDocument xDocument = XDocument.Load(strFilePathIn_CSVToXML);
            StringBuilder dataToBeWritten = new StringBuilder();

            var results = xDocument.Descendants("REG").Select(x => new
            {
                Node = (string)x.Element("Node"),
                PosxNode = (string)x.Element("PosxNode"),
                PosyNode = (string)x.Element("PosyNode"),
                Name = (string)x.Element("Name"),
                Lines = (string)x.Element("Lines"),
                Wifi = (string)x.Element("Wifi")
            }).ToList();

            for (int i = 0; i < results.Count; i++)
            {
                string tempNode = results[i].Node.Trim();
                string tempPosxNode = results[i].PosxNode.Trim();
                string tempPosyNode = results[i].PosyNode.Trim();
                string tempName = results[i].Name.Trim();
                string tempLines = results[i].Lines.Trim();
                string tempWifi = results[i].Wifi.Trim();

                dataToBeWritten.Append(tempNode);
                dataToBeWritten.Append("|");
                dataToBeWritten.Append(tempPosxNode);
                dataToBeWritten.Append("|");
                dataToBeWritten.Append(tempPosyNode);
                dataToBeWritten.Append("|");
                dataToBeWritten.Append(tempName);
                dataToBeWritten.Append("|");
                dataToBeWritten.Append(tempLines);
                dataToBeWritten.Append("|");
                dataToBeWritten.Append(tempWifi);
                dataToBeWritten.Append("|");
                dataToBeWritten.Append(DateTime.Now);
                dataToBeWritten.Append(Environment.NewLine);
            }

            // Guardamos el Fichero CSV en su sitio correspondiente
            File.WriteAllText(strFilePathOut_CSVToXML, dataToBeWritten.ToString());
        }
        #endregion BUS_GetListLines

        #region BUS_GetRouteLines
        public static void BUS_GetRouteLines_WS(string strDirectoryPathIn, string strDirectoryPathOut, string strUsuario, string strPassword, string strFechaWS, string stridBus)
        {
            string strNameFunction = "GetRouteLines";
            //Creamos un nuevo servicio WS
            ServiceBusSoapClient WS_ServiceBUS = new WS_TraficoMadrid_ServiceBUS.ServiceBusSoapClient();

            //Llamamos a la funcion GetListLines que "Recupera la relación general de líneas con su descripción y subgrupo al que pertenencen."
            XmlNode xN = WS_ServiceBUS.GetRouteLines(strUsuario, strPassword, strFechaWS, stridBus);

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
            BUS_GetRouteLines_XMLToCSV(strFilePathIn, strFilePathOut);

            //Borramos el fichero temporal, puesto que ya no sirve para nada
            File.Delete(strFilePathIn);

        }
        /// <summary>
        /// OBTENEMOS EL LISTADO DE TODAS LAS PARADAS DE UNA LINEA DETERMINADA
        /// </summary>
        /// <param name="strDirectoryPathIn"></param>
        /// <param name="strDirectoryPathOut"></param>
        /// <param name="strUsuario"></param>
        /// <param name="strPassword"></param>
        /// <param name="strFechaWS"></param>
        /// <param name="stridBus"></param>
        /// <returns></returns>

        public static List<ServiceBUS> BUS_lstNodeRouteLines(string strDirectoryPathIn, string strDirectoryPathOut, string strUsuario, string strPassword, string strFechaWS, string stridBus)
        {

            string strNameFunction = "GetRouteLines";
            //Creamos un nuevo servicio WS
            ServiceBusSoapClient WS_ServiceBUS = new WS_TraficoMadrid_ServiceBUS.ServiceBusSoapClient();

            //Llamamos a la funcion GetListLines que "Recupera la relación general de líneas con su descripción y subgrupo al que pertenencen."
            XmlNode xN = WS_ServiceBUS.GetRouteLines(strUsuario, strPassword, strFechaWS, stridBus);

            //Construimos el nombre de fichero de entrada y de salida
            string strFilePathIn = strDirectoryPathIn + strNameFunction + ".xml";
            string strFilePathOut = strDirectoryPathOut + strNameFunction + ".csv";

            //Guardamos los valores del WS en un fichero XML
            using (XmlWriter writer = XmlWriter.Create(strFilePathIn))
            {
                xN.WriteTo(writer);
            }



            List<ServiceBUS> lstNodeRouteLines = new List<ServiceBUS>();

            XDocument xDocument = XDocument.Load(strFilePathIn);
            StringBuilder dataToBeWritten = new StringBuilder();

            var results = xDocument.Descendants("REG").Select(x => new
            {
                SecDetail = (string)x.Element("SecDetail"),
                Node = (string)x.Element("Node"),
            }).ToList();

            for (int i = 0; i < results.Count; i++)
            {
                string tempSecDetail = results[i].SecDetail.Trim();
                string tempNode = results[i].Node.Trim();
                if (tempSecDetail == "10") //Solo hacemos la ida
                {
                    lstNodeRouteLines.Add(new ServiceBUS(tempNode));
                }
            }

            //Borramos el fichero temporal, puesto que ya no sirve para nada
            File.Delete(strFilePathIn);

            return lstNodeRouteLines;
        }




        public static void BUS_GetRouteLines_XMLToCSV(string strFilePathIn_CSVToXML, string strFilePathOut_CSVToXML)
        {
            //This method converts an xml file into a .csv file

            XDocument xDocument = XDocument.Load(strFilePathIn_CSVToXML);
            StringBuilder dataToBeWritten = new StringBuilder();

            var results = xDocument.Descendants("REG").Select(x => new
            {
                Line = (string)x.Element("Line"),
                SecDetail = (string)x.Element("SecDetail"),
                OrderDetail = (string)x.Element("OrderDetail"),
                Node = (string)x.Element("Node"),
                Distance = (string)x.Element("Distance"),
                DistStopPrev = (string)x.Element("DistStopPrev"),
                Name = (string)x.Element("Name"),
                PosxNode = (string)x.Element("PosxNode"),
                PosyNode = (string)x.Element("PosyNode")
            }).ToList();

            for (int i = 0; i < results.Count; i++)
            {
                string tempLine = results[i].Line.Trim();
                string tempSecDetail = results[i].SecDetail.Trim();
                string tempOrderDetail = results[i].OrderDetail.Trim();
                string tempNode = results[i].Node.Trim();
                string tempDistance = results[i].Distance.Trim();
                string tempDistStopPrev = results[i].DistStopPrev.Trim();
                string tempName = results[i].Name.Trim();
                string tempPosxNode = results[i].PosxNode.Trim();
                string tempPosyNode = results[i].PosyNode.Trim();

                dataToBeWritten.Append(tempLine);
                dataToBeWritten.Append("|");
                dataToBeWritten.Append(tempSecDetail);
                dataToBeWritten.Append("|");
                dataToBeWritten.Append(tempOrderDetail);
                dataToBeWritten.Append("|");
                dataToBeWritten.Append(tempNode);
                dataToBeWritten.Append("|");
                dataToBeWritten.Append(tempDistance);
                dataToBeWritten.Append("|");
                dataToBeWritten.Append(tempDistStopPrev);
                dataToBeWritten.Append("|");
                dataToBeWritten.Append(tempName);
                dataToBeWritten.Append("|");
                dataToBeWritten.Append(tempPosxNode);
                dataToBeWritten.Append("|");
                dataToBeWritten.Append(tempPosyNode);
                dataToBeWritten.Append("|");
                dataToBeWritten.Append(DateTime.Now);
                dataToBeWritten.Append(Environment.NewLine);
            }

            // Guardamos el Fichero CSV en su sitio correspondiente
            File.AppendAllText(strFilePathOut_CSVToXML, dataToBeWritten.ToString());
        }
        #endregion BUS_GetListLines
    }
}
