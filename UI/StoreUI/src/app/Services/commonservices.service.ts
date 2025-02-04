import { HttpClient, HttpClientModule } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { CustomerDetailsDTO } from '../Interfaces/CustomerDetailsDTO';

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
  
  //#endregion Post Methods
}
