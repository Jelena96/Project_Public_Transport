import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Passenger } from './models/Passenger';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class RegistrationService {

  private url: string;
  constructor(private client: HttpClient) {
    this.url = "http://localhost:52295/Api/Passengers";
   }

  public createPassenger(priceList: Passenger): Observable<Passenger> {  
    const httpOptions = { headers: new HttpHeaders({ 'Content-Type': 'application/json'}) };  
    return this.client.post<Passenger>(this.url + '/InsertPassenger',  
    priceList, httpOptions);  
  }  
}
