import { Component, OnInit, ViewChild } from '@angular/core';
import { BaseRecordFilterRequest } from 'src/app/core/models/base-record-filter-request-model';
import { MatSnackbarComponent } from '../../../../shared/ui/mat-snackbar/mat-snackbar.component'
import { MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { GroupService } from 'src/app/core/services/group.service';
import { ConfirmDialogComponent, ConfirmDialogModel } from '../../../../shared/ui/modals/confirmation-modal/confirm-dialog.component';



import { MatPaginator } from '@angular/material/paginator';
import { NgForm } from '@angular/forms';
import { MatTabGroup } from '@angular/material/tabs'
import { MatTable } from '@angular/material/table'
import { MatTableDataSource } from '@angular/material/table/table-data-source';
import { GroupInformationViewModel } from 'src/app/core/models/group-model';

import PerfectScrollbar from 'perfect-scrollbar';
import { StallComponent } from '../stall/stall.component';
import { GlobalService } from 'src/app/core/services/global.service';


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
    SearchTerm: null

  }
  currentDate = new Date();
  cutOffDate = new Date();
  groupsList: any;
  groupExhibitorsList: any;
  groupFinancialsList: any;
  groupFinancialsTotals: any;

  PreTotal: number = 0;
  PostTotal: number = 0;
  PrePostTotal: number = 0;

  FeeTypes: any;
  TimeFrameTypes: any;
  groupFinancialsRequest: any = {
    GroupFinancialId: 0,
    GroupId: 0,
    FeeTypeId: 0,
    TimeFrameId: 0,
    Amount: 0,
  }

  updateGroupFinancialsRequest: any = {
    GroupFinancialId: 0,
    Amount: 0,
  }

  FinancialsFeeTypeId: number = null;
  FinancialsTimeFrameTypeId: number = null;
  FinancialsAmount: number = null;
  UpdatedFinancialAmount: number = null;

  enablePagination: boolean = true;
  sortColumn: string = "";
  reverseSort: boolean = false;
  loading = true;

  selectedRowIndex: any;
  selectedGroupId = 0;
  citiesResponse: any;
  statesResponse: any;
  zipCodesResponse: any;
  result: string = '';
  totalItems: number = 0;
  updatemode = false;
  updateRowIndex = -1;
  groupInfo: GroupInformationViewModel = {
    GroupName: null,
    ContactName: null,
    Phone: null,
    Email: null,
    Address: null,
    CityId: null,
    StateId: null,
    ZipCodeId: null,
    AmountReceived: '0.00',
    GroupId: 0,
    groupStallAssignmentRequests: null

  }

  StallAssignmentRequestsData: any = [];
  groupStallAssignmentResponses: any = [];
  StallTypes: any = [];
  horsestalllength: number = 0;
  tackstalllength: number = 0;
  UnassignedStallNumbers:any=[];

  constructor(private groupService: GroupService,
    private dialog: MatDialog,
    private snackBar: MatSnackbarComponent,
    private data: GlobalService
  ) { }
  ngOnInit(): void {
    this.data.searchTerm.subscribe((searchTerm: string) => {
      this.baseRequest.SearchTerm = searchTerm;
      this.getAllGroups();
    });
    this.getAllStates();
    this.getAllTimeFrameTypes();
    this.getAllStallTypes();
  }

  getAllGroups() {
    return new Promise((resolve, reject) => {
      this.loading = true;
      this.groupsList = null;
      this.groupService.getAllGroups(this.baseRequest).subscribe(response => {
        if (response.Data != null && response.Data.TotalRecords > 0) {
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
    this.UnassignedStallNumbers=[];
    this.groupService.getGroup(id).subscribe(response => {
      if (response.Data != null) {
        this.getCities(response.Data.StateId).then(res => {
          this.getZipCodes(response.Data.CityId).then(res => {
            this.groupInfo = response.Data;
            debugger
            this.groupStallAssignmentResponses = response.Data.groupStallAssignmentResponses;

            var horseStalltype = this.StallTypes.filter(x => x.CodeName == "HorseStall");
            var tackStalltype = this.StallTypes.filter(x => x.CodeName == "TackStall");
            if (this.groupStallAssignmentResponses != null && this.groupStallAssignmentResponses.length > 0) {
              this.horsestalllength = this.groupStallAssignmentResponses.filter(x => x.StallAssignmentTypeId
                == horseStalltype[0].GlobalCodeId).length;
              this.tackstalllength = this.groupStallAssignmentResponses.filter(x => x.StallAssignmentTypeId
                == tackStalltype[0].GlobalCodeId).length;
            }
            else {
              this.horsestalllength = 0;
              this.tackstalllength = 0;
            }

            this.selectedRowIndex = selectedRowIndex;
            this.groupInfo.AmountReceived = Number(this.groupInfo.AmountReceived.toFixed(2));
          });
        });

      }
      this.loading = false;
    }, error => {
      this.loading = false;
    })
  }

  GetGroupExhibitors(GroupId: number) {
    this.loading = true;
    this.groupExhibitorsList = null;
    this.groupService.getGroupExhibitors(GroupId).subscribe(response => {
      if (response.Data != null && response.Data.TotalRecords > 0) {
        this.groupExhibitorsList = response.Data.getGroupExhibitors;
      }
      this.loading = false;
    }, error => {

      this.loading = false;
    })
  }

  GetGroupFinancials(GroupId: number) {
    this.loading = true;
    this.groupFinancialsList = null;
    this.groupFinancialsTotals = null;
    this.groupService.getAllGroupFinancials(GroupId).subscribe(response => {
      if (response.Data != null && response.Data.TotalRecords > 0) {
        this.groupFinancialsList = response.Data.getGroupFinacials;
        this.groupFinancialsTotals = response.Data.getGroupFinacialsTotals;
        this.PreTotal = Number(this.groupFinancialsTotals.PreTotal.toFixed(2));
        this.PostTotal = Number(this.groupFinancialsTotals.PostTotal.toFixed(2));
        this.PrePostTotal = Number(this.groupFinancialsTotals.PrePostTotal.toFixed(2));
      }
      this.loading = false;
    }, error => {

      this.loading = false;
    })
  }

  AddUpdateGroup = (group) => {

    this.loading = true;
    this.groupInfo.AmountReceived = Number(this.groupInfo.AmountReceived == null ? 0 : this.groupInfo.AmountReceived);
    this.StallAssignmentRequestsData = [];
    if (this.groupStallAssignmentResponses.length > 0) {
      this.groupStallAssignmentResponses.forEach(resp => {
        var groupstallData = {
          SelectedStallId: resp.StallId,
          StallAssignmentId: resp.StallAssignmentId,
          StallAssignmentTypeId: resp.StallAssignmentTypeId,
          StallAssignmentDate:resp.StallAssignmentDate
        }
        this.StallAssignmentRequestsData.push(groupstallData);
      });
    }

    this.groupInfo.groupStallAssignmentRequests = this.StallAssignmentRequestsData;
    this.groupService.addUpdateGroup(this.groupInfo).subscribe(response => {
      this.snackBar.openSnackBar(response.Message, 'Close', 'green-snackbar');

      this.getAllGroups().then(res => {
        if (response.Data.NewId != null && response.Data.NewId > 0) {
          if (this.groupInfo.GroupId > 0) {
            this.highlight(response.Data.NewId, this.selectedRowIndex);
          }
          else {
            this.highlight(response.Data.NewId, 0);
          }
        }
      });

    }, error => {
      this.snackBar.openSnackBar(error, 'Close', 'red-snackbar');
      this.loading = false;
    })

  }

  AddUpdateGroupFinancials() {

    this.loading = true;
    this.groupFinancialsRequest.GroupFinancialId = 0;
    this.groupFinancialsRequest.GroupId = this.selectedGroupId;
    this.groupFinancialsRequest.FeeTypeId = this.FinancialsFeeTypeId;
    this.groupFinancialsRequest.TimeFrameId = this.FinancialsTimeFrameTypeId;
    this.groupFinancialsRequest.Amount = this.FinancialsAmount;

    this.groupService.addUpdateGroupFinancials(this.groupFinancialsRequest).subscribe(response => {
      this.snackBar.openSnackBar(response.Message, 'Close', 'green-snackbar');
      this.GetGroupFinancials(this.selectedGroupId);
      // this.FinancialsTimeFrameTypeId = null;
      this.FinancialsAmount = null;
      this.FinancialsFeeTypeId = null;
      this.groupFinancialForm.resetForm({ FinancialsAmount: null, FinancialsFeeTypeId: null });

    }, error => {
      this.snackBar.openSnackBar(error, 'Close', 'red-snackbar');
      this.loading = false;
    })

  }

  getCities(id: number) {
    return new Promise((resolve, reject) => {
      this.loading = true;
      this.citiesResponse = null;
      this.groupService.getCities(Number(id)).subscribe(response => {
        this.citiesResponse = response.Data.City;
        this.loading = false;
      }, error => {
        this.loading = false;
      })
      resolve();
    });
  }

  getZipCodes(id: number) {
    return new Promise((resolve, reject) => {
      this.loading = true;
      this.zipCodesResponse = null;
      this.groupService.getZipCodes(Number(id)).subscribe(response => {

        this.zipCodesResponse = response.Data.ZipCode;
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
    this.FeeTypes = null;
    this.groupService.getGlobalCodes('FeeType').subscribe(response => {
      if (response.Data != null && response.Data.totalRecords > 0) {
        this.FeeTypes = response.Data.globalCodeResponse;
      }
      this.loading = false;
    }, error => {
      this.loading = false;
    })
  }

  getAllStallTypes() {

    this.StallTypes = [];
    this.groupService.getGlobalCodes('StallType').subscribe(response => {
      if (response.Data != null && response.Data.totalRecords > 0) {
        this.StallTypes = response.Data.globalCodeResponse;
      }
    }, error => {

    })
  }


  getAllTimeFrameTypes() {
    this.loading = true;
    this.TimeFrameTypes = null;
    this.groupService.getGlobalCodes('TimeFrameType').subscribe(response => {
      if (response.Data != null && response.Data.totalRecords > 0) {
        this.TimeFrameTypes = response.Data.globalCodeResponse;
        this.setFinancialsTimeFrameType(this.TimeFrameTypes[0].GlobalCodeId);
      }
      this.loading = false;
    }, error => {
      this.loading = false;
    })
  }

  editFinancialsAmount(e, index, GroupFinancialId, Amount) {

    this.updatemode = true;
    this.updateRowIndex = index;
    this.UpdatedFinancialAmount = Number(Amount);
  }
  setUpdatedFinancialAmount(data) {
    this.UpdatedFinancialAmount = Number(data);
    if(this.UpdatedFinancialAmount <=0)
    {
      this.UpdatedFinancialAmount =0;
    }
  }
  cancelUpdateFinancialsAmount(e, index, GroupFinancialId) {
    this.updatemode = false;
    this.updateRowIndex = index;
  }

  updateGroupFinancialsAmount(e, index, GroupFinancialId, timeframename) {

    this.loading = true;
    this.updateRowIndex = index;
    this.updateGroupFinancialsRequest.GroupFinancialId = GroupFinancialId;
    this.updateGroupFinancialsRequest.Amount = this.UpdatedFinancialAmount;


    this.groupService.UpdateGroupFinancialsAmount(this.updateGroupFinancialsRequest).subscribe(response => {
      this.snackBar.openSnackBar(response.Message, 'Close', 'green-snackbar');
      this.loading = false;
      this.updatemode = false;
      this.GetGroupFinancials(this.selectedGroupId);
    }, error => {
      this.snackBar.openSnackBar(error, 'Close', 'red-snackbar');
      this.loading = false;
      this.updatemode = false;
    })
  }

  setFinancialsFeeType(id) {
    this.FinancialsFeeTypeId = Number(id);
  }
  setFinancialsTimeFrameType(id) {
    this.FinancialsTimeFrameTypeId = Number(id);
  }

  setFinancialsAmount(data) {
    this.FinancialsAmount = Number(data);
    if(this.FinancialsAmount<=0)
    {
      this.FinancialsAmount=0;
    }
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
      if (this.result) { this.deleteGroup(Groupid, index) }
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
      if (this.result) { this.deleteGroupExhibitor(GroupExhibitorid, index) }
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
      if (this.result) { this.deleteGroupFinancials(GroupFinancialId, index) }
    });
  }




  //delete record
  deleteGroup(Groupid, index) {
    this.loading = true;
    this.groupService.deleteGroup(Groupid).subscribe(response => {

      if (response.Success == true) {
        this.loading = false;
        this.getAllGroups();
        this.resetForm();
        this.snackBar.openSnackBar(response.Message, 'Close', 'green-snackbar');
      }
      else {
        this.loading = false;
        this.snackBar.openSnackBar(response.Message, 'Close', 'red-snackbar');
      }
    }, error => {
      this.loading = false;
    })

  }

  deleteGroupExhibitor(GroupExhibitorid, index) {

    this.loading = true;
    this.groupService.deleteGroupExhibitors(GroupExhibitorid).subscribe(response => {

      if (response.Success == true) {
        this.GetGroupExhibitors(this.selectedGroupId)
        this.snackBar.openSnackBar(response.Message, 'Close', 'green-snackbar');
      }
      else {
        this.loading = false;
        this.snackBar.openSnackBar(response.Message, 'Close', 'red-snackbar');

      }
    }, error => {
      this.loading = false;
    })

  }

  deleteGroupFinancials(GroupFinancialId, index) {

    this.loading = true;
    this.groupService.deleteGroupFinancials(GroupFinancialId).subscribe(response => {

      if (response.Success == true) {
        this.GetGroupFinancials(this.selectedGroupId)
        this.snackBar.openSnackBar(response.Message, 'Close', 'green-snackbar');
      }
      else {
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
    this.groupInfo.StateId = null;
    this.groupInfo.ZipCodeId = null;
    this.groupInfo.AmountReceived = 0;
    this.groupInfo.GroupId = 0;
    this.tabGroup.selectedIndex = 0
    this.groupInfoForm.resetForm();
    this.selectedGroupId = 0;
    this.selectedRowIndex = -1;
    this.FeeTypes = null;
    this.groupStallAssignmentResponses = [];
    this.horsestalllength=0;
    this.tackstalllength=0;
  }

  getNext(event) {
    this.baseRequest.Page = (event.pageIndex) + 1;
    this.getAllGroupsForPagination()
  }

  getAllGroupsForPagination() {
    return new Promise((resolve, reject) => {
      this.loading = true;
      this.groupsList = null;
      this.groupService.getAllGroups(this.baseRequest).subscribe(response => {
        if (response.Data != null && response.Data.TotalRecords > 0) {
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
    this.selectedGroupId = selectedGroupId;
    this.getGroupDetails(selectedGroupId, i);
    this.GetGroupExhibitors(selectedGroupId);
    this.GetGroupFinancials(selectedGroupId);
    this.getAllFeeTypes();
    this.groupFinancialForm.resetForm({ FinancialsAmount: null, FinancialsFeeTypeId: null });
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
    this.groupInfo.StateId = Number(e.target.options[e.target.selectedIndex].value)
  }

  getCityName(e) {
    this.groupInfo.CityId = Number(e.target.options[e.target.selectedIndex].value)
  }
  getZipNumber(e) {
    this.groupInfo.ZipCodeId = Number(e.target.options[e.target.selectedIndex].value)
  }

  openStallDiagram() {
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
      data: { groupStallAssignment: this.groupStallAssignmentResponses, 
        StallTypes: this.StallTypes ,
        unassignedStallNumbers:this.UnassignedStallNumbers},

    };

    const dialogRef = this.dialog.open(StallComponent, config,

    );
    dialogRef.afterClosed().subscribe(dialogResult => {
     
      const result: any = dialogResult;
      if (result && result.submitted == true) {
        this.groupStallAssignmentResponses=[];
        this.groupStallAssignmentResponses = result.data.groupAssignedStalls;
        this.UnassignedStallNumbers=result.data.unassignedStallNumbers;

        var horseStalltype = this.StallTypes.filter(x => x.CodeName == "HorseStall");
        var tackStalltype = this.StallTypes.filter(x => x.CodeName == "TackStall");
        if (this.groupStallAssignmentResponses != null && this.groupStallAssignmentResponses.length > 0) {
          this.horsestalllength = this.groupStallAssignmentResponses.filter(x => x.StallAssignmentTypeId
            == horseStalltype[0].GlobalCodeId).length;
          this.tackstalllength = this.groupStallAssignmentResponses.filter(x => x.StallAssignmentTypeId
            == tackStalltype[0].GlobalCodeId).length;
        }
        else {
          this.horsestalllength = 0;
          this.tackstalllength = 0;
        }
      }
      else{
        this.UnassignedStallNumbers=[];
      }
    });
  }
  setAmount(val) {
    if (val <= 0) {
        this.groupInfo.AmountReceived =Number(0);
      }
      else {
        this.groupInfo.AmountReceived = Number(val);
      }
    }

    

  printGroupFinancials() {
    let printContents, popupWin, printbutton, hideRow, gridTableDesc;
    hideRow = document.getElementById('groupFinancialsentry').hidden = true;
    printbutton = document.getElementById('inputprintbutton').style.display = "none";
    gridTableDesc = document.getElementById('gridTableDescPrint').style.display = "block";

    printContents = document.getElementById('print-entries').innerHTML;
    popupWin = window.open('', '_blank', 'top=0,left=0,height=100%,width=auto');
    popupWin.document.open();
    popupWin.document.write(`
      <html>
        <head>
      
          <title>Print tab</title>
          <style media="print">
    
          * {
            -webkit-print-color-adjust: exact; /*Chrome, Safari */
            color-adjust: exact;  /*Firefox*/
            box-sizing: border-box;
            font-family: Roboto, "Helvetica Neue", sans-serif;
            height:auto !important;
            }
            table {
              border-collapse: collapse;
              border-spacing: 2px;
              margin-bottom:0 !important; 
              padding-bottom:0 !important; 
              width:100%;   
          }
            table thead tr th {
              background-color: #a0b8f9;
              font-family: "Roboto-Medium" ,sans-serif;
              font-size: 13px;
              text-transform: uppercase;
              border: 1px solid #a0b8f9;
              text-align: center;
              padding: 6px;
              vertical-align: middle;
              line-height: 16px;
              cursor: pointer;
              letter-spacing: 1px;
          }
          .mat-tab-group {
            font-family: "Roboto-Regular", sans-serif;
        }
          table tbody tr td {
            border: 1px solid #a0b8f9;
            text-align: center;
            color: #000;
            font-weight: 500;
            font-size: 13px;
            line-height: 24px;
            vertical-align: middle;
            padding: 6px 10px;
            font-family: "Roboto-Medium" ,sans-serif;
        }
        .dynDataSeclect {
          width: 100%;
          padding: 2px 15px 2px 5px;
          border: 1px solid #ccc;
          border-radius: 3px;
          min-height: 30px;
      }
      select {
        -webkit-appearance: none;
        background-image: url(select-arrow.png);
        background-repeat: no-repeat;
        background-position: center right;
        margin: 0;
        font-family: inherit;
        font-size: inherit;
        line-height: inherit;
    }
    select {
      -webkit-writing-mode: horizontal-tb !important;
      text-rendering: auto;
      color: -internal-light-dark(black, white);
      letter-spacing: normal;
      word-spacing: normal;
      text-transform: none;
      text-indent: 0px;
      text-shadow: none;
      display: inline-block;
      text-align: start;
      appearance: menulist;
      box-sizing: border-box;
      align-items: center;
      white-space: pre;
      -webkit-rtl-ordering: logical;
      background-color: -internal-light-dark(rgb(255, 255, 255), rgb(59, 59, 59));
      cursor: default;
      margin: 0em;
      font: 400 13.3333px Arial;
      border-radius: 0px;
      border-width: 1px;
      border-style: solid;
      border-color: -internal-light-dark(rgb(118, 118, 118), rgb(195, 195, 195));
      border-image: initial;
  }
  .table-responsive {
    display: block;
    width: 100%;
  } 
  .table.table-bordered.tableBodyScroll.removeSpaceTop {
    margin-bottom: 10px !important;
}
  table.pdfTable{
    margin-bottom: 20px !important;
    display:table;
    
  }
  .wideSpace td{
    border:none;
    width:33.33%;
    text-align:left;
  }
  
  table.pdfTable,table.pdfTable tbody,table.pdfTable tr {
    width:100%;
    display:table;
    border:none;
  }
  table.pdfTable tbody tr td{
      margin: 0;
      padding: 5px 0px !important;
      position: relative; 
      border:none;
      text-align:left;
      display:block;
      
  }
  

  .print-element { display: block !important;}
  .non-print-element {display: none !important;}
   
          </style>
        </head>
    <body onload="window.print();window.close()">${printContents}</body>
      </html>`
    );

    hideRow = document.getElementById('groupFinancialsentry').hidden = false;
    printbutton = document.getElementById('inputprintbutton').style.display = "inline-block";
    gridTableDesc = document.getElementById('gridTableDescPrint').style.display = "none";
    popupWin.document.close();
  }



  printGroupExhibitor() {
    let printContents, popupWin, printbutton, hideRow, gridTableDesc;

    printbutton = document.getElementById('inputprintbutton').style.display = "none";
    gridTableDesc = document.getElementById('gridTableDescPrint').style.display = "block";
    printContents = document.getElementById('contentscroll2').innerHTML;
    popupWin = window.open('', '_blank', 'top=0,left=0,height=100%,width=auto');
    popupWin.document.open();
    popupWin.document.write(`
      <html>
        <head>
      
          <title>Print tab</title>
          <style media="print">
    
          * {
            -webkit-print-color-adjust: exact; /*Chrome, Safari */
            color-adjust: exact;  /*Firefox*/
            box-sizing: border-box;
            font-family: Roboto, "Helvetica Neue", sans-serif;
            height:auto !important;
            }
            table {
              border-collapse: collapse;
              border-spacing: 2px;
              margin-bottom:0 !important; 
              padding-bottom:0 !important; 
              width:100%;  
          }
            table thead tr th {
              background-color: #a0b8f9;
              font-family: "Roboto-Medium" ,sans-serif;
              font-size: 13px;
              text-transform: uppercase;
              border: 1px solid #a0b8f9;
              text-align: center;
              padding: 6px;
              vertical-align: middle;
              line-height: 16px;
              cursor: pointer;
              letter-spacing: 1px;
          }
          .mat-tab-group {
            font-family: "Roboto-Regular", sans-serif;
        }
          table tbody tr td {
            border: 1px solid #a0b8f9;
            text-align: center;
            color: #000;
            font-weight: 500;
            font-size: 13px;
            line-height: 24px;
            vertical-align: middle;
            padding: 6px 10px;
            font-family: "Roboto-Medium" ,sans-serif;
        }
        .dynDataSeclect {
          width: 100%;
          padding: 2px 15px 2px 5px;
          border: 1px solid #ccc;
          border-radius: 3px;
          min-height: 30px;
      }
      select {
        -webkit-appearance: none;
        background-image: url(select-arrow.png);
        background-repeat: no-repeat;
        background-position: center right;
        margin: 0;
        font-family: inherit;
        font-size: inherit;
        line-height: inherit;
    }
    select {
      -webkit-writing-mode: horizontal-tb !important;
      text-rendering: auto;
      color: -internal-light-dark(black, white);
      letter-spacing: normal;
      word-spacing: normal;
      text-transform: none;
      text-indent: 0px;
      text-shadow: none;
      display: inline-block;
      text-align: start;
      appearance: menulist;
      box-sizing: border-box;
      align-items: center;
      white-space: pre;
      -webkit-rtl-ordering: logical;
      background-color: -internal-light-dark(rgb(255, 255, 255), rgb(59, 59, 59));
      cursor: default;
      margin: 0em;
      font: 400 13.3333px Arial;
      border-radius: 0px;
      border-width: 1px;
      border-style: solid;
      border-color: -internal-light-dark(rgb(118, 118, 118), rgb(195, 195, 195));
      border-image: initial;
  }
  .table-responsive {
    display: block;
    width: 100%;
  }
  table.table.table-bordered.tableBodyScroll.removeSpaceTop {
    margin-bottom: 10px !important;
}
  
  table.pdfTable{
    margin-bottom: 20px !important;
    display:table;
  }
  
  table.pdfTable,table.pdfTable tbody,table.pdfTable tr {
    width:100%;
    display:table;
    border:none;
  }
  table.pdfTable tbody tr td{
      margin: 0;
      padding: 5px 0px !important;
      position: relative; 
      border:none;
      text-align:left;
      display:block;      
  }

 /* .pdfdataTable {
    position: absolute;
    top: 120px;
    width: 100%;
    left:0;
  }*/


  .print-element { display: block !important;}
  .non-print-element {display: none !important;}
   
          </style>
        </head>
    <body onload="window.print();window.close()">${printContents}</body>
      </html>`
    );
    printbutton = document.getElementById('inputprintbutton').style.display = "inline-block";
    gridTableDesc = document.getElementById('gridTableDescPrint').style.display = "none";
    popupWin.document.close();
  }


}


