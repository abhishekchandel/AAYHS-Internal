import { Component, OnInit,ViewChild } from '@angular/core';
import{  ClassViewModel,ClassInfoModel} from '../../../../../core/models/class-model'
import {  ConfirmDialogComponent,ConfirmDialogModel} from '../../../../../shared/ui/modals/confirmation-modal/confirm-dialog/confirm-dialog.component';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import {  AddSplitClassModalComponent} from '../../../../../shared/ui/modals/add-split-class-modal/add-split-class-modal.component';
import{ClassService} from '../../../../../core/services/class.service';
import{BaseRecordFilterRequest} from '../../../../../core/models/base-record-filter-request-model'
import { MatSnackbarComponent } from '../../../../../shared/ui/mat-snackbar/mat-snackbar/mat-snackbar.component'
import { MatTabGroup } from '@angular/material/tabs'
import { NgForm } from '@angular/forms';
import{MatTabChangeEvent}from '@angular/material/tabs'

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
  @ViewChild('tabGroup') tabGroup: MatTabGroup;
  @ViewChild('classInfoForm') classInfoForm: NgForm;


  classesDisplayedColumns: string[] = ['ClassNumber', 'ClassName', 'AgeGroup','Enteries','Remove'];
  data: ClassViewModel[] = ELEMENT_DATA;
  result: string = '';
  selectedRowIndex: any;
  classRequest={
    ClassId:0,
    Page: 1,
    Limit: 10,
    OrderBy: 'ExhibitorClassId',
    OrderByDescending: true,
    AllRecords: false
  }
  @ViewChild(MatPaginator) paginator: MatPaginator;
  totalItems: number=0;
  sortColumn:string="";
  reverseSort : boolean = false
  classesList:any
  loading = false;
  classEntries:any;
  exhibitorsResponse:any
  exhibitorsHorsesResponse:any
  baseRequest :BaseRecordFilterRequest={
    Page: 1,
    Limit: 10,
    OrderBy: 'ClassId',
    OrderByDescending: false,
    AllRecords: false
   };
   classInfo: ClassInfoModel = {
    ClassHeaderId:null,
    ClassNumber:null,
    Name:null,
    AgeGroup:null,
    ScheduleDate:null

  }
  constructor(
    public dialog: MatDialog,
    private classService: ClassService,
    private snackBar: MatSnackbarComponent
  ) { }

  ngOnInit(): void {
    this.getAllClasses();
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
      // if (this.result) this.deleteSponsor(data)
      // this.data=   this.data.splice(index,1);
      debugger;
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
      this.classesList=response.Data.classesResponse;
      this.totalItems=response.Data.TotalRecords
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

highlight(id, i) {
  debugger;
  this.selectedRowIndex = i;
  //  this.getSponsorDetails(id);
  this.getClassEntries(id);
  this.getClassExhibitors(id)
}
getClassDetails = (id: number) => {
  this.loading=true;
  this.classService.getClassById(id).subscribe(response => {
  this.classInfo = response.Data;
  this.loading=false;

  }, error => {
    this.loading=false;
    this.classInfo =null;
  }
  )
}

addClass = (sponsor) => {
  this.loading = true;
  this.classService.createUpdateClass(this.classInfo).subscribe(response => {
    this.snackBar.openSnackBar(response.message, 'Close', 'green-snackbar');
    this.loading = false;
   this. resetForm()
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
 
  this.classInfoForm.resetForm();
  this.tabGroup.selectedIndex = 0
}

getClassEntries(id:number){
  debugger;
  this.loading = true;
  this.classRequest.ClassId=id
  this.classService.getClassEnteries(this.classRequest).subscribe(response => {
    debugger;
    this.classEntries=response.Data.getClassEntries;
    this.loading = false;
   this. resetForm()
  }, error => {
    debugger;
    this.snackBar.openSnackBar(error, 'Close', 'red-snackbar');
    this.loading = false;
    this.classEntries=null
  })
}

onChange(event: MatTabChangeEvent) {
  debugger;
  const tab = event.index;
  console.log(tab);
  if(tab===0)
   {
    //  this.getClassDetails()
    }
    else if(tab===1)
    {
      // this.getClassEntries()
    }
    else{

    }
}

getClassExhibitors(id:number){
  this.classService.getClassExhibitors(id).subscribe(response => {
    this.exhibitorsResponse = response.Data.getClassExhibitors;
}, error => {

})
}

getExhibitorHorses(id:number){
  this.classService.getExhibitorHorses(id).subscribe(response => {
    this.exhibitorsHorsesResponse = response.Data.getExhibitorHorses;
}, error => {

})

}
}
