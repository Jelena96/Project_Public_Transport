import { Component, OnInit } from '@angular/core';
import { google } from '@agm/core/services/google-maps-types';
import { map } from 'rxjs/operators';

@Component({
  selector: 'app-timetracking',
  templateUrl: './timetracking.component.html',
  styleUrls: ['./timetracking.component.css']
})
export class TimetrackingComponent implements OnInit {

  
//Load google map


numDeltas:any  = 100;
delay: any = 10; //milliseconds
i: any = 0;
deltaLat: any;
deltaLng: any;
marker: any;

  ngOnInit()
  {
    this.initialize();

  }

  position: any = [40.748774, -73.985763];

 initialize() { 
    var latlng = new google.maps.LatLng(this.position[0], this.position[1]);
    var myOptions = {
        zoom: 16,
        center: latlng,
        mapTypeId: google.maps.MapTypeId.ROADMAP
    };
    map: Object  = new google.maps.Map(document.getElementById("mapCanvas"), myOptions);

    this.marker = new google.maps.Marker({
        position: latlng,
        map: map,
        title: "Latitude:"+this.position[0]+" | Longitude:"+this.position[1]
    });

    google.maps.event.addListener(map, 'click', function(event) {
        var result = [event.latLng.lat(), event.latLng.lng()];
        this.transition(result);
    });
    google.maps.event.addDomListener(window, 'load', this.initialize);


}

 transition(result){
    this.i = 0;
    this.deltaLat = (result[0] - this.position[0])/this.numDeltas;
    this.deltaLng = (result[1] - this.position[1])/this.numDeltas;
    this.moveMarker();
}

 moveMarker(){
  this.position[0] += this.deltaLat;
  this.position[1] += this.deltaLng;
    var latlng = new google.maps.LatLng(this.position[0], this.position[1]);
    this.marker.setTitle("Latitude:"+this.position[0]+" | Longitude:"+this.position[1]);
    this.marker.setPosition(latlng);
    if(this.i != this.numDeltas){
        this.i ++;
        setTimeout(this.moveMarker, this.delay);
    }
}
}
