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
  citiesResponse: any;
  statesResponse: any;
  result: string = '';
  totalItems: number = 0;
  selectedSponsorId:number=1;
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
    CityId: null,
    StateId: null,
    ZipCode: null,
    AmountReceived: 0,
    SponsorId: 0,
    sponsorExhibitors: null,
    sponsorClasses: null,

  }
  sponsorsList: any
  sponsorsExhibitorsList: any
  sponsorClassesList:any
  sponsorsClassesList: any
  baseRequest: BaseRecordFilterRequest = {
    Page: 1,
    Limit: 10,
    OrderBy: 'SponsorId',
    OrderByDescending: false,
    AllRecords: false
  }


  sponsors: SponsorInformationViewModel[];

  constructor(private sponsorService: SponsorService,
    private dialog: MatDialog,
    private snackBar: MatSnackbarComponent
  ) { }
  ngOnInit(): void {
    this.getAllSponsors();
    this.getAllStates();
    this.GetSponsorExhibitorBySponsorId(this.selectedSponsorId);
    this.GetSponsorClasses(this.selectedSponsorId);
  }


  getAllSponsors() {
    this.loading = true;
    this.sponsorsList=null;
    this.sponsorService.getAllSponsers(this.baseRequest).subscribe(response => {
      if(response.Data!=null && response.Data.TotalRecords>0)
      {
     this.sponsorsList = response.Data.sponsorResponses;
      }
      console.log(this.sponsorsList)
    }, error => {
    }
    )
    this.loading = false;
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
    this.sponsorInfo.AmountReceived=Number(this.sponsorInfo.AmountReceived)
    this.sponsorInfo.SponsorId=this.sponsorInfo.SponsorId !=null ? this.sponsorInfo.SponsorId : 0

    this.sponsorService.addSponsor(this.sponsorInfo).subscribe(response => {
      this.snackBar.openSnackBar(response.message, 'Close', 'green-snackbar');
      this.loading = false;
     this. resetForm()
    }, error => {
      this.snackBar.openSnackBar(error, 'Close', 'red-snackbar');
      this.loading = false;

    })
  }

  GetSponsorExhibitorBySponsorId(selectedSponsorId:number){
    this.loading=true;
    this.sponsorsExhibitorsList=null;
    
    this.sponsorService.GetSponsorExhibitorBySponsorId(selectedSponsorId).subscribe(response=>{ 
     
      if(response.Data!=null && response.Data.TotalRecords>0)
      {
     this.sponsorsExhibitorsList = response.Data.SponsorExhibitorResponses;
      }
    },error=>{

    })
    this.loading=false;
  }

  GetSponsorClasses(SponsorId:number){
    this.loading=true;
    this.sponsorClassesList=null;
    debugger
    this.sponsorService.GetSponsorClasses(SponsorId).subscribe(response=>{ 
      debugger;
      if(response.Data!=null && response.Data.TotalRecords>0)
      {
     this.sponsorClassesList = response.Data.sponsorClassesListResponses;
      }
    },error=>{

    })
    this.loading=false;
  }

  //confirm alert
  confirmRemoveSponsor(e, index, data): void {
    
    e.stopPropagation();
    const message = `Are you sure you want to remove the sponsor?`;
    const dialogData = new ConfirmDialogModel("Confirm Action", message);
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      maxWidth: "400px",
      data: dialogData
    });
    dialogRef.afterClosed().subscribe(dialogResult => {
      this.result = dialogResult;
     
      if (this.result){ this.deleteSponsor(data) }
      // this.data=   this.data.splice(index,1);
     
    });
  }
  confirmRemoveExhibitor(index, data): void {

    const message = `Are you sure you want to remove this sponsor exhibitor?`;
    const dialogData = new ConfirmDialogModel("Confirm Action", message);
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      maxWidth: "400px",
      data: dialogData
    });

    dialogRef.afterClosed().subscribe(dialogResult => {
      this.result = dialogResult;
      if(this.result)
      {
        if (this.result){ this.deleteSponsorExhibitor(data) }
      }
    });

  }

  confirmRemoveSponsorClass(index, data): void {
    const message = `Are you sure you want to remove this sponsor class?`;
    const dialogData = new ConfirmDialogModel("Confirm Action", message);
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      maxWidth: "400px",
      data: dialogData
    });

    dialogRef.afterClosed().subscribe(dialogResult => {
      this.result = dialogResult;
      if(this.result)
      {
        if (this.result){ this.deleteSponsorClass(data) }
      }
    });

  }

  
//delete record
  deleteSponsor(id: number) {
    debugger;
    this.sponsorService.deleteSponsor(id).subscribe(response => {
      if(response.Success==true)
      {
        const dialog = new ConfirmDialogModel("Confirm Action", response.Message);
        this.getAllSponsors();
      }
      else{
        const dialog = new ConfirmDialogModel("Confirm Action", response.Message);
      }
    }, error => {

    })
  }

  deleteSponsorExhibitor(SponsorExhibitorId: number) {
   
    this.sponsorService.deleteSponsorExhibitor(SponsorExhibitorId).subscribe(response => {
      if(response.Success==true)
      {
        const dialog = new ConfirmDialogModel("Confirm Action", response.Message);
        this.GetSponsorExhibitorBySponsorId(this.selectedSponsorId);
      }
      else{
        const dialog = new ConfirmDialogModel("Confirm Action", response.Message);
      }
    }, error => {

    })
  }

  deleteSponsorClass(ClassSponsorId: number) {
    debugger;
    this.sponsorService.DeleteSponsorClasse(ClassSponsorId).subscribe(response => {
      if(response.Success==true)
      {
        const dialog = new ConfirmDialogModel("Confirm Action", response.Message);
        this.GetSponsorClasses(this.selectedSponsorId);
      }
      else{
        const dialog = new ConfirmDialogModel("Confirm Action", response.Message);
      }
    }, error => {

    })
  }


  resetForm() {
    this.sponsorInfo.SponsorName = null;
    this.sponsorInfo.ContactName = null;
    this.sponsorInfo.Phone = null;
    this.sponsorInfo.Email = null;
    this.sponsorInfo.Address = null;
    this.sponsorInfo.CityId = null;
    this.sponsorInfo.StateId= null;
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

  getCities(id: number) {
    debugger;
    this.sponsorService.getCities(Number(id)).subscribe(response => {
        this.citiesResponse = response.Data.City;
    }, error => {

    })
  }

  getAllStates() {
    debugger;
      this.loading = true;
      this.sponsorService.getAllStates().subscribe(response => {
        debugger;
          this.statesResponse = response.Data.State;
      }, error => {
      })
      this.loading = false;
  }

  getStateName(e) {
    this.sponsorInfo.StateId =Number( e.target.options[e.target.selectedIndex].value)
}
 getCityName(e) {
  this.sponsorInfo.CityId = Number(e.target.options[e.target.selectedIndex].value)
}
}

