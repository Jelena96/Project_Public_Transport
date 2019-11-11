import {TicketType} from '../ticket-type';
import {Passenger} from '../passenger';

export class Ticket {
    Id: number;
    TicketType: TicketType;
    Passenger: Passenger;
    IsValid: boolean;
    TimeIssued: string;
    IdTypeOfUser: number;
    PayPalId: string;
    IdTicketType: number;
}
