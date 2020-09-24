import { Component, OnInit, ViewChild } from '@angular/core';
import { SponsorInformationViewModel, TypesList } from '../../../../core/models/sponsor-model';
import { SponsorService } from '../../../../core/services/sponsor.service';
import { AdvertisementService } from '../../../../core/services/advertisement.service';
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
import { GlobalService } from '../../../../core/services/global.service'



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
  zipCodesResponse: any;
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
    ZipCodeId: null,
    AmountReceived: 0.00,
    SponsorId: 0,
    sponsorExhibitors: null,
    sponsorClasses: null,

  }
  sponsorsList: any
  sponsorsExhibitorsList: any
  sponsorClassesList: any
  UnassignedSponsorExhibitor: any
  UnassignedSponsorClasses: any
  //advertisementsList:any
  SponsorTypes: any;
  AdTypes: any;
  selectedSponsorId: number = 0;

  exhibitorId: number = null;
  sponsortypeId: number = null;
  adTypeId: number = null;
  typeId: string = null;
  sponsorClassId: number = null;
  showAds = false;
  showClasses = false;
  typeList: any = [];

  sponsorExhibitorRequest: any = {
    SponsorExhibitorId: null,
    SponsorId: null,
    ExhibitorId: null,
    SponsorTypeId: null,
    AdTypeId: null,
    TypeId: null
  }
  sponsorClassRequest: any = {
    ClassSponsorId: null,
    SponsorId: null,
    ClassId: null,
  }

  baseRequest: BaseRecordFilterRequest = {
    Page: 1,
    Limit: 5,
    OrderBy: 'SponsorId',
    OrderByDescending: true,
    AllRecords: false,
    SearchTerm: null
  }

  sponsors: SponsorInformationViewModel[];

  constructor(private sponsorService: SponsorService,
    private dialog: MatDialog,
    private snackBar: MatSnackbarComponent,
    private data: GlobalService
  ) { }



  ngOnInit(): void {
    this.data.searchTerm.subscribe((searchTerm: string) => {
      this.baseRequest.SearchTerm = searchTerm;
      this.baseRequest.Page = 1;
      this.getAllSponsors();
    });
    this.getAllStates();
  }


  getAllSponsors() {
    return new Promise((resolve, reject) => {
      this.loading = true;
      this.sponsorsList = null;
      this.sponsorService.getAllSponsers(this.baseRequest).subscribe(response => {
        if (response.Data != null && response.Data.TotalRecords > 0) {
          this.sponsorsList = response.Data.sponsorResponses;
          this.totalItems = response.Data.TotalRecords;
          if(this.baseRequest.Page === 1){
            this.paginator.pageIndex =0;
          }
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
    this.SponsorTypes = null;
    this.sponsorService.getAllTypes('SponsorTypes').subscribe(response => {
      if (response.Data != null && response.Data.totalRecords > 0) {
        this.SponsorTypes = response.Data.globalCodeResponse;
      }
      this.loading = false;
    }, error => {
      this.loading = false;
    })
  }

  getAllAdTypes() {
    this.loading = true;
    this.AdTypes = null;
    this.sponsorService.getAllTypes('AdTypes').subscribe(response => {
      if (response.Data != null && response.Data.totalRecords > 0) {
        this.AdTypes = response.Data.globalCodeResponse;
      }
      this.loading = false;
    }, error => {
      this.loading = false;
    })
  }

  getSponsorDetails = (id: number, selectedRowIndex) => {

    this.loading = true;
    this.sponsorService.getSponsor(id).subscribe(response => {

      if (response.Data != null) {

        this.getCities(response.Data.StateId).then(res => {
          this.getZipCodes(response.Data.CityName, true).then(res => {
            this.sponsorInfo = response.Data;
            this.selectedRowIndex = selectedRowIndex;
            this.sponsorInfo.AmountReceived = Number(this.sponsorInfo.AmountReceived.toFixed(2));
          });
        });
      }
      this.loading = false;
    }, error => {
      this.loading = false;
    })
  }


  GetSponsorExhibitorBySponsorId(selectedSponsorId: number) {
    this.loading = true;
    this.sponsorsExhibitorsList = null;
    this.UnassignedSponsorExhibitor = null;
    this.sponsorService.GetSponsorExhibitorBySponsorId(selectedSponsorId).subscribe(response => {
      if (response.Data != null && response.Data.TotalRecords > 0) {
        this.sponsorsExhibitorsList = response.Data.SponsorExhibitorResponses;
      }
      this.UnassignedSponsorExhibitor = response.Data.UnassignedSponsorExhibitor;
      this.loading = false;
    }, error => {
      this.loading = false;
    })

  }

  GetSponsorClasses(SponsorId: number) {
    this.loading = true;
    this.sponsorClassesList = null;
    this.UnassignedSponsorClasses = null;
    this.sponsorService.GetSponsorClasses(SponsorId).subscribe(response => {
      if (response.Data != null && response.Data.TotalRecords > 0) {
        this.sponsorClassesList = response.Data.sponsorClassesListResponses;
        this.setSponsorType(this.sponsortypeId)
      }
      this.UnassignedSponsorClasses = response.Data.unassignedSponsorClasses;
      this.loading = false;
    }, error => {
      this.loading = false;
    })

  }




  AddUpdateSponsor = (sponsor) => {
    console.log(this.sponsorInfo);
    //return
    this.loading = true;
    this.sponsorInfo.AmountReceived = this.sponsorInfo.AmountReceived.replace(",","");
    this.sponsorInfo.AmountReceived = Number(Number(this.sponsorInfo.AmountReceived == null 
                                    || this.sponsorInfo.AmountReceived == undefined
                                   || this.sponsorInfo.AmountReceived == NaN ? 0 : 
                                   this.sponsorInfo.AmountReceived).toFixed(2));
    this.sponsorService.addUpdateSponsor(this.sponsorInfo).subscribe(response => {
      this.snackBar.openSnackBar(response.Message, 'Close', 'green-snackbar');
      this.getAllSponsors().then(res => {
        if (response.Data.NewId != null && response.Data.NewId > 0) {
          if (this.sponsorInfo.SponsorId > 0) {
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

  AddUpdateSponsorExhibitor() {
    this.loading = true;
    this.sponsorExhibitorRequest.SponsorExhibitorId = 0;
    this.sponsorExhibitorRequest.SponsorId = this.selectedSponsorId;
    this.sponsorExhibitorRequest.ExhibitorId = this.exhibitorId;
    this.sponsorExhibitorRequest.SponsorTypeId = this.sponsortypeId;
    this.sponsorExhibitorRequest.AdTypeId = this.adTypeId != null ? this.adTypeId : 0;
    this.sponsorExhibitorRequest.TypeId = this.typeId != null ? this.typeId : "";

    this.sponsorService.AddUpdateSponsorExhibitor(this.sponsorExhibitorRequest).subscribe(response => {
      this.snackBar.openSnackBar(response.Message, 'Close', 'green-snackbar');
      this.GetSponsorExhibitorBySponsorId(this.selectedSponsorId);
      this.exhibitorId = null;
      this.sponsortypeId = null;
      this.sponsorExhibitorForm.resetForm({ exhibitorId: null, sponsortypeId: null });

    }, error => {
      this.snackBar.openSnackBar(error, 'Close', 'red-snackbar');
      this.loading = false;
    })

  }

  AddUpdateSponsorClass() {
    this.loading = true;
    this.sponsorClassRequest.ClassSponsorId = 0;
    this.sponsorClassRequest.SponsorId = this.selectedSponsorId;
    this.sponsorClassRequest.ClassId = this.sponsorClassId;
    this.sponsorService.AddUpdateSponsorClass(this.sponsorClassRequest).subscribe(response => {
      this.snackBar.openSnackBar(response.Message, 'Close', 'green-snackbar');
      this.GetSponsorClasses(this.selectedSponsorId);
      this.sponsorClassId = null;
      this.sponsorClassForm.resetForm({ sponsorClassId: null });
    }, error => {
      this.snackBar.openSnackBar(error, 'Close', 'red-snackbar');
      this.loading = false;
    })
  }

  setSponsorExhibitor(id) {
    this.exhibitorId = Number(id);
  }

  setAdType(id) {
    debugger
    this.adTypeId = Number(id);
  }


  setSponsorType(id) {

    this.sponsortypeId = Number(id);
    this.typeList = [];
    this.typeId = null;
    if (this.SponsorTypes != null && this.SponsorTypes != undefined && this.sponsortypeId != null && this.sponsortypeId > 0) {

      var sponsorTypename = this.SponsorTypes.filter((x) => { return x.GlobalCodeId == this.sponsortypeId; });

      if (sponsorTypename[0].CodeName == "Class") {
        this.showClasses = true;
        this.showAds = false;
        debugger
        this.sponsorClassesList.forEach((data) => {
          var listdata: TypesList = {
            Id: data.ClassId,
            Name: data.ClassNumber + '/' + data.Name
          }
          this.typeList.push(listdata)
        })
      }
      if (sponsorTypename[0].CodeName == "Ad") {
        this.showClasses = false;
        this.showAds = true;
      }
    }
  }

  setType(value) {
    this.typeId = value;
  }

  setSponsorClass(id) {
    this.sponsorClassId = Number(id);
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
      if (this.result) { this.deleteSponsor(Sponsorid, index) }
    });
  }

  confirmRemoveExhibitor(e, index, SponsorExhibitorId): void {
    const message = `Are you sure you want to remove this sponsor exhibitor?`;
    const dialogData = new ConfirmDialogModel("Confirm Action", message);
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      maxWidth: "400px",
      data: dialogData
    });
    dialogRef.afterClosed().subscribe(dialogResult => {
      this.result = dialogResult;
      if (this.result) {
        if (this.result) { this.deleteSponsorExhibitor(SponsorExhibitorId, index) }
      }
    });

  }

  confirmRemoveSponsorClass(e, index, ClassSponsorId): void {
    const message = `Are you sure you want to remove this sponsor class?`;
    const dialogData = new ConfirmDialogModel("Confirm Action", message);
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      maxWidth: "400px",
      data: dialogData
    });
    dialogRef.afterClosed().subscribe(dialogResult => {
      this.result = dialogResult;
      if (this.result) {
        if (this.result) { this.deleteSponsorClass(ClassSponsorId, index) }
      }
    });
  }




  //delete record
  deleteSponsor(Sponsorid, index) {

    this.loading = true;
    this.sponsorService.deleteSponsor(Sponsorid).subscribe(response => {

      if (response.Success == true) {
        this.loading = false;
        this.getAllSponsors();
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

  deleteSponsorExhibitor(SponsorExhibitorId, index) {
    this.loading = true;
    this.sponsorService.deleteSponsorExhibitor(SponsorExhibitorId).subscribe(response => {
      if (response.Success == true) {
        this.GetSponsorExhibitorBySponsorId(this.selectedSponsorId);
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

  deleteSponsorClass(ClassSponsorId, index) {
    this.loading = true;
    this.sponsorService.DeleteSponsorClasse(ClassSponsorId).subscribe(response => {
      if (response.Success == true) {
        this.GetSponsorClasses(this.selectedSponsorId);
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
    this.sponsorInfo.SponsorName = null;
    this.sponsorInfo.ContactName = null;
    this.sponsorInfo.Phone = null;
    this.sponsorInfo.Email = null;
    this.sponsorInfo.Address = null;
    this.sponsorInfo.CityId = null;
    this.sponsorInfo.StateId = null;
    this.sponsorInfo.ZipCodeId = null;
    this.sponsorInfo.AmountReceived = 0.00;
    this.sponsorInfo.SponsorId = 0;
    this.sponsorInfoForm.resetForm();
    this.tabGroup.selectedIndex = 0

    this.selectedSponsorId = 0;
    this.sponsorsExhibitorsList = null;
    this.sponsorClassesList = null;
    this.UnassignedSponsorExhibitor = null;
    this.UnassignedSponsorClasses = null;
    this.SponsorTypes = null;
    this.selectedRowIndex = -1;

    this.exhibitorId = null;
    this.sponsortypeId = null;
    this.sponsorClassId = null;
  }

  getNext(event) {
    this.baseRequest.Page = (event.pageIndex) + 1;
    this.getAllSponsorsForPagination()
  }


  getAllSponsorsForPagination() {
    return new Promise((resolve, reject) => {
      this.loading = true;
      this.sponsorsList = null;
      this.sponsorService.getAllSponsers(this.baseRequest).subscribe(response => {
        if (response.Data != null && response.Data.TotalRecords > 0) {
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
    this.selectedSponsorId = selectedSponsorId;
    this.getSponsorDetails(selectedSponsorId, this.selectedRowIndex);
    this.GetSponsorExhibitorBySponsorId(selectedSponsorId);
    this.getAllSponsorTypes();
    this.getAllAdTypes();
    this.exhibitorId = null;
    this.sponsortypeId = null;
    this.sponsorClassId = null;
    this.GetSponsorClasses(selectedSponsorId);
    this.sponsorClassForm.resetForm({ sponsorClassId: null });
    this.sponsorExhibitorForm.resetForm({ exhibitorId: null, sponsortypeId: null });
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
      this.citiesResponse = null;
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

  getZipCodes(event, Notfromhtml) {

    var cityname;
    Notfromhtml == true ? cityname = event : cityname = event.target.options[event.target.options.selectedIndex].text;
    return new Promise((resolve, reject) => {
      this.loading = true;
      this.zipCodesResponse = null;
      this.sponsorService.getZipCodes(cityname).subscribe(response => {
        debugger
        this.zipCodesResponse = response.Data.ZipCode;
        this.loading = false;
      }, error => {
        this.loading = false;
      })
      resolve();
    });
  }

  getStateName(e) {
    this.sponsorInfo.StateId = Number(e.target.options[e.target.selectedIndex].value)
  }

  getCityName(e) {
    this.sponsorInfo.CityId = Number(e.target.options[e.target.selectedIndex].value)
  }
  getZipNumber(e) {
    this.sponsorInfo.ZipCodeId = Number(e.target.options[e.target.selectedIndex].value)
  }

  goToLink(url: string) {
    window.open(url, "_blank");
  }
  setAmount(val) {

    val=val.replace(",","");
    if (val <= 0) {
      this.sponsorInfo.AmountReceived = 0.00;
    }
    else if (val > 9999.99) {
      this.sponsorInfo.AmountReceived = 9999.99;
      this.snackBar.openSnackBar("Amount cannot be greater then 9999.99", 'Close', 'red-snackbar');
    }
    else {
      this.sponsorInfo.AmountReceived =Number(Number(val).toFixed(2));
    }
    
    var element=document.getElementById('sponsoramount');
    element.value= this.sponsorInfo.AmountReceived; 
    element.innerHTML= this.sponsorInfo.AmountReceived; 
    element.innerText= this.sponsorInfo.AmountReceived; 

  }


  printSponsorExhibitor() {
    let printContents, popupWin, printbutton, hideRow, gridTableDesc;
    hideRow = document.getElementById('sponsorExhibitorentry').hidden = true;
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
.table.table-bordered.tableBodyScroll.removeSpaceTop {
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
/*.pdfdataTable {
  position: absolute;
  top: 90px;
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
    hideRow = document.getElementById('sponsorExhibitorentry').hidden = false;
    gridTableDesc = document.getElementById('gridTableDescPrint').style.display = "none";
    popupWin.document.close();
  }


  printSponsorClasses() {
    let printContents, popupWin, printbutton, hideRow, gridTableDesc;
    hideRow = document.getElementById('sponsorClassesentry').hidden = true;
    printbutton = document.getElementById('inputprintbutton').style.display = "none";
    gridTableDesc = document.getElementById('gridTableDescPrint1').style.display = "block";
    printContents = document.getElementById('tblSponsorClasses').innerHTML;
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
/*.pdfdataTable {
  position: absolute;
  top: 90px;
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
    hideRow = document.getElementById('sponsorClassesentry').hidden = false;
    gridTableDesc = document.getElementById('gridTableDescPrint1').style.display = "none";
    popupWin.document.close();
  }



}

