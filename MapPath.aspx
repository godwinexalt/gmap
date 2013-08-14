<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MapPath.aspx.cs" Inherits="MapPath" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="jquery-1.7.1.min.js" type="text/javascript"></script>
     <script src="http://maps.google.com/maps/api/js?sensor=false" type="text/javascript"></script>
     <script language="javascript">
         $(document).ready(function() {
             Plot();
         });

         function Plot() {
            
            $.ajax({
                type: "POST",
                url: "WebService.asmx/GetPath",
                contentType: "application/xml; charset=utf-8",
                dataType: "xml",
                success: function(xml) {
                //alert($(xml).find('Latitude').text());
                var polylineCoordinates = new Array();
                    $(xml).find('Location').each(function() {
                        //alert($(this).find('Latitude').text());
                    polylineCoordinates.push(new google.maps.LatLng($(this).find('Latitude').text(), $(this).find('Longitude').text()));
                    });
                    var center = new google.maps.LatLng(10.012869, 76.328802);
                    var myOptions = {
                        zoom: 18,
                        center: center,
                        mapTypeControl: true,
                        mapTypeControlOptions: { style: google.maps.MapTypeControlStyle.ROADMAP },
                        navigationControl: true,
                        mapTypeId: google.maps.MapTypeId.HYBRID
                    }
                    var map = new google.maps.Map(document.getElementById("map_canvas"), myOptions);


                    var polyline = new google.maps.Polyline({
                        path: polylineCoordinates,
                        strokeColor: '#FF0000',
                        strokeOpacity: 1.0,
                        strokeWeight: 2
                    });

                    polyline.setMap(map);
                }
            });
             setTimeout(function() { Plot(); }, 5000);
         }
  </script>
     
     <style>
     #map_canvas{ width:800px; height:600px; }
     </style>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <div id="map_canvas"></div>
    </div>
    </form>
</body>
</html>
