using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET_TraficoMadrid
{
    public class Utilidades
    {
        public static void TFM_GetArriveStopEnd(string strDirectoryPathIn)
        {
            string strNameFunction = "getArriveStopEnd";
            string strNameFunctionOrigen = "getArriveStop";
            //Montamos el día de Hoy
            DateTime GetFechaHoy = DateTime.Now;
            string strGetFechaHoy = GetFechaHoy.ToShortDateString();
            string strGetFechaHoyFormat = strGetFechaHoy.Substring(6, 4) + strGetFechaHoy.Substring(3, 2) + strGetFechaHoy.Substring(0, 2) + "_";

            //Construimos el nombre de fichero de entrada y de salida
            string strFilePathIn = strDirectoryPathIn + strGetFechaHoyFormat + strNameFunctionOrigen + ".csv";
            string strFilePathOut = strDirectoryPathIn + strGetFechaHoyFormat + strNameFunction + ".csv";

            DataTable dtMultasEntradaDatos = ConvertCSVtoDataTable(strFilePathIn);

            var Rows = (from row in dtMultasEntradaDatos.AsEnumerable()
                        orderby row.Field<String>("idLine").Trim() descending
                             , row.Field<String>("idBus").Trim() descending
                             , row.Field<String>("idStop").Trim() descending
                             , row.Field<String>("FechaAuditoria").Trim() ascending
                        select row);
            dtMultasEntradaDatos = Rows.AsDataView().ToTable();
            Get_dt_ArriveStopEnd(dtMultasEntradaDatos, strFilePathOut);
       }

        public static void Get_dt_ArriveStopEnd(DataTable dt, string strFilePathOut)
        {
            string strAuxidStop = "";
            string strAuxidLine = "";
            string strAuxisHead = "";
            string strAuxidDestination = "";
            string strAuxidBus = "";
            string strAuxTimeLeftBus = "";
            string strAuxDistanceBus = "";
            string strAuxPositionxBus = "";
            string strAuxPositionyBus = "";
            string strAuxPositionTypeBus = "";
            string strAuxFechaAuditoriaIni = "";
            string strAuxFechaAuditoriaFin = "";

            bool bsw = true;

            StringBuilder sb = new StringBuilder();
            foreach (DataRow row in dt.Rows)
            {
                //Para la primera vuelta
                if (bsw == true)
                {
                    strAuxidStop = row[0].ToString();
                    strAuxidLine = row[1].ToString();
                    strAuxisHead = row[2].ToString();
                    strAuxidDestination = row[3].ToString();
                    strAuxidBus = row[4].ToString();
                    strAuxTimeLeftBus = row[5].ToString();
                    strAuxDistanceBus = row[6].ToString();
                    strAuxPositionxBus = row[7].ToString();
                    strAuxPositionyBus = row[8].ToString();
                    strAuxPositionTypeBus = row[9].ToString();
                    strAuxFechaAuditoriaIni = row[10].ToString();
                    bsw = false;
                }
                else //para el resto de las vueltas
                {
                    /*Buscamos los cambios de tiempo, por lo que quiere decir que 
                    //Ha llegado al destino, ha vuelto y ha vuelto a salir, Ejemplo
                    TimeLeftBus	    DistanceBus		FechaAuditoria
                    999999	        3963	   		06/05/2017 12:06:06
                    1180	        3815	    	06/05/2017 12:08:03
                    968	            3270	    	06/05/2017 12:10:12
                    -----------------------------------------------------
                    1852	        4898	    	06/05/2017 14:12:02
                    728	            2476	    	06/05/2017 14:14:06
                     */
                    if (strAuxidStop == row[0].ToString() && strAuxidLine == row[1].ToString() && strAuxidBus == row[4].ToString() && Int32.Parse(strAuxTimeLeftBus) < Int32.Parse(row[5].ToString()))
                    {
                        //Console.WriteLine(string.Format("{0}|{1}|{2}|{3}|{4} - {5}", row[0], row[1], row[4], row[5], strAuxFechaAuditoriaIni, strAuxFechaAuditoriaFin));
                        sb.Append(string.Format("{0}|{1}|{2}|{3}|{4}", row[0], row[1], row[4], strAuxFechaAuditoriaIni, strAuxFechaAuditoriaFin));
                        sb.Append(Environment.NewLine);
                        strAuxidStop = row[0].ToString();
                        strAuxidLine = row[1].ToString();
                        strAuxisHead = row[2].ToString();
                        strAuxidDestination = row[3].ToString();
                        strAuxidBus = row[4].ToString();
                        strAuxTimeLeftBus = row[5].ToString();
                        strAuxDistanceBus = row[6].ToString();
                        strAuxPositionxBus = row[7].ToString();
                        strAuxPositionyBus = row[8].ToString();
                        strAuxPositionTypeBus = row[9].ToString();
                        strAuxFechaAuditoriaIni = row[10].ToString();
                    }
                    else
                    {
                        //Buscamos un cambio de parada
                        /*
                            idStop	idLine	idBus
                            162	    1	    8323
                            162	    1	    8323
                            162	    1	    8323
                            --------------------------------
                            168	    1	    8454
                            168	    1	    8454
                            168	    1	    8454
                            168	    1	    8454
                         */
                        if (strAuxidStop != row[0].ToString() || strAuxidLine != row[1].ToString() || strAuxidBus != row[4].ToString())
                        {
                            //Console.WriteLine(string.Format("{0}|{1}|{2}|{3}|{4} - {5}", row[0], row[1], row[4], row[5], strAuxFechaAuditoriaIni, strAuxFechaAuditoriaFin));
                            sb.Append(string.Format("{0}|{1}|{2}|{3}|{4}", row[0], row[1], row[4], strAuxFechaAuditoriaIni, strAuxFechaAuditoriaFin));
                            sb.Append(Environment.NewLine);
                        
                            strAuxidStop = row[0].ToString();
                            strAuxidLine = row[1].ToString();
                            strAuxisHead = row[2].ToString();
                            strAuxidDestination = row[3].ToString();
                            strAuxidBus = row[4].ToString();
                            strAuxTimeLeftBus = row[5].ToString();
                            strAuxDistanceBus = row[6].ToString();
                            strAuxPositionxBus = row[7].ToString();
                            strAuxPositionyBus = row[8].ToString();
                            strAuxPositionTypeBus = row[9].ToString();
                            strAuxFechaAuditoriaIni = row[10].ToString();
                        }
                    }
                    strAuxFechaAuditoriaFin = row[10].ToString();
                }
               
                
            }
            File.WriteAllText(strFilePathOut, sb.ToString());
        }

        public static DataTable ConvertCSVtoDataTable(string strFilePath)
        {
           
            int numberOfColumns = 11;
            
            //DataTable de Destino
            DataTable dt = new DataTable();
            //res.Columns.Add(new DataColumn("Id")); //
            dt.Columns.Add(new DataColumn("IdStop")); //0
            dt.Columns.Add(new DataColumn("idLine")); //1
            dt.Columns.Add(new DataColumn("IsHead")); //2
            dt.Columns.Add(new DataColumn("Destination")); //3
            dt.Columns.Add(new DataColumn("IdBus")); //4
            dt.Columns.Add(new DataColumn("TimeLeftBus")); //5
            dt.Columns.Add(new DataColumn("DistanceBus")); //6
            dt.Columns.Add(new DataColumn("PositionXBus")); //7
            dt.Columns.Add(new DataColumn("PositionYBus")); //8
            dt.Columns.Add(new DataColumn("PositionTypeBus")); //9
            dt.Columns.Add(new DataColumn("FechaAuditoria")); //10

            string[] lines = File.ReadAllLines(strFilePath, Encoding.Default);
           
            foreach (string line in lines)
            {
                string[] cols = line.Split('|');
                //El número máximo de columnas lo definimos en funcion del tamaño maximo del array
                numberOfColumns = cols.Length;
                DataRow dr = dt.NewRow();
                for (int cIndex = 0; cIndex < numberOfColumns; cIndex++)
                {
                    dr[cIndex] = cols[cIndex].Trim(); //Quitamos los espacios en blanco
                }
                dt.Rows.Add(dr);
            }
            return dt;

        }

        



    }
}
