import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import {ActivatedRoute } from '@angular/router';
import { Http } from '@angular/http';
import { RequestOptionsArgs } from '@angular/http';
import {Headers} from '@angular/http';
@Component({
  selector: 'app-browser',
  templateUrl: './browser.component.html',
  styleUrls: ['./browser.component.css'],
  encapsulation: ViewEncapsulation.None
})
export class BrowserComponent implements OnInit {

  constructor(private _httpService: Http) {
    this.Vehicles = [];
    this.Drivers = [];
   }

  public Vehicles: any[];
  public Drivers: any[];


  ngOnInit() {
    this._httpService.get('/api/values/GetVehicles').subscribe(values => {
            var object = JSON.parse(values.text());
            var me = this;
          
            object.forEach(function (item) {
                me.Vehicles.push(item)                
            }); 
            console.log('added');
        }); 
/*
        this._httpService.get('/api/values/GetPriceList').subscribe(values => {
            var object = JSON.parse(values.text());
            var me = this;
            me.PriceNames = [];

            object.forEach(function (item) {
                me.PriceNames.push(item)                
            }); 

            console.log('added');
        }); */
  }

}
