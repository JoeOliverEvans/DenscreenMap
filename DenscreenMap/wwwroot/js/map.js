
let latitude = 54.9;
let longitude = -4.8449;

let map = false;
let marker = false;
let markers = [];

let latestPosition = {lat: 54.9, lng: -4.8449}
let oldId = 1;

function initMap() {
    let center = {lat: latitude, lng: longitude};

    map = new google.maps.Map(document.getElementById('map'), {zoom:6.5, center: center, disableDefaultUI: true, gestureHandling: 'cooperative', maxZoom: '8', styles: [
            {
                "elementType": "geometry",
                "stylers": [
                    {
                        "color": "#f5f5f5"
                    }
                ]
            },
            {
                "elementType": "labels.icon",
                "stylers": [
                    {
                        "visibility": "off"
                    }
                ]
            },
            {
                "elementType": "labels.text.fill",
                "stylers": [
                    {
                        "color": "#616161"
                    }
                ]
            },
            {
                "elementType": "labels.text.stroke",
                "stylers": [
                    {
                        "color": "#f5f5f5"
                    }
                ]
            },
            {
                "featureType": "administrative.land_parcel",
                "elementType": "labels.text.fill",
                "stylers": [
                    {
                        "color": "#bdbdbd"
                    }
                ]
            },
            {
                "featureType": "poi",
                "elementType": "geometry",
                "stylers": [
                    {
                        "color": "#eeeeee"
                    }
                ]
            },
            {
                "featureType": "poi",
                "elementType": "labels.text.fill",
                "stylers": [
                    {
                        "color": "#757575"
                    }
                ]
            },
            {
                "featureType": "poi.park",
                "elementType": "geometry",
                "stylers": [
                    {
                        "color": "#e5e5e5"
                    }
                ]
            },
            {
                "featureType": "poi.park",
                "elementType": "labels.text.fill",
                "stylers": [
                    {
                        "color": "#9e9e9e"
                    }
                ]
            },
            {
                "featureType": "road",
                "elementType": "geometry",
                "stylers": [
                    {
                        "color": "#ffffff"
                    }
                ]
            },
            {
                "featureType": "road.arterial",
                "elementType": "labels.text.fill",
                "stylers": [
                    {
                        "color": "#757575"
                    }
                ]
            },
            {
                "featureType": "road.highway",
                "elementType": "geometry",
                "stylers": [
                    {
                        "color": "#dadada"
                    }
                ]
            },
            {
                "featureType": "road.highway",
                "elementType": "labels.text.fill",
                "stylers": [
                    {
                        "color": "#616161"
                    }
                ]
            },
            {
                "featureType": "road.local",
                "elementType": "labels.text.fill",
                "stylers": [
                    {
                        "color": "#9e9e9e"
                    }
                ]
            },
            {
                "featureType": "transit.line",
                "elementType": "geometry",
                "stylers": [
                    {
                        "color": "#e5e5e5"
                    }
                ]
            },
            {
                "featureType": "transit.station",
                "elementType": "geometry",
                "stylers": [
                    {
                        "color": "#eeeeee"
                    }
                ]
            },
            {
                "featureType": "water",
                "elementType": "geometry",
                "stylers": [
                    {
                        "color": "#c9c9c9"
                    }
                ]
            },
            {
                "featureType": "water",
                "elementType": "labels.text.fill",
                "stylers": [
                    {
                        "color": "#9e9e9e"
                    }
                ]
            }
        ]});
    
    startupMap();
}

