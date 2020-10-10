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
import { AddRoleModalComponent } from 'src/app/shared/ui/modals/add-role-modal/add-role-modal.component';
import {YearlyMaintenanceModel, ContactInfo} from '../../../../core/models/yearly-maintenance-model'
import { ExhibitorService } from 'src/app/core/services/exhibitor.service';

@Component({
  selector: 'app-yearly-maintenance',
  templateUrl: './yearly-maintenance.component.html',
  styleUrls: ['./yearly-maintenance.component.scss']
})
export class YearlyMaintenanceComponent implements OnInit {
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild('addYearForm') addYearForm: NgForm;
  @ViewChild('addContactForm') addContactForm: NgForm;
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
  verifiedUsers:any;
  roles:any;
  classCategoryList:any;
  generalFeesList:any;
  feeDetails:any;
  statesResponse: any;
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
  
  contactInfo : ContactInfo ={
    Location:null,
    Email1:null,
    Email2:null,
    Phone1:null,
    Phone2:null,
    exhibitorSponsorAddress:null,
    exhibitorSponsorCity:null,
    exhibitorSponsorZip:null,
    exhibitorSponsorState:null,
    exhibitorRefundAddress:null,
    exhibitorRefundCity:null,
    exhibitorRefundZip:null,
    exhibitorRefundState:null,
    returnAddress:null,
    returnCity:null,
    returnZip:null,
    returnState:null,
    AAYHSContactId:null,
    yearlyMaintenanceId:null,
   
  }

  constructor(public dialog: MatDialog,
             private yearlyService: YearlyMaintenanceService,
             private data: GlobalService,
             private exhibitorService: ExhibitorService,
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
    this.getFees();
    this.getAllStates()
  }

  highlight(id, i) {
    this.resetForm();
    this.selectedRowIndex = i;
   this.getYearlyMaintenanceByDetails(id);
   this.getContactInfo(id);
  }

  openAddFeeModal(){
    if(this.validateyear())
    {
    var data={
      YearlyMaintainenceId:this.yearlyMaintenanceSummary.YearlyMaintenanceId
    }
    const dialogRef = this.dialog.open(AddSizeFeeModalComponent, {
      data
    });
    dialogRef.afterClosed().subscribe(dialogResult => {

      })
    }
  }

  openClassCategoryModal(){
    if(this.validateyear())
    {
    const dialogRef = this.dialog.open(ClassCategoryModalComponent, {
    });
    dialogRef.afterClosed().subscribe(dialogResult => {

      })
    }
  }

  openGeneralFeeModal(){
    if(this.validateyear())
    {
    var data={
      YearlyMaintainenceId:this.yearlyMaintenanceSummary.YearlyMaintenanceId
    }
    const dialogRef = this.dialog.open(GeneralFeeModalComponent, {
      data
    });
    dialogRef.afterClosed().subscribe(dialogResult => {

      })
    }
  }

  openRefundCalculationFeeModal(){
    if(this.validateyear())
    {
    var data={
      YearlyMaintainenceId:this.yearlyMaintenanceSummary.YearlyMaintenanceId,
    }

    const dialogRef = this.dialog.open(RefundCalculationModalComponent, {
      data
    });
    dialogRef.afterClosed().subscribe(dialogResult => {

      })
    }
  }

  openShowLocationModal(){
    if(this.validateyear())
    {
    var data={
      YearlyMaintainenceId:this.yearlyMaintenanceSummary.YearlyMaintenanceId,
      StatesResponse:this.statesResponse
    }
    const dialogRef = this.dialog.open(ShowLocationsComponent, {
      data
    });
    dialogRef.afterClosed().subscribe(dialogResult => {

      })
    }
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
        this.getVerifiedUsers()

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
        this.registeredUsers =null
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
    this.startDate = moment(this.yearlyMaintenanceSummary.ShowStartDate).format('YYYY-MM-DD');
  }


