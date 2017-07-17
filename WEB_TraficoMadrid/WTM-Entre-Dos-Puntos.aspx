<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WTM-Entre-Dos-Puntos.aspx.cs" Inherits="WEB_TraficoMadrid.WTM_Entre_Dos_Puntos" %>

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
                Previsión entre dos puntos
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
                <asp:DropDownList ID="ddlHora" runat="server" Width="100%" BackColor="#ffffff" ForeColor="#333333" CssClass="ddl" AutoPostBack="True" OnTextChanged="ddlHora_TextChanged" >
                    <asp:ListItem Value="00">--Selecciona Hora--</asp:ListItem>
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
                <br /><br />
                <asp:Label ID="lblRutaInicio"  runat="server" CssClass="label" Text="Ruta de Inicio: "></asp:Label>
                <asp:DropDownList ID="ddlRutaInicio" runat="server" Width="100%" BackColor="#ffffff" ForeColor="#333333" CssClass="ddl" AutoPostBack="True" OnTextChanged="ddlRutaInicio_TextChanged" >
                    
                </asp:DropDownList>
                <asp:Label ID="lblRutaFin"  runat="server" CssClass="label" Text="Ruta de Fin: "></asp:Label>
                <asp:DropDownList ID="ddlRutaFin" runat="server" Width="100%" BackColor="#ffffff" ForeColor="#333333" CssClass="ddl">
                    
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
    <!-- ENTRE DOS PUNTOS -->
    <script>
        
        function Entre_Dos_Puntos(strOrigenDestinoX, strOrigenDestinoY, strPuntosintermediosX, strPuntosintermediosY, strIdLine, strName_First, strName_End, strMinutos_Estim_CON_Predic_Tiempo, iHora, strPrevisionTiempo) {

            
            // Posición inicial en el mapa
            var directionsService = new google.maps.DirectionsService;
            var directionsDisplay = new google.maps.DirectionsRenderer;
            var map = new google.maps.Map(document.getElementById('ventana-estado-trafico'), {
                zoom: 7,
                center: { lat: 40.41, lng: -3.70 }
            });

            // Definimos las variables que vamos a utilizar en la función
            var arrOrigenDestinoX = strOrigenDestinoX.split('|');
            var arrOrigenDestinoY = strOrigenDestinoY.split('|');
            var arrPuntosintermediosX = strPuntosintermediosX.split('|');
            var arrPuntosintermediosY = strPuntosintermediosY.split('|');
            latlon = new Array(2);
            var x, y, zone, southhemi;
            x = parseFloat(arrOrigenDestinoX[0]);
            y = parseFloat(arrOrigenDestinoY[0]);
            zone = parseFloat("30");//por defecto España es 30
            southhemi = false;

            // Convertimos las coordenadas UTM en latitud y longitud
            UTMXYToLatLon(x, y, zone, southhemi, latlon);

            var OrigenLat = RadToDeg(latlon[0]);
            var OrigenLong = RadToDeg(latlon[1]);


            x = parseFloat(arrOrigenDestinoX[1]);
            y = parseFloat(arrOrigenDestinoY[1]);

            UTMXYToLatLon(x, y, zone, southhemi, latlon);

            var DestinoLat = RadToDeg(latlon[0]);
            var DestinoLong = RadToDeg(latlon[1]);


            // Por defecto en España la conversión de UTM a Lat/Long la Zona es 30 
            var strOrigen = new google.maps.LatLng(OrigenLat, OrigenLong);
            var strDestino = new google.maps.LatLng(DestinoLat, DestinoLong);


            var aPuntosintermedios = ""; 

            setMarkers(map, strOrigen, strDestino, strIdLine, strName_First, strName_End, strMinutos_Estim_CON_Predic_Tiempo, iHora, strPrevisionTiempo)

            directionsDisplay.setMap(map);

            // Llamamos a la función que calcula la ruta entre dos puntos seleccionados
            calculateAndDisplayRoute(directionsService, directionsDisplay, strOrigen, strDestino, arrPuntosintermediosX, arrPuntosintermediosY);

        }

        //Calculamos la ruta entre dos puntos
        function calculateAndDisplayRoute(directionsService, directionsDisplay, strOrigen, strDestino, arrPuntosintermediosX, arrPuntosintermediosY) {
            var waypts = [];
            var Lat;
            var Long;
            var checkboxArrayX = arrPuntosintermediosX;
            var checkboxArrayY = arrPuntosintermediosY;
            for (var i = 0; i < checkboxArrayX.length; i++) {
                x = parseFloat(checkboxArrayX[i]);
                y = parseFloat(checkboxArrayY[i]);
                zone = parseFloat("30");
                southhemi = false;

                UTMXYToLatLon(x, y, zone, southhemi, latlon);

                Lat = RadToDeg(latlon[0]);
                Long = RadToDeg(latlon[1]);

                waypts.push({
                    location: new google.maps.LatLng(Lat, Long),
                    stopover: true
                });
            }



            directionsService.route({
                origin: strOrigen,
                destination: strDestino,
                //waypoints: waypts,
                optimizeWaypoints: true,
                travelMode: 'DRIVING',
                drivingOptions: {
                    departureTime: new Date(Date.now()),
                    trafficModel: 'bestguess'
                },
                unitSystem: google.maps.UnitSystem.METRIC
            }, function (response, status) {
                if (status === 'OK') {
                    directionsDisplay.setDirections(response);
               } else {
                    window.alert('Directions request failed due to ' + status);
                }
            });
        }

        // Funcion que dispara la ventana emergente, al pinchar sobre la marca
        function setMarkers(map, strOrigen, strDestino, strIdLine, strName_First, strName_End, strMinutos_Estim_CON_Predic_Tiempo, iHora, strPrevisionTiempo) {

            var marker, i

            
                
                //latlngset = new google.maps.LatLng(OrigenLat, OrigenLong);

                var marker = new google.maps.Marker({
                    map: map, title: "T.Estimado: " + strMinutos_Estim_CON_Predic_Tiempo + " min", position: strDestino
                });
                map.setCenter(marker.getPosition())


                var content = '<div id="content">' +
                                '<div id="siteNotice">' +
                                '</div>' +
                                '<h1 id="firstHeading" class="firstHeading">Tiempo estimado : ' + strMinutos_Estim_CON_Predic_Tiempo + ' minutos</h1>' +
                                '<div id="bodyContent">' +
                                '<p>El tiempo estimado de este trayecto pertenece a la linea <b>' + strIdLine  + '</b>, que tiene su inicio en ' +
                                ' la calle <b>' + strName_First + '</b> y finaliza en la calle <b>' + strName_End + '</b></p>' +
                                '<p>La fecha seleccionada es el <b>25/05/2017</b> a las <b>' + iHora + ':00</b> con una probabilidad de lluvia del <b>' + strPrevisionTiempo + '%</b></p>' +
                                '</div>' +
                                '</div>'

                var infowindow = new google.maps.InfoWindow()

                google.maps.event.addListener(marker, 'click', (function (marker, content, infowindow) {
                    return function () {
                        infowindow.setContent(content);
                        infowindow.open(map, marker);
                    };
                })(marker, content, infowindow));

            }
        
        //Utilizado para el posicionamiento de las etiquetas -- Dejo el codigo pero no es utilizado en este ejemplo
        // Con esta función, permitimos que en vez de pasar longitud y latitud, podemos pasar directamente el nombre de la calle que deseamos posicionar
        function geocodeAddress(geocoder, resultsMap) {
            var address1 = "AV.ANDALUCIA-ALCOCER";
            geocoder.geocode({ 'address': address1 }, function (results, status) {
                if (status === 'OK') {
                    resultsMap.setCenter(results[0].geometry.location);
                    markerConLabel = new MarkerWithLabel({
                        position: results[0].geometry.location,
                        //title: "Etiqueta RMG",
                        labelContent: "Madrid",
                        labelAnchor: new google.maps.Point(50, 70),
                        labelClass: "labels", // the CSS class for the label
                        map: resultsMap
                    });
                } else {
                    alert('Geocode was not successful for the following reason: ' + status);
                }
            });

            var address2 = "MARIA DROC-SAN DALMACIO";
            geocoder.geocode({ 'address': address2 }, function (results, status) {
                if (status === 'OK') {
                    resultsMap.setCenter(results[0].geometry.location);
                    markerConLabel = new MarkerWithLabel({
                        position: results[0].geometry.location,
                        //title: "Etiqueta RMG",
                        labelContent: "Aranjuez",
                        labelAnchor: new google.maps.Point(50, 70),
                        labelClass: "labels", // the CSS class for the label
                        map: resultsMap
                    });
                } else {
                    alert('Geocode was not successful for the following reason: ' + status);
                }
            });
        }


       
    </script>
    
   </form>
    
</body>
</html>

