import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { Observable, of } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ValidateTicketService {

  registerUrl: string =  'http://localhost:52295/Api/ticket/';
  constructor(private http: HttpClient,private route:Router) { }

  getInfo(id:number): Observable<string>{
      return this.http.get<string>(this.registerUrl+"CheckValidation?id="+id)
      .pipe();}

  private handleError<T>(operation = 'operation', result?: T) {
    return (error: any): Observable<T> => {
      alert("Greska");
      return of(result as T);
    };

  }}
