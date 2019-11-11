import { Injectable } from '@angular/core';
import { Schedule } from './models/Schedule';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { FormBuilder } from '@angular/forms';
import { map, catchError } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class AdminScheduleService {

  url: string = 'http://localhost:52295/Api/Schedules';
  constructor(private http: HttpClient, private fb: FormBuilder) { }  

  private handleError<T>(operation='operation',result?:T)
  {
    return (error:any):Observable<T> => {
      if(error.error.Message != undefined)
      alert(error.error.Message);

      return of (result as T);
    };
  }

  public getAllSchedules(): Promise<Schedule[]>
  { 
    return this.http.get<Schedule[]>(this.url + '/AdminSchedules').toPromise();  
  }

  public deleteScheduleById(scheduleId: number): Observable<number> 
  {  
    const httpOptions = { headers: new HttpHeaders({ 'Content-Type': 'application/json'}) };  
    return this.http.delete<number>(this.url + '/DeleteSchedule?id=' +scheduleId,  
 httpOptions);  
  }  

  public updateScheduleById(schedule: Schedule): Observable<Schedule>
   {  
    return this.http.put<string>('http://localhost:52295/Api/Schedules/PutSchedule/'+schedule.Id,schedule ,{ 'headers': { 'Content-type': 'application/json' }} ).pipe(
      map(res => {
      alert("Successfully update!");
      }),
      catchError(this.handleError<any>('login'))
    );
  }

  public createSchedule(schedule: Schedule): Observable<Schedule>
   {  
    const httpOptions = { headers: new HttpHeaders({ 'Content-Type': 'application/json'}) };  
    return this.http.post<Schedule>(this.url + '/InsertSchedule',  
    schedule, httpOptions);  
  }  
 
}