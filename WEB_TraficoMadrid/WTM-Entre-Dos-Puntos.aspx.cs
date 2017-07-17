using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WEB_TraficoMadrid
{
    public partial class WTM_Entre_Dos_Puntos : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void ddlHora_TextChanged(object sender, EventArgs e)
        {
            string strPathNomFicheroLocal = @"D:\TRABAJO\PERSONAL\MASTER_UNIVERSITARIO_BIG_DATA_UNIR\TFM\DOCUMENTACION\CSV_CONJUNTO_FINAL\20170525_000.csv";
            
            // Volcamos los valores del txt a un datatable
            DataTable dtEntradaDatos = ConvertirADataTable(strPathNomFicheroLocal);
            
            // Realizamos una busqueda sobre LINQ
            var varSQL_RutaInicio = (from x in dtEntradaDatos.AsEnumerable()
                                     where Convert.ToInt32(x.Field<string>("Tiempo_Por_Hora")) == Convert.ToInt32(ddlHora.Text)
                                     group x by x.Field<string>("Name_First") into grupo
                                     orderby grupo.Key
                                     select new
                                     {
                                         linqKey = grupo.Key
                                     }).ToList();

            ddlRutaInicio.DataSource = varSQL_RutaInicio;
            ddlRutaInicio.DataTextField = "linqKey";
            ddlRutaInicio.DataValueField = "linqKey";
            ddlRutaInicio.DataBind();
        }
        protected void ddlRutaInicio_TextChanged(object sender, EventArgs e)
        {
            string strPathNomFicheroLocal = @"D:\TRABAJO\PERSONAL\MASTER_UNIVERSITARIO_BIG_DATA_UNIR\TFM\DOCUMENTACION\CSV_CONJUNTO_FINAL\20170525_000.csv";
            
            // Volcamos los valores del txt a un datatable
            DataTable dtEntradaDatos = ConvertirADataTable(strPathNomFicheroLocal);

            // Realizamos una busqueda sobre LINQ
            var varSQL_RutaInicio = (from x in dtEntradaDatos.AsEnumerable()
                                     where Convert.ToInt32(x.Field<string>("Tiempo_Por_Hora")) == Convert.ToInt32(ddlHora.Text) && x.Field<string>("Name_First") == ddlRutaInicio.Text
                                     group x by x.Field<string>("Name_End") into grupo
                                     orderby grupo.Key
                                     select new
                                     {
                                         linqKey = grupo.Key
                                     }).ToList();

            ddlRutaFin.DataSource = varSQL_RutaInicio;
            ddlRutaFin.DataTextField = "linqKey";
            ddlRutaFin.DataValueField = "linqKey";
            ddlRutaFin.DataBind();
        }


        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            // Creamos las variables que posteriormente vamos a utilizar
            string strOrigenX = "";
            string strOrigenY = "";
            string strDestinoX = "";
            string strDestinoY = "";
            string strIdLine = "";
            string strName_First = "";
            string strName_End = "";
            string strMinutos_Estim_CON_Predic_Tiempo = "";
            int contador = 0;
            string strPrevisionTiempo = "";
            string FechaHora = "";
            string Fecha = "";
            string FechaAuditoria = "";
            string strOrigenDestinoX = "";
            string strOrigenDestinoY = "";

            // Obtenemos los valores de entrada
            string strFechaFormulario = Request["txtFecha"]; //Fecha del formulario
            string strHora = ddlHora.Text; // HOra del formulario
            int iHora = Convert.ToInt32(strHora);

            //Obtenemos las previsión meteorologica en funcion de los próximos dos días
            DateTime FechaHoy = DateTime.Now;

            string strFechaHoy = Convert.ToString(FechaHoy);
            string strFechaHoyFichero = strFechaHoy.Substring(6, 4) + strFechaHoy.Substring(3, 2) + strFechaHoy.Substring(0, 2);
            //Para pruebas nos aseguramos el día correcto
            strFechaHoyFichero = "20170525";
            strFechaHoy = "25/05/2017";

            //Obtenemos las Fecha y la Hora que buscamos en el formulario de entrada de datos
            FechaHora = strFechaHoy + " " + iHora + ":00";

            string strPathNomFichero_PrevisionTiempo = @"D:\TRABAJO\PERSONAL\MASTER_UNIVERSITARIO_BIG_DATA_UNIR\TFM\DOCUMENTACION\CSV_CONJUNTO_FINAL\" + strFechaHoyFichero + "_getPrevisionTiempoFinal.csv";

            DataTable dtEntradaDatos_PrevisionTiempo = ConvertirADataTable_PrevisionTiempo(strPathNomFichero_PrevisionTiempo);


            // Realizamos una busqueda sobre LINQ
            var varSQL_PrevisionTiempo = from x in dtEntradaDatos_PrevisionTiempo.AsEnumerable()
            where x.Field<string>("Fecha") == strFechaHoy && Convert.ToDateTime(x.Field<string>("FechaAuditoria")) <= Convert.ToDateTime(FechaHora)
                                         orderby Convert.ToDateTime(x.Field<string>("FechaAuditoria"))
                                         select new
                                         {
                                             linqFecha = x.Field<string>("Fecha"),
                                             linqFechaAuditoria = x.Field<string>("FechaAuditoria"),
                                             linqPrediccion_00_12 = x.Field<string>("Prediccion_00_12"),
                                             linqPrediccion_12_24 = x.Field<string>("Prediccion_12_24"),
                                             linqPrediccion_00_06 = x.Field<string>("Prediccion_00_06"),
                                             linqPrediccion_06_12 = x.Field<string>("Prediccion_06_12"),
                                             linqPrediccion_12_18 = x.Field<string>("Prediccion_12_18"),
                                             linqPrediccion_18_24 = x.Field<string>("Prediccion_18_24")

                                         };
            foreach (var campo in varSQL_PrevisionTiempo)
            {
                Fecha = Convert.ToString(campo.linqFecha);
                FechaAuditoria = Convert.ToString(campo.linqFechaAuditoria);
                if (iHora >= 0 && iHora < 6)
                {
                    strPrevisionTiempo = Convert.ToString(campo.linqPrediccion_00_06);
                }
                if (iHora >= 6 && iHora < 12)
                {
                    strPrevisionTiempo = Convert.ToString(campo.linqPrediccion_06_12);
                }
                if (iHora >= 12 && iHora < 18)
                {
                    strPrevisionTiempo = Convert.ToString(campo.linqPrediccion_12_18);
                }
                if (iHora >= 18 && iHora < 24)
                {
                    strPrevisionTiempo = Convert.ToString(campo.linqPrediccion_18_24);
                }
            }


            ////Los porcentajes en el WS van de 5 en 5,
            ////Los ficheros van de 10 en 10, por lo tanto, si nos viene un número acabado en 5 debemos pasarle al nivel superior
            //// Por ejemplo si viene un porcentaje del 15% debemos comprobar el archivo del 10%
            if ((Convert.ToInt32(strPrevisionTiempo) % 10) == 0 && Convert.ToInt32(strPrevisionTiempo) >= 10)
            {
                strPrevisionTiempo = strPrevisionTiempo;
            }
            else
            {
                if (Convert.ToInt32(strPrevisionTiempo) > 0)
                {
                    strPrevisionTiempo = Convert.ToString(Convert.ToInt32(strPrevisionTiempo) - 5);
                }
            }
            //Añadimos 000 por la izquierda para conseguir el nombre del fichero
            char pad = '0';
            strPrevisionTiempo = strPrevisionTiempo.PadLeft(3, pad);

            // Obtenemos la fecha del formulario en formato string
            strFechaFormulario = strFechaFormulario.Substring(0, 4) + strFechaFormulario.Substring(5, 2) + strFechaFormulario.Substring(8, 2);
            // Generamos la ruta del archivo que vamos a visualizar
            string strPathNomFicheroLocal = @"D:\TRABAJO\PERSONAL\MASTER_UNIVERSITARIO_BIG_DATA_UNIR\TFM\DOCUMENTACION\CSV_CONJUNTO_FINAL\" + strFechaFormulario + "_" + strPrevisionTiempo + ".csv";
            
            // Volcamos los valores del txt a un datatable
            DataTable dtEntradaDatos = ConvertirADataTable(strPathNomFicheroLocal);


            // Realizamos una busqueda sobre LINQ
            var varSQL = from x in dtEntradaDatos.AsEnumerable()
            where Convert.ToInt32(x.Field<string>("Tiempo_Por_Hora")) == iHora && x.Field<string>("Name_First") == ddlRutaInicio.Text && x.Field<string>("Name_End") == ddlRutaFin.Text
                         select new
                         {
                             linqid = x.Field<string>("id"),
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
                strOrigenDestinoX = Convert.ToString(campo.linqPosxNode_First) + "|" + Convert.ToString(campo.linqPosxNode_End);
                strOrigenDestinoY = Convert.ToString(campo.linqPosyNode_First) + "|" + Convert.ToString(campo.linqPosyNode_End);
                strIdLine = Convert.ToString(campo.linqIdLine);
                strName_First = Convert.ToString(campo.linqName_First);
                strName_End = Convert.ToString(campo.linqName_End);
                strMinutos_Estim_CON_Predic_Tiempo = Convert.ToString(campo.linqMinutos_Estim_CON_Predic_Tiempo);
            }

            //Abrimos el div de prediccion y mostramos las información deseada

            lblPrediccionTiempo.Text = "La fecha seleccionada es el <b>" + strFechaHoy + "</b> a las <b>" + iHora + ":00</b> con una probabilidad de lluvia del <b>" + Convert.ToInt32(strPrevisionTiempo) + "%</b>";

            
            string strPuntosintermediosX = "441931,1|442467";
            string strPuntosintermediosY = "4474856|4474978";
            // Realizamos la llamada a la función de javascript
            ClientScript.RegisterStartupScript(this.GetType(), "myScript", "Entre_Dos_Puntos('" + strOrigenDestinoX + "','" + strOrigenDestinoY + "','" + strPuntosintermediosX + "','" + strPuntosintermediosY + "','" + strIdLine + "','" + strName_First + "','" + strName_End + "','" + strMinutos_Estim_CON_Predic_Tiempo + "','" + iHora + "','" + Convert.ToInt32(strPrevisionTiempo) + "');", true);

        }

        /// <summary>
        /// CONVIERTE UN FICHERO TXT A UN DATATABLE CON 16 CAMPOS
        /// </summary>
        /// <param name="filePath">Nombre del Fichero (txt)</param>
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

        /// <summary>
        /// CONVIERTE UN FICHERO TXT A UN DATATABLE CON 8 CAMPOS, SE USA PARA LA PREDICCIÓN DEL TIEMPO
        /// </summary>
        /// <param name="filePath">Nombre del Fichero (txt)</param>
        /// <returns>Devuelve un datatable</returns>
        public static DataTable ConvertirADataTable_PrevisionTiempo(string filePath)
        {
            int numberOfColumns = 9;
            DataTable tbl = new DataTable();
            tbl.Columns.Add(new DataColumn("id")); //0
            tbl.Columns.Add(new DataColumn("Prediccion_00_12")); //0
            tbl.Columns.Add(new DataColumn("Prediccion_12_24")); //1
            tbl.Columns.Add(new DataColumn("Prediccion_00_06")); //2
            tbl.Columns.Add(new DataColumn("Prediccion_06_12")); //3
            tbl.Columns.Add(new DataColumn("Prediccion_12_18")); //4
            tbl.Columns.Add(new DataColumn("Prediccion_18_24")); //5
            tbl.Columns.Add(new DataColumn("Fecha")); //6
            tbl.Columns.Add(new DataColumn("FechaAuditoria")); //7
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