import { Component, OnInit } from '@angular/core';
import { AccountService } from '../_services/account.service';
import { Observable, of } from 'rxjs';
import { User } from '../_models/user';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {
  model: any = {}
  // loggedIn = false;//a
  // currentUser$: Observable<User | null> = of(null);//b

  constructor(public accountService: AccountService, private router: Router, private toastr: ToastrService) { }

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
        this.router.navigateByUrl('/members')
        // console.log(response);
        // this.loggedIn = true;//a
      },
      error: error=> {
        this.toastr.error(error.error)
        console.log(error)}
    })
  }


  logout() {
    this.accountService.logout();
    this.router.navigateByUrl('/');
    // this.loggedIn = false;//a
  }

}


