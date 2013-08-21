<%@ Page Language="C#" AutoEventWireup="true"  CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
   <title>Google Maps Multiple Markers</title> 

    <script src="jquery-1.7.1.min.js" type="text/javascript"></script>
  <script src="http://maps.google.com/maps/api/js?sensor=false" type="text/javascript"></script>
  <script language="javascript">
      $(document).ready(function() {
          $.ajax({
              type: "POST",
              url: "WebService.asmx/GetCurrentPosition",
              contentType: "application/xml; charset=utf-8",
              dataType: "xml",
              success: function(xml) {
                 // alert($(xml).find('Lat').text());
              }
          });
      });
  </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <div id="map" style="width: 500px; height: 400px;"></div>

  <script type="text/javascript">
      var locations = [
['19.0018014', '72.914170300'],
['18.4915595', '73.886065199'],
['17.5272371', '75.957780299'],
['19.0787678', '75.752507899'],
['19.9971895', '73.916395800'],
['21.16536', '72.79387']
];

      var map;
      var NewLatLng = [];
      var markers = [];
      var marker;
      function initialize() {
          var latlng = new google.maps.LatLng(21.16536, 72.79387);
          var myOptions = {
              zoom: 5,
              center: latlng,
              mapTypeId: google.maps.MapTypeId.ROADMAP
          };
          map = new google.maps.Map(document.getElementById("map"), myOptions);
          map = new google.maps.Map(document.getElementById("map"), myOptions);
          marker = new google.maps.Marker(
        {
            position: new google.maps.LatLng(21.1673643, 72.7851802),
            map: map,
            title: 'Click me'
        });
          markers.push(marker);
          $.each(locations, function(i, item) {
              NewLatLng.push(new google.maps.LatLng(item[0], item[1]));
          });
          var infowindow = new google.maps.InfoWindow({
              content: 'Location info:<br/>Movable Marker !!'

          });
          google.maps.event.addListener(marker, 'click', function() {
              infowindow.open(map, marker);
              setTimeout(function() { infowindow.close(); }, '5000');
          });
          $.each(NewLatLng, function(i, item) {
              setTimeout(function() {
                  marker.setPosition(item);

                  markers.push(marker);

              }, i * 3000);

          });

      }
      window.onload = initialize;
  </script>
    </div>
    </form>
</body>
</html>
