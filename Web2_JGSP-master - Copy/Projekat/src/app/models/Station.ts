import { Coordinate } from './Coordinate';
import { Line } from './Line';

export class Station{
    Id: number;
    Name: string;
    Address : string;
    Coordinate : Coordinate;
    Lines : Line[];
}