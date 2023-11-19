import { Component, OnInit } from '@angular/core';
import { Observable, take } from 'rxjs';
import { Member } from 'src/app/_models/member';
import { Pagination } from 'src/app/_models/pagination';
import { User } from 'src/app/_models/user';
import { UserParams } from 'src/app/_models/userParams';
import { AccountService } from 'src/app/_services/account.service';
import { MembersService } from 'src/app/_services/members.service';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css']
})
export class MemberListComponent implements OnInit {

  //class property to store our members
  members: Member[] = [];//f//making it an observable
  // members$: Observable<Member[]> | undefined;
  pagination: Pagination | undefined;
  // pageNumber = 1;//inside userParams now
  // pageSize = 5;

  userParams: UserParams | undefined;
  // user: User | undefined;
  genderList = [{value: 'male', display: 'Males'},{value: 'female',display:'Females'},{value: 'others',display:'Others'}]


  constructor(private memberService: MembersService) {

    this.userParams = this.memberService.getUserParams();

    // this.accountService.currentUser$.pipe(take(1)).subscribe({
    //   next: user => {
    //     if(user) {
    //       this.userParams = new UserParams(user);
    //       this.user = user;
    //     }
    //   }
    // })// made this in member.service.ts file


   }

  ngOnInit(): void {
    this.loadMembers();//f
    // this.members$= this.memberService.getMembers();
  }

  loadMembers() {
    if(this.userParams){
      this.memberService.setUserParams(this.userParams);
      this.memberService.getMembers(this.userParams).subscribe({
        next: response => {
          if(response.result && response.pagination) {
            this.members = response.result;
            this.pagination = response.pagination;
          }
        }
      })

    }

  }

  resetFilters() {
    // this.userParams = new UserParams(this.user);//bring values back to default/made method in member.service

    this.userParams = this.memberService.resetUserParams();
    this.loadMembers();
  }

  pageChanged(event: any){
    if(this.userParams && this.userParams?.pageNumber !== event.page) {
      this.userParams.pageNumber = event.page;
      this.memberService.setUserParams(this.userParams);
      this.loadMembers();
    }

  }

  // loadMembers() {
  //   //returns an observable
  //   this.memberService.getMembers().subscribe({
  //     next: members => this.members = members
  //   })
  // }//f//removed after we created the member as observable

}
