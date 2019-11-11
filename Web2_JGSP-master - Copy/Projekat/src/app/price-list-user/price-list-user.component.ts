import { Component, OnInit, AfterViewChecked  } from '@angular/core';
import { PriceListService } from '../price-list.service';
import { PriceList } from '../models/PriceList';
import { Passenger } from '../passenger';
import { PassengerType } from '../passenger-type';
import { TicketType } from '../ticket-type';
import { RegistrateUser } from '../models/RegistrateUser';
import { Ticket } from '../models/ticket';
import { ProfileService } from '../profile-service.service';
import { BuyticketService } from '../buyticket.service';
import { PayPal } from '../models/PayPal';
import { FormGroup, FormBuilder } from '@angular/forms';

declare let paypal: any;

@Component({
  selector: 'app-price-list-user',
  templateUrl: './price-list-user.component.html',
  styleUrls: ['./price-list-user.component.css']
})
export class PriceListUserComponent implements OnInit  {

  public priceLists: PriceList[] = [];
  public passengers: PassengerType[] = [];
  public tickets: TicketType[] = [];
  public OneHourCardOnly: boolean = false;
  public user: RegistrateUser;
  public t: Ticket = new Ticket();
  pricelist: PriceList;
  public priceListsShow: PriceList[] = [];
  public payPal: PayPal = new PayPal;
  public pricelistform: FormGroup;
  public card: TicketType;
  
  //for paypal
  addScript: boolean = false;
  paypalLoad: boolean = true;
  finalAmount: number = 1;

  TypeCard:Array<Object> = [
    {name: "Vremenska"},
    {name: "Dnevna"},
    {name: "Mesecna"},
    {name: "Godisnja"},
];

TypePassenger:Array<Object> = [
  {name: "Student"},
  {name: "Penzioner"},
  {name: "Regularni"},
];
 
  constructor(private fb: FormBuilder,private priceListService: PriceListService,private profileService: ProfileService, private buyTicketService:BuyticketService)
  {
    this.pricelistform = this.fb.group({
      card: [''],
      passenger: ['']
    
    });
   }
  
  ngOnInit() 
  {
      if(localStorage.role == null)
      {
        this.OneHourCardOnly = true;
      }
  }

  async findPassenger()
  {
    let typeOfCard = this.pricelistform.controls['card'].value;
    let typeOfPass = this.pricelistform.controls['passenger'].value;
    this.priceListsShow = await this.priceListService.getPriceList(typeOfCard,typeOfPass);
    
    if(localStorage.role != null)
    {
      this.profileService.showProfile(localStorage.email).subscribe(regUser=>{
      this.user = regUser;
     });
    }
  }

  buyTicket(priceList){

    if(this.user.VerificationStatus != "Valid" )
    {
      alert("Your profile didn't validate by controllor")
    }
    else
    {
    this.pricelist = priceList;
    this.finalAmount = priceList.TicketPrice + 0.00;
    if (!this.addScript) {
      this.addPaypalScript().then(() => {
        paypal.Button.render(this.paypalConfig, '#paypal-checkout-btn');
        this.paypalLoad = false;
      })
      }
    }
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

          if(this.user.VerificationStatus == "Valid")
          {
            this.t.Passenger = new Passenger();
            this.t.Passenger.Id = this.pricelist.PassengerTypeId;
            this.t.TicketType = new TicketType();
            this.t.TicketType.Id = this.pricelist.TicketTypeId;
            this.t.IdTypeOfUser = this.pricelist.TicketTypeId;
            this.t.PayPalId = payment.id;
            console.log(payment.transactions[0].amount.total);
            this.buyTicketService.createTicket(this.t).subscribe((data) => {
              if(data!=null)
              {
                alert("You have successfully purchased the ticket!")
              }
              else
              {
                alert("The purchase was not successful")
              } 
                this.buyTicketService.savePayPal(this.payPal).subscribe((data) => {
                });
              });
            
            }
            else
            {
              alert("Kontroler nije verifikovao Vas profil.");
            } 
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
