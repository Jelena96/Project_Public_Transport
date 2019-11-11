import { Component, OnInit } from '@angular/core';
import { Validators, FormBuilder } from '@angular/forms';
import { RegistrateUser } from '../models/RegistrateUser';
import { ProfileService } from '../profile-service.service';


@Component({
  selector: 'app-user-profile',
  templateUrl: './user-profile.component.html',
  styleUrls: ['./user-profile.component.css']
})
export class UserProfileComponent implements OnInit {

  regUser: RegistrateUser;
  mySrc;
  message: string;
  isRegular: boolean = false;

  updateUserForm = this.fb.group({
    Email: ['', Validators.email],
    LastName: ['', Validators.required],
    Address: ['', Validators.required],
    FirstName: ['', Validators.required],
  });

  constructor(public profileService: ProfileService,private fb: FormBuilder) {
    this.showProfile();
   }

  ngOnInit() {
  }

  showProfile():void{
    this.profileService.showProfile(localStorage.email).subscribe(regUser=>{
    this.regUser=regUser;
    this.updateUserForm.controls['FirstName'].setValue(this.regUser.FirstName);
    this.updateUserForm.controls['LastName'].setValue(this.regUser.LastName);
    this.updateUserForm.controls['Address'].setValue(this.regUser.Address);
    this.updateUserForm.controls['Email'].setValue(this.regUser.Email);
    this.mySrc ="data:image/png;base64," + this.regUser.ImageUrl;
    console.log(this.regUser.FirstName);
    if(this.regUser.IDtypeOfUser == 3)
      this.isRegular = true;
    
    });
  }

  update(){
    this.regUser.FirstName =this.updateUserForm.controls['FirstName'].value;
    this.regUser.LastName =this.updateUserForm.controls['LastName'].value;
    this.regUser.Address =this.updateUserForm.controls['Address'].value;
    this.regUser.Email =this.updateUserForm.controls['Email'].value;
    this.regUser.ImageUrl = this.base64textString;
    this.profileService.update(this.regUser).subscribe(ok=>{
    this.message=ok;
    if(this.message=="ok")
      this.showProfile();
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
            this.mySrc="data:image/png;base64," + this.base64textString;
    }

}
