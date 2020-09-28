import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ShowLocationsComponent } from 'src/app/shared/ui/modals/show-locations/show-locations.component';
import { RefundCalculationModalComponent } from 'src/app/shared/ui/modals/refund-calculation-modal/refund-calculation-modal.component';
import { GeneralFeeModalComponent } from 'src/app/shared/ui/modals/general-fee-modal/general-fee-modal.component';
import { ClassCategoryModalComponent } from 'src/app/shared/ui/modals/class-category-modal/class-category-modal.component';
import { AddSizeFeeModalComponent } from 'src/app/shared/ui/modals/add-size-fee-modal/add-size-fee-modal.component';
import { ConfirmDialogComponent,ConfirmDialogModel } from 'src/app/shared/ui/modals/confirmation-modal/confirm-dialog.component';
import { BaseRecordFilterRequest } from 'src/app/core/models/base-record-filter-request-model';
import {YearlyMaintenanceService} from 'src/app/core/services/yearly-maintenance.service'
import { MatPaginator } from '@angular/material/paginator';
import { GlobalService } from 'src/app/core/services/global.service';
import { MatSnackbarComponent } from 'src/app/shared/ui/mat-snackbar/mat-snackbar.component';
import * as moment from 'moment';
import { NgForm } from '@angular/forms';
import {YearlyMaintenanceModel} from '../../../../core/models/yearly-maintenance-model'


@Component({
  selector: 'app-yearly-maintenance',
  templateUrl: './yearly-maintenance.component.html',
  styleUrls: ['./yearly-maintenance.component.scss']
})
export class YearlyMaintenanceComponent implements OnInit {
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild('addYearForm') addYearForm: NgForm;
  
  result: string = ''
  loading = false;
  totalItems: number = 0;
  yearlyMaintenanceSummaryList :any;
  registeredUsers:any;
  startDate:any;
  endDate:any;
  entryCutOffDate:any;
  sponsorDate:any;
  maxyear: any;
  minyear:any;
  years=[];
yearlyMaintenanceSummary:YearlyMaintenanceModel={
  ShowStartDate:null,
  ShowEndDate:null,
  PreCutOffDate:null,
  SponcerCutOffDate:null,
  Year:null,
  YearlyMaintainenceId:null
}

  baseRequest: BaseRecordFilterRequest = {
    Page: 1,
    Limit: 5,
    OrderBy: 'YearlyMaintenanceId',
    OrderByDescending: true,
    AllRecords: false,
    SearchTerm: null

  };
  
  constructor(public dialog: MatDialog,
             private yearlyService: YearlyMaintenanceService,
             private data: GlobalService,
             private snackBar: MatSnackbarComponent
            ) { }

  ngOnInit(): void {
    this.data.searchTerm.subscribe((searchTerm: string) => {
      this.baseRequest.SearchTerm = searchTerm;
      this.getYearlyMaintenanceSummary();
      this.baseRequest.Page = 1;
    });
    this.getNewRegisteredUsers()
    this.setYears();
  }

  openAddFeeModal(){
    const dialogRef = this.dialog.open(AddSizeFeeModalComponent, {
    });
    dialogRef.afterClosed().subscribe(dialogResult => {

      })
    
  }

  openClassCategoryModal(){
    const dialogRef = this.dialog.open(ClassCategoryModalComponent, {
      maxWidth: "400px",
    });
    dialogRef.afterClosed().subscribe(dialogResult => {

      })
  }

  openGeneralFeeModal(){
    const dialogRef = this.dialog.open(GeneralFeeModalComponent, {
      maxWidth: "400px",
    });
    dialogRef.afterClosed().subscribe(dialogResult => {

      })
  }

  openRefundCalculationFeeModal(){
    const dialogRef = this.dialog.open(RefundCalculationModalComponent, {
      maxWidth: "400px",
    });
    dialogRef.afterClosed().subscribe(dialogResult => {

      })
  }

  openShowLocationModal(){
    const dialogRef = this.dialog.open(ShowLocationsComponent, {
      maxWidth: "400px",
    });
    dialogRef.afterClosed().subscribe(dialogResult => {

      })
  }

