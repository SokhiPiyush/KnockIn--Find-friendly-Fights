import { Component, HostListener, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { take } from 'rxjs';
import { User } from 'src/app/_models/user';
import { Member } from 'src/app/_modules/member';
import { AccountService } from 'src/app/_services/account.service';
import { MembersService } from 'src/app/_services/members.service';

@Component({
  selector: 'app-member-edit',
  templateUrl: './member-edit.component.html',
  styleUrls: ['./member-edit.component.css']
})
export class MemberEditComponent implements OnInit {
  @ViewChild('editForm') editForm: NgForm | undefined;//to access template in the component//template is a child to our component
  @HostListener('window:beforeunload', ['$event']) unloadNotification($event:any){
   if (this.editForm?.dirty) {
    $event.returnValue = true;
   } 
  }  //host is the browser//angular cant listen to changes outside the components therefore//window:beforeunload is a specified event
  member: Member |undefined;
  user: User | null=null;

  constructor(private accountService: AccountService, private memberService: MembersService, private toastr: ToastrService) {
    this.accountService.currentUser$.pipe(take(1)).subscribe({
      next: user => this.user = user
    })
   }

  ngOnInit(): void {
    this.loadMember();
  }

  loadMember(){
    if (!this.user) return;
    this.memberService.getMember(this.user.username).subscribe({
      next: member => this.member = member
    })
  }

  updateMember(){
    this.memberService.updateMember(this.editForm?.value).subscribe({
      next: _ => {
        console.log(this.member);

        this.toastr.success('Profile updated successfully')
        this.editForm?.reset(this.member);
      }
    })

  }

}
