import { Component, OnInit } from '@angular/core';
import {MatGridListModule} from '@angular/material/grid-list';
import { Router } from '@angular/router';
import { CommonservicesService } from '../../../Services/commonservices.service';

export interface Tile {
  color: string;
  cols: number;
  rows: number;
  text: string;
}


@Component({
  selector: 'app-home',
  standalone: true,
  imports: [MatGridListModule],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent implements OnInit {
  tiles: Tile[] = [
    {text: 'One', cols: 3, rows: 1, color: 'lightblue'},
    {text: 'Two', cols: 1, rows: 2, color: 'lightgreen'},
    {text: 'Three', cols: 1, rows: 1, color: 'lightpink'},
    {text: 'Four', cols: 2, rows: 1, color: '#DDBDF1'},
  ];
  output:string="";
  isAuthenticated:boolean=false;
  isMenuOpen = false;
  isMenuClicked=true;
  loggedInUserEmail:string="";
  loggedInUserName:string="";
  constructor(private route:Router,private authService:CommonservicesService){
    this.isAuthenticated = this.authService.isAuthenticated();
    
  }
 

  ngOnInit(){
    this.isMenuClicked=!this.isMenuClicked;
    console.log(this.authService.getDecodedUserDetails().email);
    const decodeDetails= this.authService.getDecodedUserDetails();
    this.authService.getUserDetails(this.authService.getDecodedUserDetails().phone||"").subscribe((data:any)=>{
      console.log(data);
      this.loggedInUserName=(data.firstName+" "+data.lastName) || "Guest";
    }
);
    this.loggedInUserEmail=decodeDetails.email||"Guest";
  }

  logout() {
    this.authService.logout();
    this.route.navigate(['/login']);
  }
  toggleAuth(){ 
    this.isMenuClicked=!this.isMenuClicked;
}
}
