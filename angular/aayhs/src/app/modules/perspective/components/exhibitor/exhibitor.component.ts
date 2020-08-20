import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import {FinancialTransactionsComponent} from '../../../../shared/ui/modals/financial-transactions/financial-transactions.component'

@Component({
  selector: 'app-exhibitor',
  templateUrl: './exhibitor.component.html',
  styleUrls: ['./exhibitor.component.scss']
})
export class ExhibitorComponent implements OnInit {

  result: string = '';

  constructor(public dialog: MatDialog) { }

  ngOnInit(): void {
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

}
