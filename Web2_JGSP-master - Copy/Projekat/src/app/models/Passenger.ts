import { PassengerType } from '../passenger-type';
import { ApplicationUser } from '../application-user';

     export class Passenger extends ApplicationUser {
      Approved:string;
      PassengerType: PassengerType;
      Password: string;
}
            