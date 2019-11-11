import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Line } from './models/Line';
import { Observable, of } from 'rxjs';
import { catchError, map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class LineserviceService {
  
  private handleError<T>(operation='operation',result?:T)
  {
    return (error:any):Observable<T> => {
      if(error.error.Message != undefined)
      alert(error.error.Message);

      return of (result as T);
    };
  }


  private url: string;
  constructor(private client: HttpClient) {
    this.url = "http://localhost:52295/api/Line";
   }

  public createLine(line: Line): Observable<Line> {  
    const httpOptions = { headers: new HttpHeaders({ 'Content-Type': 'application/json'}) };  
    return this.client.post<Line>(this.url + '/InsertLine',  
    line, httpOptions);  
  }  


  public getAllLines(): Observable<Line[]>{
   
    return this.client.get<Line[]>(this.url + '/AllLines');  
  }

  public delete(scheduleId: number): Promise<number> {  
    const httpOptions = { headers: new HttpHeaders({ 'Content-Type': 'application/json'}) };  
    return this.client.delete<number>(this.url + '/DeleteLine?id=' +scheduleId,  
 httpOptions).toPromise();  
  } 

// public updateLine(ticketType: Line): Observable<Line> {  
//   const httpOptions = { headers: new HttpHeaders({ 'Content-Type': 'application/json'}) };  
//   return this.client.put<Line>('http://localhost:52295/Api/Line/UpdateLine/' +ticketType.Id,ticketType,  
// httpOptions);  


// }



public updateLine(ticketType: Line): Observable<Line> {

  return this.client.put<string>('http://localhost:52295/Api/Line/UpdateLine/'+ticketType.Id,ticketType ,{ 'headers': { 'Content-type': 'application/json' }} ).pipe(
    map(res => {
    alert("Successfully update!");
    }),
    catchError(this.handleError<any>('login'))
  );
}


}
