import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators, ReactiveFormsModule } from '@angular/forms';
import { FileHandle } from '../../../../Interfaces/FileHandle/FileHandle';
import { CommonservicesService } from '../../../../Services/commonservices.service';


@Component({
  selector: 'app-addproduct',
  standalone: true,
  imports:[ReactiveFormsModule],
  templateUrl: './addproduct.component.html',
  styleUrl: './addproduct.component.css'
})
export class AddproductComponent implements OnInit{

  productForm:FormGroup;
  selectedFile: FileHandle | null = null; 
  isLoading:boolean=false;
  errorMessage:string='';
  constructor(private fb:FormBuilder, private serivce:CommonservicesService)
  {
    this.productForm=this.fb.group({
      productName:['',Validators.required],
      productMRP:['',[Validators.required, Validators.min(0)]],
      purchasePrice:['',[Validators.required, Validators.min(0)]],
      purchaseDate:['',Validators.required],
      totalQuantity:['',[Validators.required, Validators.min(1)]],
      sellingPrice:['',[Validators.required, Validators.min(0)]],
      packetSize:['',Validators.required],
      productBills: ['', Validators.required] 
    })
  }
  ngOnInit(): void {
  
  }

  onFileSelected(event: any) {
    const file: File = event.target.files[0];
    if (file) {
      this.selectedFile = { file, url: '', progress: 0 };
      // Handle file upload (e.g., preview, upload to server)
      this.productForm.patchValue({ file });
      this.productForm.get('file')?.updateValueAndValidity();
    }
    else{
      this.selectedFile=null;
    }
  }
  //submit function
  onSubmit()
  {
    console.log("method is getting called")
    if(this.productForm.valid)
    {
      this.isLoading=true;
      this.errorMessage='';
      const formData = new FormData();

      //Append form fields to FormData
      formData.append('ProductName', this.productForm.get('productName')?.value);
      formData.append('ProductMRP', this.productForm.get('productMRP')?.value);
      formData.append('PurchasePrice', this.productForm.get('purchasePrice')?.value);
      formData.append('PurchaseDate', this.productForm.get('purchaseDate')?.value);  
      formData.append('TotalQuantity', this.productForm.get('totalQuantity')?.value);
      formData.append('SellingPrice', this.productForm.get('sellingPrice')?.value);
      formData.append('PacketSize', this.productForm.get('packetSize')?.value);
      //Append file
      if(this.selectedFile?.file){
        formData.append('productBills',this.selectedFile.file,this.selectedFile.file.name);
      }
      this.serivce.addProductDetails(formData).subscribe((data)=>{
        console.log(data);
        console.log(this.productForm.value);
        this.isLoading=false;
        this.productForm.reset();
      },(error)=>{
        this.errorMessage="Error Saving product details";
        console.log('Error Saving product details',error);
        this.isLoading=false;
      }
    );
     
    }
    else{
      console.log("Form data is not valid",this.productForm.errors)
    }
  }
}
