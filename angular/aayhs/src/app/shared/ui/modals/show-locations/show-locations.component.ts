import { Component, OnInit,Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialog } from '@angular/material/dialog';
import { YearlyMaintenanceService } from 'src/app/core/services/yearly-maintenance.service';
import { MatSnackbarComponent } from '../../mat-snackbar/mat-snackbar.component';
@Component({
  selector: 'app-show-locations',
  templateUrl: './show-locations.component.html',
  styleUrls: ['./show-locations.component.scss']
})
export class ShowLocationsComponent implements OnInit {
  yearlyMaintainenceId:any;
  loading = false;
  statesResponse: any;
  showLocation:any={
    Name:null,
    Address:null,
    City:null,
    StateId:null,
    ZipCode:null,
    Phone:null,
    AAYHSContactAddressId:null
  }
  constructor(public dialogRef: MatDialogRef<ShowLocationsComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private yearlyService: YearlyMaintenanceService,
    private snackBar: MatSnackbarComponent,
    public dialog: MatDialog) {
    
  }

  ngOnInit(): void {
    this.yearlyMaintainenceId=this.data.YearlyMaintainenceId;
    this.getLocation(this.data.YearlyMaintainenceId);
    this.statesResponse=this.data.StatesResponse
  }

  onDismiss(): void {
    // Close the dialog, return false
    this.dialogRef.close(false);
  }

  addUpdateLocation(){
    if(this.yearlyMaintainenceId ==null)
    {
      this.snackBar.openSnackBar("Please select a year", 'Close', 'red-snackbar');
      return false;
    }
    return new Promise((resolve, reject) => {   
      this.loading = true;
      var locationRequest={
        YearlyMaintenanceId:this.yearlyMaintainenceId,
         name:this.showLocation.Name,
         address:this.showLocation.Address,
         city:this.showLocation.City,
         stateId:Number(this.showLocation.StateId),
         zipCode:this.showLocation.ZipCode,
         phone:this.showLocation.Phone

      }
      this.yearlyService.addUpdateLocation(locationRequest).subscribe(response => {
        this.getLocation(this.yearlyMaintainenceId);
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

  getLocation(id){
    return new Promise((resolve, reject) => {
      this.loading = true;
      this.yearlyService.getLocation(id).subscribe(response => {
        this.showLocation = response.Data;
        this.loading = false;
      }, error => {
        this.loading = false;
        this.snackBar.openSnackBar(error, 'Close', 'red-snackbar');

      }
      )
      resolve();
    });
  }


    }
