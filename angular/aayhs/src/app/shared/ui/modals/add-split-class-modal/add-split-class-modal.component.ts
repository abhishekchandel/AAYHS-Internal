import { Component, OnInit,Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'app-add-split-class-modal',
  templateUrl: './add-split-class-modal.component.html',
  styleUrls: ['./add-split-class-modal.component.scss']
})
export class AddSplitClassModalComponent implements OnInit {
  t

  constructor(public dialogRef: MatDialogRef<AddSplitClassModalComponent>){}
    
  ngOnInit(): void {
  }
  onConfirm(): void {
    // Close the dialog, return true
    this.dialogRef.close(true);
  }

  onDismiss(): void {
    // Close the dialog, return false
    this.dialogRef.close(false);
  }
}
