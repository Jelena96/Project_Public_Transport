import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { ValidateTicketService } from '../validate-ticket.service';

@Component({
  selector: 'app-controlor-tickets',
  templateUrl: './controlor-tickets.component.html',
  styleUrls: ['./controlor-tickets.component.css']
})
export class ControlorTicketsComponent implements OnInit {

  message:string;
  ticketForm = this.fb.group({
    idTicket: ['', Validators.required],
  });


  constructor(public validateTicketService: ValidateTicketService, private fb: FormBuilder) { 
    this.message="";
  }

  ngOnInit() {
  }

  isValid():void{
    
    if(this.ticketForm.value.idTicket == null){
      
      this.message="Invalid input";

    }else{

        this.validateTicketService.getInfo(this.ticketForm.value.idTicket).subscribe(data=>{
        this.message=data;

    });
  }
}

}