function startupMap() {
    $.ajax({
        url: `/api/startup`,
        method: 'GET',
        success: function(result) {
            if (result.status == "success"){

                result.markers.forEach(addToMarkers);

                let lastIndex = result.markers.length;
                oldId = result.markers[lastIndex - 1].id;

                function addToMarkers(item){
                    var icon = {
                        url: item.colour,
                        scaledSize: new google.maps.Size(60,60),
                        anchor: new google.maps.Point(30, 30)
                    }
                    if ((item.colour == "https://images.squarespace-cdn.com/content/v1/5ea969a9c6ec5a41d0623807/1597755800745-MHN0I1AN6X6QZHR7BEXG/ke17ZwdGBToddI8pDm48kAf-OpKpNsh_OjjU8JOdDKBZw-zPPgdn4jUwVcJE1ZvWQUxwkmyExglNqGp0IvTJZUJFbgE-7XRK3dMEBRBhUpxq_kr5JmUSWzdbeQhVZ8KGVEAfgr0ybhloMHNIqvT8SMftTfHgE5YY4gbHxAHibYY/DenScreen-Map-GIF-green.gif") || (item.colour == "https://images.squarespace-cdn.com/content/v1/5ea969a9c6ec5a41d0623807/1597755387587-85SSSTGQB6636UZTZBEH/ke17ZwdGBToddI8pDm48kAf-OpKpNsh_OjjU8JOdDKBZw-zPPgdn4jUwVcJE1ZvWQUxwkmyExglNqGp0IvTJZUJFbgE-7XRK3dMEBRBhUpxq_kr5JmUSWzdbeQhVZ8KGVEAfgr0ybhloMHNIqvT8SMftTfHgE5YY4gbHxAHibYY/DenScreen-Map-GIF-red5.gif")){
                        let currentMarker = new google.maps.Marker({
                            position: {lat: item.lat, lng: item.lng},
                            zIndex: 100,
                            map: map,
                            title: item.timeStamp,
                            icon: icon,
                            optimized: false,
                        })
                        markers.push(currentMarker)
                    }
                    else {
                        let currentMarker = new google.maps.Marker({
                            position: {lat: item.lat, lng: item.lng},
                            zIndex: 10,
                            map: map,
                            title: item.timeStamp,
                            icon: icon,
                            optimized: false,
                        })
                        markers.push(currentMarker)
                    }
                }
                console.log(result.message);
            }
            else {
                console.log(result.message);
            }
        },
        error: function(result) {
            console.log(result.message);
        }
    })
}

const updateMap = function(lat, lng, timeStamp, colour) {
    latitude = (lat * 1);
    longitude = (lng * 1);
    var icon = {
        url: colour,
        scaledSize: new google.maps.Size(60,60),
        anchor: new google.maps.Point(30, 30)
    }
    if ((colour == "https://images.squarespace-cdn.com/content/v1/5ea969a9c6ec5a41d0623807/1597755800745-MHN0I1AN6X6QZHR7BEXG/ke17ZwdGBToddI8pDm48kAf-OpKpNsh_OjjU8JOdDKBZw-zPPgdn4jUwVcJE1ZvWQUxwkmyExglNqGp0IvTJZUJFbgE-7XRK3dMEBRBhUpxq_kr5JmUSWzdbeQhVZ8KGVEAfgr0ybhloMHNIqvT8SMftTfHgE5YY4gbHxAHibYY/DenScreen-Map-GIF-green.gif") || (colour == "https://images.squarespace-cdn.com/content/v1/5ea969a9c6ec5a41d0623807/1597755387587-85SSSTGQB6636UZTZBEH/ke17ZwdGBToddI8pDm48kAf-OpKpNsh_OjjU8JOdDKBZw-zPPgdn4jUwVcJE1ZvWQUxwkmyExglNqGp0IvTJZUJFbgE-7XRK3dMEBRBhUpxq_kr5JmUSWzdbeQhVZ8KGVEAfgr0ybhloMHNIqvT8SMftTfHgE5YY4gbHxAHibYY/DenScreen-Map-GIF-red5.gif")){
        let currentMarker = new google.maps.Marker({
            position: {lat: latitude, lng: longitude},
            zIndex: 100,
            map: map,
            title: timeStamp,
            icon: icon,
            optimized: false,
        })
        markers.push(currentMarker)
    }
    else {
        let currentMarker = new google.maps.Marker({
            position: {lat: latitude, lng: longitude},
            zIndex: 10,
            map: map,
            title: timeStamp,
            icon: icon,
            optimized: false,
        })
        markers.push(currentMarker)
    }
}

var checkingForNewForm = setInterval(checkForNewForm, 7000);

function checkForNewForm() {

    $.ajax({
        url: `/api/check`,
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        data: JSON.stringify({
            oldId
        }),
        success: function(result) {
            if (result.status == "found"){
                oldId = result.marker.id;

                updateMap(result.marker.lat, result.marker.lng, result.marker.timeStamp, result.marker.colour)
                
                console.log(result.message);
            }
            else {
                console.log(result.message);
            }
        },
        error: function(result) {
            console.log(result.message);
        }
    })
}
