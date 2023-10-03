import { Component, OnInit } from '@angular/core';
import { AccountService } from '../_services/account.service';
import { Observable, of } from 'rxjs';
import { User } from '../_models/user';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  model: any = {}
  // loggedIn = false;//a
  // currentUser$: Observable<User | null> = of(null);//b

  constructor(public accountService: AccountService) { }

  ngOnInit(): void {
    // this.getCurrentUser();//a
    // this.currentUser$ = this.accountService.currentUser$;//b

  }

  // getCurrentUser(){
  //   this.accountService.currentUser$.subscribe({
  //     next: user => this.loggedIn = !!user,// !! makes the object into a boolean , if user exists true else false
  //     error: error=> console.log(error)
  //   })
  // }   //a

  login() {
    // console.log(this.model);
    this.accountService.login(this.model).subscribe({
      next: response => {
        console.log(response);
        // this.loggedIn = true;//a
      },
      error: error=> console.log(error)
    })
  }


  logout() {
    this.accountService.logout();
    // this.loggedIn = false;//a
  }

}


