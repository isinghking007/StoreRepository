import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { HeadersComponent } from './Components/Headers/headers/headers.component';
import { HomeComponent } from './Components/Home/home/home.component';
import { ReactiveFormsModule } from '@angular/forms';
import { AddproductComponent } from './Components/Product/AddProducts/addproduct/addproduct.component';
import { RouterModule } from '@angular/router';
@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet,RouterModule, HeadersComponent, HomeComponent, ReactiveFormsModule,AddproductComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'StoreUI';
}
