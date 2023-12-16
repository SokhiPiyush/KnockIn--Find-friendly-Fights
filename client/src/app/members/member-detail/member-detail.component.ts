import { CommonModule } from '@angular/common';
import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { GalleryItem, GalleryModule, ImageItem } from 'ng-gallery';
// import { NgxGalleryImage } from '@kolkov/ngx-gallery';
import { TabDirective, TabsModule, TabsetComponent } from 'ngx-bootstrap/tabs';
import { Member } from 'src/app/_models/member';
import { MembersService } from 'src/app/_services/members.service';
import { TimeagoModule } from 'ngx-timeago';
import { MemberMessagesComponent } from '../member-messages/member-messages.component';
import { MessageService } from 'src/app/_services/message.service';
import { Message } from 'src/app/_models/message';
import { PresenceService } from 'src/app/_services/presence.service';
import { AccountService } from 'src/app/_services/account.service';
import { take } from 'rxjs';
import { User } from 'src/app/_models/user';

@Component({
  selector: 'app-member-detail',
  standalone: true,
  templateUrl: './member-detail.component.html',
  styleUrls: ['./member-detail.component.css'],
  imports: [GalleryModule,CommonModule,TabsModule,TimeagoModule,MemberMessagesComponent]
})
export class MemberDetailComponent implements OnInit, OnDestroy {
  @ViewChild('memberTabs', {static: true}) memberTabs?: TabsetComponent;
  member: Member = {} as Member;
  images: GalleryItem[] = [];
  activeTab?: TabDirective;
  messages: Message[]=[];
  user?: User;

  constructor(private accountService: AccountService, private route: ActivatedRoute, private messageService: MessageService, public presenceService: PresenceService, private router: Router) {

    this.accountService.currentUser$.pipe(take(1)).subscribe({
      next: user => {
        if (user) this.user = user;
      }
    });

    this.router.routeReuseStrategy.shouldReuseRoute = () => false;

   }

  ngOnInit(): void {
    // this.loadMember();
    this.route.data.subscribe({
      next: data => this.member = data['member']
    })

    this.route.queryParams.subscribe({
      next: params => {
        params['tab'] && this.selectTab(params['tab'])
      }
    })

    this.getImages()


  }

  ngOnDestroy(): void {
    this.messageService.stopHubConnection();
  }

  // loadMember() {
  //   const username = this.route.snapshot.paramMap.get('username');
  //   if (!username) return;
  //   this.memberService.getMember(username).subscribe({
  //     next: member => {
  //       this.member = member,
  //     }
  //   })
  // }

  selectTab(heading: string){
    if (this.memberTabs){
      this.memberTabs.tabs.find(x=>x.heading === heading)!.active = true;//!. turns off type safety for typescript
    }
  }

  loadMessages() {
    if (this.member) {
      this.messageService.getMessageThread(this.member.userName).subscribe({
        next: messages => this.messages = messages
      })
    }
  }

  onTabActivated(data: TabDirective){
    this.activeTab = data;
    if (this.activeTab.heading === 'Messages' && this.user) {
      this.messageService.createHubConnection(this.user, this.member.userName);
    } else {
        this.messageService.stopHubConnection();
    }
  }

  getImages() {
    if (!this.member) return;
    for (const photo of this.member?.photos) {
      this.images.push(new ImageItem({src: photo.url, thumb: photo.url}));
    }
  }

}