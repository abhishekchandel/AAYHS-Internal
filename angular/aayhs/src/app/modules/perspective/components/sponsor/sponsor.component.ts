import { Component, OnInit, ViewChild } from '@angular/core';
import { SponsorInformationViewModel } from '../../../../core/models/sponsor-model';
import { SponsorService } from '../../../../core/services/sponsor.service';
import { ConfirmDialogComponent, ConfirmDialogModel } from '../../../../shared/ui/modals/confirmation-modal/confirm-dialog/confirm-dialog.component';
import { MatDialog } from '@angular/material/dialog';
import { MatSnackbarComponent } from '../../../../shared/ui/mat-snackbar/mat-snackbar/mat-snackbar.component';
import { MatPaginator } from '@angular/material/paginator';
import { NgForm } from '@angular/forms';
import { MatTabGroup } from '@angular/material/tabs'
import { MatTable } from '@angular/material/table'
import { MatTableDataSource } from '@angular/material/table/table-data-source';
import { BaseRecordFilterRequest } from '../../../../core/models/base-record-filter-request-model'
import { SponsorViewModel } from '../../../../core/models/sponsor-model'
import PerfectScrollbar from 'perfect-scrollbar';


@Component({
  selector: 'app-sponsor',
  templateUrl: './sponsor.component.html',
  styleUrls: ['./sponsor.component.scss']
})
export class SponsorComponent implements OnInit {
  sponsersDisplayedColumns: string[] = ['SponsorId', 'Sponsor', 'Remove'];
  sponsersExhibitorsDisplayedColumns: string[] = ['ExhibitorId', 'ExhibitorName', 'SponsorType', 'IdNumber', 'BirthYear', 'Remove'];
  sponsersClassesDisplayedColumns: string[] = ['ClassNumber', 'ClassName', 'AgeGroup', 'Exhibitor', 'HorseName', 'Remove'];
  sponsersAddExhibitorsDisplayedColumns: string[] = ['value', 'value1'];

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild('sponsorInfoForm') sponsorInfoForm: NgForm;
  @ViewChild('tabGroup') tabGroup: MatTabGroup;
  @ViewChild('perfect-scrollbar ') perfectScrollbar: PerfectScrollbar
  selectedRowIndex: any;


  result: string = '';
  totalItems: number = 0;
  enablePagination: boolean = true;
  sortColumn: string = "";
  reverseSort: boolean = false
  loading = false;
  sponsorInfo: SponsorInformationViewModel = {
    SponsorName: null,
    ContactName: null,
    Phone: null,
    Email: null,
    Address: null,
    City: null,
    State: null,
    ZipCode: null,
    AmountReceived: 0,
    SponsorId: 0,
    sponsorExhibitors: null,
    sponsorClasses: null,

  }
  sponsorsList: any
  baseRequest: BaseRecordFilterRequest = {
    Page: 1,
    Limit: 10,
    OrderBy: 'SponsorId',
    OrderByDescending: true,
    AllRecords: false
  }


  sponsors: SponsorInformationViewModel[];

  constructor(private sponsorService: SponsorService,
    private dialog: MatDialog,
    private snackBar: MatSnackbarComponent
  ) { }
  ngOnInit(): void {
    this.getAllSponsors();
  }


  getAllSponsors() {
    debugger;
    this.sponsorService.getAllSponsers(this.baseRequest).subscribe(response => {
      this.sponsorsList = response.Data.sponsorResponses;
      debugger;
      console.log(this.sponsorsList)
    }, error => {
    }
    )
  }


  getSponsorDetails = (id: number) => {
    this.sponsorService.getSponsor(id).subscribe(response => {
      this.sponsorInfo = response
    }, error => {
    }
    )
  }

  addSponsor = (sponsor) => {
    this.loading = true;
    this.sponsorService.addSponsor(this.sponsorInfo).subscribe(response => {
      this.snackBar.openSnackBar(response.Message, 'Close', 'green-snackbar');
    }, error => {
      this.snackBar.openSnackBar(error, 'Close', 'red-snackbar');
    })
    this.loading = false;
  }


  confirmRemoveExhibitor(index, data): void {

    const message = `Are you sure you want to remove this exhibitor?`;
    const dialogData = new ConfirmDialogModel("Confirm Action", message);
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      maxWidth: "400px",
      data: dialogData
    });

    dialogRef.afterClosed().subscribe(dialogResult => {
      this.result = dialogResult;
    });

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
  confirmRemoveSponsor(e, index, data): void {
    debugger;
    e.stopPropagation();
    const message = `Are you sure you want to remove the sponsor?`;
    const dialogData = new ConfirmDialogModel("Confirm Action", message);
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      maxWidth: "400px",
      data: dialogData
    });
    dialogRef.afterClosed().subscribe(dialogResult => {
      this.result = dialogResult;
      if (this.result) this.deleteSponsor(data)
      // this.data=   this.data.splice(index,1);
      debugger;
    });
  }
  deleteSponsor(id: number) {
    this.sponsorService.deleteSponsor(id).subscribe(response => {
    }, error => {

    })
  }

  resetForm() {
    this.sponsorInfo.SponsorName = null;
    this.sponsorInfo.ContactName = null;
    this.sponsorInfo.Phone = null;
    this.sponsorInfo.Email = null;
    this.sponsorInfo.Address = null;
    this.sponsorInfo.City = null;
    this.sponsorInfo.State = null;
    this.sponsorInfo.ZipCode = null;
    this.sponsorInfo.AmountReceived = 0;
    this.sponsorInfo.SponsorId = 0;
    this.sponsorInfoForm.resetForm();
    this.tabGroup.selectedIndex = 0
  }
  getNext(event, type) {
    this.baseRequest.Page = (event.pageIndex) + 1;
    this.getAllSponsors()
  }

  highlight(row, i) {
    debugger;
    this.selectedRowIndex = i;
    // this.getSponsorDetails(row.SponsorId);

  }

  sortData(column) {
    this.reverseSort = (this.sortColumn === column) ? !this.reverseSort : false
    this.sortColumn = column
  }

  getSort(column) {

    if (this.sortColumn === column) {
      return this.reverseSort ? 'arrow-down'
        : 'arrow-up';
    }
  }
}
