﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MarkerUpdate.aspx.cs" Inherits="MarkerUpdate" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
   <script src="http://code.jquery.com/jquery-1.8.2.min.js"></script>
        <script type="text/javascript" src="http://maps.google.com/maps/api/js?v=3&sensor=false&language=en"></script>
        <script type="text/javascript">

            var map,
                currentPositionMarker,
                mapCenter = new google.maps.LatLng(40.700683, -73.925972),
                map;

            function initializeMap()
            {
                map = new google.maps.Map(document.getElementById('map_canvas'), {
                   zoom: 13,
                   center: mapCenter,
                   mapTypeId: google.maps.MapTypeId.ROADMAP
                 });
            }

            function locError(error) {
                // the current position could not be located
                alert("The current position could not be found!");
            }

            function setCurrentPosition(pos) {
                currentPositionMarker = new google.maps.Marker({
                    map: map,
                    position: new google.maps.LatLng(
                        pos.coords.latitude,
                        pos.coords.longitude
                    ),
                    title: "Current Position"
                });
                map.panTo(new google.maps.LatLng(
                        pos.coords.latitude,
                        pos.coords.longitude
                    ));
            }

            function displayAndWatch(position) {
                // set current position
                setCurrentPosition(position);
                // watch position
                watchCurrentPosition();
            }

            function watchCurrentPosition() {
                var positionTimer = navigator.geolocation.watchPosition(
                    function (position) {
                        setMarkerPosition(
                            currentPositionMarker,
                            position
                        );
                    });
            }

            function setMarkerPosition(marker, position) {
                marker.setPosition(
                    new google.maps.LatLng(
                        position.coords.latitude,
                        position.coords.longitude)
                );
            }

            function initLocationProcedure() {
                initializeMap();
                if (navigator.geolocation) {
                    navigator.geolocation.getCurrentPosition(displayAndWatch, locError);
                } else {
                    alert("Your browser does not support the Geolocation API");
                }
            }

            $(document).ready(function() {
                initLocationProcedure();
            });

        </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <div id="map_canvas" style="height:600px;"></div>
    </div>
    </form>
</body>
</html>
