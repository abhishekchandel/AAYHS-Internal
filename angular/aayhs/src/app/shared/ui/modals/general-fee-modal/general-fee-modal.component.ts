import { Component, OnInit,Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
@Component({
  selector: 'app-general-fee-modal',
  templateUrl: './general-fee-modal.component.html',
  styleUrls: ['./general-fee-modal.component.scss']
})
export class GeneralFeeModalComponent implements OnInit {
feeAmount : any;
feeType:any;
timeframe:any;

  constructor(public dialogRef: MatDialogRef<GeneralFeeModalComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any) {
    
  }

  ngOnInit(): void {
  }

  onDismiss(): void {
    // Close the dialog, return false
    this.dialogRef.close(false);
  }

  addGeneralFee(){

  }
}
