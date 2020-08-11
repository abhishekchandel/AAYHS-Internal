import { Component, OnInit, ViewChild } from '@angular/core';
import { BaseRecordFilterRequest } from 'src/app/core/models/base-record-filter-request-model';
import { MatSnackbarComponent } from 'src/app/shared/ui/mat-snackbar/mat-snackbar/mat-snackbar.component';
import { MatDialog } from '@angular/material/dialog';
import { GroupService } from 'src/app/core/services/group.service';
import { ConfirmDialogComponent, ConfirmDialogModel } from 'src/app/shared/ui/modals/confirmation-modal/confirm-dialog/confirm-dialog.component';



import { MatPaginator } from '@angular/material/paginator';
import { NgForm } from '@angular/forms';
import { MatTabGroup } from '@angular/material/tabs'
import { MatTable } from '@angular/material/table'
import { MatTableDataSource } from '@angular/material/table/table-data-source';
import { GroupInformationViewModel } from 'src/app/core/models/group-model';


import PerfectScrollbar from 'perfect-scrollbar';


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

  baseRequest: BaseRecordFilterRequest = {
    Page: 1,
    Limit: 5,
    OrderBy: 'SponsorId',
    OrderByDescending: true,
    AllRecords: false
  }
  groupsList: any;
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
  groupInfo: GroupInformationViewModel = {
    GroupName: null,
    ContactName: null,
    Phone: null,
    Email: null,
    Address: null,
    CityId: null,
    StateId: null,
    ZipCode: null,
    AmountReceived: 0,
    GroupId: 0,
   

  }
  constructor(private groupService: GroupService,
    private dialog: MatDialog,
    private snackBar: MatSnackbarComponent
  ) { }
  ngOnInit(): void {
    this.getAllGroups();
  }

  getAllGroups() {
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
    
  }


  getSponsorDetails = (id: number) => {
    this.loading = true;
    this.groupService.getGroup(id).subscribe(response => {
      if(response.Data!=null)
      {
        debugger;
      this.getCities(response.Data.StateId).then(res => 
       
         this.groupInfo = response.Data
          );
      debugger;
      }
      this.loading = false;
    }, error => {
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

  //delete record
  deleteGroup(Groupid,index) {
    debugger
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

  resetForm() {
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
    this.groupInfoForm.resetForm();
    this.tabGroup.selectedIndex = 0
    this.selectedGroupId=0;
    this.selectedRowIndex =-1;
  }

  getNext(event) {
    this.baseRequest.Page = (event.pageIndex) + 1;
    this.getAllGroups()
  }
  
  
  highlight(selectedGroupIdId, i) {
    
    this.selectedRowIndex = i;
    this.selectedGroupId=selectedGroupIdId;
    this.getSponsorDetails(selectedGroupIdId);
  }


  sortData(column) {
    this.reverseSort = (this.sortColumn === column) ? !this.reverseSort : false
    this.sortColumn = column
    this.baseRequest.OrderBy = column;
    this.baseRequest.OrderByDescending = this.reverseSort;
    this.getAllGroups()
  }

  getSort(column) {

    if (this.sortColumn === column) {
      return this.reverseSort ? 'arrow-down'
        : 'arrow-up';
    }
  }
}


