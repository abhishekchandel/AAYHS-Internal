import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ShowLocationsComponent } from 'src/app/shared/ui/modals/show-locations/show-locations.component';
import { RefundCalculationModalComponent } from 'src/app/shared/ui/modals/refund-calculation-modal/refund-calculation-modal.component';
import { GeneralFeeModalComponent } from 'src/app/shared/ui/modals/general-fee-modal/general-fee-modal.component';
import { ClassCategoryModalComponent } from 'src/app/shared/ui/modals/class-category-modal/class-category-modal.component';
import { AddSizeFeeModalComponent } from 'src/app/shared/ui/modals/add-size-fee-modal/add-size-fee-modal.component';

@Component({
  selector: 'app-yearly-maintenance',
  templateUrl: './yearly-maintenance.component.html',
  styleUrls: ['./yearly-maintenance.component.scss']
})
export class YearlyMaintenanceComponent implements OnInit {

  constructor(public dialog: MatDialog) { }

  ngOnInit(): void {
  }

  openAddFeeModal(){
    const dialogRef = this.dialog.open(AddSizeFeeModalComponent, {
    });
    dialogRef.afterClosed().subscribe(dialogResult => {

      })
    
  }

  openClassCategoryModal(){
    const dialogRef = this.dialog.open(ClassCategoryModalComponent, {
      maxWidth: "400px",
    });
    dialogRef.afterClosed().subscribe(dialogResult => {

      })
  }

  openGeneralFeeModal(){
    const dialogRef = this.dialog.open(GeneralFeeModalComponent, {
      maxWidth: "400px",
    });
    dialogRef.afterClosed().subscribe(dialogResult => {

      })
  }

  openRefundCalculationFeeModal(){
    const dialogRef = this.dialog.open(RefundCalculationModalComponent, {
      maxWidth: "400px",
    });
    dialogRef.afterClosed().subscribe(dialogResult => {

      })
  }

  openShowLocationModal(){
    const dialogRef = this.dialog.open(ShowLocationsComponent, {
      maxWidth: "400px",
    });
    dialogRef.afterClosed().subscribe(dialogResult => {

      })
  }
}
