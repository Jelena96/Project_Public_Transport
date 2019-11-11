import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { PassengerType } from '../passenger-type';
import { TicketType } from '../ticket-type';
import { Passenger } from '../models/Passenger';
import { DatePipe } from '@angular/common';
import { RegistrationService } from '../registration.service';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.css']
})
export class RegistrationComponent implements OnInit {

  public ime:string;
  public prezime:string;
  public pass:PassengerType=new PassengerType();
  public tick:TicketType=new TicketType();
  public datum:string;
  public password:string;
  public password2:string;
  public email:string;
  public passenger: Passenger = new Passenger();
  selectedFile: File

  PassType:Array<Object> = [
    {name: "Student"},
    {name: "Penzioner"},
    {name: "Regularni"},

 ];

TickType:Array<Object> = [
  {name: "Mesecna"},
  {name: "Dnevna"},
  {name: "Godisnja"},
  {name: "Vremenska"},

];

Students = {
  dob:'',
  dob2:''
}

registrationForm = this.fb.group({
  ime: ['', Validators.required],
  prezime: ['', Validators.required],
  pass: ['', Validators.required],
  datum:['', Validators.required],
  adresa:['', Validators.required],
  password:['', Validators.required],
  password2:['', Validators.required],
  email:['', Validators.required]
});

  constructor(private fb:FormBuilder,private registrationService: RegistrationService,private http: HttpClient) { }

  ngOnInit() {
  }


  onFileChanged(event) {
    this.selectedFile = event.target.files[0]
  }


  public Registrate()
  {
    this.passenger.FirstName = this.registrationForm.controls['ime'].value;
    this.passenger.LastName = this.registrationForm.controls['prezime'].value;
    //this.passenger.ImageUrl = this.selectedFile.name;
    this.passenger.BirthDate = new DatePipe('en-US').transform(this.Students.dob,'dd/MM/yyyy');
    this.passenger.PassengerType = new PassengerType();
    this.passenger.PassengerType.Name = this.registrationForm.controls['pass'].value;
    this.passenger.ImageUrl = this.base64textString;
    this.passenger.Approved = this.registrationForm.controls['pass'].value;
    this.passenger.Address = this.registrationForm.controls['adresa'].value;
    this.passenger.UserName = this.registrationForm.controls['email'].value;
    this.passenger.Role = "AppUser";
    let passwordic = this.registrationForm.controls['password'].value;
    
    if(passwordic == this.registrationForm.controls['password2'].value)
      this.passenger.Password = passwordic;

    this.passenger.Email = this.registrationForm.controls['email'].value;
    this.registrationService.createPassenger(this.passenger).subscribe(  
      (data) => { if(data != null)
                  {
                    alert("You are successfully registrated");

                  }
      });  
    }
    
    private base64textString:string="";
  
    handleFileSelect(evt){
        var files = evt.target.files;
        var file = files[0];
      
      if (files && file) {
          var reader = new FileReader();
  
          reader.onload =this._handleReaderLoaded.bind(this);
  
          reader.readAsBinaryString(file);
      }
    }
    
    _handleReaderLoaded(readerEvt) {
       var binaryString = readerEvt.target.result;
              this.base64textString= btoa(binaryString);
      }
   
}
