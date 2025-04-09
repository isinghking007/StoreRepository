import { Component, computed, OnInit, Signal, signal, WritableSignal } from '@angular/core';
import { CommonservicesService } from '../../../Services/commonservices.service';
import { Console } from 'console';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { FileHandle } from '../../../Interfaces/FileHandle/FileHandle';
import { WindowService } from '../../../Services/WindowService.service';

@Component({
  selector: 'app-borrowerscreen',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './borrowerscreen.component.html',
  styleUrl: './borrowerscreen.component.css'
})
export class BorrowerscreenComponent implements OnInit {
  selectedFile: FileHandle | null = null;
  amountDuePopUp:any;
  isModalVisible = false;
  amountPayModal=false;
  AmountDueForm!:FormGroup
  searchTerm: WritableSignal<string> = signal('');
  firstrowDetails: WritableSignal<any[]> = signal([]);
  filteredBorrowers: Signal<any[]> = computed(() => {
    const term = this.searchTerm().toLowerCase();
    return this.firstrowDetails().filter((borrower: any) =>
      borrower.customerName.toLowerCase().includes(term)
    );
  });

  constructor(private service: CommonservicesService,private fb:FormBuilder,private windowService:WindowService) {
    // this.AmountDueForm=this.fb.group({
    //   customerName:[{value:this.amountDuePopUp[0].customerName,disabled:true}],
    //   address:[''],
    //   phone:[''],
    //   totalBillAmount:[''],
    //   newAmount:[''],
    //   previousAmount:['']
    // })
  }
test=false;
ngOnInit() {
  this.service.getAllUserDueDetails().subscribe((data) => {
   // console.log("Data received from API:", data);

    if (Array.isArray(data)) {
    //  console.log("Initial Data (Unmodified):", data);

      // Create a deep copy of the API response to avoid modifying the original data
      const copiedData = JSON.parse(JSON.stringify(data));

      const aggregatedData = copiedData.reduce((acc: any, curr: any) => {
        const customerId = curr.customerId;
        const dueId = +curr.dueId; // Ensure dueId is a number

        // console.log(`Processing customerId: ${customerId}, dueId: ${dueId}`);
        // console.log("Total Bill Amount initial", curr.totalBillAmount);
        // console.log("New Amount initial", curr.paidAmount);

        // If customer does not exist in acc or current dueId is greater, update entry
        if (!acc[customerId] || dueId > acc[customerId].dueId) {
        //  console.log(`Updating customerId: ${customerId} with dueId: ${dueId}`);

          // console.log("Total Bill Amount", curr.totalBillAmount);
          // console.log("New Amount", curr.paidAmount);

          // Log previous entry before updating
          if (acc[customerId]) {
          //  console.log("Previous Entry:", acc[customerId]);
          }

          // If totalBillAmount is 0 and newAmount > 0, set totalBillAmount to newAmount
          if (curr.totalBillAmount == 0 && curr.paidAmount > 0 && curr.newAmount!=0) {
          //  console.log(`Adjusting totalBillAmount for customerId: ${customerId}`);
            curr.totalBillAmount = curr.paidAmount;
          }
          if (curr.totalBillAmount > 0 && curr.dueAmount > 0 && curr.paidAmount!=0) {
            curr.totalBillAmount = curr.totalBillAmount - curr.dueAmount;
          }

        //  console.log("before Updated Entry:", acc[customerId]);
          if (curr.totalBillAmount == 0 && curr.newAmount == 0 && curr.paidAmount > 0) {
            curr.totalBillAmount = 0;
            curr.newAmount = 0;
          }
          acc[customerId] = { ...curr, dueId };

        //  console.log("Updated Entry:", acc[customerId]);
          curr.newAmount = curr.totalBillAmount;
        }
        return acc;
      }, {});

  //    console.log("Final Aggregated Data:", Object.values(aggregatedData));
      this.firstrowDetails.set(Object.values(aggregatedData));
    } else {
   //   console.log("Data is not an array");
      this.firstrowDetails.set([]);
    }
  });
}
  // ngOnInit() {
  //   this.service.getAllUserDueDetails().subscribe((data) => {
    
  //     console.log("Data received from API:", data);
  //       // const aggregatedData = data.reduce((acc: any, curr: any) => {
  //       //   const customerId = curr.customerId;
  //       //   const totalBillAmount = +curr.totalBillAmount;
  //       //   const newAmount = +curr.newAmount;
  //       //   const previousAmount = +curr.previousAmount;

