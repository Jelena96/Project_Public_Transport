import { Schedule } from './Schedule';

export class ScheduleType{
    Id:number;
    Name:string;
    Schedules:Schedule[];


    constructor(private name: string,private id: number,private schedules: Schedule[])
    {
        this.Name=name;
        this.Id=id;
        this.Schedules=schedules;   

    }
}


