
import { Component, OnInit, Input, NgZone } from '@angular/core';
import { GeoLocation } from '../models/map/geolocations';
import { MarkerInfo } from '../models/map/marker-info-model';
import { Polyline } from '../models/map/polyline';
import { StationsService } from '../stations.service';
import { Coordinate } from '../models/Coordinate';
import { Station } from '../models/Station';
import { Validators, FormBuilder } from '@angular/forms';
import { Line } from '../models/Line';

@Component({
  selector: 'app-map',
  templateUrl: './map.component.html',
  styleUrls: ['./map.component.css'],
  styles: ['agm-map {height: 500px; width: 700px;}'] //postavljamo sirinu i visinu mape
})
export class MapComponent implements OnInit {

  markerInfo: MarkerInfo;
  public polyline: Polyline;
  public zoom: number;
  public message: string;
  public coordinate: Coordinate=new Coordinate();
  public station:Station=new Station();
  public lines:Line[]=[];
  public stations: Array<Station> = [];
  public geoLocation: GeoLocation;
  public selectedstation: Station = new Station;
  public name: string;
  public adrress: string;
  public id: number;

  addStationForm = this.fb.group({
    name: ['', Validators.required],
    address: ['', Validators.required],
  });

  updateForm = this.fb.group({
    Name: [this.name, Validators.required],
    Address: [this.adrress, Validators.required],
    Id: [this.id, Validators.required]
  });

   ngOnInit() 
   {
      this.polyline = new Polyline([], 'blue', { url:"assets/busicon.png", scaledSize: {width: 50, height: 50}});

      this.stationService.getAllStations().subscribe((data)=>{
        Object.assign(this.stations, data);
        
      });   
  }

  constructor(private ngZone: NgZone,private stationService:StationsService,private fb: FormBuilder){
  }

  public Update(station: any)
  {
    this.selectedstation = station;
  }

  public UpdateSubmit()
  {
    this.stationService.updateStation(this.selectedstation).subscribe(
      (data) => {
        if(data)
          alert("Successfully updated station")
          this.ngOnInit();

      }
      );
  }

  public SubmitStation()
  {
    this.station.Name = this.addStationForm.controls['name'].value;
    this.station.Address = this.addStationForm.controls['address'].value;
    this.station.Lines = this.lines;
    
    this.stationService.createStation(this.station).subscribe(  
      (data) => {  
        if(data)
        {
          alert("Successfully added station");  
        }
        else
        {
          alert("Unsuccessfully added station");
        }
      }  
    );

      this.ngOnInit();
  }

  public DeleteStation(selectedstation: any)
  {
    this.stationService.deleteStationById(selectedstation.Id).subscribe(data=>{
      this.ngOnInit();

    });

  }

  placeMarker($event){
    this.polyline.addLocation(new GeoLocation($event.coords.lat, $event.coords.lng))

    this.coordinate.X = $event.coords.lat;
    this.coordinate.Y = $event.coords.lng;
    this.station.Coordinate = this.coordinate;

    console.log(this.polyline)
  }

}