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
  UnassignedSponsorExhibitor:any
  UnassignedSponsorClasses:any
  SponsorTypes:any
  selectedSponsorId:number=0;


  exhibitorId: number = null;
  sponsortypeId:number=null;
  sponsorClassId:number=null;

  
  sponsorExhibitorRequest: any={
    SponsorExhibitorId: null,
    SponsorId:null,
    ExhibitorId:null,
    SponsorTypeId:null,
  }
  sponsorClassRequest: any={
    ClassSponsorId:null,
    SponsorId:null,
    ClassId:null,
  }

  baseRequest: BaseRecordFilterRequest = {
    Page: 1,
    Limit: 5,
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
    this.getAllStates();

  }

  
  getAllSponsors() {
    this.loading = true;
    this.sponsorsList=null;
    this.sponsorService.getAllSponsers(this.baseRequest).subscribe(response => {
      if(response.Data!=null && response.Data.TotalRecords>0)
      {
     this.sponsorsList = response.Data.sponsorResponses;
     this.totalItems = response.Data.TotalRecords;
     this.resetForm();
      }
      this.loading = false;
    }, error => {
      this.loading = false;
    })
    
  }

  getAllSponsorTypes() {
    this.loading = true;
    this.SponsorTypes=null;
    this.sponsorService.getAllSponsorTypes('SponsorTypes').subscribe(response => {
      if(response.Data!=null && response.Data.totalRecords>0)
      {
     this.SponsorTypes = response.Data.globalCodeResponse;
      }
      this.loading = false;
    }, error => {
      this.loading = false;
    })
  }

  getSponsorDetails = (id: number) => {
    this.loading = true;
    this.sponsorService.getSponsor(id).subscribe(response => {
      if(response.Data!=null)
      {
        debugger;
      this.getCities(response.Data.StateId).then(res => 
       
         this.sponsorInfo = response.Data
          );;
      debugger;
      }
      this.loading = false;
    }, error => {
      this.loading = false;
    })
  }

  GetSponsorExhibitorBySponsorId(selectedSponsorId:number){
    this.loading=true;
    this.sponsorsExhibitorsList=null;
    this.UnassignedSponsorExhibitor=null;
    this.sponsorService.GetSponsorExhibitorBySponsorId(selectedSponsorId).subscribe(response=>{ 
      if(response.Data!=null && response.Data.TotalRecords>0)
      {
      this.sponsorsExhibitorsList = response.Data.SponsorExhibitorResponses;
      }
      this.UnassignedSponsorExhibitor=response.Data.UnassignedSponsorExhibitor;
      this.loading=false;
    },error=>{
      this.loading=false;
    })
    
  }

  GetSponsorClasses(SponsorId:number){
    this.loading=true;
    this.sponsorClassesList=null;
    this.UnassignedSponsorClasses=null;
    this.sponsorService.GetSponsorClasses(SponsorId).subscribe(response=>{ 
      if(response.Data!=null && response.Data.TotalRecords>0)
      {
      this.sponsorClassesList = response.Data.sponsorClassesListResponses;
      }
      this.UnassignedSponsorClasses=response.Data.unassignedSponsorClasses;
      this.loading=false;
    },error=>{
      this.loading=false;
    })
    
  }


  AddUpdateSponsor=(sponsor)=>{
    debugger
    this.loading=true;
    this.sponsorInfo.AmountReceived=this.sponsorInfo.AmountReceived==null ?0:this.sponsorInfo.AmountReceived;
    this.sponsorService.addUpdateSponsor(this.sponsorInfo).subscribe(response=>{
     this.snackBar.openSnackBar(response.Message, 'Close', 'green-snackbar');
     this.getAllSponsors();
    }, error=>{
       this.snackBar.openSnackBar(error.error.Message, 'Close', 'red-snackbar');
       this.loading = false;
    })
    
    }

  AddUpdateSponsorExhibitor(){
    this.loading=true;
    this.sponsorExhibitorRequest.SponsorExhibitorId=0;
    this.sponsorExhibitorRequest.SponsorId=this.selectedSponsorId;
    this.sponsorExhibitorRequest.ExhibitorId=this.exhibitorId;
    this.sponsorExhibitorRequest.SponsorTypeId=this.sponsortypeId;
    this.sponsorService.AddUpdateSponsorExhibitor(this.sponsorExhibitorRequest).subscribe(response=>{
      this.snackBar.openSnackBar(response.Message, 'Close', 'green-snackbar');
      this.GetSponsorExhibitorBySponsorId(this.selectedSponsorId);
      this.exhibitorId = null;
      this.sponsortypeId=null;
     }, error=>{
        this.snackBar.openSnackBar(error.error.Message, 'Close', 'red-snackbar');
        this.loading = false;
     })
    
    }

  AddUpdateSponsorClass(){
    this.loading=true;
    this.sponsorClassRequest.ClassSponsorId=0;
    this.sponsorClassRequest.SponsorId=this.selectedSponsorId;
    this.sponsorClassRequest.ClassId=this.sponsorClassId;
    this.sponsorService.AddUpdateSponsorClass(this.sponsorClassRequest).subscribe(response=>{
      this.snackBar.openSnackBar(response.Message, 'Close', 'green-snackbar');
      this.GetSponsorClasses(this.selectedSponsorId);
      this.sponsorClassId=null;
     }, error=>{
        this.snackBar.openSnackBar(error.error.Message, 'Close', 'red-snackbar');
        this.loading = false;
     })
     }





  setSponsorExhibitor(id){
    this.exhibitorId=Number(id);
  }
  
  setSponsorType(id){
    
    this.sponsortypeId=Number(id);
  }

  setSponsorClass(id){
    this.sponsorClassId=Number(id);
  }

 



  //confirm delete 
  confirmRemoveSponsor(e, index, Sponsorid): void {
    e.stopPropagation();
    const message = `Are you sure you want to remove the sponsor?`;
    const dialogData = new ConfirmDialogModel("Confirm Action", message);
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      maxWidth: "400px",
      data: dialogData
    });
    dialogRef.afterClosed().subscribe(dialogResult => {
      this.result = dialogResult;
      if (this.result){ this.deleteSponsor(Sponsorid,index) }
    });
  }

  confirmRemoveExhibitor(e,index, SponsorExhibitorId): void {
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
        if (this.result){ this.deleteSponsorExhibitor(SponsorExhibitorId,index) }
      }
    });

  }

  confirmRemoveSponsorClass(e,index, ClassSponsorId): void {
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
        if (this.result){ this.deleteSponsorClass(ClassSponsorId,index) }
      }
    });
  }

  
