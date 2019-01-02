// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function changeMaxValue() {
    var val = document.getElementById("boolInputMinValue").value;
    document.getElementById("boolInputMaxValue").value = val;
}

/* Register map */
// Initialize and add the map
function initialMap() {
    var myLatlng = new google.maps.LatLng(42.69, 23.32);
    var myOptions = {
        zoom: 5,
        center: myLatlng
    }

    var map = new google.maps.Map(document.getElementById("registerMap"), myOptions);
    var geocoder = new google.maps.Geocoder();
    var marker;
    var latitudeInputValue = document.getElementById('latitude');
    var longitudeInputValue = document.getElementById('longtitude');
    var location;

    if (latitudeInputValue != "" && longitudeInputValue.value != "") {
        latitudeInputValue = Number.parseFloat(latitudeInputValue.value);
        longitudeInputValue = Number.parseFloat(longitudeInputValue.value);
        location = { lat: latitudeInputValue, lng: longitudeInputValue };
        marker = new google.maps.Marker({ position: location, map: map });
    }

    google.maps.event.addListener(map, 'click', function (event) {
        geocoder.geocode({
            'latLng': event.latLng
        }, function (results, status) {
            if (status == google.maps.GeocoderStatus.OK) {

                var splittedLatLang = String(event.latLng).replace('(', '').replace(')', '').replace(' ', '').split(',');
                latitudeInputValue = document.getElementById('latitude').value = splittedLatLang[0];
                longitudeInputValue = document.getElementById('longtitude').value = splittedLatLang[1];

                latitudeInputValue = Number.parseFloat(splittedLatLang[0]);
                longitudeInputValue = Number.parseFloat(splittedLatLang[1]);

                location = { lat: latitudeInputValue, lng: longitudeInputValue };

                if (marker == null) {
                    marker = new google.maps.Marker({ position: location, map: map });
                }
                else {
                    marker.setMap(null);
                    marker = new google.maps.Marker({ position: location, map: map });
                }
            }
        });
    });
}