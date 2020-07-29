import { Component, OnInit,ViewChild } from '@angular/core';
import{  ClassViewModel} from '../../../../../core/models/class-model'
import {  ConfirmDialogComponent,ConfirmDialogModel} from '../../../../../shared/ui/modals/confirmation-modal/confirm-dialog/confirm-dialog.component';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';

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



  constructor(
    public dialog: MatDialog,
  ) { }

  ngOnInit(): void {
  }


  confirmRemoveClass(index, data): void {
    debugger;
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

}
