import { Component, OnInit } from '@angular/core';
import { CommonservicesService } from '../../../Services/commonservices.service';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { Router } from '@angular/router';

export interface authResult{
  expirationTime:Date;
  idToken:string;
}

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

  onSubmit() {
    console.log(this.login.value);
    if (this.login.invalid) {
      alert("Please fill in all required fields.");
      return;
    }
  
    this.service.login(this.login.value).subscribe(
      (data: any) => {
        console.log("data type",typeof(data));
        const parsed = JSON.parse(data);
        console.log("Login success", data);
        const date = new Date(parsed.expirationTime).getTime();
        console.log("Expiration Time (IST):", date);        
        const authResult = {
          idToken: parsed.idToken, // Replace with the actual token key from the API response
          expirationTime: date // Replace with the actual expiration time key
        };
        this.service.saveToken(parsed.idToken); // Save the token in local storage or session storage
        this.service.setSession(authResult); // Use setSession to handle token and expiration
        if (this.service.isLoggedIn()) {
          console.log("Token saved successfully. Navigating to home...");
          this.router.navigate(['/home']);
        } else {
          console.error("Failed to save token.");
        }
      },
      (error) => {
        console.error("Login failed", error);
        alert("Login failed. Please check your credentials.");
      }
    );
  }
}
