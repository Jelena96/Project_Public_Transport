import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { Observable, of } from 'rxjs';
import { RegistrateUser } from './models/RegistrateUser';
import { map, catchError } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class ValidateService {

  registerUrl: string =  'http://localhost:52295/api/Account/';

  constructor(private http: HttpClient,private route:Router) { }

  getUsers(): Observable<RegistrateUser[]> {
    return this.http.get<RegistrateUser[]>(this.registerUrl+"GetUsers")
    .pipe();
  }

  validateProfile(user:RegistrateUser ){
      return this.http.put<string>(this.registerUrl+"Validate",user,{ 'headers': { 'Content-type': 'application/json' }} ).pipe(
          map(res => {
          alert("Successfully validate!");
          }),
          catchError(this.handleError<any>('login'))
        );
  }

  private handleError<T>(operation = 'operation', result?: T) {
    return (error: any): Observable<T> => {
      alert("Greska");
      return of(result as T);
    };
  }
}