//delete record
  deleteSponsor(Sponsorid,index) {
    debugger
    this.loading = true;
    this.sponsorService.deleteSponsor(Sponsorid).subscribe(response => {
      
      if(response.Success==true)
      {
     
       // this.getAllSponsors();
        this.sponsorsList.splice(index, 1);
        this.totalItems=this.totalItems-1;

        if(this.selectedSponsorId==Sponsorid){
          this.selectedSponsorId=0;
          this.sponsorsExhibitorsList= null;
          this.sponsorClassesList=null;
          this.UnassignedSponsorExhibitor=null;
          this.UnassignedSponsorClasses=null;
          this.SponsorTypes=null;
          this.resetForm();
        }
        this.loading = false;
        this.snackBar.openSnackBar(response.Message, 'Close', 'green-snackbar');
      }
      else{
        this.loading = false;
        this.snackBar.openSnackBar(response.Message, 'Close', 'red-snackbar');
       
      }
    }, error => {
      this.loading = false;
    })
   
  }

  deleteSponsorExhibitor(SponsorExhibitorId,index) {
    this.loading = true;
    this.sponsorService.deleteSponsorExhibitor(SponsorExhibitorId).subscribe(response => {
      if(response.Success==true)
      {
        this.GetSponsorExhibitorBySponsorId(this.selectedSponsorId);
        this.snackBar.openSnackBar(response.Message, 'Close', 'green-snackbar');
      }
      else{
        this.loading = false;
        this.snackBar.openSnackBar(response.Message, 'Close', 'red-snackbar');
      }
    }, error => {
      this.loading = false;
    })
  }

  deleteSponsorClass(ClassSponsorId,index) {
    this.loading = true;
    this.sponsorService.DeleteSponsorClasse(ClassSponsorId).subscribe(response => {
      if(response.Success==true)
      {
        this.GetSponsorClasses(this.selectedSponsorId);
        this.snackBar.openSnackBar(response.Message, 'Close', 'green-snackbar');
      }
      else{
        this.loading = false;
        this.snackBar.openSnackBar(response.Message, 'Close', 'red-snackbar');
      }
    }, error => {
      this.loading = false;
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

    this.selectedSponsorId=0;
    this.sponsorsExhibitorsList= null;
    this.sponsorClassesList=null;
    this.UnassignedSponsorExhibitor=null;
    this.UnassignedSponsorClasses=null;
    this.SponsorTypes=null;
    this.selectedRowIndex =-1;

    this.exhibitorId = null;
    this.sponsortypeId=null;
    this.sponsorClassId=null;
  }

  getNext(event) {
    this.baseRequest.Page = (event.pageIndex) + 1;
    this.getAllSponsors()
  }
  
  
  highlight(selectedSponsorId, i) {
    
    this.selectedRowIndex = i;
    this.selectedSponsorId=selectedSponsorId;
    this.getSponsorDetails(selectedSponsorId);
    this.GetSponsorExhibitorBySponsorId(selectedSponsorId);
    this.getAllSponsorTypes();
    this.exhibitorId = null;
    this.sponsortypeId=null;
    this.sponsorClassId=null;
    this.GetSponsorClasses(selectedSponsorId);
  }


  sortData(column) {
    this.reverseSort = (this.sortColumn === column) ? !this.reverseSort : false
    this.sortColumn = column
    this.baseRequest.OrderBy = column;
    this.baseRequest.OrderByDescending = this.reverseSort;
    this.getAllSponsors()
  }

  getSort(column) {

    if (this.sortColumn === column) {
      return this.reverseSort ? 'arrow-down'
        : 'arrow-up';
    }
  }

  // getCities(id: number) {
  //   this.loading = true;
  //   this.sponsorService.getCities(Number(id)).subscribe(response => {
  //       this.citiesResponse = response.Data.City;
  //       this.loading = false;
  //   }, error => {
  //     this.loading = false;
  //   })
    
  // }
 

  getCities(id: number) {
    return new Promise((resolve, reject) => {
      this.loading = true;
      this.citiesResponse=null;
      this.sponsorService.getCities(Number(id)).subscribe(response => {
          this.citiesResponse = response.Data.City;
          this.loading = false;
      }, error => {
        this.loading = false;
      })
        resolve();
    });
 }

  getAllStates() {
    
      this.loading = true;
      this.sponsorService.getAllStates().subscribe(response => {
          this.statesResponse = response.Data.State;
          this.loading = false;
      }, error => {
        this.loading = false;
      })
     
  }

  getStateName(e) {
    this.sponsorInfo.StateId =Number( e.target.options[e.target.selectedIndex].value)
  }

  getCityName(e) {
  this.sponsorInfo.CityId = Number(e.target.options[e.target.selectedIndex].value)
  }

  goToLink(url: string){
  window.open(url, "_blank");
}


}

