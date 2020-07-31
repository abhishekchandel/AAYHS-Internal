import { Component, OnInit,ViewChild } from '@angular/core';
import{  ClassViewModel} from '../../../../../core/models/class-model'
import {  ConfirmDialogComponent,ConfirmDialogModel} from '../../../../../shared/ui/modals/confirmation-modal/confirm-dialog/confirm-dialog.component';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import {  AddSplitClassModalComponent} from '../../../../../shared/ui/modals/add-split-class-modal/add-split-class-modal.component';
import{ClassService} from '../../../../../core/services/class.service';
import{BaseRecordFilterRequest} from '../../../../../core/models/base-record-filter-request-model'

const ELEMENT_DATA: ClassViewModel[] = [
  {ClassNumber: '103 N', ClassName: 'Barrels Pony',AgeGroup:'15',Enteries:15},
  {ClassNumber: '121 W', ClassName: 'Barrels Pony',AgeGroup:'18',Enteries:11},
  {ClassNumber: '231 E', ClassName: 'Barrels Pony',AgeGroup:'16-17',Enteries:10},
  {ClassNumber: '342 N', ClassName: 'Barrels Pony',AgeGroup:'10',Enteries:10},
];

@Component({
  selector: 'app-class',
  templateUrl: './class.component.html',
  styleUrls: ['./class.component.scss']
})
export class ClassComponent implements OnInit {
  classesDisplayedColumns: string[] = ['ClassNumber', 'ClassName', 'AgeGroup','Enteries','Remove'];
  data: ClassViewModel[] = ELEMENT_DATA;
  result: string = '';
  @ViewChild(MatPaginator) paginator: MatPaginator;
  totalItems: number=10;
  sortColumn:string="";
  reverseSort : boolean = false

  baseRequest :BaseRecordFilterRequest={
    Page: 1,
    Limit: 10,
    OrderBy: 'ClassId',
    OrderByDescending: false,
    AllRecords: false
   }

  constructor(
    public dialog: MatDialog,
    private classService: ClassService
  ) { }

  ngOnInit(): void {
    this.getAllClasses();
  }


  confirmRemoveClass(index, data): void {
    const message = `Are you sure you want to remove this class?`;
    const dialogData = new ConfirmDialogModel("Confirm Action", message);
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      maxWidth: "400px",
      data: dialogData
    });

    dialogRef.afterClosed().subscribe(dialogResult => {
      this.result = dialogResult;
      // this.removeCartObit(data.ObituaryId, this.result)
    });

  }
  showSplitClass()
{
  const dialogRef = this.dialog.open(AddSplitClassModalComponent, {
    maxWidth: "400px",
    data: ''
  });

  dialogRef.afterClosed().subscribe(dialogResult => {
    this.result = dialogResult;
    debugger;
    // this.removeCartObit(data.ObituaryId, this.result)
  });
  }

  getAllClasses(){
    this.classService.getAllClasses(this.baseRequest).subscribe(response =>{
    },error=>{
    }
    )
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
}
