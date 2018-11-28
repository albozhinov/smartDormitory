// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
var myLatlng = new google.maps.LatLng(42.6983, 23.32);
var myOptions = {
    zoom: 13,
    center: myLatlng
}
var map = new google.maps.Map(document.getElementById("mapps"), myOptions);
var geocoder = new google.maps.Geocoder();

google.maps.event.addListener(map, 'click', function (event) {
    geocoder.geocode({
        'latLng': event.latLng
    }, function (results, status) {
        if (status == google.maps.GeocoderStatus.OK) {
            var splittedLatLang = String(event.latLng).replace('(', '').replace(')', '').split(',');

            alert(splittedLatLang[0])
            alert(splittedLatLang[1])
            if (results[0]) {
                alert(results[0].formatted_address);
            }
        }
    });
});

