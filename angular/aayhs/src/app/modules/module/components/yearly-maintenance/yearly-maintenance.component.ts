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
import { AddRoleModalComponent } from 'src/app/shared/ui/modals/add-role-modal/add-role-modal.component';


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
  selectedRowIndex: any;
  adFeesList:any;
  verifiedUsers:any
  roles:any
yearlyMaintenanceSummary:YearlyMaintenanceModel={
  ShowStartDate:null,
  ShowEndDate:null,
  PreEntryCutOffDate:null,
  SponcerCutOffDate:null,
  Year:null,
  YearlyMaintenanceId:null
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
    this.getVerifiedUsers();
    this.getAllRoles();

  }

  highlight(id, i) {
    this.selectedRowIndex = i;
    this.getAdFees(id);
   this.getYearlyMaintenanceByDetails(id);
  }

  openAddFeeModal(){
    debugger;
    var data={
      AddFeesList:this.adFeesList,
      YearlyMaintainenceId:this.yearlyMaintenanceSummary.YearlyMaintenanceId
    }


    const dialogRef = this.dialog.open(AddSizeFeeModalComponent, {
      data
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

  confirmVerifyUnverify(id): void {
    var  data={
    UserId:id,
    Roles:this.roles
    }
    const dialogRef = this.dialog.open(AddRoleModalComponent, {
      maxWidth: "400px",
      data
    });
    dialogRef.afterClosed().subscribe(dialogResult => {
      this.result = dialogResult;
      if (this.result) {
        this.getNewRegisteredUsers()
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
        this.deleteUser(id)
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

  confirmRemoveYear(e,id): void {
    e.stopPropagation();
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


getYearlyMaintenanceByDetails(id){
  return new Promise((resolve, reject) => {
    this.loading = true;
    this.yearlyService.getYearlyMaintenanceById(id).subscribe(response => {
      this.yearlyMaintenanceSummary = response.Data;
      this.loading = false;
    }, error => {
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

getVerifiedUsers(){
  return new Promise((resolve, reject) => {
    this.loading = true;
    this.yearlyService.getApprovedUser().subscribe(response => {
      this.verifiedUsers = response.Data.getUsers;
      this.loading = false;
    }, error => {
      this.loading = false;

    }
    )
    resolve();
  });
}

confirmRemoveApprovedUser(id){
  const message = `Are you sure you want to remove the user?`;
  const dialogData = new ConfirmDialogModel("Confirm Action", message);
  const dialogRef = this.dialog.open(ConfirmDialogComponent, {
    maxWidth: "400px",
    data: dialogData
  });
  dialogRef.afterClosed().subscribe(dialogResult => {
    this.result = dialogResult;
    if (this.result) {
      this.deleteApprovedUser(id);
    }
  });
}

deleteApprovedUser(id){
  return new Promise((resolve, reject) => {   
    this.loading = true;
    this.yearlyService.deleteApprovedUser(id).subscribe(response => {
      this.getVerifiedUsers();
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

getAllRoles(){
  return new Promise((resolve, reject) => {
    this.loading = true;
    this.yearlyService.getRoles().subscribe(response => {
      this.roles = response.Data.getRoles;
      this.loading = false;
    }, error => {
      this.loading = false;

    }
    )
    resolve();
  });
}

}
