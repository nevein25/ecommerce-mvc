document.addEventListener("DOMContentLoaded", function () {
    let apiUrl;
    let lat;
    let lang;
    let fullAddress = {
        city: "",
        countryName: "",
        locality: "",
        longitude :"",
        latitude :""
    };

    /// get location using api
    navigator.geolocation.getCurrentPosition(position => {
        lat = position.coords.latitude;
        lang = position.coords.longitude;
        apiUrl = `https://api.bigdatacloud.net/data/reverse-geocode-client?latitude=${lat}&longitude=${lang}&localityLanguage=en`

        fetch(apiUrl)
            .then(response => response.json())
            .then(data => {
                fullAddress.city = data.city;
                fullAddress.countryName = data.countryName;
                fullAddress.locality = data.locality;
                fullAddress.longitude = data.longitude;
                fullAddress.latitude = data.latitude;
                console.log(fullAddress)
                initMap();
            })
    }, error => console.log("error"));
    ///


    let map;
    let marker;
    let geocoder; 

    function initMap() {
        map = new google.maps.Map(document.getElementById('map'), {
            center: { lat: fullAddress.latitude, lng: fullAddress.longitude },
            zoom: 20,
        });

        geocoder = new google.maps.Geocoder(); 

        marker = new google.maps.Marker({
            position: { lat: fullAddress.latitude, lng: fullAddress.longitude },
            map: map,
            draggable: true,
            animation: google.maps.Animation.DROP
        });

        marker.addListener('dragend', function (event) {
            let newPosition = marker.getPosition();
            console.log('New marker position:', newPosition.lat(), newPosition.lng());
            getStreetName(newPosition.lat(), newPosition.lng());
            console.log(fullAddress);
        });
    }

    function getStreetName(lat, lng) {
        let latlng = { lat: lat, lng: lng };
        console.log("gggggggggggggg")
      
        geocoder.geocode({ 'location': latlng }, function (results, status) {
            if (status === 'OK') {
                if (results[0]) {
                    let addressComponents = results[0].address_components;
                    let streetName = '';

                    for (let i = 0; i < addressComponents.length; i++) {
                        if (addressComponents[i].types[0] === 'route') {
                            streetName = addressComponents[i].long_name;
                            break;
                        }
                    }

                    console.log('Street Name:', streetName);
                } else {
                    console.log('No results found');
                }
            } else {
                console.log('Geocoder failed due to: ' + status);
            }
        });
    }

});
