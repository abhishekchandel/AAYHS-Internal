import { Component, OnInit,Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
@Component({
  selector: 'app-export-confirmation-modal',
  templateUrl: './export-confirmation-modal.component.html',
  styleUrls: ['./export-confirmation-modal.component.scss']
})
export class ExportConfirmationModalComponent implements OnInit {

  title: string;
  message: string;
  name: string;

  constructor(public dialogRef: MatDialogRef<ExportConfirmationModalComponent>,
    @Inject(MAT_DIALOG_DATA) public data: ConfirmDialogModel) {
    this.title = data.title;
    this.message = data.message;
  }
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

export class ConfirmDialogModel {

  constructor(public title: string, public message: string) {
  }
}

