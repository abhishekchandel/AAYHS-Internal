import { Component, OnInit, ViewChild } from '@angular/core';
import { BaseRecordFilterRequest } from 'src/app/core/models/base-record-filter-request-model';
import { MatSnackbarComponent } from '../../../../shared/ui/mat-snackbar/mat-snackbar.component'
import { MatDialog,MatDialogConfig } from '@angular/material/dialog';
import { GroupService } from 'src/app/core/services/group.service';
import { ConfirmDialogComponent, ConfirmDialogModel } from'../../../../shared/ui/modals/confirmation-modal/confirm-dialog.component';



import { MatPaginator } from '@angular/material/paginator';
import { NgForm } from '@angular/forms';
import { MatTabGroup } from '@angular/material/tabs'
import { MatTable } from '@angular/material/table'
import { MatTableDataSource } from '@angular/material/table/table-data-source';
import { GroupInformationViewModel } from 'src/app/core/models/group-model';


import PerfectScrollbar from 'perfect-scrollbar';
import { StallComponent } from '../stall/stall.component';


@Component({
  selector: 'app-group',
  templateUrl: './group.component.html',
  styleUrls: ['./group.component.scss']
})
export class GroupComponent implements OnInit {


  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild('groupInfoForm') groupInfoForm: NgForm;
  @ViewChild('tabGroup') tabGroup: MatTabGroup;
  @ViewChild('perfect-scrollbar ') perfectScrollbar: PerfectScrollbar
  @ViewChild('groupFinancialForm') groupFinancialForm: NgForm;

  baseRequest: BaseRecordFilterRequest = {
    Page: 1,
    Limit: 5,
    OrderBy: 'GroupId',
    OrderByDescending: true,
    AllRecords: false,
    SearchTerm:null

  }
  currentDate = new Date();
  cutOffDate = new Date();
  groupsList: any;
  groupExhibitorsList: any;
  groupFinancialsList:any;
  groupFinancialsTotals:any;

  PreTotal:number=0;
  PostTotal:number=0;
  PrePostTotal:number=0;

  FeeTypes:any;
  TimeFrameTypes:any;
  groupFinancialsRequest: any={
    GroupFinancialId: 0,
    GroupId:0,
    FeeTypeId:0,
    TimeFrameId:0,
    Amount:0,
  }

  updateGroupFinancialsRequest: any={
    GroupFinancialId: 0,
    Amount:0,
  }

FinancialsFeeTypeId:number=null;
FinancialsTimeFrameTypeId:number=null;
FinancialsAmount:number=null;
UpdatedFinancialAmount:number=null;


  enablePagination: boolean = true;
  sortColumn: string = "";
  reverseSort: boolean = false;
  loading = true;

  selectedRowIndex: any;
  selectedGroupId=0;
  citiesResponse: any;
  statesResponse: any;
  result: string = '';
  totalItems: number = 0;
  updatemode=false;
  updateRowIndex=-1;
  groupInfo: GroupInformationViewModel = {
    GroupName: null,
    ContactName: null,
    Phone: null,
    Email: null,
    Address: null,
    CityId: null,
    StateId: null,
    ZipCode: null,
    AmountReceived: '0.00',
    GroupId: 0,
   

  }
  constructor(private groupService: GroupService,
    private dialog: MatDialog,
    private snackBar: MatSnackbarComponent
  ) { }
  ngOnInit(): void {
    this.getAllGroups();
    this.getAllStates();
    this.getAllTimeFrameTypes();
  }

  getAllGroups() {
    return new Promise((resolve, reject) => {
    this.loading = true;
    this.groupsList=null;
    this.groupService.getAllGroups(this.baseRequest).subscribe(response => {
      if(response.Data!=null && response.Data.TotalRecords>0)
      {
     this.groupsList = response.Data.groupResponses;
     this.totalItems = response.Data.TotalRecords;
     //this.resetForm();
      }
      this.loading = false;
    }, error => {
     
      this.loading = false;
    })
    resolve();
  });
  }



