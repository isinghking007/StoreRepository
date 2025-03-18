import { Component, OnInit } from '@angular/core';
import { CommonservicesService } from '../../../Services/commonservices.service';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { Router } from '@angular/router';
@Component({
  selector: 'app-login',
  standalone: true,
  imports: [ReactiveFormsModule,RouterModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent implements OnInit {

  login:FormGroup;
  auth_token:any;
  constructor(private service:CommonservicesService,private fb:FormBuilder,private router:Router){
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
        console.log("login success"+data);
        this.auth_token=data;
        console.log("auth token data"+this.auth_token);
        this.service.saveToken(this.auth_token);
        console.log(data);
        if (this.service.isAuthenticated()) {
          console.log("Token saved successfully. Navigating to home...");
          this.router.navigate(['/home']);
        } else {
          console.error("Failed to save token.");
        }
//        this.router.navigate(['/home']);
     
      });
    }
  }
}
