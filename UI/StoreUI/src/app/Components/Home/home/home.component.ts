import { AfterViewInit, Component, OnInit } from '@angular/core';
import {MatGridListModule} from '@angular/material/grid-list';
import { CommonservicesService } from '../../../Services/commonservices.service';
import { Chart,registerables } from 'chart.js';

export interface Tile {
  color: string;
  cols: number;
  rows: number;
  text: string;
}


@Component({
  selector: 'app-home',
  standalone: true,
  imports: [MatGridListModule],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent implements AfterViewInit,OnInit {
  latestEntries: any[] = [];
  totalDueAmount: number = 0;
  totalPaidAmount: number = 0;
  totalAmount: number = 0;
  totalCustomer:number=0;
  ngAfterViewInit() {
    this.loadInventoryChart();
    this.loadShipmentChart();
  }
  constructor(private service:CommonservicesService){
  Chart.register(...registerables);
  }

  ngOnInit(): void {
    this.service.getAllUserDueDetails().subscribe((data) => {
      console.log("Data from home comp", data);
      console.log(typeof data);
      if (Array.isArray(data)) {
        // Create a deep copy of the API response to avoid modifying the original data
        console.log("inside method");
        const copiedData = JSON.parse(JSON.stringify(data));
  
        const uniqueIds=new Set(copiedData.map((item:any)=>item.customerId));
        console.log("total Unique IDs:",uniqueIds.size);
        this.totalCustomer=uniqueIds.size;
        // Use reduce to aggregate data into an object
        const aggregatedData = copiedData.reduce((acc: any, current: any) => {
          const { dueId, customerId, totalBillAmount, newAmount, paidAmount, modifiedDate } = current;
          // Convert string values to numbers
        const totalBillAmountNum = parseFloat(totalBillAmount) || 0;
        const newAmountNum = parseFloat(newAmount) || 0;
        const paidAmountNum = parseFloat(paidAmount) || 0;
        if (!acc[customerId] ||new Date(modifiedDate) > new Date(acc[customerId].modifiedDate))
        {
          acc[customerId] = {
            dueId: dueId,
            customerId,
            totalBillAmount: totalBillAmountNum,
            newAmount: newAmountNum,
            paidAmount: paidAmountNum,
            modifiedDate,
          };
        }
        return acc;
      }, {} as Record<number, { customerId: number; dueId: number; totalBillAmount: number; newAmount: number; paidAmount: number; modifiedDate: string }>);

      // Convert the aggregated object into an array
      this.latestEntries = Object.values(aggregatedData);
      
      // Calculate totals using reduce
       this.totalAmount = this.latestEntries.reduce((sum, item) => sum + (item.totalBillAmount || 0),0);
       this.totalDueAmount = this.latestEntries.reduce((sum, item) => sum + (item.newAmount || 0),0);
      this.totalPaidAmount = this.latestEntries.reduce((sum, item) => sum + (item.paidAmount || 0),0);

      console.log("Total Amount:", this.totalAmount);
      console.log("Total Due:", this.totalDueAmount);
      console.log("Total Paid Amount:", this.totalPaidAmount);
    } else {
      console.warn("Data is not an array");
    }
  });


}

loadInventoryChart() {
    new Chart("inventoryChart", {
      type: 'bar',
      data: {
        labels: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul'],
        datasets: [
          { label: 'Received', data: [800, 600, 700, 750, 720, 740, 780], backgroundColor: 'yellow' },
          { label: 'On Hand', data: [500, 450, 470, 480, 490, 495, 500], backgroundColor: 'blue' },
          { label: 'Shipped', data: [200, 220, 230, 240, 250, 260, 270], backgroundColor: 'green' }
        ]
      }
    });
  }

  loadShipmentChart() {
    new Chart("shipmentChart", {
      type: 'doughnut',
      data: {
        labels: ['Dispatch', 'In Transit', 'Delayed'],
        datasets: [{
          data: [6589, 3330, 500],
          backgroundColor: ['orange', 'blue', 'red']
        }]
      }
    });
  }
 }
