import { Component, OnInit } from '@angular/core';
import { PriceListService } from '../price-list.service';
import { FormBuilder, Validators } from '@angular/forms';
import { PriceList } from '../models/PriceList';
import { DatePicker } from '../models/DatePicker';
import { Passenger } from '../passenger';
import { Ticket } from '../models/ticket';
import { Observable } from 'rxjs';
import { PassengerType } from '../passenger-type';
import { TicketType } from '../ticket-type';
import { DatePipe } from '@angular/common';
import { RouteConfigLoadEnd } from '@angular/router';
import { ProfileService } from '../profile-service.service';
import { RegistrateUser } from '../models/RegistrateUser';
import { BuyticketService } from '../buyticket.service';

@Component({
  selector: 'app-price-list',
  templateUrl: './price-list.component.html',
  styleUrls: ['./price-list.component.css']
})


export class PriceListComponent implements OnInit {
  Students = {
      dob:'',
      dob2:''

  }

  public priceList: PriceList;
  public model: DatePipe;
  public model2: DatePipe;
  public passenger: PassengerType = new PassengerType();
  public ticket: TicketType = new TicketType();
  public basePrice: number;
  public discount: number;
  public ticketType: TicketType;
  public passengerType: PassengerType;
  public ticketTypeHelp:TicketType;
  public passTypeHelp : any;
  public ticketId: number;
  public pasId: number;
  public ticket2: Ticket;
  public priceLists: PriceList[];
  public To: string;
  public From: string;
  public TicketPrice: number;
  public TicketTypeId: number;
  public PassengerTypeId: number;
  public id: number;
  public newPriceList: PriceList = new PriceList();
  public CurrentValid: boolean;

  sklForm = this.fb.group({
    ticket: ['', Validators.required],
    passenger: ['', Validators.required],
    basePrice: ['', Validators.required],
    discount: ['', Validators.required],

  });
  
  updateForm = this.fb.group({
    From: [this.From, Validators.required],
    To: [this.To, Validators.required],
    TicketTypeId: [this.TicketTypeId, Validators.required],
    PassengerTypeId: [this.PassengerTypeId, Validators.required],
    TicketPrice: [this.TicketPrice, Validators.required],
    CurrentValid: [this.CurrentValid, Validators.required],
    Id: [''],

  });

  TicketType:Array<Object> = [
    {name: "Dnevna"},
    {name: "Mesecna"},
    {name: "Godisnja"},
    {name: "Vremenska"},
  
  ];

  PassengerType:Array<Object> = [
    {name: "Penzioner"},
    {name: "Student"},
    {name: "Regularni"},
  
  ];

  constructor(private priceListService: PriceListService,private fb: FormBuilder,private buyTicketService:BuyticketService) { }

  async ngOnInit() {
    
    this.priceLists = await this.priceListService.getAllPriceList();
  }

  updateTicket()
  {
    this.basePrice = this.sklForm.controls['basePrice'].value;
    this.priceListService.getTicketById(this.ticketId).subscribe(data => {
      this.ticket = data;
      this.ticket.Price = this.basePrice;
      this.priceListService.updateTicketTypeById(this.ticket).subscribe((data) => {
      });
     });

  }

  updatePassenger()
  {
    this.discount = this.sklForm.controls['discount'].value;
    this.priceListService.getPassengerById(this.pasId).subscribe(data => {
      this.passenger=data;
      this.passenger.Discount = this.discount;
      this.priceListService.updatePassengerTypeById(this.passenger).subscribe((data) => {
      });
    }); 
  }
   AddSubmit(){

     let to = this.model;
     let from = this.model2;
     let tick = this.sklForm.controls['ticket'].value;
     let pas = this.sklForm.controls['passenger'].value;
  
     if(pas == "Student")
     {
      this.pasId = 1;      
     }
     else if(pas == "Penzioner")
     {
      this.pasId = 2;     
     }
     else
     {
        this.pasId = 3;
     }
   
      if(tick == "Vremenska")
      {
        this.ticketId = 1;      
      }
      else if(tick == "Dnevna")
      {
        this.ticketId = 2;     
       }
       else if(tick == "Mesecna")
       {
        this.ticketId = 3;
       }else{

        this.ticketId = 4;
       }

        this.updateTicket();
        this.updatePassenger();
       

      const d = new DatePipe('en-US').transform(this.Students.dob,'dd/MM/yyyy');
      const d2 = new DatePipe('en-US').transform(this.Students.dob2,'dd/MM/yyyy')

      let price = this.basePrice*this.discount;
      this.priceList = new PriceList();
      this.priceList.From = d;
      this.priceList.To = d2;
      this.priceList.PassengerTypeId = this.pasId;
      this.priceList.TicketPrice = price;
      this.priceList.TicketTypeId = this.ticketId;
      this.priceList.CurrentValid = true;
      this.priceListService.createPriceList(this.priceList).subscribe(  
        () => {  
          this.ngOnInit();
          alert("Successfully added pricelist");
        }  
      );  
     }   

     public Delete(selectedSchedule: any)
     {
       this.priceListService.deletePriceListById(selectedSchedule.Id).subscribe((data) => {
         this.ngOnInit();
         alert("Successfully deleted pricelist");

       });
       
     }

    public UpdateSubmit()
    {
     // this.newPriceList = new PriceList(this.id,this.From,this.To,this.PassengerTypeId,this.TicketPrice,this.TicketTypeId,this.CurrentValid);
      this.priceListService.updatePriceList(this.newPriceList).subscribe((data) => {
        this.ngOnInit();
        if(data)
          alert("Successfully updated pricelist");

      });
    }

  public Update(selectedSchedule: any)
  {
    // this.CurrentValid = selectedSchedule.CurrentValid;
    // this.To = selectedSchedule.To;
    // this.From = selectedSchedule.From;
    // this.id = selectedSchedule.Id; 
    // this.TicketPrice = selectedSchedule.TicketPrice;
    // this.PassengerTypeId = selectedSchedule.PassengerTypeId;
    // this.TicketTypeId = selectedSchedule.TicketTypeId;
    this.newPriceList = selectedSchedule;
  }
}
