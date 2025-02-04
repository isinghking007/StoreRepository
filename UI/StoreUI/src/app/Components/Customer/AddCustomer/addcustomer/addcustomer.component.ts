import { Component } from '@angular/core';
import { FormGroup, FormBuilder, Validators, ReactiveFormsModule } from '@angular/forms';
import { FileHandle } from '../../../../Interfaces/FileHandle/FileHandle';
import { CommonservicesService } from '../../../../Services/commonservices.service';

@Component({
  selector: 'app-addcustomer',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './addcustomer.component.html',
  styleUrl: './addcustomer.component.css'
})
export class AddcustomerComponent {
  customerForm: FormGroup;
  selectedFile: FileHandle | null = null;

  constructor(private fb: FormBuilder, private service: CommonservicesService) {
    this.customerForm = this.fb.group({
      customerName: ['', Validators.required],
      phonenumber: ['', [Validators.required, Validators.pattern('^[0-9]{10}$')]], // Validate as a 10-digit number
      address: ['', [Validators.required]],
      purchaseDate: ['', Validators.required],
      totalamount: ['', [Validators.required, Validators.min(1)]],
      amountpaid: ['', [Validators.required, Validators.min(0)]],
      remainingamount: ['', [Validators.required, Validators.min(0)]],
      file: [null, Validators.required] // Changed to accept a file object
    });
  }

  ngOnInit(): void {}

  onFileSelected(event: any): void {
    const file: File = event.target.files[0];
    if (file) {
      // Store the selected file
      this.selectedFile = { file, url: '', progress: 0 };
  
      // Patch the `file` form control with the file object (useful for validation or tracking)
      this.customerForm.patchValue({ file });
      this.customerForm.get('file')?.updateValueAndValidity();
    } else {
      this.selectedFile = null;
    }
  }
  
  
  onSubmit(): void {
    if (this.customerForm.valid) {
      const formData = new FormData();
  
      // Append form fields to FormData
      formData.append('CustomerName', this.customerForm.get('customerName')?.value);
      formData.append('Mobile', this.customerForm.get('phonenumber')?.value);
      formData.append('Address', this.customerForm.get('address')?.value);
      formData.append('PurchaseDate', this.customerForm.get('purchaseDate')?.value);
      formData.append('TotalAmount', this.customerForm.get('totalamount')?.value);
      formData.append('AmountPaid', this.customerForm.get('amountpaid')?.value);
      formData.append('RemainingAmount', this.customerForm.get('remainingamount')?.value);
  
      // Append file
      if (this.selectedFile?.file) {
        formData.append('File', this.selectedFile.file, this.selectedFile.file.name);
      }
  
      // Call the service to send data
      this.service.addCustomerDetails(formData).subscribe(
        (res) => {
          console.log('Customer details sent to API successfully:', res);
        },
        (err) => {
          console.error('Error while sending customer details:', err);
        }
      );
  
      this.customerForm.reset();
    } else {
      console.log('Form data is not valid:', this.customerForm.value);
      this.customerForm.markAllAsTouched(); // Mark all controls as touched to show validation errors
    }
  }
  
}
