import { Component, OnInit, NgZone, ViewChild, ElementRef } from '@angular/core';
import { GeoLocation } from '../models/map/geolocations';
import { MarkerInfo } from '../models/map/marker-info-model';
import { Polyline } from '../models/map/polyline';
import { StationsService } from '../stations.service';
import { Coordinate } from '../models/Coordinate';
import { Station } from '../models/Station';
import { Validators, FormBuilder } from '@angular/forms';
import { Line } from '../models/Line';
import { LineserviceService } from '../lineservice.service';
import { Observable, of } from 'rxjs';

@Component({
  selector: 'app-admin-lines',
  templateUrl: './admin-lines.component.html',
  styleUrls: ['./admin-lines.component.css'],
  styles: ['agm-map {height: 500px; width: 700px;}'] //postavljamo sirinu i visinu mape
})
export class AdminLinesComponent implements OnInit {

  markerInfo: MarkerInfo;
  public polyline: Polyline;
  public zoom: number;
  public message: string;
  public coordinate: Coordinate=new Coordinate();
  public station:Station=new Station();
  public lines:Line[]=[];
  public stations: Array<Station> = [];
  public geoLocation: GeoLocation;
  public markerInfos: MarkerInfo;
  public stationsArray: Station[] = [];
  public line: Line = new Line();
  public lines2: Array<Line> = [];

  public selectedLine: Line = new Line();
  public typeOfLine: string;
  public LineNumber: number;

  addLinesForm = this.fb.group({
    number: ['', Validators.required],
    typelines:[]
  });

  TypeLine:Array<Object> = [
    {name: "Gradski"},
    {name: "Prigradski"},

]; 
  ngOnInit() {

    this.polyline = new Polyline([], 'blue', { url:"assets/busicon.png", scaledSize: {width: 50, height: 50}});
      this.stationService.getAllStations().subscribe((data)=>{
        Object.assign(this.stations, data);
      });
      console.log(this.stations);

     this.lineService.getAllLines().subscribe((data)=>{
        Object.assign(this.lines2, data);
      });
  }

  constructor(private ngZone: NgZone,private stationService:StationsService,private lineService: LineserviceService, private fb: FormBuilder){
  }

   addStation(station){
    this.stationsArray.push(station);
  }

  public SubmitLine()
  {
    this.line.LineNumber = this.addLinesForm.controls['number'].value;
    if(this.addLinesForm.controls['typelines'].value == "Gradski")
    {
      this.line.TypeOfLine = 1;
    }
    else
    {
      this.line.TypeOfLine = 2;
    }

    this.line.Stations = this.stationsArray;
    this.lineService.createLine(this.line).subscribe((data)=>{
      if(data)
      {
        alert("Successfully added line");
      }
      else
      {
        alert("Unsuccessfully added line");
      }
      this.ngOnInit();
    });  
  }

  updateForm = this.fb.group({
   
    LineNumber: ['', Validators.required],
    TypeOfLine: ['', Validators.required],

  });

  public ChangeLine()
  {
    this.lineService.updateLine(this.selectedLine).subscribe((data) => {
      if(data)
        alert("Successfully updated line");
      else
        alert("Unuccessfully updated line");
        this.ngOnInit();

   });

  }

  public  DeleteLine()
  {
    if(this.selectedLine != null)
    {
        this.lineService.delete(this.selectedLine.Id);
        alert("Successfully deleted line" + this.selectedLine.LineNumber);
        this.ngOnInit();

    }
    else
      alert("Not successfully deleted line" + this.selectedLine.LineNumber);


  }  
}
