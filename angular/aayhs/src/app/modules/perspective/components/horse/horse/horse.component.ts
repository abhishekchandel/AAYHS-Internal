import { Component, OnInit,ViewChild } from '@angular/core';
import { MatSnackbarComponent } from '../../../../../shared/ui/mat-snackbar/mat-snackbar/mat-snackbar.component'
import {HorseService } from '../../../../../core/services/horse.service';
import { BaseRecordFilterRequest } from '../../../../../core/models/base-record-filter-request-model'
import {  HorseInfoModel } from '../../../../../core/models/horse-model'
import { MatTabGroup } from '@angular/material/tabs'
import { NgForm } from '@angular/forms';
import { ConfirmDialogComponent, ConfirmDialogModel } from'../../../../../shared/ui/modals/confirmation-modal/confirm-dialog/confirm-dialog.component';
import { MatDialog } from '@angular/material/dialog';


@Component({
  selector: 'app-horse',
  templateUrl: './horse.component.html',
  styleUrls: ['./horse.component.scss']
})
export class HorseComponent implements OnInit {
  
  @ViewChild('tabGroup') tabGroup: MatTabGroup;
  @ViewChild('horseInfoForm') horseInfoForm: NgForm;

  sortColumn:string="";
  reverseSort : boolean = false;
  loading = false;
  totalItems: number = 0;
  horsesList: any;
  linkedExhibitors: any;
  selectedRowIndex: any;
  result: string = '';
  groups:any;
  horseType:any;
  jumpHeight:any;
  searchTerm:any;

  horseInfo:HorseInfoModel={
    Name:null,
    HorseTypeId:null,
    HorseId:null,
    NSBAIndicator:false,
    GroupId:null,
    JumpHeightId:null
  }
  exhibitorRequest = {
    HorseId: 0,
    Page: 1,
    Limit: 5,
    OrderBy: 'ExhibitorId',
    OrderByDescending: true,
    AllRecords: true
  }
  baseRequest: BaseRecordFilterRequest = {
    Page: 1,
    Limit: 5,
    OrderBy: 'HorseId',
    OrderByDescending: false,
    AllRecords: false
  };


  constructor(private snackBar: MatSnackbarComponent,
              private horseService: HorseService,
              public dialog: MatDialog,) { }

  ngOnInit(): void {
    this.getAllHorses();
    this.getAllGroups();
    this.getHorseType();
    this.getJumpHeight();
  }

  sortData(column){
    this.reverseSort=(this.sortColumn===column)?!this.reverseSort:false
    this.sortColumn=column
    this.baseRequest.OrderBy = column;
    this.baseRequest.OrderByDescending = this.reverseSort;
    this.getAllHorses()
  }
  
  getSort(column){
    if(this.sortColumn===column)
    {
    return this.reverseSort ? 'arrow-down'
    : 'arrow-up';
    }
  }

  addHorse= () => {
    this.loading = true;
    this.horseInfo.HorseId=this.horseInfo.HorseId !=null ? Number(this.horseInfo.HorseId) :0
    this.horseInfo.HorseTypeId=this.horseInfo.HorseTypeId !=null ? Number(this.horseInfo.HorseTypeId) :0
    this.horseInfo.GroupId=this.horseInfo.GroupId !=null ? Number(this.horseInfo.GroupId) :0
    this.horseInfo.JumpHeightId=this.horseInfo.JumpHeightId !=null ? Number(this.horseInfo.JumpHeightId) :0
    this.horseInfo.NSBAIndicator=this.horseInfo.NSBAIndicator !=null ? this.horseInfo.NSBAIndicator :false

    this.horseService.createUpdateHorse(this.horseInfo).subscribe(response => {
      this.snackBar.openSnackBar(response.Message, 'Close', 'green-snackbar');
      this.loading = false;
      this.resetForm();
      this.getAllHorses();
    }, error => {
      this.snackBar.openSnackBar(error, 'Close', 'red-snackbar');
      this.loading = false;

    })
  }

