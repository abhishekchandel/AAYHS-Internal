import { Component, OnInit,ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import {FinancialTransactionsComponent} from '../../../../shared/ui/modals/financial-transactions/financial-transactions.component'
import {ExhibitorService } from '../../../../core/services/exhibitor.service';
import { BaseRecordFilterRequest } from '../../../../core/models/base-record-filter-request-model'
import { ConfirmDialogComponent, ConfirmDialogModel } from'../../../../shared/ui/modals/confirmation-modal/confirm-dialog.component';
import { MatSnackbarComponent } from '../../../../shared/ui/mat-snackbar/mat-snackbar.component'
import {ExhibitorInfoModel} from '../../../../core/models/exhibitor-model'
import { MatTabGroup } from '@angular/material/tabs'
import {GlobalService} from '../../../../core/services/global.service'

import { NgForm } from '@angular/forms';

@Component({
  selector: 'app-exhibitor',
  templateUrl: './exhibitor.component.html',
  styleUrls: ['./exhibitor.component.scss']
})
export class ExhibitorComponent implements OnInit {

  @ViewChild('tabGroup') tabGroup: MatTabGroup;
  @ViewChild('exhibitorInfoForm') exhibitorInfoForm: NgForm;
  @ViewChild('horsesForm') horsesForm: NgForm;

  searchTerm:string;
  maxyear: any;
  minyear:any;
  result: string = '';
  loading = false;
  exhibitorsList: any;
  totalItems: number = 0;
  selectedRowIndex: any;
  sortColumn:string="";
  reverseSort : boolean = false;
  citiesResponse: any;
  statesResponse: any;
  zipCodesResponse:any;
  groups:any;
  years=[]
  exhibitorHorses:any;
  horses:any;
  linkedHorseId:number=null;
  horseType:string=null;
  backNumberLinked:any;
  isFirstBackNumber:boolean=false;
  exhibitorClasses:any;
  classes:any;
  linkedClassId:number=null;
  baseRequest: BaseRecordFilterRequest = {
    Page: 1,
    Limit: 5,
    OrderBy: 'ExhibitorId',
    OrderByDescending: true,
    AllRecords: false,
    SearchTerm:null
  };
  
  exhibitorInfo:ExhibitorInfoModel={
    ExhibitorId:null,
    BackNumber:null,
    FirstName:null,
    LastName:null,
    StateId:null,
    CityId:null,
    ZipCodeId:null,
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
  classDetails:any={
    entries:null,
    scratch:null
  }
  constructor(
            public dialog: MatDialog,
            private exhibitorService: ExhibitorService,
            private snackBar: MatSnackbarComponent,
            private data: GlobalService
            ) { }

  ngOnInit(): void {
    this.data.searchTerm.subscribe((searchTerm: string) => {
      this.baseRequest.SearchTerm = searchTerm;
      this.getAllExhibitors();

    });    this.getAllStates();
    this.getAllGroups();
    this.setYears();
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
    this.getExhibitorHorses(id);
    this.getAllHorses(id);
    this.getExhibitorClasses(id);
    this.getAllClasses(id);
  }

  resetForm(){
    
      this.exhibitorInfo.ExhibitorId=null,
      this.exhibitorInfo.BackNumber=null,
      this.exhibitorInfo.FirstName=null,
      this.exhibitorInfo.LastName=null,
      this.exhibitorInfo.StateId=null,
      this.exhibitorInfo.CityId=null,
      this.exhibitorInfo.ZipCodeId=null,
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
      this.selectedRowIndex = null

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


getZipCodes(id: number) {
  return new Promise((resolve, reject) => {
    this.loading = true;
    this.zipCodesResponse=null;
    this.exhibitorService.getZipCodes(Number(id)).subscribe(response => {
      debugger
        this.zipCodesResponse = response.Data.ZipCode;
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

setBirthYear(e){
  this.exhibitorInfo.BirthYear =Number( e.target.value)

}

getCityName(e) {
this.exhibitorInfo.CityId = Number(e.target.options[e.target.selectedIndex].value)
}

getZipNumber(e) {
  this.exhibitorInfo.ZipCodeId = Number(e.target.options[e.target.selectedIndex].value)
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
        this.getZipCodes(response.Data.CityId).then(res => {
        this.exhibitorInfo = response.Data.exhibitorResponses[0];
      });
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

setYears(){
   this.maxyear = new Date().getFullYear();
    this.minyear = this.maxyear - 18;
  for (var i = this.minyear; i<=this.maxyear; i++){
   this.years.push(i)
}
}

getExhibitorHorses(id){
  return new Promise((resolve, reject) => {
  this.loading = true;
  this.exhibitorService.getExhibitorHorses(id).subscribe(response => {
      this.exhibitorHorses=response.Data.exhibitorHorses;
      this.isFirstBackNumber=false
    this.loading = false;
  }, error => {
    this.loading = false;
    this.exhibitorHorses = null;
    this.isFirstBackNumber=true
    this.horseType=null;

  }
  )
  resolve();
  })
}

deleteExhibitorHorse(id){
  this.loading = true;
    this.exhibitorService.deleteExhibitorHorse(id).subscribe(response => {
      this.loading = false;
      this.horsesForm.resetForm({ horseControl: null,backNumberControl:null });
      this.horseType=null;
      this.getExhibitorHorses(this.exhibitorInfo.ExhibitorId);
      this.snackBar.openSnackBar(response.Message, 'Close', 'green-snackbar');
    }, error => {
      this.snackBar.openSnackBar(error.error.Message, 'Close', 'red-snackbar');
      this.loading = false;

    })
}

confirmRemoveExhibitorHorse(data): void {
  const message = `Are you sure you want to remove the horse?`;
  const dialogData = new ConfirmDialogModel("Confirm Action", message);
  const dialogRef = this.dialog.open(ConfirmDialogComponent, {
    maxWidth: "400px",
    data: dialogData
  });
  dialogRef.afterClosed().subscribe(dialogResult => {
    this.result = dialogResult;
    if (this.result) {
      this.deleteExhibitorHorse(data)
    }
  });

}

getAllHorses(id){
  this.loading = true;
  this.exhibitorService.getAllHorses(id).subscribe(response => {
    this.horses = response.Data.getHorses;
    this.loading = false;
  }, error => {
    this.loading = false;
    this.horses =null;
  })
}

addHorseToExhibitor(){
  this.loading = true;
  var addHorse = {
    exhibitorId: this.exhibitorInfo.ExhibitorId,
    horseId:Number(this.linkedHorseId),
    backNumber: this.backNumberLinked !=null ? Number(this.backNumberLinked) : this.exhibitorInfo.BackNumber
  }
  this.exhibitorService.addHorseToExhibitor(addHorse).subscribe(response => {
    this.loading = false;
    this.horsesForm.resetForm({ horseControl: null,backNumberControl:null });
    this.resetLinkedhorse();
    this.getExhibitorHorses(this.exhibitorInfo.ExhibitorId);
    this.snackBar.openSnackBar(response.Message, 'Close', 'green-snackbar');

  }, error => {
    this.snackBar.openSnackBar(error.error.Message, 'Close', 'red-snackbar');
    this.loading = false;

  })
}

getHorseType(id){
  this.loading = true;
  this.linkedHorseId=id;
  this.exhibitorService.getHorseDetail(Number(id)).subscribe(response => {
   this.horseType=response.Data.HorseType;
    this.loading = false;
  }, error => {
    this.loading = false;
    this.horseType = null;
  }
  )
}

resetLinkedhorse(){
  this.backNumberLinked=null;
  this.linkedHorseId=null;
  this.horseType=null;
}


getExhibitorClasses(id){
  return new Promise((resolve, reject) => {
    this.loading = true;
    this.exhibitorService.getExhibitorClasses(id).subscribe(response => {
        this.exhibitorClasses=response.Data.getClassesOfExhibitors;
      this.loading = false;
    }, error => {
      this.loading = false;
      this.exhibitorClasses = null;  
    }
    )
    resolve();
    })
}

confirmScratch(index, isScratch, id): void {
  const message = `Are you sure you want to make the changes?`;
  const dialogData = new ConfirmDialogModel("Confirm Action", message);
  const dialogRef = this.dialog.open(ConfirmDialogComponent, {
    maxWidth: "400px",
    data: dialogData
  });
  dialogRef.afterClosed().subscribe(dialogResult => {
    this.result = dialogResult;
    if (this.result) {
      // this.updateScratch(id, isScratch, index);
    }
  });

}

confirmRemoveExhibitorClass(data){

  const message = `Are you sure you want to remove the class?`;
  const dialogData = new ConfirmDialogModel("Confirm Action", message);
  const dialogRef = this.dialog.open(ConfirmDialogComponent, {
    maxWidth: "400px",
    data: dialogData
  });
  dialogRef.afterClosed().subscribe(dialogResult => {
    this.result = dialogResult;
    if (this.result) {
      this.deleteExhibitorHorse(data)
    }
  });

  
}

deleteExhibitorClass(id){
  this.loading = true;
  this.exhibitorService.deleteExhibitorClass(id).subscribe(response => {
    this.loading = false;
    this.getExhibitorClasses(this.exhibitorInfo.ExhibitorId);
    this.snackBar.openSnackBar(response.Message, 'Close', 'green-snackbar');
  }, error => {
    this.snackBar.openSnackBar(error.error.Message, 'Close', 'red-snackbar');
    this.loading = false;

  })
}

getAllClasses(id){
  this.loading = true;
  this.exhibitorService.getAllClasses(id).subscribe(response => {
    this.classes = response.Data.getClassesForExhibitor;
    this.loading = false;
  }, error => {
    this.loading = false;
    this.classes =null;
  })
}

getClassDetails(id){
  this.loading = true;
  this.linkedClassId=id;
  this.exhibitorService.getClassDetail(Number(id)).subscribe(response => {
   this.classDetails=response.Data;
    this.loading = false;
  }, error => {
    this.loading = false;
    this.classDetails = null;
  }
  )
}

addClassToExhibitor(){
  this.loading = true;
  var addClass = {
    exhibitorId: this.exhibitorInfo.ExhibitorId,
    classId:Number(this.linkedClassId),
  }
  this.exhibitorService.addExhibitorToClass(addClass).subscribe(response => {
    this.loading = false;
    this.horsesForm.resetForm({ classControl: null });
    this.resetLinkClass();
    this.getExhibitorClasses(this.exhibitorInfo.ExhibitorId);
    this.snackBar.openSnackBar(response.Message, 'Close', 'green-snackbar');

  }, error => {
    this.snackBar.openSnackBar(error.error.Message, 'Close', 'red-snackbar');
    this.loading = false;
  })
}

resetLinkClass(){
  this.linkedClassId=null;
  this.classDetails=null;
}
}
