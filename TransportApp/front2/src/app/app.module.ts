import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { RouterModule, Routes } from '@angular/router';


import { GoogleMapComponent } from '../app/components/google-map.component';

import { MapService } from '../app/services/map.service';
import { GeolocationService } from '../app/services/geolocation.service';
import { GeocodingService } from '../app/services/geocoding.service';

import { AppComponent } from './app.component';
import { VehiclesComponent } from './vehicles/vehicles.component';
import { MainComponent } from './main/main.component';
import { DriversComponent } from './drivers/drivers.component';
import { BrowserComponent } from './browser/browser.component';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';
import {CalendarModule} from 'primeng/primeng';
import {SliderModule} from 'primeng/primeng';
import { AllMarkersComponent } from './all-markers/all-markers.component';
import {HttpClientModule} from '@angular/common/http';

const appRoutes: Routes = [
  { path: 'vehicles', component: VehiclesComponent }
];

@NgModule({
    declarations: [
        AppComponent,
        GoogleMapComponent,
        VehiclesComponent,
        MainComponent,
        DriversComponent,
        BrowserComponent,
        AllMarkersComponent
    ],
    imports: [
        BrowserModule,
        BrowserAnimationsModule,
        FormsModule,
        HttpModule,
        CalendarModule,
        SliderModule,
        HttpClientModule,
        RouterModule.forRoot([
        { path: 'vehicles', component: VehiclesComponent },
        { path: '', component: MainComponent },
        { path: 'drivers', component: DriversComponent },
        { path: 'markers', component: AllMarkersComponent },
        { path: 'browse', component: BrowserComponent }
        ])
    ],
    providers: [
        MapService
    ],
    bootstrap: [AppComponent]
})
export class AppModule { }
