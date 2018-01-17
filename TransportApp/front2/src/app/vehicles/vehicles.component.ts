import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import {ActivatedRoute } from '@angular/router';
import { Http } from '@angular/http';
import { RequestOptionsArgs } from '@angular/http';
import {Headers} from '@angular/http';

@Component({
  selector: 'app-vehicles',
  templateUrl: './vehicles.component.html',
  styleUrls: ['./vehicles.component.css'],
  encapsulation: ViewEncapsulation.None
})
export class VehiclesComponent implements OnInit {

  private Name: string;
  private CarModel: string;
  private CarNumber: string;
  private Year: number;

  constructor(private route: ActivatedRoute, private _httpService: Http) {

    this.Name = "";
   }  
  ngOnInit() {
  }


submitCar() {


  var vehicle = {
    Id: 0,
    Name: this.Name,
    CarModel: this.CarModel,
    CarNumber: this.CarNumber,
    Year: /*parseInt(this.Year)*/this.Year
  };



  let headers = new Headers({ 'Content-Type': 'application/json' });
  this._httpService.post('api/vehicle/', vehicle, { headers: headers }).subscribe(
            res => {
                this.Name = null;
                this.CarModel = null
                this.CarNumber = null;
                this.Year = null;
                alert('Zapisano');
            },
            err => {alert('błąd zapisu do bazy danych');}
            );
}

}
