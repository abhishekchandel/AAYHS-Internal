import { Component, OnInit } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-assign-stall-modal',
  templateUrl: './assign-stall-modal.component.html',
  styleUrls: ['./assign-stall-modal.component.scss']
})
export class AssignStallModalComponent implements OnInit {

  constructor(public dialogRef: MatDialogRef<AssignStallModalComponent>) { }

  ngOnInit(): void {
  }

  onDismiss(): void {
    // Close the dialog, return false
    this.dialogRef.close(false);
  }
}
