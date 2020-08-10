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
  @ViewChild('classInfoForm') classInfoForm: NgForm;

  sortColumn:string="";
  reverseSort : boolean = false
  loading = false;
  totalItems: number = 0;
  horsesList: any
  linkedExhibitors: any;
  selectedRowIndex: any;
  result: string = '';

  horseInfo:HorseInfoModel={
    HorseName:null,
    HorseType:null,
    HorseId:null,
    Stall:null,
    TackStall:null,
    NSBAIndicator:false,
    Group:0,
    JumpHeight:null
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
    this.getAllHorses()
  }

  sortData(column){
    this.reverseSort=(this.sortColumn===column)?!this.reverseSort:false
    this.sortColumn=column
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
    this.horseInfo.HorseType=Number(this.horseInfo.HorseType)
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
      this.linkedExhibitors = response.Data.getClassEntries;
      this.loading = false;
    }, error => {
      this.loading = false;
      this.linkedExhibitors = null
    })
  }

  resetForm() {
    this.horseInfo.HorseName = null;
    this.horseInfo.Stall = null;
    this.horseInfo.TackStall = null;
    this.horseInfo.HorseId = 0;
    this.horseInfo.HorseType = 0;
    this.horseInfo.Group = 0;
    this.horseInfo.JumpHeight = null;
    this.linkedExhibitors = null;
    this.classInfoForm.resetForm();
    this.tabGroup.selectedIndex = 0
    this.selectedRowIndex = null
  }

  highlight(id, i) {
    this.resetForm()
    this.selectedRowIndex = i;
    
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
      this.snackBar.openSnackBar(error.error.Message, 'Close', 'red-snackbar');
      this.loading = false;

    })
  }
}
