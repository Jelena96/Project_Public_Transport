import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { Observable, pipe, of, observable } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { User } from './models/user';
@Injectable({
  providedIn: 'root'
})
export class LoginService {

  isLoggedIn = false;

  loginUrl: string = 'http://localhost:52295/oauth/token';

  constructor(private http: HttpClient) { }

  login(user: User): Observable<any> {
    return Observable.create((observable) => {
    /* return*/ this.http.post<any>(this.loginUrl, `username=`+ user.username +`&password=`+ user.password + `&grant_type=password`, { 'headers': { 'Content-type': 'x-www-form-urlencoded' } }).subscribe(
      res => {
        console.log(res.access_token);
        this.isLoggedIn=true;
        let jwt = res.access_token;

        let jwtData = jwt.split('.')[1]
        let decodedJwtJsonData = window.atob(jwtData)
        let decodedJwtData = JSON.parse(decodedJwtJsonData)

        let role = decodedJwtData.role
        let email = decodedJwtData.unique_name

        console.log('jwtData: ' + jwtData)
        console.log('decodedJwtJsonData: ' + decodedJwtJsonData)
        console.log('decodedJwtData: ' + decodedJwtData)
        console.log('Role ' + role)
        console.log('email ' + email)


        localStorage.setItem('jwt', jwt)
        localStorage.setItem('role', role);
        localStorage.setItem('email', email);

        
        if(role==="Admin")
        {
          observable.next('Admin');
          observable.complete();
        } else if(role === 'AppUser')
        {
          observable.next("AppUser");
          observable.complete();
        }
        else if(role === 'Controller')
        {
          observable.next("Controller");
          observable.complete();
        }
      },
      err => {
        observable.next('greska')
        observable.complete();
      }
      )
    }
    )    
  }

  logout(): void {
    this.isLoggedIn = false;
    localStorage.removeItem('jwt');
    localStorage.removeItem('role');
    localStorage.removeItem('email');


  }

  private handleError<T>(operation = 'operation', result?: T) {
    return (error: any): Observable<T> => {
      return of(result as T);
    };
  }}
