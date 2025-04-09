import { HttpClient, HttpClientModule } from '@angular/common/http';
import { Inject, Injectable, PLATFORM_ID } from '@angular/core';
import { Observable, tap } from 'rxjs';
import { CustomerDetailsDTO } from '../Interfaces/CustomerDetailsDTO';
import { UserDetails } from '../Interfaces/UserDetails.interface';
import { Login } from '../Interfaces/login.interface';
import { jwtDecode } from 'jwt-decode';
import { ResetPassword } from '../Interfaces/ResetPassword.interface';
import { isPlatformBrowser } from '@angular/common';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class CommonservicesService {

  private tokenExpirationTime: number=0;
  private url="http://localhost:5170/api/";

//  @Inject(PLATFORM_ID) private platformId!: object;

  constructor(private http: HttpClient,private router:Router,@Inject(PLATFORM_ID) private platformId: object,) { }
  
  //#region JIRA 17 - Log out Method
  private tokenKey = 'id_token';

  // Save JWT token to localStorage
  saveToken(token: string): void {
    localStorage.setItem(this.tokenKey, token);
  }

  // Get JWT token from localStorage
  getToken(): string | null {
    if(isPlatformBrowser(this.platformId)){

      return localStorage.getItem(this.tokenKey);
    }
    return null;
  }

  getDecodedToken():any{
    const token=this.getToken();
    if(token){
      try{
        return jwtDecode(token);
      }
      catch(Error){
        return null;
    }
  }
} 
getDecodedUserDetails():{email?:string,phone?:string}{
  const decodedToken=this.getDecodedToken();
  if(decodedToken){
    return { email: decodedToken.email,phone:decodedToken.phone_number};
  }
  return {};
};

 
  // Clear token for logout
  
  //#endregion JIRA 17 - Log out Method
  //#region Get Methods

  getAllUserDueDetails(){
    return this.http.get(this.url+"common/allUserDueDetails");
  }

  getSingleUserDueDetails(customerId:number){
    return this.http.get(this.url+"common/dueAmountDetails/"+customerId);
  }
  getCompanyDetails(companyId:number){
    return this.http.get(this.url+"company/companyDetails/"+companyId);
  }

getUserDetails(phone:string){
  return this.http.get(this.url+"user/userDetails/"+phone);
}
  //#endregion Get Methods

  //#region  Post Methods

  // addCustomerDetails(data:CustomerDetailsDTO):Observable<any>{
  //   return this.http.post<CustomerDetailsDTO>(this.url+"User/AddCustomerDetails",data,{responseType:'text' as 'json'});
  // }

  addProductDetails(data: FormData): Observable<any> {
    return this.http.post(this.url + 'product/addproductdetails', data, {
      responseType: 'text' as 'json', // Adjust response type as needed
    });
  }

  addCustomerDetails(data: FormData): Observable<any> {
    return this.http.post(this.url + 'User/AddCustomerDetails', data, {
      responseType: 'text' as 'json', // Adjust response type as needed
    });
  }
  
  addUserDetails(data:UserDetails):Observable<UserDetails>{
    return this.http.post<UserDetails>(this.url+"User/AddUserDetails",data,{responseType:'text' as 'json'});
  }

  login(data: Login): Observable<Login> {
    return this.http.post<Login>(this.url + "User/Login", data, { responseType: 'text' as 'json' })
        .pipe(
            tap((response: any) => {
                const authResult = {
                    idToken: response.idToken, // Replace with actual token key from response
                    expirationTime: response.expirationTime // Replace with actual expiration time key
                };
                this.setSession(authResult);
            })
        );
}

  resetUser(data:ResetPassword):Observable<ResetPassword>{
    return this.http.post<ResetPassword>(this.url+"User/reset-user",data,{responseType:'text' as 'json'});
  }

  confirmPassword(data:ResetPassword):Observable<ResetPassword>{
    return this.http.post<ResetPassword>(this.url+"User/confirm-password",data,{responseType:'text' as 'json'});
  } 

  addDueAmountDetails(data:FormData):Observable<any>{
    return this.http.post(this.url+"common/addDueAmount",data,{responseType:'text' as 'json'});
  }
  //#endregion Post Methods


  //#region Setting Session Start

  setSession(authResult: any) {
    const expiresAt = new Date(authResult.expirationTime).getTime();
    this.tokenExpirationTime = expiresAt;
    localStorage.setItem('id_token', authResult.idToken);
    localStorage.setItem('expires_at', JSON.stringify(expiresAt));
    this.startSessionTimer();
  }

  startSessionTimer() {
    const checkInterval = 60000; // Check every minute
    const warningThreshold = 5 * 60 * 1000; // 5 minutes

    const checkSession = () => {
      const currentTime = new Date().getTime();
      const timeRemaining = this.tokenExpirationTime - currentTime;
      console.log("remaining time",timeRemaining);
      if (timeRemaining <= warningThreshold && timeRemaining > 0) {
        this.notifyUser(timeRemaining,warningThreshold);
      }

      if (timeRemaining > 0) {
        setTimeout(checkSession, checkInterval);
      } else {
        this.logout();
      }
    };

    setTimeout(checkSession, checkInterval);
  }

  notifyUser(timeRemaining: number, warningThreshold: number) {
    var minutes = Math.floor((timeRemaining % (1000 * 60 * 60)) / (1000 * 60));
    var seconds = Math.floor((timeRemaining % (1000 * 60)) / 1000);
    var timeRemainings = minutes + "m " + seconds + "s ";
    alert('Your session is about to expire. Time = '+timeRemainings);
  }

 // Consolidated logout method
logout(): void {
  localStorage.removeItem('id_token');
  localStorage.removeItem('expires_at');
  this.router.navigate(['/login']);
  alert('You have been logged out.');
}

// Consolidated authentication check
isLoggedIn(): boolean {
  if (isPlatformBrowser(this.platformId)) {
    console.log('Running in browser environment');
    const expiresAt = JSON.parse(localStorage.getItem('expires_at') || '0');
    return !!localStorage.getItem('id_token') && new Date().getTime() < expiresAt;
  }
  console.log('Not running in browser environment');
  return false; // Return false if not in a browser environment
}

 // Check if the user is authenticated
 isAuthenticated(): boolean {
  const token=this.getToken();
  return !!token;
}

  //#region Setting Session End

}
