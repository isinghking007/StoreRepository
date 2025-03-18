import { HttpClient, HttpClientModule } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { CustomerDetailsDTO } from '../Interfaces/CustomerDetailsDTO';
import { UserDetails } from '../Interfaces/UserDetails.interface';
import { Login } from '../Interfaces/login.interface';
import { jwtDecode } from 'jwt-decode';

@Injectable({
  providedIn: 'root'
})
export class CommonservicesService {

  private url="http://localhost:5170/api/";

  constructor(private http:HttpClient) { }
  
  //#region JIRA 17 - Log out Method
  private tokenKey = 'auth_token';

  // Save JWT token to localStorage
  saveToken(token: string): void {
    localStorage.setItem(this.tokenKey, token);
  }

  // Get JWT token from localStorage
  getToken(): string | null {
    return localStorage.getItem(this.tokenKey);
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

  // Check if the user is authenticated
  isAuthenticated(): boolean {
    return this.getToken() !== null;
  }

  // Clear token for logout
  logout(): void {
    localStorage.removeItem(this.tokenKey);
  }

  //#endregion JIRA 17 - Log out Method
  //#region Get Methods

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

  login(data:Login):Observable<Login>{
    return this.http.post<Login>(this.url+"User/Login",data,{responseType:'text' as 'json'});
  }
  //#endregion Post Methods
}
