import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit {

  constructor() { }

  ngOnInit() {
    this.method();
  }

  public show: boolean = false
  showController: boolean = false;

  title = 'Projekat';

  public method()
  {
      if(localStorage.role == 'Admin')
      {
        this.show = true;
      }

      if(localStorage.role == 'Controller')
      {
        this.showController = true;
      }
  }
}
