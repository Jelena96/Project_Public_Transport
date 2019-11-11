import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Line } from './models/Line';
import { Observable } from 'rxjs';


@Injectable({
  providedIn: 'root'
})
export class ScheduleService {

  private baseUrl: string;
  constructor(private client: HttpClient) {
    this.baseUrl = "http://localhost:52295/";
   }

  public getScheduleLines(typeOfLine :string): Observable<Line[]>{
   
   console.log(typeOfLine);
   return this.client.get<Line[]>(this.baseUrl+"api/Line/GetScheduleLines?typeOfLine="+typeOfLine);
  }

  public getSchedule(typeOfLine :string, typeOfDay: string, Number: string) {
    return this.client.get<string>(this.baseUrl+"api/Line/GetSchedule?typeOfLine="+typeOfLine+"&typeOfDay="+typeOfDay+"&Number="+Number).toPromise<string>();
  }


}