  //       //   if (!acc[customerId]) {
  //       //     acc[customerId] = { ...curr, totalBillAmount, newAmount, previousAmount };
  //       //   } else {
  //       //     acc[customerId].totalBillAmount += totalBillAmount;
  //       //     acc[customerId].newAmount += newAmount;
  //       //     acc[customerId].previousAmount += previousAmount;
  //       //   }
  //       //   return acc;
  //       // }, {});
  //       if (Array.isArray(data) && this.test) {
  //         console.log("Initial Data:", data);
        
  //         const aggregatedData = data.reduce((acc: any, curr: any) => {
  //           const customerId = curr.customerId;
  //           const dueId = +curr.dueId; // Ensure dueId is a number
        
  //           console.log(`Processing customerId: ${customerId}, dueId: ${dueId}`);            
  //           console.log("Total Bill Amount initial",curr.totalBillAmount);
  //           console.log("New Amount initial",curr.paidAmount);
        
  //           // If customer does not exist in acc or current dueId is greater, update entry
  //           if (!acc[customerId] || dueId > acc[customerId].dueId) {
  //             console.log(`Updating customerId: ${customerId} with dueId: ${dueId}`);
        

  //             console.log("Total Bill Amount",curr.totalBillAmount);
  //             console.log("New Amount",curr.paidAmount);

  //             // Log previous entry before updating
  //             if (acc[customerId]) {
  //               console.log("Previous Entry:", acc[customerId]);
  //             }
        
  //             // If totalBillAmount is 0 and newAmount > 0, set totalBillAmount to newAmount
  //             if (curr.totalBillAmount == 0 && curr.paidAmount > 0) {
  //               console.log(`Adjusting totalBillAmount for customerId: ${customerId}`);
  //               curr.totalBillAmount = curr.paidAmount;
  //             }
  //             if(curr.totalBillAmount>0 && curr.dueAmount>0){
  //               curr.totalBillAmount=curr.totalBillAmount - curr.dueAmount;
  //             }
              
  //             console.log("before Updated Entry:", acc[customerId]);
  //             if(curr.totalBillAmount == 0 && curr.newAmount ==0 && curr.paidAmount>0){
  //               curr.totalBillAmount=0;
  //               curr.newAmount=0;
  //             }
  //             acc[customerId] = { ...curr, dueId };
        
  //             console.log("Updated Entry:", acc[customerId]);
  //             curr.newAmount=curr.totalBillAmount;
  //           }
  //           return acc;
  //         }, {});
        
  //         console.log("Final Aggregated Data:", Object.values(aggregatedData));
  //         this.firstrowDetails.set(Object.values(aggregatedData));
  //       } else {
  //         console.log("Data is not an array");
  //         this.firstrowDetails.set([]);
  //       }
                
  //   });
  // }

  updateSearchTerm(event: Event): void {
    const inputElement = event.target as HTMLInputElement;
    this.searchTerm.set(inputElement.value);
  }

  addDue(event:number):void{
    //const eventValue=event.target as HTMLInputElement;
    console.log(event);
  }


  showModal(event:number,buttonType:string) {
  //  console.log("amountPayModal ",this.amountPayModal);
  
    if(buttonType=="paynow"){
      this.amountPayModal=true;
      //this.isModalVisible = true;
    }
    else{
      this.isModalVisible = true;
      //this.amountPayModal=false;
    }

    // this.service.getSingleUserDueDetails(event).subscribe((data) => {
    //   console.log(data);
    // });
 //   console.log("checking firstrow method");
   this.amountDuePopUp = this.firstrowDetails().find((item) => item.customerId === event);
 //   console.log("before method ",this.amountDuePopUp);
    if (this.amountDuePopUp) {
      const paidAmount = parseInt(this.amountDuePopUp.totalBillAmount || '0', 10);
      this.AmountDueForm = this.fb.group({
        customerId:[{value:this.amountDuePopUp.customerId,disabled:true}],
        customerName: [{ value: this.amountDuePopUp.customerName, disabled: true }],
        address: [{value:this.amountDuePopUp.address,disabled:true}],
        phone: [{value:this.amountDuePopUp.mobile,disabled:true}],
        totalBillAmount: [{value:this.amountDuePopUp.totalBillAmount,disabled:true}],
        newAmount: [''],
        paidAmount: [''],
        file:[null]
      });
    }
    this.onChanges();
  }

