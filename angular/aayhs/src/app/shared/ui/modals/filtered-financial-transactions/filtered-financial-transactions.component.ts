import { Component, OnInit,Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatSnackbarComponent } from '../../mat-snackbar/mat-snackbar.component'

@Component({
  selector: 'app-filtered-financial-transactions',
  templateUrl: './filtered-financial-transactions.component.html',
  styleUrls: ['./filtered-financial-transactions.component.scss']
})
export class FilteredFinancialTransactionsComponent implements OnInit {

  constructor(public dialogRef: MatDialogRef<FilteredFinancialTransactionsComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private snackBar: MatSnackbarComponent,
    ) { }


  ngOnInit(): void {
  }
  onDismiss(): void {
    // Close the dialog, return false
    this.dialogRef.close(false);
  }
}
