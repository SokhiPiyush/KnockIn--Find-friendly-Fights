import { CommonModule } from '@angular/common';
import { ChangeDetectionStrategy, Component, Input, OnInit, ViewChild } from '@angular/core';
import { Message } from 'src/app/_models/message';
import { MessageService } from 'src/app/_services/message.service';
import { TimeagoModule } from 'ngx-timeago';
import { FormsModule, NgForm } from '@angular/forms';

@Component({
  changeDetection: ChangeDetectionStrategy.OnPush,
  selector: 'app-member-messages',
  standalone: true,
  templateUrl: './member-messages.component.html',
  styleUrls: ['./member-messages.component.css'],
  imports: [CommonModule,TimeagoModule,FormsModule]

})
export class MemberMessagesComponent implements OnInit {
  @ViewChild('messageForm') messageForm?: NgForm
  @Input() username?: string;
  // @Input() messages: Message[] = [];
  messageContent = '';

  constructor(public messageService : MessageService) { }

  ngOnInit(): void {
    // this.loadMessages();
  }

  // loadMessages() {
  //   if (this.username) {
  //     this.messageService.getMessageThread(this.username).subscribe({
  //       next: messages => this.messages = messages
  //     })
  //   }
  // }//moved to member-detail component

  sendMessage(){
    if (!this.username) return;
    this.messageService.sendMessage(this.username, this.messageContent).then(()=> {
      this.messageForm?.reset();
    })

  }

}