  handleShowEndDate(){
    this.endDate = moment(this.yearlyMaintenanceSummary.ShowEndDate).format('YYYY-MM-DD');

  }

  handlePreEntryCutOffDate(){
    this.entryCutOffDate = moment(this.yearlyMaintenanceSummary.PreEntryCutOffDate).format('YYYY-MM-DD');
  }

  handleSponsorCutOffDate(){
    this.sponsorDate = moment(this.yearlyMaintenanceSummary.SponcerCutOffDate).format('YYYY-MM-DD');
  }

  addUpdateYear(){
    return new Promise((resolve, reject) => {   
      this.loading = true;
      var yearlyRequest={
        showStartDate:this.startDate,
        showEndDate:this.endDate,
        preCutOffDate:this.entryCutOffDate,
        sponcerCutOffDate:this.sponsorDate,
        year:this.yearlyMaintenanceSummary.Year,
        yearlyMaintainenceId : this.yearlyMaintenanceSummary.YearlyMaintenanceId ==null ? 0 : this.yearlyMaintenanceSummary.YearlyMaintenanceId      }
        this.yearlyService.addYear(yearlyRequest).subscribe(response => {
        // this.reserYearForm();
        // this.getYearlyMaintenanceSummary();
        this.snackBar.openSnackBar(response.Message, 'Close', 'green-snackbar');
        this.loading = false;

        this.getYearlyMaintenanceSummary().then(res =>{ 
          if(response.NewId !=null && response.NewId>0)
          {
            if(this.yearlyMaintenanceSummary.YearlyMaintenanceId>0)
            {
              this.highlight(response.NewId,this.selectedRowIndex);
            }
            else{
              this.highlight(response.NewId,0);
            }
          
          }
        });


      }, error => {
        this.snackBar.openSnackBar(error, 'Close', 'red-snackbar');
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

getClassCategory(){
  return new Promise((resolve, reject) => {
    this.loading = true;
    this.yearlyService.getClassCategory().subscribe(response => {
      this.classCategoryList = response.Data.getClassCategories;
      this.loading = false;
    }, error => {
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


getContactInfo(id){
  return new Promise((resolve, reject) => {
    this.loading = true;
    this.yearlyService.getContactInfo(id).subscribe(response => {
      this.contactInfo.Location=response.Data.contactInfo.ShowLocation,
      this.contactInfo.Email1=response.Data.contactInfo.Email1,
      this.contactInfo.Email2=response.Data.contactInfo.Email2,
      this.contactInfo.Phone1=response.Data.contactInfo.Phone1,
      this.contactInfo.Phone2=response.Data.contactInfo.Phone2,

      this.contactInfo.exhibitorSponsorAddress=response.Data.contactInfo.exhibitorSponsorConfirmationResponse.Address,
      this.contactInfo.exhibitorSponsorCity=response.Data.contactInfo.exhibitorSponsorConfirmationResponse.City,
      this.contactInfo.exhibitorSponsorZip=response.Data.contactInfo.exhibitorSponsorConfirmationResponse.ZipCode,
      this.contactInfo.exhibitorSponsorState=response.Data.contactInfo.exhibitorSponsorConfirmationResponse.StateId,

      this.contactInfo.exhibitorRefundAddress=response.Data.contactInfo.exhibitorSponsorRefundStatementResponse.Address,
      this.contactInfo.exhibitorRefundCity=response.Data.contactInfo.exhibitorSponsorRefundStatementResponse.City,
      this.contactInfo.exhibitorRefundZip=response.Data.contactInfo.exhibitorSponsorRefundStatementResponse.ZipCode,
      this.contactInfo.exhibitorRefundState=response.Data.contactInfo.exhibitorSponsorRefundStatementResponse.StateId,

      this.contactInfo.returnAddress=response.Data.contactInfo.exhibitorConfirmationEntriesResponse.Address,
      this.contactInfo.returnCity=response.Data.contactInfo.exhibitorConfirmationEntriesResponse.City,
      this.contactInfo.returnZip=response.Data.contactInfo.exhibitorConfirmationEntriesResponse.ZipCode,
      this.contactInfo.returnState=response.Data.contactInfo.exhibitorConfirmationEntriesResponse.StateId

      this.contactInfo.AAYHSContactId=response.Data.contactInfo.AAYHSContactId
    
      this.loading = false;
    }, error => {
      this.loading = false;

    }
    )
    resolve();
  });
}

resetForm(){

  this.contactInfo.Location=null,
  this.contactInfo.Email1=null,
  this.contactInfo.Email2=null,
  this.contactInfo.Phone1=null,
  this.contactInfo.Phone2=null,
  this.contactInfo.exhibitorSponsorAddress=null,
  this.contactInfo.exhibitorSponsorCity=null,
  this.contactInfo.exhibitorSponsorZip=null,
  this.contactInfo.exhibitorSponsorState=null,
  this.contactInfo.exhibitorRefundAddress=null,
  this.contactInfo.exhibitorRefundCity=null,
  this.contactInfo.exhibitorRefundZip=null,
  this.contactInfo.exhibitorRefundState=null,
  this.contactInfo.returnAddress=null,
  this.contactInfo.returnCity=null,
  this.contactInfo.returnZip=null,
  this.contactInfo.returnState=null,
  this.contactInfo.AAYHSContactId=null,
  this.contactInfo.yearlyMaintenanceId=null


  this.adFeesList=null,
  this.classCategoryList=null,
  this.generalFeesList=null,

 this.yearlyMaintenanceSummary.ShowStartDate=null
 this.yearlyMaintenanceSummary.ShowEndDate=null
 this.yearlyMaintenanceSummary.PreEntryCutOffDate=null
 this.yearlyMaintenanceSummary.SponcerCutOffDate=null
 this.yearlyMaintenanceSummary.YearlyMaintenanceId=null
 this.yearlyMaintenanceSummary.Year=null
 this.selectedRowIndex = null;

}


getFees(){
  this.loading = true;
  this.yearlyService.getFees().subscribe(response => {    
   this.feeDetails = response.Data.getFees;
    this.loading = false;
  }, error => {
    this.loading = false;
  })
}


addUpdateContactInfo(){
  if(this.yearlyMaintenanceSummary.YearlyMaintenanceId ==null)
  {
    this.snackBar.openSnackBar("Please select a year", 'Close', 'red-snackbar');
    return false;
  }
  return new Promise((resolve, reject) => {   
    this.loading = true;
    this.contactInfo.AAYHSContactId=this.contactInfo.AAYHSContactId==null ? 0 :this.contactInfo.AAYHSContactId,
    this.contactInfo.exhibitorSponsorState=Number(this.contactInfo.exhibitorSponsorState)
    this.contactInfo.exhibitorRefundState=Number(this.contactInfo.exhibitorRefundState),
    this.contactInfo.returnState=Number(this.contactInfo.returnState)
    this.contactInfo.yearlyMaintenanceId=this.yearlyMaintenanceSummary.YearlyMaintenanceId

    debugger;

    this.yearlyService.addUpdateContact(this.contactInfo).subscribe(response => {
      this.getContactInfo(this.yearlyMaintenanceSummary.YearlyMaintenanceId);
      this.addContactForm.resetForm();
      this.getYearlyMaintenanceByDetails(this.yearlyMaintenanceSummary.YearlyMaintenanceId)
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

getAllStates() {
    
  this.loading = true;
  this.exhibitorService.getAllStates().subscribe(response => {
      this.statesResponse = response.Data.State;
      this.loading = false;
  }, error => {
    this.loading = false;
  })
 
}

validateyear(){
  if(this.yearlyMaintenanceSummary.YearlyMaintenanceId ==null)
  {
    this.snackBar.openSnackBar("Please select a year", 'Close', 'red-snackbar');
    return false;
  }
  return true;
}

}
