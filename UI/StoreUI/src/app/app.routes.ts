import { Routes } from '@angular/router';
import { AddproductComponent } from './Components/Product/AddProducts/addproduct/addproduct.component';
import { AddcustomerComponent } from './Components/Customer/AddCustomer/addcustomer/addcustomer.component';
import { CompanyComponent } from './Components/Company/company/company.component';
import { TestComponent } from './Components/Test/test/test.component';
import { SignupComponent } from './Components/SignUp/signup/signup.component';
import { LoginComponent } from './Components/Login/login/login.component';
import { HomeComponent } from './Components/Home/home/home.component';
import { AuthGuard } from './Services/auth.guard';

export const routes: Routes = [
    {
        path:"home",component:HomeComponent
    },
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
    },
    {
        path:'signup',component:SignupComponent
    },
    {
        path:'login',component:LoginComponent
    },
    {
        path:'**',redirectTo:'login'
    }
];
