<%@ Page Language="C#" AutoEventWireup="true" CodeFile="GeoCode.aspx.cs" Inherits="GeoCode" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Reverse Geocoding</title>
   <script src="http://maps.google.com/maps/api/js?sensor=false" type="text/javascript"></script>
    <script>
var geocoder;
var map;
var infowindow = new google.maps.InfoWindow();
var marker;
function initialize() {
  geocoder = new google.maps.Geocoder();
  var latlng = new google.maps.LatLng(40.730885,-73.997383);
  var mapOptions = {
    zoom: 8,
    center: latlng,
    mapTypeId: 'roadmap'
  }
  map = new google.maps.Map(document.getElementById('map-canvas'), mapOptions);
  var input = '10.013566,76.331549';
  var latlngStr = input.split(',', 2);
  var lat = parseFloat(latlngStr[0]);
  var lng = parseFloat(latlngStr[1]);
  var latlng = new google.maps.LatLng(lat, lng);
  geocoder.geocode({ 'latLng': latlng }, function(results, status) {
      if (status == google.maps.GeocoderStatus.OK) {
          if (results[1]) {
              map.setZoom(11);
              marker = new google.maps.Marker({
                  position: latlng,
                  map: map
              });
              infowindow.setContent(results[1].formatted_address);
              infowindow.open(map, marker);
          } else {
              alert('No results found');
          }
      } else {
          alert('Geocoder failed due to: ' + status);
      }
  });
}
google.maps.event.addDomListener(window, 'load', initialize);

    </script>
  </head>
<body>
    <form id="form1" runat="server">
    <div id="map-canvas" style="width:800px;height:800px;"></div>
    </form>
</body>
</html>
