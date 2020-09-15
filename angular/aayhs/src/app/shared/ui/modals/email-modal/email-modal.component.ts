import { Component, OnInit,Inject,ViewChild } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA,MatDialog } from '@angular/material/dialog';
import { MatSnackbarComponent } from '../../mat-snackbar/mat-snackbar.component'
import { ExhibitorService } from 'src/app/core/services/exhibitor.service';
import { NgForm } from '@angular/forms';

@Component({
  selector: 'app-email-modal',
  templateUrl: './email-modal.component.html',
  styleUrls: ['./email-modal.component.scss']
})
export class EmailModalComponent implements OnInit {
  @ViewChild('emailForm') emailForm: NgForm;

  email:string=null;
  loading = false;
  constructor(public dialogRef: MatDialogRef<EmailModalComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private snackBar: MatSnackbarComponent,
    public dialog: MatDialog,
    private exhibitorService: ExhibitorService
    ) { }

  ngOnInit(): void {
  }

  onDismiss(): void {
    // Close the dialog, return false
    this.dialogRef.close(false);
  }

  sendEmail(){
    this.loading = true;
    this.exhibitorService.addFee(this.email).subscribe(response => {
      this.loading = false;
      this.emailForm.resetForm({ emailControl: null });
      this.snackBar.openSnackBar(response.Message, 'Close', 'green-snackbar');
  
    }, error => {
      this.snackBar.openSnackBar(error, 'Close', 'red-snackbar');
      this.loading = false;
    })   
  }
}
