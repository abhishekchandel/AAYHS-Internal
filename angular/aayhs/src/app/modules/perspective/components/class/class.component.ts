import { Component, OnInit, ViewChild } from '@angular/core';
import {  ClassInfoModel } from '../../../../core/models/class-model'
import { ConfirmDialogComponent, ConfirmDialogModel } from '../../../../shared/ui/modals/confirmation-modal/confirm-dialog/confirm-dialog.component';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { AddSplitClassModalComponent } from '../../../../shared/ui/modals/add-split-class-modal/add-split-class-modal.component';
import { BaseRecordFilterRequest } from '../../../../core/models/base-record-filter-request-model'
import { MatSnackbarComponent } from '../../../../shared/ui/mat-snackbar/mat-snackbar/mat-snackbar.component'
import { MatTabGroup } from '@angular/material/tabs'
import { NgForm } from '@angular/forms';
import { MatTabChangeEvent } from '@angular/material/tabs'
import { ClassService } from '../../../../core/services/class.service';
import { MatSort } from '@angular/material/sort';
import * as moment from 'moment';



@Component({
  selector: 'app-class',
  templateUrl: './class.component.html',
  styleUrls: ['./class.component.scss']
})
export class ClassComponent implements OnInit {
  @ViewChild('tabGroup') tabGroup: MatTabGroup;
  @ViewChild('classInfoForm') classInfoForm: NgForm;
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  @ViewChild('resultForm') resultForm: NgForm;
  @ViewChild('entriesForm') entriesForm: NgForm;


  result: string = '';
  selectedRowIndex: any;
  classRequest = {
    ClassId: 0,
    Page: 1,
    Limit: 5,
    OrderBy: 'ExhibitorClassId',
    OrderByDescending: true,
    AllRecords: false
  }
  totalItems: number = 0;
  sortColumn: string = "";
  reverseSort: boolean = false
  entriesSortColumn:string="Exhibitor"
  entriesReverseSort:boolean=false
  classesList: any
  loading = false;
  classEntries: any;
  exhibitorsResponse: any
  classHeaders:any;
  exhibitorsHorsesResponse: any
  exhibitorId: number = null;
  horseId: number = null;
  backNumber:number=null;
  initialPostion:string='1st';
  resultResponse: any
  showPosition:boolean=false;
  backNumbersResponse:any;
  exhibitorInfo={
    ExhibitorId:null,
    ExhibitorName:null,
    BirthYear:null,
    HorseName:null,
    Address:null,
    AmountPaid:null,
    AmountDue:null,
    Place:null
  };
  baseRequest: BaseRecordFilterRequest = {
    Page: 1,
    Limit: 5,
    OrderBy: 'ClassId',
    OrderByDescending: false,
    AllRecords: false
  };
  classInfo: ClassInfoModel = {
    ClassId: 0,
    ClassHeaderId: null,
    ClassNumber: null,
    Name: null,
    AgeGroup: null,
    ScheduleDate: null,
    getClassSplit: null,
    SplitNumber: 0,
    ChampionShipIndicator:false
  }
  constructor(
    public dialog: MatDialog,
    private classService: ClassService,
    private snackBar: MatSnackbarComponent
  ) { }

  ngOnInit(): void {
    this.getAllClasses();
    this.getClassheaders();
  }


