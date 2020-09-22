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
import { SponsorInfoModalComponent} from '../../../../shared/ui/modals/sponsor-info-modal/sponsor-info-modal.component'
import { NgForm } from '@angular/forms';
import {TypesList} from '../../../../core/models/sponsor-model';
import { SponsorService} from '../../../../core/services/sponsor.service';
import { BaseUrl } from 'src/app/config/url-config';
import { FilteredFinancialTransactionsComponent } from 'src/app/shared/ui/modals/filtered-financial-transactions/filtered-financial-transactions.component';
import * as moment from 'moment';
import { EmailModalComponent } from 'src/app/shared/ui/modals/email-modal/email-modal.component';
import { HttpEventType } from '@angular/common/http';
import { MatPaginator } from '@angular/material/paginator';

@Component({
  selector: 'app-exhibitor',
  templateUrl: './exhibitor.component.html',
  styleUrls: ['./exhibitor.component.scss']
})
export class ExhibitorComponent implements OnInit {

  @ViewChild('tabGroup') tabGroup: MatTabGroup;
  @ViewChild('exhibitorInfoForm') exhibitorInfoForm: NgForm;
  @ViewChild('horsesForm') horsesForm: NgForm;
  @ViewChild('classesForm') classesForm: NgForm;
  @ViewChild('sponsorsForm') sponsorsForm: NgForm;
  @ViewChild('scanForm') scanForm: NgForm;
  @ViewChild('paginator') paginator: MatPaginator;

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
  exhibitorSponsors:any;
  showScratch:boolean=false;
  sponsors:any;
  linkedSponsorId:number=null;
  addnumber:number=null;
  addType:string=null;
  sponsorTypes:any
  sponsortypeId=null;
  showClasses=false;
  typeList:any=[];
  typeId:number=null;
  showAds=false;
  sponsorClassesList:any
  UnassignedSponsorClasses:any
  adTypeId:number=null;
  billedSummary:any;
  receievedSummary:any;
  outstanding:any;
  overPayment:any;
  refunds:any;
  AdTypes:any;
  myFiles:File;
  documentTypeId:any;
  pdf:any;
  feeBilledTotal:any;
  moneyReceivedTotal:any
  documentTypes:any;
  documentId:number=null;
  scannedDocuments:any;
  horseDate:any;
  classDate:any;
  exhibitorTransactions:any;
  feeDetails:any
  isRefund:boolean=false;
  minDate=moment( new Date()).format('YYYY-MM-DD');
  //for binding images with server url
  filesUrl = BaseUrl.filesUrl;
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
    QTYProgram:1,
    BirthYear:null,
    Phone:null,
    PrimaryEmail:null,
    SecondaryEmail:null,
    IsNSBAMember:false,
    IsDoctorNote:false,
    GroupId:null,
    GroupName:null
  };
  classDetails:any={
    Entries:null,
    IsScratch:null
  };
  sponsorDetails:any={
  Email:null,
  AmountReceived:null,
  SponsorId:null,
  SponsorName:null,
  ContactName:null,
  Phone:null,
  Address:null,
  City:null,
  State:null,
  };
  constructor(
            public dialog: MatDialog,
            private exhibitorService: ExhibitorService,
            private snackBar: MatSnackbarComponent,
            private data: GlobalService,
            private sponsorService: SponsorService
            ) { }

  ngOnInit(): void {
    this.data.searchTerm.subscribe((searchTerm: string) => {
      debugger;
      this.baseRequest.SearchTerm = searchTerm;
      this.baseRequest.Page = 1;
      this.getAllExhibitors();
    });    
    this.getAllStates();
    this.getAllGroups();
    this.setYears();
    this.getAllAdTypes();
    this.getDocumentTypes();
  }

  showFinancialTransaction(){
    var data={
      ExhibitorId:this.exhibitorInfo.ExhibitorId,
      ExhibitorName:this.exhibitorInfo.FirstName +' '+ this.exhibitorInfo.LastName,
      feeDetails:this.feeDetails,
      exhibitorTransactions:this.exhibitorTransactions,
      isRefund:this.isRefund

    }
    debugger;
    const dialogRef = this.dialog.open(FinancialTransactionsComponent, {
      maxWidth: "400px",
      data
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

  highlight(id, i) {
    this.resetForm()
    this.selectedRowIndex = i;
    this.getbilledFeesSummary(id);
    this.getExhibitorTransactions(id);
    this.getExhibitorDetails(id);
    this.getExhibitorHorses(id);
    this.getAllHorses(id);
    this.getExhibitorClasses(id);
    this.getAllClasses(id);
    this.getExhibitorSponsors(id);
    this.getAllSponsors(id);
    this.getAllSponsorTypes();
    this.getScannedDocuments(id);
    this.getFees();
  }

  resetForm(){
    
      // info section
      this.exhibitorInfo.ExhibitorId=null,
      this.exhibitorInfo.BackNumber=null,
      this.exhibitorInfo.FirstName=null,
      this.exhibitorInfo.LastName=null,
      this.exhibitorInfo.StateId=null,
      this.exhibitorInfo.CityId=null,
      this.exhibitorInfo.ZipCodeId=null,
      this.exhibitorInfo.QTYProgram=1,
      this.exhibitorInfo.BirthYear=null,
      this.exhibitorInfo.Phone=null,
      this.exhibitorInfo. PrimaryEmail=null,
      this.exhibitorInfo.SecondaryEmail=null,
      this.exhibitorInfo.IsNSBAMember=false,
      this.exhibitorInfo.IsDoctorNote=false,
      this.exhibitorInfo.GroupId=null,
      this.exhibitorInfo.GroupName=null
      this.exhibitorInfoForm.resetForm({quantityPrograms:1});
      this.selectedRowIndex = null

      //horses section
      this.exhibitorHorses=null;
      this.isFirstBackNumber=false;
      this.horses=null;
      this.resetLinkedhorse();
      this.horseType = null;


      //class section
      this.exhibitorClasses = null;
      this.classes =null;
      this.classDetails. Entries=null,
      this.classDetails.IsScratch= null;
      this.resetLinkClass();

      //sponsor section
      this.exhibitorSponsors = null;
      this.sponsors =null;
      this.resetLinkSponsor();
      this.sponsorDetails.Email=null,
      this.sponsorDetails.AmountReceived=null,
      this.sponsorDetails.SponsorId=null,
      this.sponsorDetails.SponsorName=null,
      this.sponsorDetails.ContactName=null,
      this.sponsorDetails.Phone=null,
      this.sponsorDetails.Address=null,
      this.sponsorDetails.City=null,
      this.sponsorDetails.State=null,
      this.sponsorTypes=null;

      //financial section
      this.billedSummary =null;
      this.receievedSummary=null;
      this.feeBilledTotal=null;
      this.moneyReceivedTotal=null;
      this.overPayment=null
      this.outstanding=null
      this.refunds=null
      this.exhibitorTransactions = null;

      //scan section
      this.documentId=null;
      this.myFiles=null;
      this.scannedDocuments =null;
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


getZipCodes(event,exhibitorDetail) {
  var name;
  exhibitorDetail==true ?name= event : name = event.target.options[event.target.options.selectedIndex].text;
  return new Promise((resolve, reject) => {
    this.loading = true;
    this.zipCodesResponse=null;
    this.exhibitorService.getZipCodes(name).subscribe(response => {
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
  this.exhibitorInfo.QTYProgram=this.exhibitorInfo.QTYProgram !=null ? Number(this.exhibitorInfo.QTYProgram) :0

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
  return new Promise((resolve, reject) => {
  this.loading = true;
 this.exhibitorService.getExhibitorById(id).subscribe(response => {
    if(response.Data!=null)
      {
      this.getCities(response.Data.exhibitorResponses[0].StateId).then(res => {
        this.getZipCodes(response.Data.exhibitorResponses[0].CityName,true).then(res => {
        this.exhibitorInfo = response.Data.exhibitorResponses[0];
        this.exhibitorInfo.BackNumber=response.Data.exhibitorResponses[0].BackNumber ===0 ? null :response.Data.exhibitorResponses[0].BackNumber;
      });
    });
    this.exhibitorInfo.GroupId=this.exhibitorInfo.GroupId >0 ? Number(this.exhibitorInfo.GroupId) :null;
    this.exhibitorInfo.QTYProgram=this.exhibitorInfo.QTYProgram >0 ? Number(this.exhibitorInfo.QTYProgram) :null
  }
   
    this.loading = false;
  }, error => {
    this.loading = false;
    this.exhibitorInfo = null;
  }
  )
  resolve();
});
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
    let count=  response.Data.TotalRecords;
    count > 0 ?  this.isFirstBackNumber=false : this.isFirstBackNumber=true
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
      this.getAllHorses(this.exhibitorInfo.ExhibitorId);
      this.getExhibitorDetails(this.exhibitorInfo.ExhibitorId);
      this.getbilledFeesSummary(this.exhibitorInfo.ExhibitorId);
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
    backNumber: this.backNumberLinked !=null ? Number(this.backNumberLinked) : Number(this.exhibitorInfo.BackNumber),
    isFirstBackNumber:this.isFirstBackNumber,
    date:this.horseDate
  }
  this.exhibitorService.addHorseToExhibitor(addHorse).subscribe(response => {
    this.loading = false;
    this.horsesForm.resetForm({ horseControl: null,backNumberControl:null });
    this.resetLinkedhorse();
    this.getExhibitorHorses(this.exhibitorInfo.ExhibitorId);
    this.getAllHorses(this.exhibitorInfo.ExhibitorId);
    this.getbilledFeesSummary(this.exhibitorInfo.ExhibitorId);
    this.snackBar.openSnackBar(response.Message, 'Close', 'green-snackbar');

  }, error => {
    this.snackBar.openSnackBar(error, 'Close', 'red-snackbar');
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
  this.horseDate=null;
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

confirmScratch( isScratch, id): void {
  const message = `Are you sure you want to make the changes?`;
  const dialogData = new ConfirmDialogModel("Confirm Action", message);
  const dialogRef = this.dialog.open(ConfirmDialogComponent, {
    maxWidth: "400px",
    data: dialogData
  });
  dialogRef.afterClosed().subscribe(dialogResult => {
    this.result = dialogResult;
    if (this.result) {
     this.updateScratch(id, isScratch);
    }
  });

}

updateScratch(id, isScratch){
  var exhibitorScratch = {
    ExhibitorClassId: id,
    IsScratch: isScratch
  }
  this.loading = true;
  this.exhibitorService.updateScratch(exhibitorScratch).subscribe(response => {
    this.loading = false;
    this.getExhibitorClasses(this.exhibitorInfo.ExhibitorId)
    this.snackBar.openSnackBar(response.Message, 'Close', 'green-snackbar');
  }, error => {
    this.snackBar.openSnackBar(error.error.Message, 'Close', 'red-snackbar');
    this.loading = false;

  })
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
      this.deleteExhibitorClass(data)
    }
  });

  
}

deleteExhibitorClass(id){
  this.loading = true;
  this.exhibitorService.deleteExhibitorClass(id).subscribe(response => {
    this.loading = false;
    this.getExhibitorClasses(this.exhibitorInfo.ExhibitorId);
    this.getAllClasses(this.exhibitorInfo.ExhibitorId);
    this.getbilledFeesSummary(this.exhibitorInfo.ExhibitorId);
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
  this.exhibitorService.getClassDetail(Number(id),this.exhibitorInfo.ExhibitorId).subscribe(response => {
   this.classDetails=response.Data;
   this.showScratch=true;
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
    date:this.classDate
  }
  this.exhibitorService.addExhibitorToClass(addClass).subscribe(response => {
    this.loading = false;
    this.resetLinkClass();
    this.classesForm.resetForm({ classControl: null });
    this.getExhibitorClasses(this.exhibitorInfo.ExhibitorId);
    this.getAllClasses(this.exhibitorInfo.ExhibitorId);
    this.getbilledFeesSummary(this.exhibitorInfo.ExhibitorId);
    this.snackBar.openSnackBar(response.Message, 'Close', 'green-snackbar');

  }, error => {
    this.snackBar.openSnackBar(error.error.Message, 'Close', 'red-snackbar');
    this.loading = false;
  })
}

resetLinkClass(){
  this.linkedClassId=null;
  this.classDetails.Entries=null;
  this.classDetails.IsScratch=null;
  this.showScratch=false;
  this.classDate=null;
}


getExhibitorSponsors(id){
  return new Promise((resolve, reject) => {
    this.loading = true;
    this.exhibitorService.getExhibitorSponsors(id).subscribe(response => {
        this.exhibitorSponsors=response.Data.getSponsorsOfExhibitors;
      this.loading = false;
    }, error => {
      this.loading = false;
      this.exhibitorSponsors = null;  
    }
    )
    resolve();
    })
}

confirmRemoveExhibitorSponsor(data){

  const message = `Are you sure you want to remove the sponsor?`;
  const dialogData = new ConfirmDialogModel("Confirm Action", message);
  const dialogRef = this.dialog.open(ConfirmDialogComponent, {
    maxWidth: "400px",
    data: dialogData
  });
  dialogRef.afterClosed().subscribe(dialogResult => {
    this.result = dialogResult;
    if (this.result) {
      this.deleteExhibitorSponsor(data)
    }
  });

  
}

deleteExhibitorSponsor(id){
  this.loading = true;
  this.exhibitorService.deleteExhibitorSponsor(id).subscribe(response => {
    this.loading = false;
    this.getExhibitorSponsors(this.exhibitorInfo.ExhibitorId);
    this.getAllSponsors(this.exhibitorInfo.ExhibitorId);
    this.snackBar.openSnackBar(response.Message, 'Close', 'green-snackbar');
  }, error => {
    this.snackBar.openSnackBar(error.error.Message, 'Close', 'red-snackbar');
    this.loading = false;

  })
}

getAllSponsors(id){
  this.loading = true;
  this.exhibitorService.getAllSponsors(id).subscribe(response => {
    this.sponsors = response.Data.getSponsorForExhibitors;
    this.loading = false;
  }, error => {
    this.loading = false;
    this.sponsors =null;
  })
}

addSponsorToExhibitor(){
  this.loading = true;
  var addSponsor = {
    exhibitorId: this.exhibitorInfo.ExhibitorId,
    sponsorId:Number(this.linkedSponsorId),
    sponsorTypeId:this.sponsortypeId,
    AdTypeId:this.adTypeId!=null ? this.adTypeId :0,
    typeId:this.typeId!=null ? this.typeId:""
  }
  this.exhibitorService.addSponsorToExhibitor(addSponsor).subscribe(response => {
    this.loading = false;
    this.resetLinkSponsor();
    this.sponsorsForm.resetForm({ addNumberControl: null ,typeControl:null,addTypeControl:null,});
    this.getExhibitorSponsors(this.exhibitorInfo.ExhibitorId);
    this.getAllSponsors(this.exhibitorInfo.ExhibitorId);
    this.getExhibitorTransactions(this.exhibitorInfo.ExhibitorId);
    this.snackBar.openSnackBar(response.Message, 'Close', 'green-snackbar');

  }, error => {
    debugger;
    this.snackBar.openSnackBar(error, 'Close', 'red-snackbar');
    this.loading = false;
  })
}

resetLinkSponsor(){
  this.typeId=null;
  this.adTypeId=null;
  this.linkedSponsorId=null;
  this.sponsortypeId=null;
  this.sponsorDetails.Email=null,
  this.sponsorDetails.AmountReceived=null,
  this.sponsorDetails.SponsorId=null,
  this.sponsorDetails.SponsorName=null,
  this.sponsorDetails.ContactName=null,
  this.sponsorDetails.Phone=null,
  this.sponsorDetails.Address=null,
  this.sponsorDetails.City=null,
  this.sponsorDetails.State=null
}

getSponsorDetails(id){
  this.loading = true;
  this.linkedSponsorId=id;
  this.exhibitorService.getSponsordetails(Number(id)).subscribe(response => {
   this.sponsorDetails=response.Data;
    this.loading = false;
  }, error => {
    this.loading = false;
    this.sponsorDetails = null;
  }
  )
}

showSponsorInfo(sponsor,isNew){
  var data;

  if(isNew)
  {
    data = {
      sponsorName: this.sponsorDetails.SponsorName,
      contactName: this.sponsorDetails.ContactName,
      phone: this.sponsorDetails.Phone,
      email: this.sponsorDetails.Email,
      address: this.sponsorDetails.Address,
      amount: this.sponsorDetails.AmountReceived,
      state: this.sponsorDetails.State,
      city: this.sponsorDetails.City,
      zipcode: this.sponsorDetails.Zipcode,
      sponsorId: this.sponsorDetails.SponsorId,
  
    }
  }
  else{
    data = {
      sponsorName: sponsor.Sponsor,
      contactName: sponsor.ContactName,
      phone: sponsor.Phone,
      email: sponsor.Email,
      address: sponsor.Address,
      amount: sponsor.Amount,
      state: sponsor.State,
      city: sponsor.City,
      zipcode: sponsor.Zipcode,
      sponsorId: sponsor.SponsorId,
  
    }
  }
   
  const dialogRef = this.dialog.open(SponsorInfoModalComponent, {
    maxWidth: "400px",
    data
  });
  dialogRef.afterClosed().subscribe(dialogResult => {    
  });
}

getAllSponsorTypes() {
  this.loading = true;
  this.sponsorTypes=null;
  this.sponsorService.getAllTypes('SponsorTypes').subscribe(response => {
    if(response.Data!=null && response.Data.totalRecords>0)
    {
   this.sponsorTypes = response.Data.globalCodeResponse;
    }
    this.loading = false;
  }, error => {
    this.loading = false;
    this.sponsorTypes=null;
  })
}

setSponsorType(id){
  this.sponsortypeId=Number(id);
  this.typeList=[];
  this.typeId=null;
  if(this.sponsorTypes!=null && this.sponsorTypes!=undefined && this.sponsortypeId!=null&& this.sponsortypeId>0)
  {
  
    var sponsorTypename=this.sponsorTypes.filter((x) => { return x.GlobalCodeId == this.sponsortypeId; });
   
    if(sponsorTypename[0].CodeName=="Class")
    {
      this.showClasses=true;
      this.showAds=false;
      
      //get sponsor classes
debugger
      this.sponsorClassesList= this.exhibitorClasses
      // this.sponsorService.GetSponsorClasses(Number(this.linkedSponsorId)).subscribe(response=>{ 
      //   if(response.Data!=null && response.Data.TotalRecords>0)
      //   {
      //   this.sponsorClassesList = response.Data.sponsorClassesListResponses;
      //   }
      //   this.loading=false;
      // },error=>{
      //   this.loading=false;
      // })

  }
    if(sponsorTypename[0].CodeName=="Ad")
    {
      this.showClasses=false;
      this.showAds=true;
    }
  }
}

setType(value){
  this.typeId=value;
}

getbilledFeesSummary(id){
  this.loading = true;
  this.exhibitorService.getbilledFeesSummary(id).subscribe(response => {
    this.billedSummary = response.Data.exhibitorFeesBilled;
    this.feeBilledTotal=response.Data.FeeBilledTotal
    this.receievedSummary=response.Data.exhibitorMoneyReceived
    this.moneyReceivedTotal=response.Data.MoneyReceivedTotal
    this.overPayment=response.Data.OverPayment
    this.outstanding=response.Data.Outstanding
    this.refunds=response.Data.Refunds
    this.loading = false;
  }, error => {
    this.loading = false;
    this.billedSummary =null;
    this.receievedSummary=null;
    this.feeBilledTotal=null;
    this.moneyReceivedTotal=null;
    this.overPayment=null
    this.outstanding=null
    this.refunds=null
  })
}

getAllAdTypes() {
  this.loading = true;
  this.AdTypes=null;
  this.sponsorService.getAllTypes('AdTypes').subscribe(response => {
    if(response.Data!=null && response.Data.totalRecords>0)
    {
   this.AdTypes = response.Data.globalCodeResponse;
    }
    this.loading = false;
  }, error => {
    this.loading = false;
  })
}

setAdType(id){
  this.adTypeId=Number(id);
}

uploadDocument(){
  if(this.exhibitorInfo.ExhibitorId==null){
    this.snackBar.openSnackBar('Please select the exhibitor', 'Close', 'red-snackbar');
    return false;
  }
   const formData :any= new FormData();
   this.loading = true;  
   formData.append('exhibitor',this.exhibitorInfo.ExhibitorId);
   formData.append('documentType',this.documentId);
   formData.append('documents', this.myFiles);
      this.exhibitorService.uploadDocument(formData).subscribe(response => {
       this.snackBar.openSnackBar(response.Message, 'Close', 'green-snackbar');
       this.getScannedDocuments(this.exhibitorInfo.ExhibitorId);
      this.scanForm.resetForm()
      this.documentId=null;
       this.myFiles=null 
       this.loading = false;     
     }
     , error => {
       this.snackBar.openSnackBar(error, 'Close', 'red-snackbar');
       this.loading = false;

     });
}

onFileChange($event) {
      this.myFiles=($event.target.files[0]);
      var reader = new FileReader();
      const file = $event.target.files[0];
      reader.readAsDataURL(file);
    
   
}

getDocumentTypes() {
  this.loading = true;
  this.exhibitorService.getDocumentTypes().subscribe(response => {
    this.documentTypes = response.Data.globalCodeResponse;
    this.loading = false;
  }, error => {
    this.loading = false;
    this.documentTypes=null;
  }
  )
}

setDocumentType(value){
  this.documentId=Number(value)
}

getScannedDocuments(id){
  this.loading = true;
  this.exhibitorService.getScannedDocuments(id).subscribe(response => {
    this.scannedDocuments = response.Data.getUploadedDocuments;
    this.loading = false;
  }, error => {
    this.loading = false;
    this.scannedDocuments =null;
  })
}

viewDocument(path){
  window.open(this.filesUrl+path.replace(/\s+/g, '%20'), '_blank');

}

openTransactionDetails(id){
  var data={
    feeTypeId:id,
    exhibitorId:this.exhibitorInfo.ExhibitorId,
  }
  debugger;
  const dialogRef = this.dialog.open(FilteredFinancialTransactionsComponent, {
    maxWidth: "400px",
    data
  });
  dialogRef.afterClosed().subscribe(dialogResult => {    
  });
}

handleHorseDate(){
  this.horseDate = moment(this.horseDate).format('YYYY-MM-DD');
}

handleClassDate(){
  this.classDate = moment(this.classDate).format('YYYY-MM-DD');
}

confirmRemoveDocument(id,path): void {
  const message = `Are you sure you want to remove the document?`;
  const dialogData = new ConfirmDialogModel("Confirm Action", message);
  const dialogRef = this.dialog.open(ConfirmDialogComponent, {
    maxWidth: "400px",
    data: dialogData
  });
  dialogRef.afterClosed().subscribe(dialogResult => {
    this.result = dialogResult;
    if (this.result) {
      this.deleteDocument(id,path)
    }
  });
}

  deleteDocument(id,path){
    this.loading = true;
    var document={
      scanId:id,
      path:path
    }
    debugger;
    this.exhibitorService.deleteDocument(document).subscribe(response => {
      this.loading = false;
      this.scanForm.resetForm();
      this.documentId=null;
      this.myFiles=null 
      this.getScannedDocuments(this.exhibitorInfo.ExhibitorId);
       this.snackBar.openSnackBar(response.Message, 'Close', 'green-snackbar');
    }, error => {
      this.snackBar.openSnackBar(error.error.Message, 'Close', 'red-snackbar');
      this.loading = false;

    })
  }

  getExhibitorTransactions(id){
    this.loading = true;
    this.exhibitorService.getExhibitorTransactions(id).subscribe(response => {
     this.exhibitorTransactions=response.Data.getExhibitorTransactions;
     this.isRefund=response.Data.IsRefund;
      this.loading = false;
    }, error => {
      this.loading = false;
      this.exhibitorTransactions = null;
    }
    )
  }

  getFees(){
    this.loading = true;
    this.exhibitorService.getFees().subscribe(response => {    
     this.feeDetails = response.Data.getFees;
      this.loading = false;
    }, error => {
      this.loading = false;
    })
  }
  recalculate(){
    this.getbilledFeesSummary(this.exhibitorInfo.ExhibitorId);
    this.getExhibitorTransactions(this.exhibitorInfo.ExhibitorId);
  }

  printDocument(url){
    url="https://localhost:44386/resources/documents/db29482b-a751-448a-bb18-c860e20104a0__Resources_Documents_93489a63-faa0-45f3-a868-c341e5307b67_download1.jpg"
    var top = (screen.availHeight - 600) / 2;
    var left = (screen.availWidth - 800) / 2;
    window.open(url, "_blank", "directories=no,height=600,width=800,location=no,menubar=no,resizable=yes," +"scrollbars=yes,status=no,toolbar=no,top=" + top + ",left=" + left);
    window.print();
  }

  openEmailModal(path){
  const dialogRef = this.dialog.open(EmailModalComponent, {
    maxWidth: "400px",
    data:path
  });
  dialogRef.afterClosed().subscribe(dialogResult => {
    this.result = dialogResult;
  });
  }

  

   downloadFile(path) {
       this.loading = true;
    this.exhibitorService.downloadFile(path).subscribe(
      data => {
        switch (data.type) {
          case HttpEventType.DownloadProgress:
            break;
          case HttpEventType.Response:
            const downloadedFile = new Blob([data.body], { type: data.body.type });
            const a = document.createElement('a');
            a.setAttribute('style', 'display:none;');
            document.body.appendChild(a);
            a.download = path;
            a.href = URL.createObjectURL(downloadedFile);
            a.target = '_blank';
            a.click();
            document.body.removeChild(a);
            break;
        }
        this.loading = false;

      },
      error => {
        this.snackBar.openSnackBar(error, 'Close', 'red-snackbar');
      }
    );
  }

}
