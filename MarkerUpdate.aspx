<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MarkerUpdate.aspx.cs" Inherits="MarkerUpdate" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <script src="jquery-1.7.1.min.js" type="text/javascript"></script>
    <script src="http://maps.google.com/maps/api/js?sensor=false" type="text/javascript"></script>
    
    <script language="javascript">
        $(document).ready(function() {
            SetCurrentPosition();
        });

        function SetCurrentPosition() {
            $.ajax({
                type: "POST",
                url: "WebService.asmx/GetCurrentPosition",
                contentType: "application/xml; charset=utf-8",
                dataType: "xml",
                success: function(xml) {
                    var myLatlng = new google.maps.LatLng($(xml).find('Latitude').text(), $(xml).find('Longitude').text());
                    var mapOptions = {
                        zoom: 10,
                        center: myLatlng,
                        mapTypeId: google.maps.MapTypeId.ROADMAP
                    }
                    var map = new google.maps.Map(document.getElementById('map'), mapOptions);

                    var marker = new google.maps.Marker({
                        position: myLatlng,
                        map: map,
                        title: 'Vehicle'
                    });

                    UpdateCurrentPosition(marker);
                }
            });
        }

        function UpdateCurrentPosition(marker) {
            $.ajax({
                type: "POST",
                url: "WebService.asmx/GetCurrentPosition",
                contentType: "application/xml; charset=utf-8",
                dataType: "xml",
                success: function(xml) {
                    var new_marker_position = new google.maps.LatLng($(xml).find('Latitude').text(), $(xml).find('Longitude').text());
                    marker.setPosition(new_marker_position);
                }
            });

            setTimeout(function() { UpdateCurrentPosition(marker); }, 5000);
        }    
  </script>
  
</head>
<body>
    <form id="form1" runat="server">
    <div style="width:900px;margin:0 auto;">
    <div id="map" style="width: 100%; height: 600px;"></div>
    </div>
    </form>
</body>
</html>
