import { Component, OnInit,ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import {FinancialTransactionsComponent} from '../../../../shared/ui/modals/financial-transactions/financial-transactions.component'
import {ExhibitorService } from '../../../../core/services/exhibitor.service';
import { BaseRecordFilterRequest } from '../../../../core/models/base-record-filter-request-model'
import { ConfirmDialogComponent, ConfirmDialogModel } from'../../../../shared/ui/modals/confirmation-modal/confirm-dialog/confirm-dialog.component';
import { MatSnackbarComponent } from '../../../../shared/ui/mat-snackbar/mat-snackbar/mat-snackbar.component'
import {ExhibitorInfoModel} from '../../../../core/models/exhibitor-model'
import { MatTabGroup } from '@angular/material/tabs'
import { BsDatepickerConfig,BsDatepickerViewMode  } from 'ngx-bootstrap/datepicker';
import { NgForm } from '@angular/forms';

@Component({
  selector: 'app-exhibitor',
  templateUrl: './exhibitor.component.html',
  styleUrls: ['./exhibitor.component.scss']
})
export class ExhibitorComponent implements OnInit {

  @ViewChild('tabGroup') tabGroup: MatTabGroup;
  @ViewChild('exhibitorInfoForm') exhibitorInfoForm: NgForm;

  datePickerConfig: Partial<BsDatepickerConfig>;
  minMode: BsDatepickerViewMode = 'year';
  minDate: Date;
  maxDate:Date;
  result: string = '';
  loading = false;
  exhibitorsList: any;
  totalItems: number = 0;
  selectedRowIndex: any;
  sortColumn:string="";
  reverseSort : boolean = false;
  citiesResponse: any;
  statesResponse: any;
  groups:any;

  baseRequest: BaseRecordFilterRequest = {
    Page: 1,
    Limit: 5,
    OrderBy: 'ExhibitorId',
    OrderByDescending: true,
    AllRecords: false
  };
  
  exhibitorInfo:ExhibitorInfoModel={
    ExhibitorId:null,
    BackNumber:null,
    FirstName:null,
    LastName:null,
    StateId:null,
    CityId:null,
    ZipCode:null,
    QTYProgram:null,
    BirthYear:null,
    Phone:null,
    PrimaryEmail:null,
    SecondaryEmail:null,
    IsNSBAMember:false,
    IsDoctorNote:false,
    GroupId:null,
    GroupName:null
  }

  constructor(
            public dialog: MatDialog,
            private exhibitorService: ExhibitorService,
            private snackBar: MatSnackbarComponent
            ) { }

  ngOnInit(): void {
    this.getAllExhibitors();
    this.getAllStates();
    this.getAllGroups();
  }

  showFinancialTransaction(){
    const dialogRef = this.dialog.open(FinancialTransactionsComponent, {
      maxWidth: "400px",
    });
    dialogRef.afterClosed().subscribe(dialogResult => {
      this.result = dialogResult;
      if (this.result) {
      }
    });
  }

  getAllExhibitors() {
    return new Promise((resolve, reject) => {
    this.loading = true;
    this.exhibitorService.getAllExhibitors(this.baseRequest).subscribe(response => {
      this.exhibitorsList = response.Data.exhibitorResponses;
      this.totalItems = response.Data.TotalRecords
      this.loading = false;
    }, error => {
      this.loading = false;
    }
    )
    resolve();
  });
  }

  highlight(id, i) {
    this.resetForm()
    this.selectedRowIndex = i;
    this.getExhibitorDetails(id);
  }

  resetForm(){
    
      this.exhibitorInfo.ExhibitorId=null,
      this.exhibitorInfo.BackNumber=null,
      this.exhibitorInfo.FirstName=null,
      this.exhibitorInfo.LastName=null,
      this.exhibitorInfo.StateId=null,
      this.exhibitorInfo.CityId=null,
      this.exhibitorInfo.ZipCode=null,
      this.exhibitorInfo.QTYProgram=null,
      this.exhibitorInfo.BirthYear=null,
      this.exhibitorInfo.Phone=null,
      this.exhibitorInfo. PrimaryEmail=null,
      this.exhibitorInfo.SecondaryEmail=null,
      this.exhibitorInfo.IsNSBAMember=false,
      this.exhibitorInfo.IsDoctorNote=false,
      this.exhibitorInfo.GroupId=null,
      this.exhibitorInfo.GroupName=null
      this.exhibitorInfoForm.resetForm();
      this.tabGroup.selectedIndex = 0
  }

  getNext(event) {
    this.resetForm()
    this.baseRequest.Page = (event.pageIndex) + 1;
    this.getAllExhibitors()
  }

  sortData(column){
    this.reverseSort=(this.sortColumn===column)?!this.reverseSort:false
    this.sortColumn=column
    this.baseRequest.OrderBy = column;
    this.baseRequest.OrderByDescending = this.reverseSort;
    this.resetForm();
    this.getAllExhibitors()
  }
  
  getSort(column){
    if(this.sortColumn===column)
    {
    return this.reverseSort ? 'arrow-down'
    : 'arrow-up';
    }
  }

  confirmRemoveExhibitor(e, index, data): void {
    e.stopPropagation();
    const message = `Are you sure you want to remove the exhibitor?`;
    const dialogData = new ConfirmDialogModel("Confirm Action", message);
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      maxWidth: "400px",
      data: dialogData
    });
    dialogRef.afterClosed().subscribe(dialogResult => {
      this.result = dialogResult;
      if (this.result) {
        this.deleteExhibitor(data, index)
      }
    });

  }

  deleteExhibitor(id, index) {
    this.loading = true;
    this.exhibitorService.deleteExhibitor(id).subscribe(response => {
      this.loading = false;
      this.getAllExhibitors()
      this.resetForm();
      this.snackBar.openSnackBar(response.Message, 'Close', 'green-snackbar');
    }, error => {
      this.snackBar.openSnackBar(error.error.Message, 'Close', 'red-snackbar');
      this.loading = false;

    })
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

getCities(id: number) {
  return new Promise((resolve, reject) => {
    this.loading = true;
    this.citiesResponse=null;
    this.exhibitorService.getCities(Number(id)).subscribe(response => {
        this.citiesResponse = response.Data.City;
        this.loading = false;
    }, error => {
      this.loading = false;
    })
      resolve();
  });
}

addUpdateExhibitor(){
  this.loading = true;
  this.exhibitorInfo.ExhibitorId=this.exhibitorInfo.ExhibitorId !=null ? Number(this.exhibitorInfo.ExhibitorId) :0
  this.exhibitorInfo.BackNumber=this.exhibitorInfo.BackNumber !=null ? Number(this.exhibitorInfo.BackNumber) :0
  this.exhibitorInfo.GroupId=this.exhibitorInfo.GroupId !=null ? Number(this.exhibitorInfo.GroupId) :0
  this.exhibitorInfo.IsNSBAMember=this.exhibitorInfo.IsNSBAMember !=null ? this.exhibitorInfo.IsNSBAMember :false
  this.exhibitorInfo.IsDoctorNote=this.exhibitorInfo.IsDoctorNote !=null ? this.exhibitorInfo.IsDoctorNote :false
  this.exhibitorInfo.BirthYear=this.exhibitorInfo.BirthYear !=null ? Number(this.exhibitorInfo.BirthYear) :0

  this.exhibitorService.createUpdateExhibitor(this.exhibitorInfo).subscribe(response => {
    this.snackBar.openSnackBar(response.Message, 'Close', 'green-snackbar');
    this.loading = false;

    this.getAllExhibitors().then(res =>{ 
      if(response.NewId !=null && response.NewId>0)
      {
        if(this.exhibitorInfo.ExhibitorId > 0)
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

  })
}

getStateName(e) {
  this.exhibitorInfo.StateId =Number( e.target.options[e.target.selectedIndex].value)
}

getCityName(e) {
this.exhibitorInfo.CityId = Number(e.target.options[e.target.selectedIndex].value)
}

getAllGroups(){
  this.loading = true;
  this.exhibitorService.getGroups().subscribe(response => {
    this.groups = response.Data.getGroups;
    this.loading = false;
  }, error => {
    this.loading = false;
    this.groups =null;
  })
}

getExhibitorDetails(id:number){
  this.loading = true;
  this.exhibitorService.getExhibitorById(id).subscribe(response => {
    if(response.Data!=null)
      {
      this.getCities(response.Data.exhibitorResponses[0].StateId).then(res => {
        this.exhibitorInfo = response.Data.exhibitorResponses[0];
      });
      }

    this.exhibitorInfo.GroupId=this.exhibitorInfo.GroupId >0 ? Number(this.exhibitorInfo.GroupId) :null
    this.loading = false;
  }, error => {
    this.loading = false;
    this.exhibitorInfo = null;
  }
  )
}

}
