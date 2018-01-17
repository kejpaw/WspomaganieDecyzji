import { Injectable } from '@angular/core';

enum RideStatus {
    manStart =1 ,
    manEnd = 2,
    bus = 3 
}//todo jako provider



/**
 * MapsService class.
 *
 * This injectable class instances the map & the markers.
 * Its methods are used by the directives and can be used by a component.
 */
@Injectable() export class MapService {

    private map: google.maps.Map;

    private markers: google.maps.Marker[] = [];

    private polylines: google.maps.Polyline[] = [];

    public startMarker: google.maps.Marker = null;
    public endMarker: google.maps.Marker = null;

    /**
     * Creates a new map inside of the given HTML container.
     *
     * @param el DIV element
     * @param mapOptions MapOptions object specification
     */
    public initMap(el: HTMLElement, mapOptions: any): void {
        this.map = new google.maps.Map(el, mapOptions);

        this.resize();
        // Adds event listener resize when the window changes size.
        google.maps.event.addDomListener(window, "resize", () => this.resize());


}

    setCenter(latLng: google.maps.LatLng): void {
        if (this.map != null && latLng != null) {
            // Changes the center of the map to the given LatLng.
            this.map.panTo(latLng);
        }
    }

    setZoom(zoom: number): void {
        if (this.map != null) {
            this.map.setZoom(zoom);
        }
    }
/*
    addMarkerSimple(): void {
        var marker = new google.maps.Marker({
            position: {lat: 51.1105, lng: 17.033},
            icon: 'assets/images/bus.PNG',
            map: this.map,
            draggable: true
          });

          var marker2 = new google.maps.Marker({
            position: {lat: 51.1205, lng: 17.033},
            icon: 'assets/images/man.PNG',
            map: this.map,
            draggable: true
          });
    } 
*/

    addMarker(lat: number, lng: number, type: RideStatus, start: boolean): void {
            var iconPath = "assets/images/";
        
        switch (type) {
            case RideStatus.bus:
            iconPath += 'bus.png';
            break;
            case RideStatus.manStart:
            iconPath += 'man.png';
            break;
            case RideStatus.manEnd:
            iconPath += 'dot.png';
            break;
        } 

        var marker = new google.maps.Marker({
            position: {lat: lat, lng: lng},
            icon: iconPath,
            map: this.map,
            draggable: true,
            appId: 1,
            start: start
          });


        this.markers.push(marker);
    }

    addCustomMarker(lat: number, lng: number, type: RideStatus, start: boolean): void {
        
        var iconPath = "assets/images/";
        
        
        switch (type) {
            case RideStatus.bus:
            iconPath += 'bus.png';
            break;
            case RideStatus.manStart:
            iconPath += 'man.png';
            break;
            case RideStatus.manEnd:
            iconPath += 'dot.png';
            break;
        } 
          
        if ((this.startMarker === null && start) || (this.endMarker === null && !start))
        {
        var marker = new google.maps.Marker({
            position: {lat: lat, lng: lng},
            icon: iconPath,
            map: this.map,
            draggable: true,
            appId: 1,
            start: start
          });
    
         if (start) {
             this.startMarker = marker;
         }      else {
             this.endMarker = marker;
         }
        }
    }



    deleteMarkers(): void {
        // Removes the markers from the map.
        for (let i: number = 0; i < this.markers.length; i++) {
            this.markers[i].setMap(null);
        }

        for (let i: number = 0; i < this.polylines.length; i++) {
            this.polylines[i].setMap(null);
        }

        this.markers = [];
        this.polylines = [];
    }

    public resize(): void {
        const latLng: google.maps.LatLng = this.map.getCenter();
        setTimeout(() => {
            google.maps.event.trigger(this.map, "resize");
            this.map.setCenter(latLng);
        });
    }



    public addPolyline(start: google.maps.LatLng, end: google.maps.LatLng, color: string): void {

        var route = new google.maps.Polyline({
          path: [start, end],
          geodesic: true,
          strokeColor: (color == null) ? '#3a77d8': color,
          strokeOpacity: 1.0,
          strokeWeight: 2
        });

        route.setMap(this.map); 
        this.polylines.push(route);
    }

}
