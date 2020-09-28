import { Component, OnInit,Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import * as moment from 'moment';

@Component({
  selector: 'app-refund-calculation-modal',
  templateUrl: './refund-calculation-modal.component.html',
  styleUrls: ['./refund-calculation-modal.component.scss']
})
export class RefundCalculationModalComponent implements OnInit {
 afterDate:any;
 beforeDate:any;
 refundType:string;
 refundPercent:number;
 typeOfRefund:number;

  constructor(public dialogRef: MatDialogRef<RefundCalculationModalComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any) {
    
  }

  ngOnInit(): void {
  }

  onDismiss(): void {
    // Close the dialog, return false
    this.dialogRef.close(false);
  }

  handleAfterDate(){
   this.afterDate== moment(this.afterDate).format('YYYY-MM-DD');
  }

  handleBeforeDate(){
    this.beforeDate== moment(this.beforeDate).format('YYYY-MM-DD');
  }


  addRefund(){

  }

  setRefundType(e){
this.typeOfRefund=Number(e.target.value);
  }
}
