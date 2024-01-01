import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { AccountService } from '../_services/account.service';
import { ToastrService } from 'ngx-toastr';
import { AbstractControl, FormBuilder, FormControl, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  // @Input() usersFromHomeComponent:any;
  @Output() cancelRegister = new EventEmitter//emiter bcz we want to emit something from the child component
  // model : any={}// replaced this.model in register method with register form since we are using reactiveforms now
  registerForm: FormGroup = new FormGroup({});//creating reactive forms
  maxDate: Date = new Date();
  validationErrors: string[] | undefined;

  constructor(private accountService : AccountService, private toastr: ToastrService, private fb: FormBuilder, private router: Router) { }

  ngOnInit(): void {
    this.initializeForm();
    this.maxDate.setFullYear(this.maxDate.getFullYear() -18);
  }

  // initializeForm() {
  //   this.registerForm = new FormGroup({
  //     username: new FormControl("",Validators.required),
  //     password: new FormControl('',[Validators.required,Validators.minLength(4),Validators.maxLength(12)]),
  //     confirmPassword: new FormControl('',[Validators.required,this.matchValues('password')]),
  //   });
  //   this.registerForm.controls['password'].valueChanges.subscribe({
  //     next: () => this.registerForm.controls['confirmPassword'].updateValueAndValidity()
  //   })

  // }

  //we used FormBuilder service like developers usually do therefore changing the function code

  initializeForm() {
    this.registerForm = this.fb.group({
      gender: ['male'],//passing as an array now
      username: ["",Validators.required],
      knownAs: ["",Validators.required],
      Weight: ["",Validators.required],
      dateOfBirth: ["",Validators.required],
      city: ["",Validators.required],
      country: ["",Validators.required],
      password: ['',[Validators.required,Validators.minLength(4),Validators.maxLength(8)]],
      confirmPassword: ['',[Validators.required,this.matchValues('password')]],
    });
    this.registerForm.controls['password'].valueChanges.subscribe({
      next: () => this.registerForm.controls['confirmPassword'].updateValueAndValidity()
    })

  }

  matchValues(matchTo: string): ValidatorFn{
    return (control: AbstractControl)=> {
      return control.value === control.parent?.get(matchTo)?.value ? null : {notMatching: true}
    }
    
  }

  register(){
    // console.log(this.registerForm?.value);
    // // console.log(this.model);
    const dob = this.getDateOnly(this.registerForm.controls['dateOfBirth'].value);
    const weight = this.registerForm.controls['Weight'].value.toString();
    const values = {...this.registerForm.value, dateOfBirth: dob, Weight: weight}; 


    this.accountService.register(values).subscribe({
      next: () =>{
        // console.log(response);
        // this.cancel();
        this.router.navigateByUrl('/members')
      },
      error: error=>{
        // this.toastr.error(error.error),
        // console.log(error)
        console.error('Registration error:', error);
        this.validationErrors = error;
        this.isPasswordValid();
      }
    })
  }

  cancel(){
    // console.log("cancelled");
    this.cancelRegister.emit(false)//emit is the property of event emitter used to emit things
  }

  private getDateOnly(dob: string | undefined){
    if(!dob) return;
    let theDob = new Date(dob);
    return new Date(theDob.setMinutes(theDob.getMinutes()-theDob.getTimezoneOffset())).toISOString().slice(0,10);//slice timestamp and get only the date
  }

  getObjectValues(obj: any): string[] {
    return Object.values(obj);
  }

  isPasswordValid(): boolean {
    console.log("Incorrect Validation for password");
    return (
      false
    );
  }

}
