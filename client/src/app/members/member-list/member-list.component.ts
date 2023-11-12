import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { Member } from 'src/app/_models/member';
import { MembersService } from 'src/app/_services/members.service';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css']
})
export class MemberListComponent implements OnInit {

  //class property to store our members
  // members: Member[] = [];//f//making it an observable
  members$: Observable<Member[]> | undefined;


  constructor(private memberService: MembersService) { }

  ngOnInit(): void {
    // this.loadMembers();//f
    this.members$= this.memberService.getMembers();
  }

  // loadMembers() {
  //   //returns an observable
  //   this.memberService.getMembers().subscribe({
  //     next: members => this.members = members
  //   })
  // }//f//removed after we created the member as observable

}