  confirmVerifyUnverify(id,action): void {
    debugger;
    const message = action=='verify' ? `Are you sure you want to verify the user?` : `Are you sure you want to unverify the user?`;
    const dialogData = new ConfirmDialogModel("Confirm Action", message);
    const verify=action=='verify' ? true :false;
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      maxWidth: "400px",
      data: dialogData
    });
    dialogRef.afterClosed().subscribe(dialogResult => {
      this.result = dialogResult;
      if (this.result) {
        debugger
        this.verifyUser(id,verify)
      }
    });
  }

  confirmRemoveUser(id): void {
    const message = `Are you sure you want to remove the user?`;
    const dialogData = new ConfirmDialogModel("Confirm Action", message);
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      maxWidth: "400px",
      data: dialogData
    });
    dialogRef.afterClosed().subscribe(dialogResult => {
      this.result = dialogResult;
      if (this.result) {
      }
    });

  }

  getYearlyMaintenanceSummary() {
    return new Promise((resolve, reject) => {
      this.loading = true;
      this.yearlyService.getYearlyMaintenanceSummary(this.baseRequest).subscribe(response => {
        this.yearlyMaintenanceSummaryList = response.Data.getYearlyMaintenances;
        this.totalItems = response.Data.TotalRecords;
        if(this.baseRequest.Page === 1){
          this.paginator.pageIndex =0;
        }
        this.loading = false;
      }, error => {
        this.loading = false;

      }
      )
      resolve();
    });
  }

  getNext(event) {
    this.baseRequest.Page = (event.pageIndex) + 1;
    this.getYearlyMaintenanceSummary()
  }

  confirmRemoveYear(id): void {
    const message = `Are you sure you want to remove the record?`;
    const dialogData = new ConfirmDialogModel("Confirm Action", message);
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      maxWidth: "400px",
      data: dialogData
    });
    dialogRef.afterClosed().subscribe(dialogResult => {
      this.result = dialogResult;
      if (this.result) {
        this.deleteYear(id)
      }
    });

  }

  getNewRegisteredUsers(){
    return new Promise((resolve, reject) => {
      this.loading = true;
      this.yearlyService.getNewRegisteredUsers().subscribe(response => {
        this.registeredUsers = response.Data.getUsers;
        this.loading = false;
      }, error => {
        this.loading = false;

      }
      )
      resolve();
    });
  }

  verifyUser(id,verify){
    return new Promise((resolve, reject) => {   
      this.loading = true;
      var verifyRequest={
        userId:id,
        isApproved:verify
      }
      this.yearlyService.verifyUser(verifyRequest).subscribe(response => {
        this.getNewRegisteredUsers();
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

  deleteUser(id){
    return new Promise((resolve, reject) => {   
      this.loading = true;
      this.yearlyService.deleteUser(id).subscribe(response => {
        this.getNewRegisteredUsers();
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

  handleShowStartDate(){
    this.startDate = moment(this.startDate).format('YYYY-MM-DD');

  }

  handleShowEndDate(){
    this.endDate = moment(this.endDate).format('YYYY-MM-DD');

  }

  handlePreEntryCutOffDate(){
    this.entryCutOffDate = moment(this.entryCutOffDate).format('YYYY-MM-DD');

  }

  handleSponsorCutOffDate(){
    this.sponsorDate = moment(this.sponsorDate).format('YYYY-MM-DD');

  }

  addUpdateYear(){
    return new Promise((resolve, reject) => {   
      this.loading = true;
      var yearlyRequest={
        showStartDate:this.startDate,
        showEndDate:this.endDate,
        preCutOffDate:this.entryCutOffDate,
        sponcerCutOffDate:this.sponsorDate,
        year:this.yearlyMaintenanceSummary.Year
      }
      this.yearlyService.addYear(yearlyRequest).subscribe(response => {
        this.reserYearForm();
        this.getYearlyMaintenanceSummary();
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

  reserYearForm(){
    this.addYearForm.resetForm({ startDate: null, endDate: null, preEntryCutOffDate: null, sponsorCutOffDate: null })
  }

  setYear(e){
    this.yearlyMaintenanceSummary.Year =Number(e.target.value)

  }

 
setYears(){
   this.maxyear = new Date().getFullYear() + 1;
    this.minyear = new Date().getFullYear();
  for (var i = this.minyear; i<=this.maxyear; i++){
   this.years.push(i)
}
}
  
deleteYear(id){
  return new Promise((resolve, reject) => {   
    this.loading = true;
    this.yearlyService.deleteYear(id).subscribe(response => {
      this.getYearlyMaintenanceSummary();
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
  
}
