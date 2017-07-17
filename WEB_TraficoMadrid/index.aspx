<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="WEB_TraficoMadrid.index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>TFM - Raúl Murillo</title>
    <link href="css/prueba.css" rel="stylesheet" />
    <script type='text/javascript' src='https://maps.googleapis.com/maps/api/js?key=AIzaSyBTCjQLNz8HsNVD5Rvr3YEf4QqxK4Pr6G0'></script>
    <script src="js/MarkerWithLabel.js"></script>
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
    </style>
</head>
<body>
    <form id="form1" runat="server">
   
    <div id="div_pri_izq">
        <div class="div_seg_izq">
            <div id="ventana-estado-trafico" class="superior">
                
            </div>
            <div class="inferior">
                Previsión del tráfico
            </div>
        </div>
        
    </div>
    <div id="div_pri_der">
        <div class="div_seg_der">
            <div id="panel-prevision-tiempo" class="superior">
                
                <asp:Image ID="Image1" runat="server" ImageUrl="~/images/PrevisionTiempo.jpg" ImageAlign="Middle"/>
                
            </div>
            
            <div class="inferior">
               Previsión del tiempo
            </div>
        </div>
        
        <div class="div_seg_der">
            <div id="panel-introducir-datos" class="superior">
                <asp:Label ID="lblFecha"  runat="server" CssClass="label" Text="Fecha: "></asp:Label>
                <asp:TextBox ID="txtFecha" CssClass="input" runat="server" ></asp:TextBox><br /><br />
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
                <asp:Button ID="btnBuscar" runat="server" Text="Buscar >>" CssClass="button"  OnClick="btnBuscar_Click" />
                <br /><br />
            </div>
            
            <div class="inferior">
                Datos de entrada
            </div>
        </div>
           
           
    </div>
    <!-- ESTADO ACTUAL DEL TRAFICO-->
    <script>
        var locations = [
  ['loan 1', 33.890542, 151.274856, 'address 1'],
  ['loan 2', 33.923036, 151.259052, 'address 2'],
  ['loan 3', 34.028249, 151.157507, 'address 3'],
  ['loan 4', 33.80010128657071, 151.28747820854187, 'address 4'],
  ['loan 5', 33.950198, 151.259302, 'address 5']
        ];
        function VentanaTiempoIntermedios(strOrigenX, strOrigenY, strDestinoX, strDestinoY, strIdLine, strName_First, strName_End, strMinutos_Estim_CON_Predic_Tiempo) {

            //POSICION INICILA DE MAPA
            var map = new google.maps.Map(document.getElementById('ventana-estado-trafico'), {
                zoom: 13,
                center: { lat: 40.41, lng: -3.70 }
            });


            setMarkers(map, strOrigenX, strOrigenY, strDestinoX, strDestinoY, strIdLine, strName_First, strName_End, strMinutos_Estim_CON_Predic_Tiempo)

            //for (var i = 0; i < arrOrigenX.length; i++) {
            //    x = parseFloat(arrOrigenX[i]);
            //    y = parseFloat(arrOrigenY[i]);
            //    zone = parseFloat("30");
            //    southhemi = false;

            //    UTMXYToLatLon(x, y, zone, southhemi, latlon);

            //    var OrigenLat = RadToDeg(latlon[0]);
            //    var OrigenLong = RadToDeg(latlon[1]);


            //    //markerConLabel = new MarkerWithLabel({
            //    //    position: new google.maps.LatLng(OrigenLat, OrigenLong),
            //    //    //title: "Etiqueta RMG",
            //    //    labelContent: "Tiempo:" + arrMinutos_Estim_CON_Predic_Tiempo[i] + " min",
            //    //    labelAnchor: new google.maps.Point(50, 45),
            //    //    labelClass: "labels", // the CSS class for the label
            //    //    map: map
            //    //});



            //    var infowindow = new google.maps.InfoWindow({
            //        content: '<div id="content">' +
            //                    '<div id="siteNotice">' +
            //                    '</div>' +
            //                    '<h1 id="firstHeading" class="firstHeading">Tiempo estimado : ' + arrMinutos_Estim_CON_Predic_Tiempo[i] + ' minutos</h1>' +
            //                    '<div id="bodyContent">' +
            //                    '<p>El tiempo estimado de este trayecto pertenece a la linea <b>' + arrIdLine[i]  + '</b>, que tiene su inicio en ' +
            //                    ' la calle <b>' + arrName_First[i] + '</b> y finaliza en la calle <b>' + arrName_End[i] + '</b></p>' +
            //                    '<p>La fecha seleccionada es el <b>25/05/2017</b> a las <b>06:00</b> con una probabilidad de lluvia del <b>0%</b></p>' +
            //                    '</div>' +
            //                    '</div>'
            //    });

            //    var marker = new google.maps.Marker({
            //        position: new google.maps.LatLng(OrigenLat, OrigenLong),
            //        map: map,
            //        title: arrMinutos_Estim_CON_Predic_Tiempo[i] + ' minutos'
            //    });
            //    marker.addListener('click', function () {
            //        infowindow.open(map, marker);
            //    });
            // }

        }


        function setMarkers(map, strOrigenX, strOrigenY, strDestinoX, strDestinoY, strIdLine, strName_First, strName_End, strMinutos_Estim_CON_Predic_Tiempo) {

            var marker, i

            var arrOrigenX = strOrigenX.split('|');
            var arrOrigenY = strOrigenY.split('|');
            var arrIdLine = strIdLine.split('|');
            var arrName_First = strName_First.split('|');
            var arrName_End = strName_End.split('|');
            var arrMinutos_Estim_CON_Predic_Tiempo = strMinutos_Estim_CON_Predic_Tiempo.split('|');

            latlon = new Array(2);
            var x, y, zone, southhemi;


            for (i = 0; i < arrOrigenX.length; i++) {

                x = parseFloat(arrOrigenX[i]);
                y = parseFloat(arrOrigenY[i]);
                zone = parseFloat("30");
                southhemi = false;

                UTMXYToLatLon(x, y, zone, southhemi, latlon);

                var OrigenLat = RadToDeg(latlon[0]);
                var OrigenLong = RadToDeg(latlon[1]);

                latlngset = new google.maps.LatLng(OrigenLat, OrigenLong);

                var marker = new google.maps.Marker({
                    map: map, title: "xx", position: latlngset
                });
                map.setCenter(marker.getPosition())


                var content = '<div id="content">' +
                                '<div id="siteNotice">' +
                                '</div>' +
                                '<h1 id="firstHeading" class="firstHeading">Tiempo estimado : ' + arrMinutos_Estim_CON_Predic_Tiempo[i] + ' minutos</h1>' +
                                '<div id="bodyContent">' +
                                '<p>El tiempo estimado de este trayecto pertenece a la linea <b>' + arrIdLine[i] + '</b>, que tiene su inicio en ' +
                                ' la calle <b>' + arrName_First[i] + '</b> y finaliza en la calle <b>' + arrName_End[i] + '</b></p>' +
                                '<p>La fecha seleccionada es el <b>23/05/2017</b> a las <b>06:00</b> con una probabilidad de lluvia del <b>0%</b></p>' +
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
        }





















        function initMap(strOrigenDestinoX, strOrigenDestinoY, strPuntosintermediosX, strPuntosintermediosY) {

            //ESTADO ACTUAL TRAFICO
            //var map_estado_trafico_actual = new google.maps.Map(document.getElementById('panel-estado-trafico-actual'), {
            //    zoom: 13,
            //    center: { lat: 40.41, lng: -3.70 }
            //});

            //var traffic_map_estado_trafico_actual = new google.maps.TrafficLayer();
            //traffic_map_estado_trafico_actual.setMap(map_estado_trafico_actual);

            //MAPA TRAFICO
            var directionsService = new google.maps.DirectionsService;
            var directionsDisplay = new google.maps.DirectionsRenderer;
            var map = new google.maps.Map(document.getElementById('panel-estado-trafico'), {
                zoom: 7,
                center: { lat: 40.41, lng: -3.70 }
            });

            //posicion del mapa
            //var posicionCentro = new google.maps.LatLng(40.41, -3.70);
            //var strOrigen = "Madrid";
            //var strDestino = "Aranjuez";



            var arrOrigenDestinoX = strOrigenDestinoX.split('|');
            var arrOrigenDestinoY = strOrigenDestinoY.split('|');
            var arrPuntosintermediosX = strPuntosintermediosX.split('|');
            var arrPuntosintermediosY = strPuntosintermediosY.split('|');

            latlon = new Array(2);
            var x, y, zone, southhemi;
            x = parseFloat(arrOrigenDestinoX[0]);
            y = parseFloat(arrOrigenDestinoY[0]);
            zone = parseFloat("30");
            southhemi = false;

            UTMXYToLatLon(x, y, zone, southhemi, latlon);

            var OrigenLat = RadToDeg(latlon[0]);
            var OrigenLong = RadToDeg(latlon[1]);


            x = parseFloat(arrOrigenDestinoX[1]);
            y = parseFloat(arrOrigenDestinoY[1]);

            UTMXYToLatLon(x, y, zone, southhemi, latlon);

            var DestinoLat = RadToDeg(latlon[0]);
            var DestinoLong = RadToDeg(latlon[1]);


            // Por defecto en España la conversión de UTM a Lat/Long la Zona es 30 
            //  var strOrigen = new google.maps.LatLng('40.422364712803635', '-3.6872329677769513');
            var strOrigen = new google.maps.LatLng(OrigenLat, OrigenLong);
            //var strDestino = new google.maps.LatLng('40.42218058795796', '-3.664796333480063');
            var strDestino = new google.maps.LatLng(DestinoLat, DestinoLong);

            var aPuntosintermedios = ""; //aPuntosintermedios.split('|');

            //var trafficLayer = new google.maps.TrafficLayer();
            //trafficLayer.setMap(map);
            directionsDisplay.setMap(map);


            //calculateAndDisplayRoute(directionsService, directionsDisplay, strOrigen, strDestino, arrPuntosintermediosX, arrPuntosintermediosY);

            //var geocoder = new google.maps.Geocoder();
            //geocodeAddress(geocoder, map);
            markerConLabel = new MarkerWithLabel({
                position: new google.maps.LatLng(OrigenLat, OrigenLong),
                //title: "Etiqueta RMG",
                labelContent: "MadridXXXX",
                labelAnchor: new google.maps.Point(50, 70),
                labelClass: "labels", // the CSS class for the label
                map: map
            });
        }

        function calculateAndDisplayRoute(directionsService, directionsDisplay, strOrigen, strDestino, arrPuntosintermediosX, arrPuntosintermediosY) {
            var waypts = [];
            //latlon = new Array(2);
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
                    //location: checkboxArray[i],
                    location: new google.maps.LatLng(Lat, Long),
                    stopover: true
                });
            }



            directionsService.route({
                origin: strOrigen,
                destination: strDestino,
                waypoints: waypts,
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

                    var route = response.routes[0];
                    var chkArray = document.getElementById('waypoints');
                    var summaryPanel = document.getElementById('directions-panel');
                    summaryPanel.innerHTML = '';
                    // For each route, display summary information.
                    for (var i = 0; i < route.legs.length; i++) {
                        var routeSegment = i + 1;
                        summaryPanel.innerHTML += '<b>Route Segment: ' + routeSegment +
                            '</b><br>';
                        summaryPanel.innerHTML += route.legs[i].start_address + ' to ';
                        summaryPanel.innerHTML += route.legs[i].end_address + '<br>';
                        summaryPanel.innerHTML += chkArray[i].value + '<br>'; //Añadimos el tiempo
                        summaryPanel.innerHTML += route.legs[i].distance.text + '<br><br>';
                    }
                } else {
                    window.alert('Directions request failed due to ' + status);
                }
            });
        }



        // Aqui pasamos un array y hacemos un for



        //Utilizado para el posicionamiento de las etiquetas
        function geocodeAddress(geocoder, resultsMap) {
            var address1 = "Madrid";
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

            var address2 = "Aranjuez";
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


        var pi = 3.14159265358979;

        /* Ellipsoid model constants (actual values here are for WGS84) */
        var sm_a = 6378137.0;
        var sm_b = 6356752.314;
        var sm_EccSquared = 6.69437999013e-03;

        var UTMScaleFactor = 0.9996;


        /*
        * DegToRad
        *
        * Converts degrees to radians.
        *
        */
        function DegToRad(deg) {
            return (deg / 180.0 * pi)
        }




        /*
        * RadToDeg
        *
        * Converts radians to degrees.
        *
        */
        function RadToDeg(rad) {
            return (rad / pi * 180.0)
        }




        /*
        * ArcLengthOfMeridian
        *
        * Computes the ellipsoidal distance from the equator to a point at a
        * given latitude.
        *
        * Reference: Hoffmann-Wellenhof, B., Lichtenegger, H., and Collins, J.,
        * GPS: Theory and Practice, 3rd ed.  New York: Springer-Verlag Wien, 1994.
        *
        * Inputs:
        *     phi - Latitude of the point, in radians.
        *
        * Globals:
        *     sm_a - Ellipsoid model major axis.
        *     sm_b - Ellipsoid model minor axis.
        *
        * Returns:
        *     The ellipsoidal distance of the point from the equator, in meters.
        *
        */
        function ArcLengthOfMeridian(phi) {
            var alpha, beta, gamma, delta, epsilon, n;
            var result;

            /* Precalculate n */
            n = (sm_a - sm_b) / (sm_a + sm_b);

            /* Precalculate alpha */
            alpha = ((sm_a + sm_b) / 2.0)
               * (1.0 + (Math.pow(n, 2.0) / 4.0) + (Math.pow(n, 4.0) / 64.0));

            /* Precalculate beta */
            beta = (-3.0 * n / 2.0) + (9.0 * Math.pow(n, 3.0) / 16.0)
               + (-3.0 * Math.pow(n, 5.0) / 32.0);

            /* Precalculate gamma */
            gamma = (15.0 * Math.pow(n, 2.0) / 16.0)
                + (-15.0 * Math.pow(n, 4.0) / 32.0);

            /* Precalculate delta */
            delta = (-35.0 * Math.pow(n, 3.0) / 48.0)
                + (105.0 * Math.pow(n, 5.0) / 256.0);

            /* Precalculate epsilon */
            epsilon = (315.0 * Math.pow(n, 4.0) / 512.0);

            /* Now calculate the sum of the series and return */
            result = alpha
                * (phi + (beta * Math.sin(2.0 * phi))
                    + (gamma * Math.sin(4.0 * phi))
                    + (delta * Math.sin(6.0 * phi))
                    + (epsilon * Math.sin(8.0 * phi)));

            return result;
        }



        /*
        * UTMCentralMeridian
        *
        * Determines the central meridian for the given UTM zone.
        *
        * Inputs:
        *     zone - An integer value designating the UTM zone, range [1,60].
        *
        * Returns:
        *   The central meridian for the given UTM zone, in radians, or zero
        *   if the UTM zone parameter is outside the range [1,60].
        *   Range of the central meridian is the radian equivalent of [-177,+177].
        *
        */
        function UTMCentralMeridian(zone) {
            var cmeridian;

            cmeridian = DegToRad(-183.0 + (zone * 6.0));

            return cmeridian;
        }



        /*
        * FootpointLatitude
        *
        * Computes the footpoint latitude for use in converting transverse
        * Mercator coordinates to ellipsoidal coordinates.
        *
        * Reference: Hoffmann-Wellenhof, B., Lichtenegger, H., and Collins, J.,
        *   GPS: Theory and Practice, 3rd ed.  New York: Springer-Verlag Wien, 1994.
        *
        * Inputs:
        *   y - The UTM northing coordinate, in meters.
        *
        * Returns:
        *   The footpoint latitude, in radians.
        *
        */
        function FootpointLatitude(y) {
            var y_, alpha_, beta_, gamma_, delta_, epsilon_, n;
            var result;

            /* Precalculate n (Eq. 10.18) */
            n = (sm_a - sm_b) / (sm_a + sm_b);

            /* Precalculate alpha_ (Eq. 10.22) */
            /* (Same as alpha in Eq. 10.17) */
            alpha_ = ((sm_a + sm_b) / 2.0)
                * (1 + (Math.pow(n, 2.0) / 4) + (Math.pow(n, 4.0) / 64));

            /* Precalculate y_ (Eq. 10.23) */
            y_ = y / alpha_;

            /* Precalculate beta_ (Eq. 10.22) */
            beta_ = (3.0 * n / 2.0) + (-27.0 * Math.pow(n, 3.0) / 32.0)
                + (269.0 * Math.pow(n, 5.0) / 512.0);

            /* Precalculate gamma_ (Eq. 10.22) */
            gamma_ = (21.0 * Math.pow(n, 2.0) / 16.0)
                + (-55.0 * Math.pow(n, 4.0) / 32.0);

            /* Precalculate delta_ (Eq. 10.22) */
            delta_ = (151.0 * Math.pow(n, 3.0) / 96.0)
                + (-417.0 * Math.pow(n, 5.0) / 128.0);

            /* Precalculate epsilon_ (Eq. 10.22) */
            epsilon_ = (1097.0 * Math.pow(n, 4.0) / 512.0);

            /* Now calculate the sum of the series (Eq. 10.21) */
            result = y_ + (beta_ * Math.sin(2.0 * y_))
                + (gamma_ * Math.sin(4.0 * y_))
                + (delta_ * Math.sin(6.0 * y_))
                + (epsilon_ * Math.sin(8.0 * y_));

            return result;
        }



        /*
        * MapLatLonToXY
        *
        * Converts a latitude/longitude pair to x and y coordinates in the
        * Transverse Mercator projection.  Note that Transverse Mercator is not
        * the same as UTM; a scale factor is required to convert between them.
        *
        * Reference: Hoffmann-Wellenhof, B., Lichtenegger, H., and Collins, J.,
        * GPS: Theory and Practice, 3rd ed.  New York: Springer-Verlag Wien, 1994.
        *
        * Inputs:
        *    phi - Latitude of the point, in radians.
        *    lambda - Longitude of the point, in radians.
        *    lambda0 - Longitude of the central meridian to be used, in radians.
        *
        * Outputs:
        *    xy - A 2-element array containing the x and y coordinates
        *         of the computed point.
        *
        * Returns:
        *    The function does not return a value.
        *
        */
        function MapLatLonToXY(phi, lambda, lambda0, xy) {
            var N, nu2, ep2, t, t2, l;
            var l3coef, l4coef, l5coef, l6coef, l7coef, l8coef;
            var tmp;

            /* Precalculate ep2 */
            ep2 = (Math.pow(sm_a, 2.0) - Math.pow(sm_b, 2.0)) / Math.pow(sm_b, 2.0);

            /* Precalculate nu2 */
            nu2 = ep2 * Math.pow(Math.cos(phi), 2.0);

            /* Precalculate N */
            N = Math.pow(sm_a, 2.0) / (sm_b * Math.sqrt(1 + nu2));

            /* Precalculate t */
            t = Math.tan(phi);
            t2 = t * t;
            tmp = (t2 * t2 * t2) - Math.pow(t, 6.0);

            /* Precalculate l */
            l = lambda - lambda0;

            /* Precalculate coefficients for l**n in the equations below
               so a normal human being can read the expressions for easting
               and northing
               -- l**1 and l**2 have coefficients of 1.0 */
            l3coef = 1.0 - t2 + nu2;

            l4coef = 5.0 - t2 + 9 * nu2 + 4.0 * (nu2 * nu2);

            l5coef = 5.0 - 18.0 * t2 + (t2 * t2) + 14.0 * nu2
                - 58.0 * t2 * nu2;

            l6coef = 61.0 - 58.0 * t2 + (t2 * t2) + 270.0 * nu2
                - 330.0 * t2 * nu2;

            l7coef = 61.0 - 479.0 * t2 + 179.0 * (t2 * t2) - (t2 * t2 * t2);

            l8coef = 1385.0 - 3111.0 * t2 + 543.0 * (t2 * t2) - (t2 * t2 * t2);

            /* Calculate easting (x) */
            xy[0] = N * Math.cos(phi) * l
                + (N / 6.0 * Math.pow(Math.cos(phi), 3.0) * l3coef * Math.pow(l, 3.0))
                + (N / 120.0 * Math.pow(Math.cos(phi), 5.0) * l5coef * Math.pow(l, 5.0))
                + (N / 5040.0 * Math.pow(Math.cos(phi), 7.0) * l7coef * Math.pow(l, 7.0));

            /* Calculate northing (y) */
            xy[1] = ArcLengthOfMeridian(phi)
                + (t / 2.0 * N * Math.pow(Math.cos(phi), 2.0) * Math.pow(l, 2.0))
                + (t / 24.0 * N * Math.pow(Math.cos(phi), 4.0) * l4coef * Math.pow(l, 4.0))
                + (t / 720.0 * N * Math.pow(Math.cos(phi), 6.0) * l6coef * Math.pow(l, 6.0))
                + (t / 40320.0 * N * Math.pow(Math.cos(phi), 8.0) * l8coef * Math.pow(l, 8.0));

            return;
        }



        /*
        * MapXYToLatLon
        *
        * Converts x and y coordinates in the Transverse Mercator projection to
        * a latitude/longitude pair.  Note that Transverse Mercator is not
        * the same as UTM; a scale factor is required to convert between them.
        *
        * Reference: Hoffmann-Wellenhof, B., Lichtenegger, H., and Collins, J.,
        *   GPS: Theory and Practice, 3rd ed.  New York: Springer-Verlag Wien, 1994.
        *
        * Inputs:
        *   x - The easting of the point, in meters.
        *   y - The northing of the point, in meters.
        *   lambda0 - Longitude of the central meridian to be used, in radians.
        *
        * Outputs:
        *   philambda - A 2-element containing the latitude and longitude
        *               in radians.
        *
        * Returns:
        *   The function does not return a value.
        *
        * Remarks:
        *   The local variables Nf, nuf2, tf, and tf2 serve the same purpose as
        *   N, nu2, t, and t2 in MapLatLonToXY, but they are computed with respect
        *   to the footpoint latitude phif.
        *
        *   x1frac, x2frac, x2poly, x3poly, etc. are to enhance readability and
        *   to optimize computations.
        *
        */
        function MapXYToLatLon(x, y, lambda0, philambda) {
            var phif, Nf, Nfpow, nuf2, ep2, tf, tf2, tf4, cf;
            var x1frac, x2frac, x3frac, x4frac, x5frac, x6frac, x7frac, x8frac;
            var x2poly, x3poly, x4poly, x5poly, x6poly, x7poly, x8poly;

            /* Get the value of phif, the footpoint latitude. */
            phif = FootpointLatitude(y);

            /* Precalculate ep2 */
            ep2 = (Math.pow(sm_a, 2.0) - Math.pow(sm_b, 2.0))
                  / Math.pow(sm_b, 2.0);

            /* Precalculate cos (phif) */
            cf = Math.cos(phif);

            /* Precalculate nuf2 */
            nuf2 = ep2 * Math.pow(cf, 2.0);

            /* Precalculate Nf and initialize Nfpow */
            Nf = Math.pow(sm_a, 2.0) / (sm_b * Math.sqrt(1 + nuf2));
            Nfpow = Nf;

            /* Precalculate tf */
            tf = Math.tan(phif);
            tf2 = tf * tf;
            tf4 = tf2 * tf2;

            /* Precalculate fractional coefficients for x**n in the equations
               below to simplify the expressions for latitude and longitude. */
            x1frac = 1.0 / (Nfpow * cf);

            Nfpow *= Nf;   /* now equals Nf**2) */
            x2frac = tf / (2.0 * Nfpow);

            Nfpow *= Nf;   /* now equals Nf**3) */
            x3frac = 1.0 / (6.0 * Nfpow * cf);

            Nfpow *= Nf;   /* now equals Nf**4) */
            x4frac = tf / (24.0 * Nfpow);

            Nfpow *= Nf;   /* now equals Nf**5) */
            x5frac = 1.0 / (120.0 * Nfpow * cf);

            Nfpow *= Nf;   /* now equals Nf**6) */
            x6frac = tf / (720.0 * Nfpow);

            Nfpow *= Nf;   /* now equals Nf**7) */
            x7frac = 1.0 / (5040.0 * Nfpow * cf);

            Nfpow *= Nf;   /* now equals Nf**8) */
            x8frac = tf / (40320.0 * Nfpow);

            /* Precalculate polynomial coefficients for x**n.
               -- x**1 does not have a polynomial coefficient. */
            x2poly = -1.0 - nuf2;

            x3poly = -1.0 - 2 * tf2 - nuf2;

            x4poly = 5.0 + 3.0 * tf2 + 6.0 * nuf2 - 6.0 * tf2 * nuf2
                - 3.0 * (nuf2 * nuf2) - 9.0 * tf2 * (nuf2 * nuf2);

            x5poly = 5.0 + 28.0 * tf2 + 24.0 * tf4 + 6.0 * nuf2 + 8.0 * tf2 * nuf2;

            x6poly = -61.0 - 90.0 * tf2 - 45.0 * tf4 - 107.0 * nuf2
                + 162.0 * tf2 * nuf2;

            x7poly = -61.0 - 662.0 * tf2 - 1320.0 * tf4 - 720.0 * (tf4 * tf2);

            x8poly = 1385.0 + 3633.0 * tf2 + 4095.0 * tf4 + 1575 * (tf4 * tf2);

            /* Calculate latitude */
            philambda[0] = phif + x2frac * x2poly * (x * x)
                + x4frac * x4poly * Math.pow(x, 4.0)
                + x6frac * x6poly * Math.pow(x, 6.0)
                + x8frac * x8poly * Math.pow(x, 8.0);

            /* Calculate longitude */
            philambda[1] = lambda0 + x1frac * x
                + x3frac * x3poly * Math.pow(x, 3.0)
                + x5frac * x5poly * Math.pow(x, 5.0)
                + x7frac * x7poly * Math.pow(x, 7.0);

            return;
        }




        /*
        * LatLonToUTMXY
        *
        * Converts a latitude/longitude pair to x and y coordinates in the
        * Universal Transverse Mercator projection.
        *
        * Inputs:
        *   lat - Latitude of the point, in radians.
        *   lon - Longitude of the point, in radians.
        *   zone - UTM zone to be used for calculating values for x and y.
        *          If zone is less than 1 or greater than 60, the routine
        *          will determine the appropriate zone from the value of lon.
        *
        * Outputs:
        *   xy - A 2-element array where the UTM x and y values will be stored.
        *
        * Returns:
        *   The UTM zone used for calculating the values of x and y.
        *
        */
        function LatLonToUTMXY(lat, lon, zone, xy) {
            MapLatLonToXY(lat, lon, UTMCentralMeridian(zone), xy);

            /* Adjust easting and northing for UTM system. */
            xy[0] = xy[0] * UTMScaleFactor + 500000.0;
            xy[1] = xy[1] * UTMScaleFactor;
            if (xy[1] < 0.0)
                xy[1] = xy[1] + 10000000.0;

            return zone;
        }



        /*
        * UTMXYToLatLon
        *
        * Converts x and y coordinates in the Universal Transverse Mercator
        * projection to a latitude/longitude pair.
        *
        * Inputs:
        *	x - The easting of the point, in meters.
        *	y - The northing of the point, in meters.
        *	zone - The UTM zone in which the point lies.
        *	southhemi - True if the point is in the southern hemisphere;
        *               false otherwise.
        *
        * Outputs:
        *	latlon - A 2-element array containing the latitude and
        *            longitude of the point, in radians.
        *
        * Returns:
        *	The function does not return a value.
        *
        */
        function UTMXYToLatLon(x, y, zone, southhemi, latlon) {
            var cmeridian;

            x -= 500000.0;
            x /= UTMScaleFactor;

            /* If in southern hemisphere, adjust y accordingly. */
            if (southhemi)
                y -= 10000000.0;

            y /= UTMScaleFactor;

            cmeridian = UTMCentralMeridian(zone);
            MapXYToLatLon(x, y, cmeridian, latlon);

            return;
        }


    </script>
    
   </form>
    
</body>
</html>