  getGroupDetails = (id: number, selectedRowIndex) => {
    this.loading = true;
    this.groupService.getGroup(id).subscribe(response => {
      if(response.Data!=null)
      {
      
      this.getCities(response.Data.StateId).then(res => {
       
         this.groupInfo = response.Data;
         this.selectedRowIndex= selectedRowIndex;
         this.groupInfo.AmountReceived= this.groupInfo.AmountReceived.toFixed(2);
       });
      
      }
      this.loading = false;
    }, error => {
      this.loading = false;
    })
  }

  GetGroupExhibitors(GroupId: number) {
    this.loading = true;
    this.groupExhibitorsList=null;
    this.groupService.getGroupExhibitors(GroupId).subscribe(response => {
      if(response.Data!=null && response.Data.TotalRecords>0)
      {
     this.groupExhibitorsList = response.Data.getGroupExhibitors;
      }
      this.loading = false;
    }, error => {
     
      this.loading = false;
    })
  }

  GetGroupFinancials(GroupId: number) {
     this.loading = true;
     this.groupFinancialsList=null;
     this.groupFinancialsTotals=null;
     this.groupService.getAllGroupFinancials(GroupId).subscribe(response => {
       if(response.Data!=null && response.Data.TotalRecords>0)
       {
      this.groupFinancialsList = response.Data.getGroupFinacials;
      this.groupFinancialsTotals=response.Data.getGroupFinacialsTotals;
      this.PreTotal=this.groupFinancialsTotals.PreTotal
      this.PostTotal=this.groupFinancialsTotals.PostTotal
      this.PrePostTotal=this.groupFinancialsTotals.PrePostTotal
       }
       this.loading = false;
     }, error => {
      
       this.loading = false;
     })
   }