  confirmRemoveClass(e, index, data): void {
    e.stopPropagation();
    const message = `Are you sure you want to remove the class?`;
    const dialogData = new ConfirmDialogModel("Confirm Action", message);
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      maxWidth: "400px",
      data: dialogData
    });
    dialogRef.afterClosed().subscribe(dialogResult => {
      this.result = dialogResult;
      if (this.result) {
        this.deleteClass(data, index)
      }
    });

  }

  confirmRemoveExhibitor(index, data): void {
    debugger;
    const message = `Are you sure you want to remove the exhibitor?`;
    const dialogData = new ConfirmDialogModel("Confirm Action", message);
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      maxWidth: "400px",
      data: dialogData
    });
    dialogRef.afterClosed().subscribe(dialogResult => {
      this.result = dialogResult;
      if (this.result) {
         this.deleteExhibitor(data,index)
      }
    });

  }

  confirmScratch(index,isScratch, id): void {
    debugger
    const message = `Are you sure you want to make the changes?`;
    const dialogData = new ConfirmDialogModel("Confirm Action", message);
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      maxWidth: "400px",
      data: dialogData
    });
    dialogRef.afterClosed().subscribe(dialogResult => {
      this.result = dialogResult;
      if (this.result) {
        this.updateScratch(id,isScratch,index);
      }
    });

  }
  updateScratch(id,isScratch,index){
    var exhibitorScratch={
      ExhibitorClassId:id,
      IsScratch:isScratch
    }

    this.loading = true;
    this.classService.updateScratch(exhibitorScratch).subscribe(response => {
      this.loading = false;
      this.classEntries[index].Scratch=isScratch
      this.snackBar.openSnackBar(response.Message, 'Close', 'green-snackbar');
    }, error => {
      this.snackBar.openSnackBar(error.error.Message, 'Close', 'red-snackbar');
      this.loading = false;

    })
  }

  showSplitClass() {
    var data = {
      splitNumber: this.classInfo.SplitNumber,
      entries: this.classInfo.getClassSplit,
      className:this.classInfo.Name,
      classNumber:this.classInfo.ClassNumber,
      championshipIndicator:this.classInfo.ChampionShipIndicator
    }
    const dialogRef = this.dialog.open(AddSplitClassModalComponent, {
      maxWidth: "400px",
      data
    });

    dialogRef.afterClosed().subscribe(dialogResult => {
      const result: any = dialogResult;
debugger;
      if (result && result.submitted == true) {
        this.classInfo.SplitNumber=result.data.splitNumber;
        this.classInfo.getClassSplit=result.data.entries;
        this.classInfo.ChampionShipIndicator=result.data.championshipIndicator
      }
    });
  }

  getAllClasses() {
    this.loading = true;
    this.classService.getAllClasses(this.baseRequest).subscribe(response => {
      this.classesList = response.Data.classesResponse;
      this.totalItems = response.Data.TotalRecords
      this.loading = false;
    }, error => {
      this.loading = false;

    }
    )
  }
  sortData(column) {
    this.reverseSort = (this.sortColumn === column) ? !this.reverseSort : false
    this.sortColumn = column
    this.baseRequest.OrderBy = column;
    this.baseRequest.OrderByDescending = this.reverseSort;
    this.getAllClasses()
  }

  getSort(column) {
    if (this.sortColumn === column) {
      return this.reverseSort ? 'arrow-down'
        : 'arrow-up';
    }
  }

  highlight(id, i) {
    this.resetForm()
    this.selectedRowIndex = i;
    this.getClassDetails(id);
    this.getClassEntries(id);
    this.getClassExhibitors(id)
    this.getClassResult(id)
    this.getAllBackNumbers(id)
  }
  getClassDetails = (id: number) => {
    this.loading = true;
    this.classService.getClassById(id).subscribe(response => {
      this.classInfo = response.Data.classResponse[0];
      this.loading = false;
    }, error => {
      this.loading = false;
      this.classInfo = null;
    }
    )
  }

  addClass = () => {
    this.loading = true;
    debugger;
    this.classInfo.ClassHeaderId=Number(this.classInfo.ClassHeaderId)
    this.classService.createUpdateClass(this.classInfo).subscribe(response => {
      this.snackBar.openSnackBar(response.Message, 'Close', 'green-snackbar');
      this.loading = false;
      this.resetForm();
      this.getAllClasses();
    }, error => {
      this.snackBar.openSnackBar(error, 'Close', 'red-snackbar');
      this.loading = false;

    })
  }

  resetForm() {
    this.classInfo.ClassHeaderId = null;
    this.classInfo.Name = null;
    this.classInfo.ClassNumber = null;
    this.classInfo.AgeGroup = null;
    this.classInfo.ClassId = 0;
    this.classInfo.ScheduleDate = null;
    this.classInfo.getClassSplit = null;
    this.classInfo.SplitNumber = 0;


    this.classEntries = null;
    this.resultResponse = null;
    this.exhibitorsResponse =null;
    this.exhibitorsHorsesResponse=null;
    this.classInfoForm.resetForm();
    this.tabGroup.selectedIndex = 0
    this.backNumbersResponse =null;
    this.entriesForm.resetForm({ exhibitorId:null,horseId:null})
    this.resultForm.resetForm({ backNumber:null});
    this.resetExhibitorInfo()
    this.initialPostion='1st';
    this.selectedRowIndex = null
  }

  getClassEntries(id: number) {
    this.loading = true;
    this.classRequest.ClassId = id;
    this.classRequest.AllRecords = true
    this.classRequest.OrderBy = 'ExhibitorClassId'
    this.classService.getClassEnteries(this.classRequest).subscribe(response => {
      this.classEntries = response.Data.getClassEntries;
      this.loading = false;
    }, error => {
      this.loading = false;
      this.classEntries = null
    })
  }

  onChange(event: MatTabChangeEvent) {
    const tab = event.index;
    console.log(tab);
    if (tab === 0) {
      //  this.getClassDetails()
    }
    else if (tab === 1) {
      // this.getClassEntries()
    }
    else {

    }
  }

  getClassExhibitors(id: number) {
    this.loading = true;
    this.classService.getClassExhibitors(id).subscribe(response => {
      this.exhibitorsResponse = response.Data.getClassExhibitors;
      this.loading = false;
    }, error => {
      this.exhibitorsResponse =null;
      this.loading = false;
    })
  }

  getExhibitorHorses(id) {
    this.classService.getExhibitorHorses(id).subscribe(response => {
      this.exhibitorsHorsesResponse = response.Data.getExhibitorHorses;
      this.exhibitorId = id

    }, error => {
      this.exhibitorsHorsesResponse=null;
    })

  }

  selectHorse(id: number) {
    this.horseId = id
  }

  addExhibitorToClass() {
    if (this.classInfo.ClassId == null) {
      this.snackBar.openSnackBar("Please select a class to add exhibitor", 'Close', 'red-snackbar');

    }
    this.loading = true;
    var addClassExhibitor = {
      ExhibitorId: Number(this.exhibitorId),
      ClassId: this.classInfo.ClassId,
      HorseId: Number(this.horseId),
    }
    this.classService.addExhibitorToClass(addClassExhibitor).subscribe(response => {
      this.snackBar.openSnackBar(response.Message, 'Close', 'green-snackbar');
      this.loading = false;
      this.getClassEntries(this.classInfo.ClassId);
      this.entriesForm.resetForm({ exhibitorId:null,horseId:null});
    }, error => {
      this.snackBar.openSnackBar(error.error.Message, 'Close', 'red-snackbar');
      this.loading = false;

    })
  }
  deleteExhibitor(id: number,index) {
    this.loading = true;
    this.classService.deleteClassExhibitor(id).subscribe(response => {
      this.classEntries.splice(index, 1);

      this.snackBar.openSnackBar(response.Message, 'Close', 'green-snackbar');
      this.loading = false;
    }, error => {
      this.snackBar.openSnackBar(error.error.Message, 'Close', 'red-snackbar');
      this.loading = false;

    })
  }
  deleteClass(id, index) {
    this.loading = true;
    this.classService.deleteClass(id).subscribe(response => {
      this.loading = false;
      this.classesList.splice(index, 1);
      this.totalItems=this.totalItems-1;
      this.snackBar.openSnackBar(response.Message, 'Close', 'green-snackbar');
    }, error => {
      this.snackBar.openSnackBar(error.error.Message, 'Close', 'red-snackbar');
      this.loading = false;

    })
  }
  getNext(event) {
    this.baseRequest.Page = (event.pageIndex) + 1;
    this.getAllClasses()
  }
  getClassResult(id: number) {
    this.loading = true;
    this.classRequest.ClassId = id
    this.classRequest.AllRecords = true
    this.classRequest.OrderBy = 'ExhibitorId'
    this.classService.getClassResult(this.classRequest).subscribe(response => {
      this.resultResponse = response.Data.getResultOfClass;
      this.initialPostion=this.ordinal_suffix_of(Number(response.Data.TotalRecords) +1)
      this.loading = false;
    }, error => {
      this.loading = false;
      this.resultResponse = null
    })
  }
  handleScheduleDateSelection() {
    this.classInfo.ScheduleDate = moment(this.classInfo.ScheduleDate).format('YYYY-MM-DD');
  }
  getAllBackNumbers(id: number){
    this.loading = true;
    this.classService.getAllBackNumbers(id).subscribe(response => {
      this.backNumbersResponse = response.Data.getBackNumbers;
      this.loading = false;
    }, error => {
      this.loading = false;
      this.backNumbersResponse =null;
    })
  }
  getExhibitorDetail(id){
    var exhibitordetailRequest={
      ClassId:this.classInfo.ClassId,
      BackNumber:Number(id)
    }
    this.classService.getExhibitorDetails(exhibitordetailRequest).subscribe(response => {
      this.exhibitorInfo = response.Data;
     this.showPosition=true
    }, error => {
      this.exhibitorInfo=null;
    })
  }
  addResult(){
    
    this.loading = true;
    var addClassResult = {
      ClassId:this.classInfo.ClassId,
      ExhibitorId:this.exhibitorInfo.ExhibitorId,
      Place:(this.initialPostion).toString()
    }
    this.classService.addResult(addClassResult).subscribe(response => {
      this.loading = false;
      this.getClassResult(this.classInfo.ClassId);
      this.resultForm.resetForm({ backNumber:null});
      this.resetExhibitorInfo();
      this.initialPostion='1st';
      this.showPosition=false
      this.snackBar.openSnackBar(response.Message, 'Close', 'green-snackbar');

    }, error => {
      this.snackBar.openSnackBar(error.error.Message, 'Close', 'red-snackbar');
      this.loading = false;

    })
  }
  resetExhibitorInfo(){
     this.exhibitorInfo. ExhibitorId=null,
     this.exhibitorInfo.ExhibitorName=null,
     this.exhibitorInfo.BirthYear=null,
     this.exhibitorInfo.HorseName=null,
     this.exhibitorInfo.Address=null,
     this.exhibitorInfo.AmountPaid=null,
     this.exhibitorInfo.AmountDue=null,
     this.exhibitorInfo.Place=null
  }
  getClassheaders(){
    this.loading = true;
    this.classService.getClassHeaders().subscribe(response => {
      this.classHeaders = response.Data.globalCodeResponse;
      this.loading = false;
    }, error => {
      this.loading = false;

    }
    )
  }
   ordinal_suffix_of(i) {
    var j = i % 10,
        k = i % 100;
    if (j == 1 && k != 11) {
        return i + "st";
    }
    if (j == 2 && k != 12) {
        return i + "nd";
    }
    if (j == 3 && k != 13) {
        return i + "rd";
    }
    return i + "th";
}
setClassHeader(value){
  this.classInfo.ClassHeaderId=Number(value)
}

sortEntriesData(column) {
  debugger;
  this.entriesReverseSort = (this.entriesSortColumn === column) ? !this.entriesReverseSort : false
  this.entriesSortColumn = column

}

getEntriesSort(column) {
  if (this.entriesSortColumn === column) {
    return this.entriesReverseSort ? 'arrow-down'
      : 'arrow-up';
  }
}

}
