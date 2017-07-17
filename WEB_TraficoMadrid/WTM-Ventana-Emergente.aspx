<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WTM-Ventana-Emergente.aspx.cs" Inherits="WEB_TraficoMadrid.WTM_Ventana_Emergente" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>TFM - Raúl Murillo</title>
    <link href="css/TFM_WebTraficoMadrid.css" rel="stylesheet" />
    <script type='text/javascript' src='https://maps.googleapis.com/maps/api/js?key=AIzaSyBTCjQLNz8HsNVD5Rvr3YEf4QqxK4Pr6G0'></script>
    <script src="js/MarcasConVentana.js"></script>
    <script src="js/CalculoPosicionamiento.js"></script>
    <style>
        .labels {
            color: #000;
            background: #fff;
            border: 1px solid #000;
            font-family: "Lucida Grande", "Arial", sans-serif;
            font-size: 12px;
            text-align: left;
            width: 100px;
            z-index: 9999999;
            position: relative;

        }
        ul {
            list-style-type: none;
            margin: 0;
            padding: 0;
            overflow: hidden;
            background-color: #333;
        }

        li {
            float: left;
        }

        li a {
            display: block;
            color: white;
            text-align: center;
            padding: 14px 16px;
            text-decoration: none;
        }

        li a:hover {
            background-color: #111;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
   <ul>
      <li><a class="active" href="WTM-Marcado-Global.aspx">Previsión Global</a></li>
      <li><a href="WTM-Ventana-Emergente.aspx">Previsión con ventana emergente</a></li>
      <li><a href="WTM-Entre-Dos-Puntos.aspx">Previsión entre dos puntos</a></li>
    </ul>
    <div id="div_pri_izq">
        <div class="div_seg_izq">
            <div id="ventana-estado-trafico" class="superior">
                
            </div>
            <div class="inferior">
                Previsión del tráfico con ventana emergente
            </div>
        </div>
        
    </div>
    <div id="div_pri_der">
        <div class="div_seg_der">
            <div id="panel_prevision_tiempo" class="superior">
                 <asp:Label ID="lblPrediccionTiempo" runat="server" Text="Selecciona una fecha y una hora para conocer la previsión del tiempo"></asp:Label><br />
            </div>
            
            <div class="inferior">
               Previsión del tiempo
            </div>
        </div>
        
        <div class="div_seg_der">
            <div id="panel-introducir-datos" class="superior">
                <asp:Label ID="lblFecha"  runat="server" CssClass="label" Text="Fecha: "></asp:Label>
                <input type="date" name="txtFecha" class="input" step="1" min="2017-01-01" max="2020-12-31" value="2017-05-25" />
                <asp:Label ID="lblHora"  runat="server" CssClass="label" Text="Hora: "></asp:Label>
                <asp:DropDownList ID="ddlHora" runat="server" Width="100%" BackColor="#ffffff" ForeColor="#333333" CssClass="ddl">
                    <asp:ListItem Value="06">06:00</asp:ListItem>
                    <asp:ListItem Value="07">07:00</asp:ListItem>
                    <asp:ListItem Value="08">08:00</asp:ListItem>
                    <asp:ListItem Value="09">09:00</asp:ListItem>
                    <asp:ListItem Value="10">10:00</asp:ListItem>
                    <asp:ListItem Value="11">11:00</asp:ListItem>
                    <asp:ListItem Value="12">12:00</asp:ListItem>
                    <asp:ListItem Value="13">13:00</asp:ListItem>
                    <asp:ListItem Value="14">14:00</asp:ListItem>
                    <asp:ListItem Value="15">15:00</asp:ListItem>
                    <asp:ListItem Value="16">16:00</asp:ListItem>
                    <asp:ListItem Value="17">17:00</asp:ListItem>
                    <asp:ListItem Value="18">18:00</asp:ListItem>
                    <asp:ListItem Value="19">19:00</asp:ListItem>
                    <asp:ListItem Value="20">20:00</asp:ListItem>
                    <asp:ListItem Value="21">21:00</asp:ListItem>
                    <asp:ListItem Value="22">22:00</asp:ListItem>
                </asp:DropDownList>
                <br /><br /><br />
                <asp:Button ID="btnBuscar" runat="server" Text="Buscar" CssClass="button"  OnClick="btnBuscar_Click" />
                <br /><br />
            </div>
            
            <div class="inferior">
                Datos de entrada
            </div>
        </div>
           
           
    </div>
    <!-- VENTANA EMERGENTE -->
    <script>
        function JS_Ventana_Emergente(strOrigenX, strOrigenY, strDestinoX, strDestinoY, strIdLine, strName_First, strName_End, strMinutos_Estim_CON_Predic_Tiempo, iHora, strPrevisionTiempo) {

            //Posición inicial en el mapa
            var map = new google.maps.Map(document.getElementById('ventana-estado-trafico'), {
                zoom: 13,
                center: { lat: 40.41, lng: -3.70 }
            });

            // Realizamos la llamada a la ventana emergente pasandole todas las variables que necesitamos
            setMarkers(map, strOrigenX, strOrigenY, strDestinoX, strDestinoY, strIdLine, strName_First, strName_End, strMinutos_Estim_CON_Predic_Tiempo, iHora, strPrevisionTiempo)
        }

        //Funcion que activa la ventana emergente
        function setMarkers(map, strOrigenX, strOrigenY, strDestinoX, strDestinoY, strIdLine, strName_First, strName_End, strMinutos_Estim_CON_Predic_Tiempo, iHora, strPrevisionTiempo) {

            // Definimos las variables que vamos a utilizar en la función
            var marker, i
            var arrOrigenX = strOrigenX.split('|');
            var arrOrigenY = strOrigenY.split('|');
            var arrIdLine = strIdLine.split('|');
            var arrName_First = strName_First.split('|');
            var arrName_End = strName_End.split('|');
            var arrMinutos_Estim_CON_Predic_Tiempo = strMinutos_Estim_CON_Predic_Tiempo.split('|');
            latlon = new Array(2);
            var x, y, zone, southhemi;

            // Recorremos todo el array buscando todas las posiciones en el mapa
            for (i = 0; i < arrOrigenX.length; i++) {

                x = parseFloat(arrOrigenX[i]);
                y = parseFloat(arrOrigenY[i]);
                zone = parseFloat("30");//por defecto España es 30
                southhemi = false;

                // Convertimos las coordenadas UTM en latitud y longitud
                UTMXYToLatLon(x, y, zone, southhemi, latlon);

                var OrigenLat = RadToDeg(latlon[0]);
                var OrigenLong = RadToDeg(latlon[1]);

                latlngset = new google.maps.LatLng(OrigenLat, OrigenLong);

                var marker = new google.maps.Marker({
                    map: map, title: "T.Estimado: " + arrMinutos_Estim_CON_Predic_Tiempo[i] + " min", position: latlngset
                });
                map.setCenter(marker.getPosition())

                var content = '<div id="content">' +
                                '<div id="siteNotice">' +
                                '</div>' +
                                '<h1 id="firstHeading" class="firstHeading">Tiempo estimado : ' + arrMinutos_Estim_CON_Predic_Tiempo[i] + ' minutos</h1>' +
                                '<div id="bodyContent">' +
                                '<p>El tiempo estimado de este trayecto pertenece a la linea <b>' + arrIdLine[i] + '</b>, que tiene su inicio en ' +
                                ' la calle <b>' + arrName_First[i] + '</b> y finaliza en la calle <b>' + arrName_End[i] + '</b></p>' +
                                '<p>La fecha seleccionada es el <b>25/05/2017</b> a las <b>' + iHora + ':00</b> con una probabilidad de lluvia del <b>' + strPrevisionTiempo + '%</b></p>' +
                                '</div>' +
                                '</div>'

                var infowindow = new google.maps.InfoWindow()

                // Cuando realizamos un click sobre una marca se dispara la ventana emergente
                google.maps.event.addListener(marker, 'click', (function (marker, content, infowindow) {
                    return function () {
                        infowindow.setContent(content);
                        infowindow.open(map, marker);
                    };
                })(marker, content, infowindow));

            }
        }

    </script>
   </form>
</body>
</html>