  getAllHorses() {
    this.loading = true;
    this.horseService.getAllHorses(this.baseRequest).subscribe(response => {
      this.horsesList = response.Data.horsesResponse;
      this.totalItems = response.Data.TotalRecords
      this.loading = false;
    }, error => {
      this.loading = false;
    }
    )
  }

  getNext(event) {
    this.baseRequest.Page = (event.pageIndex) + 1;
    this.getAllHorses()
  }

  getLinkedExhibitors(id: number) {
    this.loading = true;
    this.exhibitorRequest.HorseId = id;
    this.horseService.getLinkedExhibitors(this.exhibitorRequest).subscribe(response => {
      this.linkedExhibitors = response.Data.getLinkedExhibitors;
      this.loading = false;
    }, error => {
      this.loading = false;
      this.linkedExhibitors = null
    })
  }

  resetForm() {
    this.horseInfo.Name = null;
    this.horseInfo.HorseId = null;
    this.horseInfo.HorseTypeId = null;
    this.horseInfo.GroupId = null;
    this.horseInfo.JumpHeightId= null;
    this.horseInfo.NSBAIndicator=false;
    this.linkedExhibitors = null;
    this.horseInfoForm.resetForm();
    this.tabGroup.selectedIndex = 0;
    this.selectedRowIndex = null;
    this.linkedExhibitors = null
  }

  highlight(id, i) {
    this.resetForm()
    this.selectedRowIndex = i;
    this.getHorseDetails(id);
    this.getLinkedExhibitors(id);  
  }

  confirmRemoveHorse(e, index, data): void {
    e.stopPropagation();
    const message = `Are you sure you want to remove the horse?`;
    const dialogData = new ConfirmDialogModel("Confirm Action", message);
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      maxWidth: "400px",
      data: dialogData
    });
    dialogRef.afterClosed().subscribe(dialogResult => {
      this.result = dialogResult;
      if (this.result) {
        this.deleteHorse(data, index)
      }
    });

  }

  deleteHorse(id, index) {
    this.loading = true;
    this.horseService.deleteHorse(id).subscribe(response => {
      this.loading = false;
      this.horsesList.splice(index, 1);
      this.totalItems=this.totalItems-1;
      this.snackBar.openSnackBar(response.Message, 'Close', 'green-snackbar');
    }, error => {
      this.snackBar.openSnackBar(error, 'Close', 'red-snackbar');
      this.loading = false;

    })
  }

  getAllGroups(){
    this.loading = true;
    this.horseService.getGroups().subscribe(response => {
      this.groups = response.Data.getGroups;
      this.loading = false;
    }, error => {
      this.loading = false;
      this.groups =null;
    })
  }

getHorseDetails(id:number){
  this.loading = true;
  this.horseService.getHorseDetails(id).subscribe(response => {
    this.horseInfo = response.Data;
    this.horseInfo.GroupId=this.horseInfo.GroupId >0 ? Number(this.horseInfo.GroupId) :null
    this.horseInfo.JumpHeightId=this.horseInfo.JumpHeightId >0 ? Number(this.horseInfo.JumpHeightId) :null
    this.loading = false;
  }, error => {
    this.loading = false;
    this.horseInfo = null;
  }
  )
}

getHorseType(){
  this.loading = true;
  this.horseService.getHorseType("HorseType").subscribe(response => {
    this.horseType = response.Data.globalCodeResponse;
    this.loading = false;
  }, error => {
    this.loading = false;
    this.groups =null;
  })
}

getJumpHeight(){
  this.loading = true;
  this.horseService.getJumpHeight("JumpHeightType").subscribe(response => {
    this.jumpHeight = response.Data.globalCodeResponse;
    this.loading = false;
  }, error => {
    this.loading = false;
    this.groups =null;
  })
}
}
