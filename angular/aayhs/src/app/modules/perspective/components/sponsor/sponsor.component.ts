import { Component, OnInit, ViewChild } from '@angular/core';
import { SponsorInformationViewModel ,TypesList} from '../../../../core/models/sponsor-model';
import { SponsorService} from '../../../../core/services/sponsor.service';
import {  AdvertisementService } from '../../../../core/services/advertisement.service';
import { ConfirmDialogComponent, ConfirmDialogModel } from '../../../../shared/ui/modals/confirmation-modal/confirm-dialog.component';
import { MatDialog } from '@angular/material/dialog';
import { MatSnackbarComponent } from '../../../../shared/ui/mat-snackbar/mat-snackbar.component';
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


  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild('sponsorInfoForm') sponsorInfoForm: NgForm;
  @ViewChild('tabGroup') tabGroup: MatTabGroup;
  @ViewChild('perfect-scrollbar ') perfectScrollbar: PerfectScrollbar
  @ViewChild('sponsorExhibitorForm') sponsorExhibitorForm: NgForm;
  @ViewChild('sponsorClassForm') sponsorClassForm: NgForm;

 
  selectedRowIndex: any;
  citiesResponse: any;
  statesResponse: any;
  result: string = '';
  totalItems: number = 0;
 
  enablePagination: boolean = true;
  sortColumn: string = "";
  reverseSort: boolean = false
  loading = true;
  sponsorInfo: SponsorInformationViewModel = {
    SponsorName: null,
    ContactName: null,
    Phone: null,
    Email: null,
    Address: null,
    CityId: null,
    StateId: null,
    ZipCode: null,
    AmountReceived: '0.00',
    SponsorId: 0,
    sponsorExhibitors: null,
    sponsorClasses: null,

  }
  sponsorsList: any
  sponsorsExhibitorsList: any
  sponsorClassesList:any
  UnassignedSponsorExhibitor:any
  UnassignedSponsorClasses:any
  //advertisementsList:any
  SponsorTypes:any
  selectedSponsorId:number=0;


  exhibitorId: number = null;
  sponsortypeId:number=null;
  typeId:string=null;
  sponsorClassId:number=null;
  showAds=false;
  showClasses=false;
  typeList:any=[];
  


  sponsorExhibitorRequest: any={
    SponsorExhibitorId: null,
    SponsorId:null,
    ExhibitorId:null,
    SponsorTypeId:null,
    TypeId:null
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
    AllRecords: false,
    SearchTerm:null
  }
  adsBaseRequest: BaseRecordFilterRequest = {
    Page: 1,
    Limit: 20,
    OrderBy: 'AdvertisementId',
    OrderByDescending: true,
    AllRecords: true,
    SearchTerm:null

  }

  sponsors: SponsorInformationViewModel[];

  constructor(private sponsorService: SponsorService,
    private advertisementService: AdvertisementService,
    private dialog: MatDialog,
    private snackBar: MatSnackbarComponent
  ) { }



  ngOnInit(): void {
    this.getAllSponsors();
    this.getAllStates();
   // this.GetAllAdvertisements()
  }

  
  getAllSponsors() {
    return new Promise((resolve, reject) => {
    this.loading = true;
    this.sponsorsList=null;
    this.sponsorService.getAllSponsers(this.baseRequest).subscribe(response => {
      if(response.Data!=null && response.Data.TotalRecords>0)
      {
     this.sponsorsList = response.Data.sponsorResponses;
     this.totalItems = response.Data.TotalRecords;
    // this.resetForm();
      }
      this.loading = false;
    }, error => {
      this.loading = false;
    })
    resolve();
  });
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

  getSponsorDetails = (id: number,selectedRowIndex) => {
    
    this.loading = true;
    this.sponsorService.getSponsor(id).subscribe(response => {
      
      if(response.Data!=null)
      {
        
      this.getCities(response.Data.StateId).then(res => {
      
         this.sponsorInfo = response.Data;
         this.selectedRowIndex=selectedRowIndex;

         this.sponsorInfo.AmountReceived= this.sponsorInfo.AmountReceived.toFixed(2);
        console.log(this.sponsorInfo);
      });
      ;
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
      this.setSponsorType(this.sponsortypeId)
      }
      this.UnassignedSponsorClasses=response.Data.unassignedSponsorClasses;
      this.loading=false;
    },error=>{
      this.loading=false;
    })
    
  }

  // GetAllAdvertisements(){
  //   this.adsBaseRequest.OrderBy = "AdvertisementId";
  //   this.adsBaseRequest.OrderByDescending = true;
  //   this.adsBaseRequest.AllRecords=true;
  //   this.advertisementsList=null;
    
  //   this.advertisementService.getAllAdvertisements(this.adsBaseRequest).subscribe(response=>{ 
  //     if(response.Data!=null && response.Data.TotalRecords>0)
  //     {
        
  //     this.advertisementsList = response.Data.advertisementResponses;
  //     this.setSponsorType(this.sponsortypeId)
  //     }
  //   },error=>{
  //   })
  // }


  AddUpdateSponsor=(sponsor)=>{
    console.log(this.sponsorInfo);
//return
    this.loading=true;
    this.sponsorInfo.AmountReceived=this.sponsorInfo.AmountReceived==null ?0:this.sponsorInfo.AmountReceived;
    this.sponsorService.addUpdateSponsor(this.sponsorInfo).subscribe(response=>{
     this.snackBar.openSnackBar(response.Message, 'Close', 'green-snackbar');
    
      // this.baseRequest.Page= 1,
      // this.baseRequest.Limit= 5,
      // this.baseRequest.OrderBy= 'SponsorId',
      // this.baseRequest.OrderByDescending= true,
      // this.baseRequest.AllRecords= false
    
     this.getAllSponsors().then(res =>{ 
      if(response.Data.NewId !=null && response.Data.NewId>0)
      {
        if(this.sponsorInfo.SponsorId>0)
        {
          this.highlight(response.Data.NewId,this.selectedRowIndex);
        }
        else{
          this.highlight(response.Data.NewId,0);
        }
      
      }
    });
     
    }, error=>{
       this.snackBar.openSnackBar(error, 'Close', 'red-snackbar');
       this.loading = false;
    })
    
    }

  AddUpdateSponsorExhibitor(){
    debugger
    this.loading=true;
    this.sponsorExhibitorRequest.SponsorExhibitorId=0;
    this.sponsorExhibitorRequest.SponsorId=this.selectedSponsorId;
    this.sponsorExhibitorRequest.ExhibitorId=this.exhibitorId;
    this.sponsorExhibitorRequest.SponsorTypeId=this.sponsortypeId;
    this.sponsorExhibitorRequest.TypeId=this.typeId!=null ?this.typeId:"";
    
    this.sponsorService.AddUpdateSponsorExhibitor(this.sponsorExhibitorRequest).subscribe(response=>{
      this.snackBar.openSnackBar(response.Message, 'Close', 'green-snackbar');
      this.GetSponsorExhibitorBySponsorId(this.selectedSponsorId);
      this.exhibitorId = null;
      this.sponsortypeId=null;
      this.sponsorExhibitorForm.resetForm({ exhibitorId:null,sponsortypeId:null});

     }, error=>{
        this.snackBar.openSnackBar(error, 'Close', 'red-snackbar');
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
      this.sponsorClassForm.resetForm({ sponsorClassId:null});
     }, error=>{
        this.snackBar.openSnackBar(error, 'Close', 'red-snackbar');
        this.loading = false;
     })
     }

  setSponsorExhibitor(id){
    this.exhibitorId=Number(id);
  }

  setSponsorType(id){
    
    this.sponsortypeId=Number(id);
    this.typeList=[];
    this.typeId=null;
    if(this.SponsorTypes!=null && this.SponsorTypes!=undefined && this.sponsortypeId!=null&& this.sponsortypeId>0)
    {
    
      var sponsorTypename=this.SponsorTypes.filter((x) => { return x.GlobalCodeId == this.sponsortypeId; });
     
      if(sponsorTypename[0].CodeName=="Class")
      {
        this.showClasses=true;
        this.showAds=false;
        debugger
       this.sponsorClassesList.forEach((data) => { 
       var listdata:TypesList={
        Id:data.ClassId,
        Name:data.ClassNumber + '/' +data.Name
       }
        this.typeList.push(listdata)
      })  
    }
      if(sponsorTypename[0].CodeName=="Ad")
      {
        this.showClasses=false;
        this.showAds=true;
        // this.advertisementsList.forEach((data) => { 
        //   var listdata:TypesList={
        //    Id:data.AdvertisementId,
        //    Name:data.Name
        //   }
        //    this.typeList.push(listdata)
        //  })  
      }
    }
  }

  setType(value){
      this.typeId=value;
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
    
    this.loading = true;
    this.sponsorService.deleteSponsor(Sponsorid).subscribe(response => {
      
      if(response.Success==true)
      {
     
     
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
    this.getAllSponsorsForPagination()
  }

    
  getAllSponsorsForPagination() {
    return new Promise((resolve, reject) => {
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
    resolve();
  });
  }

  
  highlight(selectedSponsorId, i) {
    this.selectedRowIndex = i;
    this.selectedSponsorId=selectedSponsorId;
    this.getSponsorDetails(selectedSponsorId,this.selectedRowIndex);
    this.GetSponsorExhibitorBySponsorId(selectedSponsorId);
    this.getAllSponsorTypes();
    this.exhibitorId = null;
    this.sponsortypeId=null;
    this.sponsorClassId=null;
    this.GetSponsorClasses(selectedSponsorId);
    this.sponsorClassForm.resetForm({sponsorClassId:null});
    this.sponsorExhibitorForm.resetForm({ exhibitorId:null,sponsortypeId:null});
  }

  sortData(column) {
    this.reverseSort = (this.sortColumn === column) ? !this.reverseSort : false
    this.sortColumn = column
    this.baseRequest.OrderBy = column;
    this.baseRequest.OrderByDescending = this.reverseSort;
    this.getAllSponsorsForPagination()
  }

  getSort(column) {

    if (this.sortColumn === column) {
      return this.reverseSort ? 'arrow-down'
        : 'arrow-up';
    }
  }


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

