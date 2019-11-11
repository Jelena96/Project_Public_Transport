import { Component, OnInit } from '@angular/core';
import { RegistrateUser } from '../models/RegistrateUser';
import { ValidateService } from '../validate-service.service';

@Component({
  selector: 'app-controlor-profile',
  templateUrl: './controlor-profile.component.html',
  styleUrls: ['./controlor-profile.component.css']
})
export class ControlorProfileComponent implements OnInit {

  users:RegistrateUser[];
  allUsers:boolean;
  user:RegistrateUser;
  ok;

  constructor(public validateService: ValidateService) { 
      this.getUsers();
  }

  ngOnInit() {
  }

  
  getUsers():void{
    this.validateService.getUsers().subscribe(Users=>{
      this.users = Users;
      this.users.forEach(obj =>{obj.ImageUrl = "data:image/png;base64,"+obj.ImageUrl;} );
      
      if(this.users!=null){
        this.allUsers = true;
      }else
        this.allUsers=false;
    });
  }
 
  validateProfile(item){
    this.user = new RegistrateUser();
    this.user.VerificationStatus="Valid";
    this.user.Email=item;
    this.validateService.validateProfile(this.user).subscribe(ok=>{this.ok=ok;this.getUsers();});
  }

  rejectProfile(item){
    this.user = new RegistrateUser();
    this.user.VerificationStatus="Invalid";
    this.user.Email=item;
    this.validateService.validateProfile(this.user).subscribe(ok=>{this.ok=ok;this.getUsers();});
  }

}
