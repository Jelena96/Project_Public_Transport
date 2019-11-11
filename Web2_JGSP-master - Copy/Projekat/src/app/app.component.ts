import { Component, NgModule, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})



export class AppComponent implements OnInit{
  ngOnInit(): void {

    this.method();

}


  constructor(private router: Router){}
  public show: boolean = false

  title = 'Projekat';

  public method()
  {
      if(localStorage.role == 'Admin')
      {
        this.show = true;

      }


  }

  logout(): void {
    localStorage.removeItem('jwt');
    localStorage.removeItem('role');
    window.location.reload();
  }

}

