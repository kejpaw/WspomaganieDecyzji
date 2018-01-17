import { Component, OnInit, ViewEncapsulation, ElementRef, NgZone } from '@angular/core';
import { MapService } from '../../app/services/map.service';
import { Http, RequestOptions, URLSearchParams } from '@angular/http'
import {Orders} from '../../app/models/Orders'
import {Headers} from '@angular/http';
import { RouterModule, Routes, RouterOutlet } from '@angular/router';
import {SliderModule} from 'primeng/primeng';
import {HttpClientModule, HttpClient} from '@angular/common/http';

enum RideStatus {
    manStart = 1,
    manEnd = 2,
    bus = 3 
}

@Component({
  selector: 'app-all-markers',
  templateUrl: './all-markers.component.html',
  styleUrls: ['./all-markers.component.css'],
  encapsulation: ViewEncapsulation.None
})
export class AllMarkersComponent implements OnInit {

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
    startDate: Date;
    endDate: Date;
    val: number;
    barefootValue: number;
    slideLabel: String;

    constructor(
        private ngZone: NgZone,
        private elementRef: ElementRef,
        private map: MapService,
        private _httpService: Http,
        private httpClient: HttpClient
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

        this.val = 0;
        this.barefootValue = 0;
    }

  ngOnInit() {
  
    this._httpService.get('/api/values/GetMarkers').subscribe(values => {
            var object = JSON.parse(values.text());
            var me = this;

            object.forEach(function (item) {
                var status = item.Request === true ? RideStatus.manStart : RideStatus.manEnd;

                me.map.addMarker(item.StartLat as number, item.StartLng as number, status, true);
                me.map.addMarker(item.EndLat as number, item.EndLng as number, status, true);   

               if (item.Request !== true) {
                me.map.addPolyline(new google.maps.LatLng(item.StartLat,item.StartLng), new google.maps.LatLng(item.EndLat,item.EndLng), null);
               }
            }); 
        }); 
 }

 count() {


        var parameters = {
            FootPercentage: this.barefootValue,
            Switches: Math.floor(this.val / 50)          
        };

       let options = new RequestOptions({
            headers: new Headers({ 'Content-Type': 'application/json', 
                               'Accept': 'q=0.8;application/json;q=0.9' }),
            body: parameters
        });


         this._httpService.post('/api/values/GetAstar', parameters, options).subscribe(values => {

             var object = JSON.parse(values.text());
             var me = this;

             me.map.deleteMarkers();

           object.forEach(function (item) {
                var status = item.Request === true ? RideStatus.manStart : RideStatus.manEnd;

                me.map.addMarker(item.StartLat as number, item.StartLng as number, status, true);
                me.map.addMarker(item.EndLat as number, item.EndLng as number, status, true);   

                var color = item.IsInProgress !== true ? '#b1b9c6' : '#51ba25';

                if (item.Request !== true) {
                me.map.addPolyline(new google.maps.LatLng(item.StartLat,item.StartLng), new google.maps.LatLng(item.EndLat,item.EndLng), color);
                }
            }); 
        }); 
    }

handleSlideChange() {
    
    switch (this.val)
    {
        case 0:
            this.slideLabel = "Brak";
            break;

        case 50:
            this.slideLabel = "Max 1";
            break;
        case 100:
             this.slideLabel = "100%";
            break;
    }

    document.getElementById("przesiadki").innerHTML = this.slideLabel as string ;

}


}
