import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { AccountService } from '../_services/account.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  // @Input() usersFromHomeComponent:any;
  @Output() cancelRegister = new EventEmitter//emiter bcz we want to emit something from the child component
  model : any={}

  constructor(private accountService : AccountService, private toastr: ToastrService) { }

  ngOnInit(): void {
  }

  register(){
    // console.log(this.model);
    this.accountService.register(this.model).subscribe({
      next: () =>{
        // console.log(response);
        this.cancel();
      },
      error: error=>{
        this.toastr.error(error.error),
        console.log(error)}
    })
  }

  cancel(){
    // console.log("cancelled");
    this.cancelRegister.emit(false)//emit is the property of event emitter used to emit things
  }

}
