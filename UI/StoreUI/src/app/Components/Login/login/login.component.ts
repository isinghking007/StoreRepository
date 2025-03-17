import { Component, OnInit } from '@angular/core';
import { CommonservicesService } from '../../../Services/commonservices.service';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { RouterModule } from '@angular/router';
@Component({
  selector: 'app-login',
  standalone: true,
  imports: [ReactiveFormsModule,RouterModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent implements OnInit {

  login:FormGroup;

  constructor(private service:CommonservicesService,private fb:FormBuilder){
    this.login=this.fb.group({
      phone:['',Validators.required],
      password:['',Validators.required]
    })
  }

  ngOnInit(): void {
    
  }

  onSubmit(){
    console.log(this.login.value);
    if(this.login.value){
      this.service.login(this.login.value).subscribe((data)=>{
        console.log(data);
      });
    }
  }
}
