import { Component, OnInit,Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
@Component({
  selector: 'app-add-size-fee-modal',
  templateUrl: './add-size-fee-modal.component.html',
  styleUrls: ['./add-size-fee-modal.component.scss']
})
export class AddSizeFeeModalComponent implements OnInit {

  constructor(public dialogRef: MatDialogRef<AddSizeFeeModalComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any) {
    
  }

  ngOnInit(): void {
  }

  onDismiss(): void {
    // Close the dialog, return false
    this.dialogRef.close(false);
  }

}
