import { HttpClient, HttpClientModule } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { CustomerDetailsDTO } from '../Interfaces/CustomerDetailsDTO';
import { UserDetails } from '../Interfaces/UserDetails.interface';
import { Login } from '../Interfaces/login.interface';

@Injectable({
  providedIn: 'root'
})
export class CommonservicesService {

  private url="http://localhost:5170/api/";

  constructor(private http:HttpClient) { }

  //#region Get Methods

  getCompanyDetails(companyId:number){
    return this.http.get(this.url+"company/companyDetails/"+companyId);
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
