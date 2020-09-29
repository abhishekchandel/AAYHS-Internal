import { Component, OnInit,Inject, ViewChild } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialog } from '@angular/material/dialog';
import {YearlyMaintenanceService} from 'src/app/core/services/yearly-maintenance.service'
import { MatSnackbarComponent } from 'src/app/shared/ui/mat-snackbar/mat-snackbar.component';
import { ConfirmDialogModel, ConfirmDialogComponent } from '../confirmation-modal/confirm-dialog.component';
import { NgForm } from '@angular/forms';

@Component({
  selector: 'app-add-size-fee-modal',
  templateUrl: './add-size-fee-modal.component.html',
  styleUrls: ['./add-size-fee-modal.component.scss']
})
export class AddSizeFeeModalComponent implements OnInit {
  @ViewChild('addAdFeeForm') addAdFeeForm: NgForm;


  size:any;
  addAmount:any;
  loading = false;
  result:string;
  adFeesList:any;
  yearlyMaintainenceId:any;
  
  constructor(public dialogRef: MatDialogRef<AddSizeFeeModalComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private yearlyService: YearlyMaintenanceService,
    private snackBar: MatSnackbarComponent,
    public dialog: MatDialog) {
    
  }

  ngOnInit(): void {
    this.yearlyMaintainenceId=this.data.YearlyMaintainenceId;
    this.adFeesList=this.data.AddFeesList;
  }

  onDismiss(): void {
    // Close the dialog, return false
    this.dialogRef.close(false);
  }

 

  addAdFee(){
    return new Promise((resolve, reject) => {   
      this.loading = true;
      var adFeeRequest={
        yearlyMaintainenceId:this.yearlyMaintainenceId,
        adSize:this.size,
        amount:Number(this.addAmount)
      }
      this.yearlyService.addAdFee(adFeeRequest).subscribe(response => {
        this.getAdFees(this.yearlyMaintainenceId);
        this.addAdFeeForm.resetForm({addSize:null,amount:null});
        this.snackBar.openSnackBar(response.Message, 'Close', 'green-snackbar');
        this.loading = false;
      }, error => {
        this.snackBar.openSnackBar(error.error.Message, 'Close', 'red-snackbar');
      this.loading = false;
      }
      )
      resolve();
    });
  }


  confirmRemoveFee(id): void {
    const message = `Are you sure you want to remove the record?`;
    const dialogData = new ConfirmDialogModel("Confirm Action", message);
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      maxWidth: "400px",
      data: dialogData
    });
    dialogRef.afterClosed().subscribe(dialogResult => {
      this.result = dialogResult;
      if (this.result) {
        this.deleteAdFee(id)
      }
    });

  }

  
  deleteAdFee(id){
    return new Promise((resolve, reject) => {   
      this.loading = true;
      this.yearlyService.deleteAdFee(id).subscribe(response => {
        this.getAdFees(this.yearlyMaintainenceId);
        this.snackBar.openSnackBar(response.Message, 'Close', 'green-snackbar');
        this.loading = false;
      }, error => {
        this.snackBar.openSnackBar(error.error.Message, 'Close', 'red-snackbar');
      this.loading = false;
      }
      )
      resolve();
    });
  }

  getAdFees(id){
    return new Promise((resolve, reject) => {
      this.loading = true;
      this.yearlyService.getAdFees(id).subscribe(response => {
        this.adFeesList = response.Data.getAdFees;
        this.loading = false;
      }, error => {
        this.loading = false;
  
      }
      )
      resolve();
    });
  }

}
