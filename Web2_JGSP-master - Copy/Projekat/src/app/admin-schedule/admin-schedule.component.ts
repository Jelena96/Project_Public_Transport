import { Component, OnInit } from '@angular/core';
import { Line } from '../models/Line';
import { Schedule } from '../models/Schedule';
import { AdminScheduleService } from '../admin-schedule.service';
import { Observable } from 'rxjs';
import { FormBuilder, Validators } from '@angular/forms';
import { ScheduleType } from '../models/ScheduleType';

@Component({
  selector: 'app-admin-schedule',
  templateUrl: './admin-schedule.component.html',
  styleUrls: ['./admin-schedule.component.css']
})
export class AdminScheduleComponent implements OnInit {

  public schedules: Array<Schedule>;
  public  line: number;
  public Departure: string;
  public id: number;
  private schedule:Schedule;
  private LineId:number;
  private DayInWeek:string;
  private ScheduleTypeId:number;
  private Line:Line;
  public departure: string;
  public lineId: number;
  public typeOfDay: string;
  public typeOfLine: string;
  public typeOfLineAdd: ScheduleType;
  public scheduleAdd:Schedule = new Schedule();
  public scheduleTypeAd: ScheduleType;
  public scheduleType : ScheduleType;
  public selectedschedule: Schedule;
  public kk: boolean = false;

  updateForm = this.fb.group({
    LineId: [this.LineId, Validators.required],
    Departure: [this.Departure, Validators.required],
    Id: [this.id, '']

  });

  addForm = this.fb.group({
    lineId: ['', Validators.required],
    departure: ['', Validators.required],
    day: [''],
    typeOfLine: ['']
  });

  TypeLine:Array<Object> = [
    {name: "City"},
    {name: "Suburban"},

  ];

  TypeDay:Array<Object> = [
    {name: "Work day"},
    {name: "Weekend"},

    ];

  constructor(private scheduleService: AdminScheduleService,private fb: FormBuilder)
  { 
    this.schedules = new Array<Schedule>();
  }

  public Update(selectedSchedule: any)
  {
    this.selectedschedule = selectedSchedule;
    if(this.selectedschedule!=null)
      this.kk = true;
  }
  
  async ngOnInit() {

    this.schedules = await this.scheduleService.getAllSchedules();
 
  }

  public Delete(selectedSchedule: any)
  {
    this.scheduleService.deleteScheduleById(selectedSchedule.Id).subscribe((data) => {
      this.ngOnInit();
    });
    
  }

  public UpdateSubmit()
  {
    this.scheduleService.updateScheduleById(this.selectedschedule).subscribe((data) => {
     this.ngOnInit();
    });
  }

  public AddSubmit(){

    let dep = this.addForm.controls['departure'].value;
    let id = this.addForm.controls['lineId'].value;
    let day = this.addForm.controls['day'].value;
    let tof = this.addForm.controls['typeOfLine'].value;
    let tofId;

    if(tof == "City")
    {
      tofId = 1;
    }
    else
    {
      tofId = 2;
    }

    this.scheduleAdd.DayInWeek = this.addForm.controls['day'].value;
    this.scheduleAdd.ScheduleTypeId = tofId;
      this.scheduleService.createSchedule(this.scheduleAdd).subscribe(  
        (data) => {     
          if(data)
          alert("Successfully added schedule");
          this.ngOnInit();   
        }  
      );  
    }        
  } 