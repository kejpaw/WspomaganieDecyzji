import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import {ActivatedRoute } from '@angular/router';
import { Http } from '@angular/http';
import { RequestOptionsArgs } from '@angular/http';
import {Headers} from '@angular/http';

@Component({
  selector: 'app-drivers',
  templateUrl: './drivers.component.html',
  styleUrls: ['./drivers.component.css'],
  encapsulation: ViewEncapsulation.None
})
export class DriversComponent implements OnInit {

  constructor(private _httpService: Http) { }

  public Name: string;
  public Surname: string;
  public Region: number;
  public Types: string [];
  public selection: string;

  ngOnInit() {
    this.Types = ['1', '2']
}

  submitDriver() {

    var e = document.getElementById("selection") as HTMLSelectElement;

      var driver =  {
        Id: 0,
        Name: this.Name,
        Surname : this.Surname,
        Region: parseInt(e.options[e.selectedIndex].value),
        IdVehicle: null
      }

  let headers = new Headers({ 'Content-Type': 'application/json' });
  this._httpService.post('api/vehicle/addDriver', driver, { headers: headers }).subscribe(
            res => {
                this.Name = null;
                this.Surname = null
                this.Region = null;
                alert('Zapisano');
            },
            err => {alert('błąd zapisu do bazy danych');}
            );
}

}
