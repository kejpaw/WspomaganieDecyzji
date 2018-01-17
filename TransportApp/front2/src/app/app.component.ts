import { Component, ElementRef, NgZone, OnInit } from '@angular/core';
import { MapService } from '../app/services/map.service';
import { Http } from '@angular/http'
import {Orders} from '../app/models/Orders'
import { RequestOptionsArgs } from '@angular/http';
import {Headers} from '@angular/http';
import { RouterModule, Routes, RouterOutlet } from '@angular/router';



enum RideStatus {
    manStart =1 ,
    manEnd = 2,
    bus = 3 
}//todo jako provider


@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {

    // Center map. Required.
    center: google.maps.LatLng;

    // MapOptions object specification.

    // The initial map zoom level. Required.
    zoom: number;

    disableDefaultUI: boolean;
    disableDoubleClickZoom: boolean;
    mapTypeId: google.maps.MapTypeId;
    maxZoom: number;
    minZoom: number;
    styles: google.maps.MapTypeStyle[];

    // Marker position. Required.
    position: google.maps.LatLng;

    // Marker title.
    title: string;

    // Info window.
    content: string;

    // Address to be searched.
    address: string;

    // Warning flag & message.
    warning: boolean;
    message: string;
    apiValues: string;
    adding: boolean;


    constructor(
        private ngZone: NgZone,
        private elementRef: ElementRef,
        private map: MapService,
        private _httpService: Http
    ) {
        this.center = new google.maps.LatLng(51.1105, 17.033);
        this.zoom = 12;

        // Other options.
        this.disableDefaultUI = true;
        this.disableDoubleClickZoom = false;
        this.mapTypeId = google.maps.MapTypeId.ROADMAP;
        this.maxZoom = 15;
        this.minZoom = 4;
        // Styled Maps: https://developers.google.com/maps/documentation/javascript/styling
        this.styles = [
            {
                featureType: 'landscape',
                stylers: [
                    { color: '#ffffff' }
                ]
            }
        ];

        // Initially the marker isn't set.

        this.address = "";

        this.warning = false;
        this.message = "";
    }

    ngOnInit(): void {
        console.log("no no no");
        var me = this;

        let scope = this;
        this._httpService.get('/api/values/GetMarkers').subscribe(values => {
            var object = JSON.parse(values.text());
            /*
            object.forEach(function (item) {
                me.map.addCustomMarker(item.StartLat as number, item.StartLng as number, RideStatus.manStart);
                me.map.addCustomMarker(item.EndLat as number, item.EndLng as number, RideStatus.manEnd);                
            }); */
        }); 
    }

    setAdding() {
        this.map.addCustomMarker(51.1105, 17.033, RideStatus.manStart, true);
    }
    setAddingEnd() {
        this.map.addCustomMarker(51.1105, 17.033, RideStatus.manStart, false);    
    }

    submitMarkers() {

        if (this.map.startMarker && this.map.endMarker)
        {

            var stLat = this.map.startMarker.getPosition().lat();
            var stLng = this.map.startMarker.getPosition().lng();
            
            var enLat = this.map.endMarker.getPosition().lat();
            var enLng = this.map.endMarker.getPosition().lng();

            var order = {
                Id: 0,
                StartLat: stLat, 
                StartLng: stLng,
                EndLat: enLat,
                EndLng: enLng,
                IdUser: 1,
                IdDriver: 1,
                IsInProgress: false,
                StartDate: new Date()              
            };

            let headers = new Headers({ 'Content-Type': 'application/json' });

            this._httpService.post('api/values/', order, { headers: headers }).subscribe(
            res => {
                //alert('zapisano');
                this.map.startMarker.setMap(null);
                this.map.endMarker.setMap(null);
                this.map.startMarker = null;
                this.map.endMarker = null;

                alert('Zapisano');
            },
            err => {alert('błąd zapisu');}
            );
        }
    }

    markerDragHandler(event: any): void {
        this.address = "upopo";
        //todo jak to zbajndowac do klasy itp
}


}
