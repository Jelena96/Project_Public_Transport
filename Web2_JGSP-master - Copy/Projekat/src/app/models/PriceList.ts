import { DatePicker } from './DatePicker';
import { DatePipe } from '@angular/common';

export class PriceList{
    Id:number;
    From: string;
    To: string;
    TicketPrice: number;
    PassengerTypeId: number;
    TicketTypeId: number;
    CurrentValid: boolean;

    // constructor(private id: number, private from: string, private to:string,private passengerTypeId:number,private ticketPrice:number,private ticketTypeId : number,private currentValid:boolean) { 
    //     this.Id = id;
    //     this.From = from;
    //     this.To = to;
    //     this.PassengerTypeId = passengerTypeId;
    //     this.TicketTypeId = ticketTypeId;
    //     this.TicketPrice = ticketPrice;
    //     this.CurrentValid = currentValid;
    //   }

}