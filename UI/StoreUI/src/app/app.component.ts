import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { HeadersComponent } from './Components/Headers/headers/headers.component';
import { HomeComponent } from './Components/Home/home/home.component';
import { ReactiveFormsModule } from '@angular/forms';
import { AddproductComponent } from './Components/Product/AddProducts/addproduct/addproduct.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, HeadersComponent, HomeComponent, ReactiveFormsModule,AddproductComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'StoreUI';
}
