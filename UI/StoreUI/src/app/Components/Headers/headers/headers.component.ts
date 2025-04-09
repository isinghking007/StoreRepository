import { Component, ElementRef, HostListener, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { CommonservicesService } from '../../../Services/commonservices.service';
import { WindowService } from '../../../Services/WindowService.service';

@Component({
  selector: 'app-headers',
  standalone: true,
  imports: [],
  templateUrl: './headers.component.html',
  styleUrl: './headers.component.css'
})
export class HeadersComponent implements OnInit{
  output:string="";
  isAuthenticated:boolean=false;
  isMenuOpen = false;
  isMenuClicked=true;
  screenWidth: number | undefined;
  loggedInUserEmail:string="";
  loggedInUserName:string="";
window: any;
isLargeScreen: boolean; // Declare without initialization
  constructor(private elementRef:ElementRef,private route:Router,private authService:CommonservicesService,private windowService:WindowService) {
    this.isAuthenticated = this.authService.isLoggedIn();
    this.screenWidth = this.windowService.nativeWindow?.innerWidth;
    this.isLargeScreen = (this.windowService.nativeWindow?.innerWidth ?? 0) >= 768; // Initialize here
    this.windowService.nativeWindow?.addEventListener('resize', () => {
      this.screenWidth = window.innerWidth;
      if (this.screenWidth < 768) {
        this.isMenuClicked = false; // Ensure menu stays hidden on small screens
      }
    });
  
  }
 

  ngOnInit(){
    console.log("inside header component",this.isAuthenticated);
    this.isMenuClicked=!this.isMenuClicked;
    console.log("decoded user details method",this.authService.getDecodedUserDetails().email);
    const decodeDetails= this.authService.getDecodedUserDetails();
    this.authService.getUserDetails(this.authService.getDecodedUserDetails().phone||"").subscribe((data:any)=>{
      console.log("from header component",data);
      this.loggedInUserName=(data.firstName+" "+data.lastName) || "Guest";
    }
);
    this.loggedInUserEmail=decodeDetails.email||"Guest";
    // if(this.isAuthenticated)
    // {
    //   this.refreshPage();
    // }
  }
  @HostListener('window:resize', [])
  onResize() {
    this.isLargeScreen = window.innerWidth >= 768;
  }
  @HostListener('document:click', ['$event'])
  onDocumentClick(event: MouseEvent) {
    const clickedInside = this.elementRef.nativeElement.contains(event.target);
    if (!clickedInside) {
      this.isMenuClicked = false; // Close the dropdown if clicked outside
    }
  }


  logout() {
    this.authService.logout();
    this.route.navigate(['/login']);
  }
  toggleAuth(){ 
    this.isMenuClicked=!this.isMenuClicked;
}


toggleMenu() {
  console.log(this.screenWidth);
  if (this.screenWidth??0 >= 768) {
    console.log("inside toggle menu");
    
    this.isMenuClicked = !this.isMenuClicked;
    console.log(this.isMenuOpen);
  }
 
}
navigateRoutes(path:string){
  console.log("inside borrower click");
  this.route.navigate(["/"+path]);
}

private refreshPage() {
  this.route.navigateByUrl('/', { skipLocationChange: true }).then(() => {
    this.route.navigate([this.route.url]);
  });
}

}