  hideModal() {
    this.isModalVisible = false;
    this.amountPayModal=false;
  }

  onChanges(){
    this.AmountDueForm.valueChanges.subscribe(val=>{
          // Get the initial totalBillAmount as an integer
    const previousAmount = parseInt(this.amountDuePopUp?.totalBillAmount || '0', 10);

    const paidAmount=parseInt(this.AmountDueForm.get('paidAmount')?.value || '0', 10);
 //   console.log("paid amount value from onchanges method",paidAmount)
    // Get the new newAmount as an integer
    const newAmount = parseInt(this.AmountDueForm.get('newAmount')?.value || '0', 10);

    // Ensure the values are valid integers
    const validPreviousAmount = isNaN(previousAmount) ? 0 : previousAmount;
    const validNewAmount = isNaN(newAmount) ? 0 : newAmount;
    const validPaidAmount = isNaN(paidAmount) ? 0 : paidAmount;
    var updatedTotalBillAmount=0;
    // If the newAmount is empty or invalid, revert to the original totalBillAmount
    if(newAmount==0 && paidAmount>0){
      updatedTotalBillAmount=validPaidAmount === 0 ? validPreviousAmount :validPreviousAmount - paidAmount;
    }else if(paidAmount==0 && newAmount>0){
      updatedTotalBillAmount = validNewAmount === 0 ? validPreviousAmount : validPreviousAmount + validNewAmount;
    }
    else if((updatedTotalBillAmount==0 ||validPreviousAmount) && newAmount==0 && paidAmount>0){
      updatedTotalBillAmount = 0;
    }
    // Update the totalBillAmount field dynamically
    this.AmountDueForm.patchValue(
      { totalBillAmount: updatedTotalBillAmount.toString() }, // Convert back to string
      { emitEvent: false } // Prevent triggering valueChanges again
    );
    // this.AmountDueForm.patchValue({previousAmount:validPreviousAmount.toString()},{emitEvent:false});
    });
  }
  onFileSelected(event: any): void {
    const file: File = event.target.files[0];
    if (file) {
      // Store the selected file
      this.selectedFile = { file, url: '', progress: 0 };
  
      // Patch the `file` form control with the file object (useful for validation or tracking)
      this.AmountDueForm.patchValue({ file });
      this.AmountDueForm.get('file')?.updateValueAndValidity();
    } else {
      this.selectedFile = null;
    }
  }
  onSubmit() {
    console.log("Amount Due Form submit method called");
  
    if (this.AmountDueForm.valid) {
      const formData = new FormData();
  
      // Use getRawValue() to include disabled fields in the form data
      const formValues = this.AmountDueForm.getRawValue();
  
      // Append only required fields
      if (this.amountDuePopUp && this.amountDuePopUp.customerId) {
        formData.append('customerId', this.amountDuePopUp.customerId);
      } else {
        console.error('Error: customerId is missing or amountDuePopUp is not initialized.');
        return;
      }
  
      formData.append('totalBillAmount', formValues.totalBillAmount);
      formData.append('newAmount', formValues.newAmount?formValues.newAmount:0);
      formData.append('paidAmount', formValues.paidAmount?formValues.paidAmount:0);
  
      // Append file if selected
      if (this.selectedFile?.file) {
        formData.append('File', this.selectedFile.file, this.selectedFile.file.name);
      }
      else {
        formData.append('File', null as any); // Append null explicitly
      }
      // Log the form values to the console
      console.log('Form data to be submitted:');
      formData.forEach((value, key) => {
        console.log(`${key}:`, value);
      });
      // console.log('customerId:', this.amountDuePopUp.customerId);
      // console.log('totalBillAmount:', formValues.totalBillAmount);
      // console.log('newAmount:', formValues.newAmount);
      // console.log('previousAmount:', formValues.previousAmount);
  
      // Call the service to send data
      this.service.addDueAmountDetails(formData).subscribe(
        (res) => {
          console.log('Amount due details sent to API successfully:', res);
          this.isModalVisible = false; // Close the modal after successful submission
          this.AmountDueForm.reset(); // Reset the form after submission
          this.windowService.nativeWindow?.location.reload();
        });
    } else {
      console.error('Form is invalid:', this.AmountDueForm);
      alert('Please fill in all required fields before submitting.');
    }
  }
}
