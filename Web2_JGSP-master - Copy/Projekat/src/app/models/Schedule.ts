import { Line } from './Line';
import { ScheduleType } from './ScheduleType';

export class Schedule{
    Id: number;
    Departure: string;
    DayInWeek: string;
    ScheduleTypeId:number;
    //ScheduleType : ScheduleType
    LineId:number;
    Line:Line;
    
   
    // constructor(private id: number, private departure: string, private lineid:number,private day:string,private schtid:number,/*private schtype : ScheduleType*/private line:Line) { 
    //     this.Id=id;
    //     this.Departure=departure;
    //     this.LineId=lineid;
    //     this.DayInWeek=day;
    //     this.ScheduleTypeId=schtid;
    //     //this.ScheduleType=schtype;
    //     this.Line=line;
    //   }


}











