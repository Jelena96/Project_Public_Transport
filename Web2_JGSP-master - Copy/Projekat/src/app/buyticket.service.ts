import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';  
import { HttpHeaders } from '@angular/common/http';  
import { Observable } from 'rxjs';  
import {Ticket} from './models/ticket';
import { Email } from './models/Email';
import { map } from 'rxjs/operators';
import { PayPal } from './models/PayPal';

@Injectable({
  providedIn: 'root'
})
export class BuyticketService {

  url: string = 'http://localhost:52295/Api/ticket';
  constructor(private http: HttpClient) { }  
  getAllTicket(): Observable<Ticket[]> {  
    return this.http.get<Ticket[]>(this.url + '/AllTicket');  
  }  
  getTicketById(employeeId: string): Observable<Ticket> {  
    return this.http.get<Ticket>(this.url + '/GetById/' + employeeId);  
  }  
  createTicketNotRegisteredUser(email:Email) : Observable<Email> {  
    const httpOptions = { headers: new HttpHeaders({ 'Content-Type': 'application/json'}) };  
    return this.http.post<Email>(this.url + '/InsertTimeTicket/',  
    email, httpOptions);  
  }
  createTicket(employee: Ticket): Observable<Ticket> {  
    const httpOptions = { headers: new HttpHeaders({ 'Content-Type': 'application/json'}) };  
    return this.http.post<Ticket>(this.url + '/InsertTicket/',  
    employee, httpOptions);  
  } 
  savePayPal(ticket: PayPal): Observable<Ticket> {  
    const httpOptions = { headers: new HttpHeaders({ 'Content-Type': 'application/json'}) };  
    return this.http.post<Ticket>(this.url + '/SavePayPalInfo/',  
    ticket, httpOptions);  
  }   
  updateTicket(employee: Ticket): Observable<Ticket> {  
    const httpOptions = { headers: new HttpHeaders({ 'Content-Type': 'application/json'}) };  
    return this.http.put<Ticket>(this.url + '/UpdateTicket/',  
    employee, httpOptions);  
  }  
  deleteTicketById(employeeid: string): Observable<number> {  
    const httpOptions = { headers: new HttpHeaders({ 'Content-Type': 'application/json'}) };  
    return this.http.delete<number>(this.url + '/DeleteTicket?id=' +employeeid,  
 httpOptions);  
  }  

  checkIn(ticket:Ticket){
    return this.http.put<any>(this.url+"/CheckInTicket",ticket,{ 'headers': { 'Content-type': 'application/json' }} ).pipe(
      map(res => {
      alert("Checked in!");
      }),
    );
  }

  showTickets(): Observable<Ticket[]> {    
    return this.http.get<Ticket[]>(this.url+"/AllTicket")
    .pipe(    );
  }
}  

