import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, map } from 'rxjs';
import { User } from '../_models/user';
import { environment } from 'src/environments/environment';

//service is initialized when our app is initialzed and not destroyed till our application is finished.therefore using for http req instead of component(shortlived) and state is destroyed too. we want our application to remember where our user is

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  baseUrl = environment.apiUrl;
  private currentUserSource = new BehaviorSubject<User | null>(null);
  currentUser$ = this.currentUserSource.asObservable();// $ signifies that it is a observable


  constructor(private http: HttpClient) { }

  login(model:any){
    return this.http.post<User>(this.baseUrl + 'account/login',model).pipe(
      map((response:User)=>{
        const user = response;
        if(user) {
          // localStorage.setItem('user',JSON.stringify(user));
          // this.currentUserSource.next(user);//added in the method
          this.setCurrentUser(user);
        }
      })
    )
  }

  register(model:any){
    return this.http.post<User>(this.baseUrl + 'account/register',model).pipe(
      map(user =>{
        if(user) {
          // localStorage.setItem('user',JSON.stringify(user));
          // this.currentUserSource.next(user);

          this.setCurrentUser(user);
        }
        // return user;
      })
    )
  }

  setCurrentUser(user:User) {
    user.roles=[];
    const roles = this.getDecodedToken(user.token).role;
    Array.isArray(roles) ? user.roles= roles: user.roles.push(roles);
    localStorage.setItem('user',JSON.stringify(user));
    this.currentUserSource.next(user);
  }

  logout() {
    localStorage.removeItem('user');
    this.currentUserSource.next(null);

  }

  getDecodedToken(token: string) {
    return JSON.parse(atob(token.split('.')[1]));
  }

}
