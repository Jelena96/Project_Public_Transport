import { Injectable } from '@angular/core';
import { Station } from './models/Station';
import { Observable, of } from 'rxjs';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { FormBuilder } from '@angular/forms';
import { catchError, map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class StationsService {

  url: string = 'http://localhost:52295/Api/Stations';

  constructor(private http: HttpClient, private fb: FormBuilder) { }

  public createStation(station: Station): Observable<Station> {  
    const httpOptions = { headers: new HttpHeaders({ 'Content-Type': 'application/json'}) };  
    return this.http.post<Station>(this.url + '/InsertStation',station, httpOptions);  
  }  

  public getAllStations(): Observable<Station[]>{
   
    return this.http.get<Station[]>(this.url + '/AllStations');  
  }

  public updateStation(station: Station): Observable<Station> {  
    const httpOptions = { headers: new HttpHeaders({ 'Content-Type': 'application/json'}) };  
    return this.http.put<Station>(this.url + '/UpdateStation/',  
    station, httpOptions).pipe(
      map(res => {
      alert("Successfully update!");
      }),
      catchError(this.handleError<any>('login'))
    );
  }
  
  private handleError<T>(operation='operation',result?:T)
  {
    return (error:any):Observable<T> => {
      if(error.error.Message != undefined)
      alert(error.error.Message);

      return of (result as T);
    };
  }

  public deleteStationById(station: number): Observable<number> 
  {  
    const httpOptions = { headers: new HttpHeaders({ 'Content-Type': 'application/json'}) };  
    return this.http.delete<number>(this.url + '/DeleteStation?id=' +station,  
 httpOptions);  
  }  
}
