import { Component, OnInit } from '@angular/core';
import { Member } from 'src/app/_modules/member';
import { MembersService } from 'src/app/_services/members.service';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css']
})
export class MemberListComponent implements OnInit {

  //class property to store our members
  members: Member[] = [];


  constructor(private memberService: MembersService) { }

  ngOnInit(): void {
    this.loadMembers();
  }

  loadMembers() {
    //returns an observable
    this.memberService.getMembers().subscribe({
      next: members => this.members = members
    })
  }

}
