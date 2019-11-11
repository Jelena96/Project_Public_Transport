import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Observable } from 'rxjs/internal/Observable';
import { BuyticketService } from '../buyticket.service';
import { Ticket } from '../models/ticket';
import { Email } from '../models/Email';
import { PayPal } from '../models/PayPal';
import { PriceList } from '../models/PriceList';
import { PriceListService } from '../price-list.service';
import { TicketType } from '../ticket-type';

declare let paypal: any;

@Component({
  selector: 'app-buy-ticket',
  templateUrl: './buy-ticket.component.html',
  styleUrls: ['./buy-ticket.component.css']
})

export class BuyTicketComponent implements OnInit {
  
  public email:Email = new Email;
  t: Ticket;
  tickets: Ticket[] = [];
  mes: boolean = false;
  Rola: boolean = true;
  TimeType: boolean = false;
  ticket: Ticket = new Ticket;
  payPal: PayPal = new PayPal;
  priceLists: TicketType[];
   //for paypal
   addScript: boolean = false;
   paypalLoad: boolean = true;   
   finalAmount: number = 1;
    tickets_p: Ticket[] = [];
  buyForm = this.fb.group({
    email: ['', Validators.required]
  })

  constructor(private priceListService: PriceListService,private buyTicketService:BuyticketService, private fb:FormBuilder) { }  
  
  ngOnInit() 
  {  
    this.showTickets();

  } 
      
  async createTicket() 
  { 
    this.priceLists = await this.priceListService.getAllTicketTypes();
    //find current price for timeticket
    this.priceLists.forEach(element => {
      if(element.Id == 1)
          this.finalAmount = element.Price + 0.00;
      });

    if (!this.addScript) 
    {
      this.addPaypalScript().then(() => {
        paypal.Button.render(this.paypalConfig, '#paypal-checkout-btn');
        this.paypalLoad = false;
      })
      }      
  }
      
  showTickets():void
  {
    // Without duplicate
    this.tickets = [];
    this.tickets_p = [];
    this.buyTicketService.showTickets().subscribe(tickets=>{
      //this.tickets = tickets;
      tickets.forEach(element => {
        if(element.IdTicketType == 1)
          this.tickets_p.push(element);
      });

      this.tickets = this.tickets_p;
      if(this.tickets != null)
        this.mes = true; 
      });
   
       
    
    if(localStorage.role != 'AppUser')
    {
      this.Rola = false;
    }   
  }   
  
  checkIn(id)
  {
    this.t = new Ticket();
    this.t.Id = id;
    
    this.buyTicketService.checkIn(this.t).subscribe((data)=>{
      this.showTickets();

      });
  }
    
      paypalConfig = {
        env: 'sandbox',
        client: {
          sandbox: 'AVm6dV0IRSqYKnqlsk0Nd_QGdhpD9JjSK-Lm_9jzHYUeM5qVd9gLqgdAn-Ra2I4_P8Q7znRMQi09grGQ',
          production: '<your-production-key here>'
        },
        commit: true,
        payment: (data, actions) => {
          return actions.payment.create({
            payment: {
              transactions: [
                { amount: { total: this.finalAmount, currency: 'USD' } }
              ]
            }
          });
        },
        onAuthorize: (data, actions) => {
          return actions.payment.execute().then((payment) => {
            //Do something when payment is successful.
            this.payPal.cart = payment.cart;
            this.payPal.createTime = payment.create_time;
            this.payPal.paypalId = payment.id;
            this.payPal.email = payment.payer.payer_info.email;
            this.payPal.firstName = payment.payer.payer_info.first_name;
            this.payPal.lastName = payment.payer.payer_info.last_name;
            this.payPal.payerId = payment.payer.payer_info.payer_id;
            this.payPal.paymentMethod = payment.payer.payment_method;
            this.payPal.status = payment.payer.status;
            this.payPal.state = payment.state;
            this.payPal.currency = payment.transactions[0].amount.currency;
            this.payPal.total = payment.transactions[0].amount.total;

            this.email.Value = this.buyForm.controls['email'].value;
            this.email.PayPalId = payment.id;
            
            this.buyTicketService.createTicketNotRegisteredUser(this.email).subscribe(  
              (data) => {  
                if(data){
                  this.showTickets();
                  // this.buyTicketService.getAllTicket().subscribe((data)=>{
                  //   this.tickets = data;
                  // });
                this.buyTicketService.savePayPal(this.payPal).subscribe((data) => {
                });
                alert("You have successfully purchased the ticket!")

              }
              else
              {
                alert("The purchase was not successful")

              }
              }  
            );  
          })
        }
      };

      addPaypalScript() {
        this.addScript = true;
        return new Promise((resolve, reject) => {
          let scripttagElement = document.createElement('script');    
          scripttagElement.src = 'https://www.paypalobjects.com/api/checkout.js';
          scripttagElement.onload = resolve;
          document.body.appendChild(scripttagElement);
        })
      }
  }  