  AddUpdateGroup=(group)=>{
    
    this.loading=true;
    this.groupInfo.AmountReceived=this.groupInfo.AmountReceived==null ?0:this.groupInfo.AmountReceived;
    this.groupService.addUpdateGroup(this.groupInfo).subscribe(response=>{
     this.snackBar.openSnackBar(response.Message, 'Close', 'green-snackbar');
    
        // this.baseRequest.Page= 1;
        // this.baseRequest.Limit= 5;
        // this.baseRequest.OrderBy= 'GroupId';
        // this.baseRequest.OrderByDescending= true;
        // this.baseRequest.AllRecords= false;

       this.getAllGroups().then(res =>{ 
        if(response.Data.NewId !=null && response.Data.NewId>0)
        {
          if(this.groupInfo.GroupId>0)
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

  AddUpdateGroupFinancials(){
    
      this.loading=true;
      this.groupFinancialsRequest.GroupFinancialId=0;
      this.groupFinancialsRequest.GroupId=this.selectedGroupId;
      this.groupFinancialsRequest.FeeTypeId=this.FinancialsFeeTypeId;
      this.groupFinancialsRequest.TimeFrameId=this.FinancialsTimeFrameTypeId;
      this.groupFinancialsRequest.Amount=this.FinancialsAmount;

      this.groupService.addUpdateGroupFinancials(this.groupFinancialsRequest).subscribe(response=>{
        this.snackBar.openSnackBar(response.Message, 'Close', 'green-snackbar');
        this.GetGroupFinancials(this.selectedGroupId);
        this.FinancialsTimeFrameTypeId = null;
        this.FinancialsAmount=null;
        this.FinancialsFeeTypeId=null;
        this.groupFinancialForm.resetForm({FinancialsAmount:null,FinancialsFeeTypeId:null});

       }, error=>{
          this.snackBar.openSnackBar(error, 'Close', 'red-snackbar');
          this.loading = false;
       })
      
      }
    
  getCities(id: number) {
    return new Promise((resolve, reject) => {
      this.loading = true;
      this.citiesResponse=null;
      this.groupService.getCities(Number(id)).subscribe(response => {
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
      this.groupService.getAllStates().subscribe(response => {
          this.statesResponse = response.Data.State;
          this.loading = false;
      }, error => {
        this.loading = false;
      })
     
  }
  
  getAllFeeTypes() {
    this.loading = true;
    this.FeeTypes=null;
    this.groupService.getGlobalCodes('FeeType').subscribe(response => {
      if(response.Data!=null && response.Data.totalRecords>0)
      {
     this.FeeTypes = response.Data.globalCodeResponse;
      }
      this.loading = false;
    }, error => {
      this.loading = false;
    })
  }

  getAllTimeFrameTypes() {
    this.loading = true;
    this.TimeFrameTypes=null;
    this.groupService.getGlobalCodes('TimeFrameType').subscribe(response => {
      if(response.Data!=null && response.Data.totalRecords>0)
      {
     this.TimeFrameTypes = response.Data.globalCodeResponse;
     this.setFinancialsTimeFrameType(this.TimeFrameTypes[0].GlobalCodeId);
      }
      this.loading = false;
    }, error => {
      this.loading = false;
    })
  }

  editFinancialsAmount(e, index, GroupFinancialId,Amount){
    debugger
    this.updatemode=true;
    this.updateRowIndex=index;
    this.UpdatedFinancialAmount=Number(Amount);
  }
  setUpdatedFinancialAmount(data){
    this.UpdatedFinancialAmount=Number(data);
  }
  cancelUpdateFinancialsAmount(e, index, GroupFinancialId){
    this.updatemode=false;
    this.updateRowIndex=index;
  }
  
  updateGroupFinancialsAmount(e, index, GroupFinancialId,timeframename){
    debugger
    this.loading=true;
    this.updateRowIndex=index;
    this.updateGroupFinancialsRequest.GroupFinancialId=GroupFinancialId;
    this.updateGroupFinancialsRequest.Amount=this.UpdatedFinancialAmount;
    

     this.groupService.UpdateGroupFinancialsAmount(this.updateGroupFinancialsRequest).subscribe(response=>{
      this.snackBar.openSnackBar(response.Message, 'Close', 'green-snackbar');
      this.loading = false;
      this.updatemode=false;
if(timeframename=="Pre")
{
  this.PreTotal=this.UpdatedFinancialAmount;
}
else{
  this.PostTotal=this.UpdatedFinancialAmount;
}
this.PrePostTotal= this.PreTotal+this.PostTotal;

    // this.groupFinancialsTotals.PreTotal
    // this.groupFinancialsTotals.PostTotal
    // this.groupFinancialsTotals.PrePostTotal

     }, error=>{
        this.snackBar.openSnackBar(error, 'Close', 'red-snackbar');
        this.loading = false;
        this.updatemode=false;
     })
    }

 

setFinancialsFeeType(id){
  this.FinancialsFeeTypeId=Number(id);
}
setFinancialsTimeFrameType(id){
    this.FinancialsTimeFrameTypeId=Number(id);
}

setFinancialsAmount(data){
  this.FinancialsAmount=Number(data);
}



  //confirm delete 
  confirmRemoveGroup(e, index, Groupid): void {
    e.stopPropagation();
    const message = `Are you sure you want to remove the group?`;
    const dialogData = new ConfirmDialogModel("Confirm Action", message);
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      maxWidth: "400px",
      data: dialogData
    });
    dialogRef.afterClosed().subscribe(dialogResult => {
      this.result = dialogResult;
      if (this.result){ this.deleteGroup(Groupid,index) }
    });
  }

  confirmRemoveGroupExhibitor(e, index, GroupExhibitorid): void {
    e.stopPropagation();
    const message = `Are you sure you want to remove the group Exhibitor?`;
    const dialogData = new ConfirmDialogModel("Confirm Action", message);
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      maxWidth: "400px",
      data: dialogData
    });
    dialogRef.afterClosed().subscribe(dialogResult => {
      this.result = dialogResult;
      if (this.result){ this.deleteGroupExhibitor(GroupExhibitorid,index) }
    });
  }

  confirmRemoveGroupFinancials(e, index, GroupFinancialId): void {
    e.stopPropagation();
    const message = `Are you sure you want to remove the group Financials?`;
    const dialogData = new ConfirmDialogModel("Confirm Action", message);
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      maxWidth: "400px",
      data: dialogData
    });
    dialogRef.afterClosed().subscribe(dialogResult => {
      this.result = dialogResult;
      if (this.result){ this.deleteGroupFinancials(GroupFinancialId,index) }
    });
  }




  //delete record
  deleteGroup(Groupid,index) {
    this.loading = true;
    this.groupService.deleteGroup(Groupid).subscribe(response => {
      
      if(response.Success==true)
      {
     
        this.groupsList.splice(index, 1);
        this.totalItems=this.totalItems-1;

        if(this.selectedGroupId==Groupid){
          this.selectedGroupId=0;
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
  
  deleteGroupExhibitor(GroupExhibitorid,index) {
    debugger
    this.loading = true;
    this.groupService.deleteGroupExhibitors(GroupExhibitorid).subscribe(response => {
      
      if(response.Success==true)
      {
       this.GetGroupExhibitors(this.selectedGroupId)
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

  deleteGroupFinancials(GroupFinancialId,index) {
    debugger
    this.loading = true;
    this.groupService.deleteGroupFinancials(GroupFinancialId).subscribe(response => {
      
      if(response.Success==true)
      {
       this.GetGroupFinancials(this.selectedGroupId)
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
    debugger;
    this.groupInfo.GroupName = null;
    this.groupInfo.ContactName = null;
    this.groupInfo.Phone = null;
    this.groupInfo.Email = null;
    this.groupInfo.Address = null;
    this.groupInfo.CityId = null;
    this.groupInfo.StateId= null;
    this.groupInfo.ZipCode = null;
    this.groupInfo.AmountReceived = 0;
    this.groupInfo.GroupId = 0;
    this.tabGroup.selectedIndex = 0
    this.groupInfoForm.resetForm();
    this.selectedGroupId=0;
    this.selectedRowIndex =-1;
    this.FeeTypes=null;
  }

  getNext(event) {
    this.baseRequest.Page = (event.pageIndex) + 1;
    this.getAllGroupsForPagination()
  }

  getAllGroupsForPagination() {
    return new Promise((resolve, reject) => {
    this.loading = true;
    this.groupsList=null;
    this.groupService.getAllGroups(this.baseRequest).subscribe(response => {
      if(response.Data!=null && response.Data.TotalRecords>0)
      {
     this.groupsList = response.Data.groupResponses;
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
  
  
  highlight(selectedGroupId, i) {
    
    this.selectedRowIndex = i;
    this.selectedGroupId=selectedGroupId;
    this.getGroupDetails(selectedGroupId,i);
    this.GetGroupExhibitors(selectedGroupId);
    this.GetGroupFinancials(selectedGroupId);
    this.getAllFeeTypes();
    this.groupFinancialForm.resetForm({FinancialsAmount:null,FinancialsFeeTypeId:null});
  }


  sortData(column) {
    this.reverseSort = (this.sortColumn === column) ? !this.reverseSort : false
    this.sortColumn = column
    this.baseRequest.OrderBy = column;
    this.baseRequest.OrderByDescending = this.reverseSort;
    this.getAllGroupsForPagination()
  }

  getSort(column) {

    if (this.sortColumn === column) {
      return this.reverseSort ? 'arrow-down'
        : 'arrow-up';
    }
  }

  getStateName(e) {
    this.groupInfo.StateId =Number( e.target.options[e.target.selectedIndex].value)
  }

  getCityName(e) {
  this.groupInfo.CityId = Number(e.target.options[e.target.selectedIndex].value)
  }

  openStallDiagram() {
    var data = {
      
    }

    let config = new MatDialogConfig();
  config = {
    position: {
      top: '10px',
      right: '10px'
    },
    height: '98%',
    width: '100vw',
    maxWidth: '100vw',
      maxHeight: '100vh',
    panelClass: 'full-screen-modal',
  };

    const dialogRef = this.dialog.open(StallComponent, config);

    dialogRef.afterClosed().subscribe(dialogResult => {
      const result: any = dialogResult;
      if (result && result.submitted == true) {
       
      }
    });
  }
}


