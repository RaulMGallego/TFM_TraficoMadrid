<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="WEB_TraficoMadrid.index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>TFM - Raúl Murillo</title>
    <link href="css/TFM_WebTraficoMadrid.css" rel="stylesheet" />
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
        .centrar1
	    {
		    position: absolute;
		    /*nos posicionamos en el centro del navegador*/
		    top:50%;
		    left:50%;
		    /*determinamos una anchura*/
		    width:800px;
		    /*indicamos que el margen izquierdo, es la mitad de la anchura*/
		    margin-left:-400px;
            margin-top:-200px;
		    /*determinamos una altura*/
		    height:100px;
		    /*indicamos que el margen superior, es la mitad de la altura*/
		    color: white;
            font-size: 70px;
            text-align: center;
	    }
        .centrar2
	    {
		    position: absolute;
		    /*nos posicionamos en el centro del navegador*/
		    top:50%;
		    left:50%;
		    /*determinamos una anchura*/
		    width:800px;
		    /*indicamos que el margen izquierdo, es la mitad de la anchura*/
		    margin-left:-400px;
            margin-top: 150px;
		    /*determinamos una altura*/
		    height:100px;
		    /*indicamos que el margen superior, es la mitad de la altura*/
		    color: white;
            text-align: center;
            font-size: 24px;
	    }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div class='centrar1'>
        Predicción del tráfico en Madrid tomando datos abiertos de entrada<br />
	</div>
    <div class='centrar2'>
        Trabajo Final de Master<br />
        Presentado por Raúl Murillo Gallego
	</div>
        <div>
    
        <ul>
      <li><a class="active" href="WTM-Marcado-Global.aspx">Previsión Global</a></li>
      <li><a href="WTM-Ventana-Emergente.aspx">Previsión con ventana emergente</a></li>
      <li><a href="WTM-Entre-Dos-Puntos.aspx">Previsión entre dos puntos</a></li>
    </ul>
    </div>
    </form>
</body>
</html>
