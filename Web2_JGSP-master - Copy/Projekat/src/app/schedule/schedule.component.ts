import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder } from '@angular/forms';
import { ScheduleService } from 'src/app/schedule.service';
import { Line } from 'src/app/models/Line';

@Component({
  selector: 'app-schedule',
  templateUrl: './schedule.component.html',
  styleUrls: ['./schedule.component.css']
})
export class ScheduleComponent implements OnInit {
  //public typeOfDayForm: FormGroup;
  //public typeOfLineForm: FormGroup;
  //public lineForm: FormGroup;
  public ScheduleForm: FormGroup;
  public typeOfLine: string;
  public lines: Line[];
  public times: string;
  public parser:string[];

  selectedLine;
  selectedDay;
  selectedNumber;

  TypeLine:Array<Object> = [
    {name: "City"},
    {name: "Suburban"},

];

TypeDay:Array<Object> = [
  {name: "Work day"},
  {name: "Weekend"},

];

  constructor( private fb: FormBuilder, private scheduleService: ScheduleService) { 
   
    this.ScheduleForm = this.fb.group({
      line: [''],
      day: [''],
      number: ['']

    });

    this.lines = new Array<Line>();

  }

  async ngOnInit() 
  {    
    this.typeOfLine = this.ScheduleForm.controls['line'].value;
  }

  public typeSelected()
  {
     this.scheduleService.getScheduleLines(this.typeOfLine).subscribe((data) => {
       this.lines= data;
     });
    
  }

  public async ScheduleShow(){
    let typeOfLine = this.ScheduleForm.controls['line'].value;
    let typeOfDay = this.ScheduleForm.controls['day'].value;
    let Number = this.ScheduleForm.controls['number'].value;
    this.times = await this.scheduleService.getSchedule(typeOfLine,typeOfDay,Number);
    this.parser = this.times.split(',');
    
  }

}
