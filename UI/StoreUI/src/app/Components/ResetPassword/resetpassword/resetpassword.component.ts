import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { CommonservicesService } from '../../../Services/commonservices.service';

@Component({
  selector: 'app-resetpassword',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './resetpassword.component.html',
  styleUrl: './resetpassword.component.css'
})
export class ResetpasswordComponent implements OnInit{

  resetPassword:FormGroup;  
  isVerificationCodeEnabled:boolean=false;
  otpRecieved:boolean=false;
  outputValue:string="";
  isOutputValue:boolean=false;
  isPasswordRest:boolean=false ;
  constructor(private fb:FormBuilder,private service:CommonservicesService) {
    this.resetPassword=this.fb.group({
      userName:['',Validators.required],
      verificationCode:[''],
      newPassword:['']
    })
   }

   onSubmit(){
    console.log("on submit method called");
    if((this.resetPassword.value.userName!=null || this.resetPassword.value.userName !="") && 
      (this.resetPassword.value.verificationCode==null || this.resetPassword.value.verificationCode ==""))
    {
      this.service.resetUser(this.resetPassword.value).subscribe((data)=>{
        console.log(data);
         this.outputValue=data.toString();
         
        if(this.outputValue =="User is not registered, Please SignUp"){
          console.log(this.outputValue);
          this.isOutputValue=true;
          this.otpRecieved=true;
        }
        else{
          this.outputValue="OTP Sent to Registered Email Successfully";
          this.isVerificationCodeEnabled=true;
          this.otpRecieved=true;
        }
      });
      console.log("verification code is null");
      console.log(this.resetPassword.value.userName);
     
    }
    else{
      this.service.confirmPassword(this.resetPassword.value).subscribe((data)=>{
        console.log(data);
        this.outputValue=data.toString();
        console.log(this.outputValue);
        if(this.outputValue=="OK"){
          this.outputValue="Password Reset Successfully! Now you can login with new password";
          this.isPasswordRest=true;
         // this.isVerificationCodeEnabled=false;
        }
        else{
          this.isOutputValue=false;
        }
      });
    }
    console.log(this.resetPassword.value);
   }

  ngOnInit(): void {
  }

}
