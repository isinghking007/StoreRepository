import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { CommonservicesService } from '../../../../Services/commonservices.service';

@Component({
  selector: 'app-signup',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './signup.component.html',
  styleUrl: './signup.component.css'
})
export class SignupComponent implements OnInit {

  signup:FormGroup;


  constructor(private fb:FormBuilder,private service:CommonservicesService) {

    this.signup=this.fb.group({
      firstname:['',Validators.required],
      lastname:['',Validators.required],
      username:['',Validators.required],
      email:['',Validators.required],
      mobileNumber:['',Validators.required],
      password:['',Validators.required]
    })
   }

  ngOnInit(): void {
  }

  onSubmit(){
    console.log(this.signup.value);
    const updatedFormValue={...this.signup.value,companyId:0};
    // updatedFormValue.array.forEach((element:any) => {
    //   console.log(element);
    // });
    console.log(updatedFormValue.companyId);
    if(this.signup.valid){
      this.service.addUserDetails(updatedFormValue).subscribe((data)=>{
        console.log(data);
      })
    }
  }
}
