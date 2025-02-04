import { Component, OnInit } from '@angular/core';
import { CommonservicesService } from '../../../Services/commonservices.service';

@Component({
  selector: 'app-company',
  standalone: true,
  imports: [],
  templateUrl: './company.component.html',
  styleUrl: './company.component.css'
})
export class CompanyComponent implements OnInit {

  constructor(private service:CommonservicesService){}


  ngOnInit(): void {

    this.service.getCompanyDetails(1).subscribe(res=>{
      console.log(res);
    })
  }

}
