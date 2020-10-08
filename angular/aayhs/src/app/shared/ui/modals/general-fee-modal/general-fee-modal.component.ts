import { Component, OnInit,Inject, ViewChild } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialog } from '@angular/material/dialog';
import { MatSnackbarComponent } from '../../mat-snackbar/mat-snackbar.component';
import { YearlyMaintenanceService } from 'src/app/core/services/yearly-maintenance.service';
import { ConfirmDialogComponent, ConfirmDialogModel } from '../confirmation-modal/confirm-dialog.component';
import { NgForm } from '@angular/forms';

@Component({
  selector: 'app-general-fee-modal',
  templateUrl: './general-fee-modal.component.html',
  styleUrls: ['./general-fee-modal.component.scss']
})
export class GeneralFeeModalComponent implements OnInit {
  @ViewChild('addGeneralFeeForm') addGeneralFeeForm: NgForm;

feeAmount : any;
feeType:any;
timeframe:any;
loading = false;
result:string;
// generalFeesList={
//   FeeType:null,
//   Amount:null,
//   Active:null,
//   TimeFrame:null
// };
generalFeesList:any;
yearlyMaintainenceId:any;
  constructor(public dialogRef: MatDialogRef<GeneralFeeModalComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private yearlyService: YearlyMaintenanceService,
    private snackBar: MatSnackbarComponent,
    public dialog: MatDialog) {
    
  }

  ngOnInit(): void {
    this.yearlyMaintainenceId=this.data.YearlyMaintainenceId;
    this.getGeneralFees(this.yearlyMaintainenceId);
  }

  onDismiss(): void {
    // Close the dialog, return false
    this.dialogRef.close(false);
  }

  addGeneralFee(){
    if(this.yearlyMaintainenceId ==null)
    {
      this.snackBar.openSnackBar("Please select a year", 'Close', 'red-snackbar');
      return false;
    }
    return new Promise((resolve, reject) => {   
      this.loading = true;
      var adFeeRequest={
        yearlyMaintainenceId:this.yearlyMaintainenceId,
        timeFrame:this.timeframe,
        amount:Number(this.feeAmount),
        feeType:this.feeType
      }

      this.yearlyService.addGeneralFees(adFeeRequest).subscribe(response => {
        this.getGeneralFees(this.yearlyMaintainenceId);
        this.addGeneralFeeForm.resetForm({type:null,amount:null,timeframeType:null});
        this.snackBar.openSnackBar(response.Message, 'Close', 'green-snackbar');
        this.loading = false;
      }, error => {
        this.snackBar.openSnackBar(error, 'Close', 'red-snackbar');
      this.loading = false;
      }
      )
      resolve();
    });
  }


  confirmRemoveFee(id,timeframe): void {
    const message = `Are you sure you want to remove the record?`;
    const dialogData = new ConfirmDialogModel("Confirm Action", message);
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      maxWidth: "400px",
      data: dialogData
    });
    dialogRef.afterClosed().subscribe(dialogResult => {
      this.result = dialogResult;
      if (this.result) {
        this.deleteGeneralFee(id,timeframe)
      }
    });

  }

  
  deleteGeneralFee(id,timeframe){
    return new Promise((resolve, reject) => {   
      this.loading = true;
      var deleteRequest={
        yearlyMaintenanceFeeId:Number(id),
        timeFrame:timeframe
      }
      this.yearlyService.deleteGeneralFee(deleteRequest).subscribe(response => {
        this.getGeneralFees(this.yearlyMaintainenceId);
         this.snackBar.openSnackBar(response.Message, 'Close', 'green-snackbar');
        this.loading = false;
      }, error => {
        this.snackBar.openSnackBar(error, 'Close', 'red-snackbar');
      this.loading = false;
      }
      )
      resolve();
    });
  }

  getGeneralFees(id){
    return new Promise((resolve, reject) => {
      this.loading = true;
      this.yearlyService.getGeneralFees(id).subscribe(response => {
        this.generalFeesList = response.Data.getGeneralFeesResponses;
        this.loading = false;
      }, error => {
        this.loading = false;
  
      }
      )
      resolve();
    });
  }
}
