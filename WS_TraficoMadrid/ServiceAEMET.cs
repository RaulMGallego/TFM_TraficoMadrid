using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;


namespace WS_TraficoMadrid
{
    public class ServiceAEMET
    {
        public static void AEMET_getPrevisionTiempo(string strDirectoryPathIn, string strDirectoryPathOut)
        {
            string strNameFunction = "getPrevisionTiempo";


            //Montamos el día de Hoy
            DateTime GetFechaHoy = DateTime.Now;
            string strGetFechaHoy = GetFechaHoy.ToShortDateString();
            string strGetFechaHoyFormat = strGetFechaHoy.Substring(6, 4) + strGetFechaHoy.Substring(3, 2) + strGetFechaHoy.Substring(0, 2) + "_";

            //Construimos el nombre de fichero de entrada y de salida
            string strFilePathIn = strDirectoryPathIn;
            string strFilePathOut = strDirectoryPathOut + strGetFechaHoyFormat + strNameFunction + ".csv";

            //Llamamos a la funcion que nos convierte el XML en CSV
            GEO_getPrevisionTiempo_XMLToCSV(strFilePathIn, strFilePathOut);

        }

        public static void GEO_getPrevisionTiempo_XMLToCSV(string FilePathIn_CSVToXML, string FilePathOut_CSVToXML)
        {
            //This method converts an xml file into a .csv file

            XDocument xDocument = XDocument.Load(FilePathIn_CSVToXML);
            StringBuilder dataToBeWritten = new StringBuilder();

            XElement xmlAEMET = XElement.Load(FilePathIn_CSVToXML);

            //Extraemos los días que queremos
            DateTime GetFechaHoy = DateTime.Now;
            DateTime GetFechaHoy_Mas1 = GetFechaHoy.AddDays(1);
            DateTime GetFechaHoy_Mas2 = GetFechaHoy.AddDays(2);

            //Montamos el día de Hoy
            string strGetFechaHoy = GetFechaHoy.ToShortDateString();
            string strGetFechaHoyFormat = strGetFechaHoy.Substring(6, 4) + "-" + strGetFechaHoy.Substring(3, 2) + "-" + strGetFechaHoy.Substring(0, 2);

            //Obtenemos el día que queremos procesar
            var LINQ_Extrae_FechaHoy = from linq in xmlAEMET.Descendants("dia")
                                       where (string)linq.Attribute("fecha") == strGetFechaHoyFormat
                                       select linq;

            //Obtenemos los campos que necesitamos.
            //---- PROPABILIDAD DE PRECIPITACION -------------------------------------------------------------
            var LINQ_ExtraeCampos_FechaHoy = from linq in LINQ_Extrae_FechaHoy.Descendants("prob_precipitacion")
                                             //    where (string)Camp.Attribute("periodo") == "12-24" //|| (string)Camp.Attribute("periodo") == "00-12"
                                             select linq;

            //Montamos la probalidad de precipitacion
            foreach (var varCampos in LINQ_ExtraeCampos_FechaHoy)
            {
                if (varCampos.Attribute("periodo").Value.ToString() == "00-12")
                {
                    dataToBeWritten.Append(varCampos.Value.ToString());
                    dataToBeWritten.Append("|");
                }
                if (varCampos.Attribute("periodo").Value.ToString() == "12-24")
                {
                    dataToBeWritten.Append(varCampos.Value.ToString());
                    dataToBeWritten.Append("|");
                }
                if (varCampos.Attribute("periodo").Value.ToString() == "00-06")
                {
                    dataToBeWritten.Append(varCampos.Value.ToString());
                    dataToBeWritten.Append("|");
                }
                if (varCampos.Attribute("periodo").Value.ToString() == "06-12")
                {
                    dataToBeWritten.Append(varCampos.Value.ToString());
                    dataToBeWritten.Append("|");
                }
                if (varCampos.Attribute("periodo").Value.ToString() == "12-18")
                {
                    dataToBeWritten.Append(varCampos.Value.ToString());
                    dataToBeWritten.Append("|");
                }
                if (varCampos.Attribute("periodo").Value.ToString() == "18-24")
                {
                    dataToBeWritten.Append(varCampos.Value.ToString());
                    dataToBeWritten.Append("|");
                }

            }
            // Terminamos de montar los nuevos campos
            dataToBeWritten.Append(strGetFechaHoy);
            dataToBeWritten.Append("|");
            dataToBeWritten.Append(DateTime.Now);
            dataToBeWritten.Append(Environment.NewLine);


            //Montamos el dia +1 al de hoy, es decir, mañana
            string strGetFechaHoy_Mas1 = GetFechaHoy_Mas1.ToShortDateString();
            string strGetFechaHoy_Mas1Format = strGetFechaHoy_Mas1.Substring(6, 4) + "-" + strGetFechaHoy_Mas1.Substring(3, 2) + "-" + strGetFechaHoy_Mas1.Substring(0, 2);

            //Obtenemos el día que queremos procesar
            var LINQ_Extrae_FechaHoy_Mas1 = from linq in xmlAEMET.Descendants("dia")
                                            where (string)linq.Attribute("fecha") == strGetFechaHoy_Mas1Format
                                            select linq;

            //Obtenemos los campos que necesitamos.
            //---- PROPABILIDAD DE PRECIPITACION -------------------------------------------------------------
            var LINQ_ExtraeCampos_FechaHoy_Mas1 = from linq in LINQ_Extrae_FechaHoy_Mas1.Descendants("prob_precipitacion")
                                                  //    where (string)Camp.Attribute("periodo") == "12-24" //|| (string)Camp.Attribute("periodo") == "00-12"
                                                  select linq;

            //Montamos la probalidad de precipitacion
            foreach (var varCampos in LINQ_ExtraeCampos_FechaHoy_Mas1)
            {
                if (varCampos.Attribute("periodo").Value.ToString() == "00-12")
                {
                    dataToBeWritten.Append(varCampos.Value.ToString());
                    dataToBeWritten.Append("|");
                }
                if (varCampos.Attribute("periodo").Value.ToString() == "12-24")
                {
                    dataToBeWritten.Append(varCampos.Value.ToString());
                    dataToBeWritten.Append("|");
                }
                if (varCampos.Attribute("periodo").Value.ToString() == "00-06")
                {
                    dataToBeWritten.Append(varCampos.Value.ToString());
                    dataToBeWritten.Append("|");
                }
                if (varCampos.Attribute("periodo").Value.ToString() == "06-12")
                {
                    dataToBeWritten.Append(varCampos.Value.ToString());
                    dataToBeWritten.Append("|");
                }
                if (varCampos.Attribute("periodo").Value.ToString() == "12-18")
                {
                    dataToBeWritten.Append(varCampos.Value.ToString());
                    dataToBeWritten.Append("|");
                }
                if (varCampos.Attribute("periodo").Value.ToString() == "18-24")
                {
                    dataToBeWritten.Append(varCampos.Value.ToString());
                    dataToBeWritten.Append("|");
                }

            }
            // Terminamos de montar los nuevos campos
            dataToBeWritten.Append(strGetFechaHoy_Mas1);
            dataToBeWritten.Append("|");
            dataToBeWritten.Append(DateTime.Now);
            dataToBeWritten.Append(Environment.NewLine);



            //Montamos el dia +2 al de hoy, es decir, pasadomañana
            string strGetFechaHoy_Mas2 = GetFechaHoy_Mas2.ToShortDateString();
            string strGetFechaHoy_Mas2Format = strGetFechaHoy_Mas2.Substring(6, 4) + "-" + strGetFechaHoy_Mas2.Substring(3, 2) + "-" + strGetFechaHoy_Mas2.Substring(0, 2);




            //Obtenemos el día que queremos procesar
            var LINQ_Extrae_FechaHoy_Mas2 = from linq in xmlAEMET.Descendants("dia")
                                            where (string)linq.Attribute("fecha") == strGetFechaHoy_Mas2Format
                                            select linq;

            //Obtenemos los campos que necesitamos.
            //---- PROPABILIDAD DE PRECIPITACION -------------------------------------------------------------
            var LINQ_ExtraeCampos_FechaHoy_Mas2 = from linq in LINQ_Extrae_FechaHoy_Mas2.Descendants("prob_precipitacion")
                                                  //    where (string)Camp.Attribute("periodo") == "12-24" //|| (string)Camp.Attribute("periodo") == "00-12"
                                                  select linq;

            //Montamos la probalidad de precipitacion
            foreach (var varCampos in LINQ_ExtraeCampos_FechaHoy_Mas2)
            {
                if (varCampos.Attribute("periodo").Value.ToString() == "00-12")
                {
                    dataToBeWritten.Append(varCampos.Value.ToString());
                    dataToBeWritten.Append("|");
                }
                if (varCampos.Attribute("periodo").Value.ToString() == "12-24")
                {
                    dataToBeWritten.Append(varCampos.Value.ToString());
                    dataToBeWritten.Append("|");
                }
            }
            //No existe para este día de 00-06
            dataToBeWritten.Append("0");
            dataToBeWritten.Append("|");
            //No existe para este día de 06-12
            dataToBeWritten.Append("0");
            dataToBeWritten.Append("|");
            //No existe para este día de 12-18
            dataToBeWritten.Append("0");
            dataToBeWritten.Append("|");
            //No existe para este día de 18-24
            dataToBeWritten.Append("0");
            dataToBeWritten.Append("|");
            // Terminamos de montar los nuevos campos
            dataToBeWritten.Append(strGetFechaHoy_Mas2);
            dataToBeWritten.Append("|");
            dataToBeWritten.Append(DateTime.Now);
            dataToBeWritten.Append(Environment.NewLine);

            // Guardamos y añadimos el Fichero CSV en su sitio correspondiente
            File.AppendAllText(FilePathOut_CSVToXML, dataToBeWritten.ToString());
        }
    }
}
