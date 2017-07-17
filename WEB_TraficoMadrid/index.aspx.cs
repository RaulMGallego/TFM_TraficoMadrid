using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WEB_TraficoMadrid
{
    public partial class index : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void btnBuscar_Click(object sender, EventArgs e)
        {



            //string strOrigenDestinoX = "441700,8|443604,5";
            //string strOrigenDestinoY = "4474864|4474829";
            //string strPuntosintermediosX = "441931,1|442467";
            //string strPuntosintermediosY = "4474856|4474978";
            //////ClientScript.RegisterStartupScript(this.GetType(), "myScript", "inicializaMapa('hola ke ase', 40.51, -3.70);", true);
            ////ClientScript.RegisterStartupScript(this.GetType(), "myScript", "initMap('" + strOrigenDestinoX + "','" + strOrigenDestinoY + "','" + strPuntosintermediosX + "','" + strPuntosintermediosY + "');", true);


            ////string strOrigen = "PUERTA DE ALCALA";
            ////string strDestino = "O DONNELL-DR.ESQUERDO";
            ////string strPuntosintermedios = "ALCALA-LAGASCA|O DONNELL-AV.MENENDEZ PELAYO|O DONNELL-NARVAEZ|O DONNELL-MAIQUEZ|";
            //    //ClientScript.RegisterStartupScript(this.GetType(), "myScript", "inicializaMapa('hola ke ase', 40.51, -3.70);", true);
            //ClientScript.RegisterStartupScript(this.GetType(), "myScript", "initMap('" + strOrigenDestinoX + "','" + strOrigenDestinoY + "','" + strPuntosintermediosX + "','" + strPuntosintermediosY + "');", true);


            string strOrigenX = "";
            string strOrigenY = "";
            string strDestinoX = "";
            string strDestinoY = "";
            string strIdLine = "";
            string strName_First = "";
            string strName_End = "";
            string strMinutos_Estim_CON_Predic_Tiempo = "";
            int contador = 0;

            //string strPathNomFicheroLocal = @"D:\TRABAJO\PERSONAL\MASTER_UNIVERSITARIO_BIG_DATA_UNIR\TFM\CSV_CONJUNTO_FINAL\20170525_000.csv";
            string strPathNomFicheroLocal = @"C:\UNIVERSIDAD\00_TFM\TFM_TraficoMadrid\CSV\CSV_CONJUNTO_FINAL\20170525_100.csv";

            DataTable dtEntradaDatos = ConvertirADataTable(strPathNomFicheroLocal);


            // AGRUPAMOS POR NUMBILLETE Y HASH -----------------------------------------------------------------------------------------
            // En la consulta añadimos ToUpper() para evitar las Masyusculas y las Minusculas. Por ejemplo , si encontramos dos DNI 70088848P y 70088848p, seran tratados por igual
            var varSQL = from x in dtEntradaDatos.AsEnumerable()
                         select new
                         {
                             strid = x.Field<string>("id"),
                             linqPosxNode_First = x.Field<string>("PosxNode_First"),
                             linqPosyNode_First = x.Field<string>("PosyNode_First"),
                             linqPosxNode_End = x.Field<string>("PosxNode_End"),
                             linqPosyNode_End = x.Field<string>("PosyNode_End"),
                             linqIdLine = x.Field<string>("IdLine"),
                             linqName_First = x.Field<string>("Name_First"),
                             linqName_End = x.Field<string>("Name_End"),
                             linqMinutos_Estim_CON_Predic_Tiempo = x.Field<string>("Minutos_Estim_CON_Predic_Tiempo")
                         };
            foreach (var campo in varSQL)
            {
                if (contador == 0)
                {
                    strOrigenX = Convert.ToString(campo.linqPosxNode_First);
                    strOrigenY = Convert.ToString(campo.linqPosyNode_First);
                    strDestinoX = Convert.ToString(campo.linqPosxNode_End);
                    strDestinoY = Convert.ToString(campo.linqPosyNode_End);
                    strIdLine = Convert.ToString(campo.linqIdLine);
                    strName_First = Convert.ToString(campo.linqName_First);
                    strName_End = Convert.ToString(campo.linqName_End);
                    strMinutos_Estim_CON_Predic_Tiempo = Convert.ToString(campo.linqMinutos_Estim_CON_Predic_Tiempo);
                }
                else
                {
                    strOrigenX = strOrigenX + "|" + Convert.ToString(campo.linqPosxNode_First);
                    strOrigenY = strOrigenY + "|" + Convert.ToString(campo.linqPosyNode_First);
                    strDestinoX = strDestinoX + "|" + Convert.ToString(campo.linqPosxNode_End);
                    strDestinoY = strDestinoY + "|" + Convert.ToString(campo.linqPosyNode_End);
                    strIdLine = strIdLine + "|" + Convert.ToString(campo.linqIdLine);
                    strName_First = strName_First + "|" + Convert.ToString(campo.linqName_First);
                    strName_End = strName_End + "|" + Convert.ToString(campo.linqName_End);
                    strMinutos_Estim_CON_Predic_Tiempo = strMinutos_Estim_CON_Predic_Tiempo + "|" + Convert.ToString(campo.linqMinutos_Estim_CON_Predic_Tiempo);
                }
                contador = contador + 1;
            }

            ClientScript.RegisterStartupScript(this.GetType(), "myScript", "VentanaTiempoIntermedios('" + strOrigenX + "','" + strOrigenY + "','" + strDestinoX + "','" + strDestinoY + "','" + strIdLine + "','" + strName_First + "','" + strName_End + "','" + strMinutos_Estim_CON_Predic_Tiempo + "');", true);
            //ClientScript.RegisterStartupScript(this.GetType(), "myScript", "MarcadoTiempoIntermedios('" + strOrigenX + "','" + strOrigenY + "','" + strDestinoX + "','" + strDestinoY + "','" + strIdLine + "','" + strName_First + "','" + strName_End + "','" + strMinutos_Estim_CON_Predic_Tiempo + "');", true);
        }


        /// <summary>
        /// CONVIERTE UN FICHERO TXT A UN DATATABLE CON 3 CAMPOS, SE USA PARA SARA
        /// </summary>
        /// <param name="filePath">Nombre del Fichero de Vuelo (txt)</param>
        /// <param name="numberOfColumns">Número de columnas</param>
        /// <returns>Devuelve un datatable</returns>
        public static DataTable ConvertirADataTable(string filePath)
        {
            int numberOfColumns = 17;
            DataTable tbl = new DataTable();
            tbl.Columns.Add(new DataColumn("id")); //0
            tbl.Columns.Add(new DataColumn("PosxNode_First")); //1
            tbl.Columns.Add(new DataColumn("PosyNode_First")); //2
            tbl.Columns.Add(new DataColumn("Name_First")); //3
            tbl.Columns.Add(new DataColumn("PosxNode_End")); //4
            tbl.Columns.Add(new DataColumn("PosyNode_End")); //5
            tbl.Columns.Add(new DataColumn("Name_End")); //6
            tbl.Columns.Add(new DataColumn("IdStop_First")); //7
            tbl.Columns.Add(new DataColumn("IdStop_End")); //8
            tbl.Columns.Add(new DataColumn("IdLine")); //9
            tbl.Columns.Add(new DataColumn("Tiempo_Por_Hora")); //10
            tbl.Columns.Add(new DataColumn("Minutos_Estim_SIN_Predic_Tiempo")); //11
            tbl.Columns.Add(new DataColumn("Tiempo_Estim_Soleado_0")); //12
            tbl.Columns.Add(new DataColumn("Tiempo_Estim_Nublado_50")); //13
            tbl.Columns.Add(new DataColumn("Tiempo_Estim_Lluvioso_100")); //14
            tbl.Columns.Add(new DataColumn("Minutos_Estim_CON_Predic_Tiempo_ROUND")); //15
            tbl.Columns.Add(new DataColumn("Minutos_Estim_CON_Predic_Tiempo")); //16

            //
            string[] lines = System.IO.File.ReadAllLines(filePath);
            int contLineas = 1; //Primer número de linea
            foreach (string line in lines)
            {
                var cols = line.Split(';');

                DataRow dr = tbl.NewRow();
                dr[0] = contLineas; //Usamos el contador para generar el Numero de lineas
                for (int cIndex = 0; cIndex < numberOfColumns - 1; cIndex++)
                {
                    dr[cIndex + 1] = cols[cIndex];
                }
                contLineas = contLineas + 1;

                tbl.Rows.Add(dr);
            }


            return tbl;
        }
    }
}