// import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {

  registerMode = false;
  users: any;

  constructor() { }//removed injected private http: HttpClient//e

  ngOnInit(): void {
    // this.getUsers();//e
  }

  registerToggle() {
    this.registerMode = !this.registerMode;
  }

  // getUsers() {
  //   this.http.get('https://localhost:5001/api/users').subscribe({
  //     next: (response) => this.users = response,
  //     error: error => console.log(error),
  //     complete: () => console.log("req has completed")
  //   })

  // }// we made member service and did this thing in memberlist component//e

  cancelRegisterMode(event:boolean){
    this.registerMode = event;

  }

}
