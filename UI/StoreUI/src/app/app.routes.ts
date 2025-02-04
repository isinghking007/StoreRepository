import { Routes } from '@angular/router';
import { AddproductComponent } from './Components/Product/AddProducts/addproduct/addproduct.component';
import { AddcustomerComponent } from './Components/Customer/AddCustomer/addcustomer/addcustomer.component';
import { CompanyComponent } from './Components/Company/company/company.component';
import { TestComponent } from './Components/Test/test/test.component';

export const routes: Routes = [
    {
        path:'addproduct',component:AddproductComponent
    },
    {
        path:'addcustomer',component:AddcustomerComponent
    },
    {
        path:'company',component:CompanyComponent
    },
    {
        path:'test',component:TestComponent
    }
];
