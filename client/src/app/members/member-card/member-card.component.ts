import { Component, Input, OnInit, ViewEncapsulation } from '@angular/core';
import { Member } from 'src/app/_modules/member';

@Component({
  selector: 'app-member-card',
  templateUrl: './member-card.component.html',
  styleUrls: ['./member-card.component.css'],
  // encapsulation: ViewEncapsulation.Emulated//default
})
export class MemberCardComponent implements OnInit {
  @Input() member: Member| undefined;

  constructor() { }

  ngOnInit(): void {
  }

}
