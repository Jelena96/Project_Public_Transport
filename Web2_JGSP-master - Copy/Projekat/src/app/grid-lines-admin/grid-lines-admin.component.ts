import { Component, OnInit, NgZone } from '@angular/core';
import { Line } from '../models/Line';
import { Station } from '../models/Station';
import { Router } from '@angular/router';
import { StationsService } from '../stations.service';
import { LineserviceService } from '../lineservice.service';
import { Coordinate } from '../models/Coordinate';
import * as travelMarker from 'travel-marker';

declare var google: any;

@Component({
  selector: 'app-grid-lines-admin',
  templateUrl: './grid-lines-admin.component.html',
  styleUrls: ['./grid-lines-admin.component.css'],
  styles: ['agm-map {height: 500px; width: 700px;}'] //postavljamo sirinu i visinu mape
})
export class GridLinesAdminComponent implements OnInit {

  icon;
  stations: Array<Station> = [];
  imageUrl: string = "./assets/busicon.png";
  lines: Array<Line> = [];
  stationsArray: Station[] = [];
  selectedLine: Line;
  stationBus: Station = new Station;
  numDeltas = 250;
  delay = 40; //milliseconds
  i = 0;
  deltaLat: any;
  deltaLng: any;

  
  constructor(private ngZone: NgZone, private stationService: StationsService, private router: Router, private linesService: LineserviceService) { }
  
  ngOnInit() {
    this.stationService.getAllStations().subscribe((data)=>{
      Object.assign(this.stations, data);

      this.linesService.getAllLines().subscribe((data)=>{
        Object.assign(this.lines, data);
      });   
    });
    this.stationBus.Coordinate = new Coordinate;
   
   this.icon={
    url: "../../../assets/images/bus1.jpg",
    scaledSize:{
      width:75,
      height:75
    }

   };
  }

  showLine(){
    this.stationsArray = this.selectedLine.Stations;
    console.log(this.stationsArray);
    this.stationBus.Coordinate.X = this.stationsArray[0].Coordinate.X;
    this.stationBus.Coordinate.Y = this.stationsArray[0].Coordinate.Y;

    found: Station;
    for(let i = 1; i<this.stationsArray.length; i++)
    {
      this.transition(this.stationsArray[i].Coordinate);

    }
  }
  
  zoom: number = 15;
  map: any;
  line: any;
  directionsService: any;
  marker: travelMarker.TravelMarker = null;
  // speedMultiplier to control animation speed
  speedMultiplier = 1;

  transition(result: Coordinate){
    this.i = 0;
    this.deltaLat = (result.X - this.stationBus.Coordinate.X)/this.numDeltas;
    this.deltaLng = (result.Y - this.stationBus.Coordinate.Y)/this.numDeltas;
    this.moveMarker();
  }

  moveMarker(){
    this.stationBus.Coordinate.X = this.deltaLat + this.stationBus.Coordinate.X;
    this.stationBus.Coordinate.Y += this.deltaLng;
    if(this.i != this.numDeltas){
      this.i++;
        setTimeout(()=>{this.moveMarker();}, this.delay);
    }
  }
 
}