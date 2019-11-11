import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { PriceList } from './models/PriceList';
import { HttpHeaders, HttpClient } from '@angular/common/http';
import { FormBuilder } from '@angular/forms';
import { Passenger } from './passenger';
import { Ticket } from './models/ticket';
import { TicketType } from './ticket-type';
import { PassengerType } from './passenger-type';
import { map, catchError } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class PriceListService {
  
  url: string = 'http://localhost:52295/Api/PriceList';
  constructor(private http: HttpClient, private fb: FormBuilder) { }  

  private handleError<T>(operation='operation',result?:T)
  {
    return (error:any):Observable<T> => {
      if(error.error.Message != undefined)
      alert(error.error.Message);

      return of (result as T);
    };
  }

  public getAllPassTypes(): Promise<PassengerType[]>{
    return this.http.get<PassengerType[]>('http://localhost:52295/Api/PassengerType/GetAllPT').toPromise<PassengerType[]>();  
  }
  public getAllTicketTypes(): Promise<TicketType[]>{
    return this.http.get<TicketType[]>('http://localhost:52295/Api/TicketType/GetAllTT').toPromise<TicketType[]>();  
  }
  public getAllPriceList(): Promise<PriceList[]>{
    return this.http.get<PriceList[]>('http://localhost:52295/Api/PriceList/GetAllPL').toPromise<PriceList[]>();  
  }


  public getPassengerById(passenger: number): Observable<PassengerType> {  
    return this.http.get<PassengerType>('http://localhost:52295/Api/PassengerType' + '/GetById/' + passenger);  
  }  

  getTicketById(employeeId: number): Observable<TicketType> {  
    return this.http.get<TicketType>('http://localhost:52295/Api/TicketType' + '/GetById/' + employeeId);  
  }  

  deletePriceListById(employeeid: string): Observable<number> {  
    const httpOptions = { headers: new HttpHeaders({ 'Content-Type': 'application/json'}) };  
    return this.http.delete<number>(this.url + '/DeletePriceList?id=' +employeeid,  
    httpOptions);  
  }

  public createPriceList(priceList: PriceList): Observable<PriceList> {  
    const httpOptions = { headers: new HttpHeaders({ 'Content-Type': 'application/json'}) };  
    return this.http.post<PriceList>(this.url + '/InsertPriceList',  
    priceList, httpOptions);  
  }  

  public getPriceList(card :string, pass: string): Promise<PriceList[]> {
    return this.http.get<PriceList[]>(this.url+"/GetPriceList?card="+card+"&pass="+pass).toPromise<PriceList[]>();
  }

  public updatePassengerTypeById(passengerType: PassengerType): Observable<PassengerType> {  
    const httpOptions = { headers: new HttpHeaders({ 'Content-Type': 'application/json'}) };  
    return this.http.put<PassengerType>('http://localhost:52295/Api/PassengerType/PutPassengerType/' +passengerType.Id,passengerType,  
httpOptions).pipe(
      map(res => {
      }),
      catchError(this.handleError<any>('login'))
    );  

  }

  public updateTicketTypeById(ticketType: TicketType): Observable<TicketType> {  
    const httpOptions = { headers: new HttpHeaders({ 'Content-Type': 'application/json'}) };  
    return this.http.put<TicketType>('http://localhost:52295/Api/TicketType/PutTicketType/' +ticketType.Id,ticketType,  
httpOptions).pipe(
      map(res => {
      }),
      catchError(this.handleError<any>('login'))
    );  
  }

  updatePriceList(employee: PriceList): Observable<PriceList> {  
    const httpOptions = { headers: new HttpHeaders({ 'Content-Type': 'application/json'}) };  
    return this.http.put<PriceList>(this.url + '/UpdatePriceList/',  
    employee, httpOptions).pipe(
      map(res => {
      alert("Successfully update!");
      }),
      catchError(this.handleError<any>('login'))
    );
  }  
  
}
