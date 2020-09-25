import { Component, OnInit,Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
@Component({
  selector: 'app-refund-calculation-modal',
  templateUrl: './refund-calculation-modal.component.html',
  styleUrls: ['./refund-calculation-modal.component.scss']
})
export class RefundCalculationModalComponent implements OnInit {

  constructor(public dialogRef: MatDialogRef<RefundCalculationModalComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any) {
    
  }

  ngOnInit(): void {
  }

  onDismiss(): void {
    // Close the dialog, return false
    this.dialogRef.close(false);
  }

}
