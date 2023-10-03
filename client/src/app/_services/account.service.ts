import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, map } from 'rxjs';
import { User } from '../_models/user';

//service is initialized when our app is initialzed and not destroyed till our application is finished.therefore using for http req instead of component(shortlived) and state is destroyed too. we want our application to remember where our user is

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  baseUrl = 'https://localhost:5001/api/';
  private currentUserSource = new BehaviorSubject<User | null>(null);
  currentUser$ = this.currentUserSource.asObservable();// $ signifies that it is a observable


  constructor(private http: HttpClient) { }

  login(model:any){
    return this.http.post<User>(this.baseUrl + 'account/login',model).pipe(
      map((response:User)=>{
        const user = response;
        if(user) {
          localStorage.setItem('user',JSON.stringify(user));
          this.currentUserSource.next(user);
        }
      })
    )
  }

  register(model:any){
    return this.http.post<User>(this.baseUrl + 'account/register',model).pipe(
      map(user =>{
        if(user) {
          localStorage.setItem('user',JSON.stringify(user));
          this.currentUserSource.next(user);
        }
        // return user;
      })
    )
  }

  setCurrentUser(user:User) {
    this.currentUserSource.next(user);
  }

  logout() {
    localStorage.removeItem('user');
    this.currentUserSource.next(null);

  }
}
