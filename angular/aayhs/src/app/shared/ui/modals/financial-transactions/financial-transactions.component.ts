import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Component, OnInit, Inject,ViewChild } from '@angular/core';
import { MatSnackbarComponent } from '../../mat-snackbar/mat-snackbar.component'
import {FeeModel} from '../../../../core/models/fee.model'
import {ExhibitorService } from '../../../../core/services/exhibitor.service';
import { NgForm } from '@angular/forms';
import * as moment from 'moment';
import { BaseUrl } from 'src/app/config/url-config';

@Component({
  selector: 'app-financial-transactions',
  templateUrl: './financial-transactions.component.html',
  styleUrls: ['./financial-transactions.component.scss']
})
export class FinancialTransactionsComponent implements OnInit {
  @ViewChild('feeForm') feeForm: NgForm;

//for binding images with server url
filesUrl = BaseUrl.filesUrl;

  exhibitorDetails:{
    ExhibitorId:null;
    ExhibitorName:null
  }
  constructor(public dialogRef: MatDialogRef<FinancialTransactionsComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any,
    private snackBar: MatSnackbarComponent,
    private exhibitorService: ExhibitorService) { }

    date = new Date();
    document:File;
   feeDetails:any
    loading = false;
    fee:FeeModel={
      Date:new Date().toString(),
      FeeType:null,
      Amount:null,
      Paid:null,
      Refund:null,
      Timeframe:null
    }
  ngOnInit(): void {
    this.exhibitorDetails=this.data;
    this.getFees();
  }

  onDismiss(): void {
    // Close the dialog, return false
    this.dialogRef.close(false);
  }

  addFee(){
    debugger;
      this.loading = true;
      this.fee.Refund=this.fee.Refund !=null ? Number(this.fee.Refund) :0;
      this.fee.FeeType= Number(this.fee.FeeType);

      this.exhibitorService.addFee(this.fee).subscribe(response => {
        this.loading = false;
        this.resetFees();
        this.feeForm.resetForm({ classControl: null });
        this.snackBar.openSnackBar(response.Message, 'Close', 'green-snackbar');
    
      }, error => {
        debugger;
        this.snackBar.openSnackBar(error, 'Close', 'red-snackbar');
        this.loading = false;
      })   
  }

  resetFees(){
    this.fee.Date=null
    this.fee.FeeType=null
    this.fee.Amount=null
    this.fee.Paid=null
    this.fee.Refund=null
    this.fee.Timeframe=null

  }

  handleFeeDateSelection() {
    this.fee.Date = moment(this.fee.Date).format('YYYY-MM-DD');
  }

  setFeeType(e){
    debugger;
   this.fee.FeeType=Number(e.target.value);
   var feeType=e.target.options[e.target.options.selectedIndex].text;
   let amount=this.feeDetails.find(i =>i.FeeType==feeType).Amount;
    this.fee.Amount=amount;
   if(feeType =="Class Entry"|| feeType=="Stall" || feeType=="Tack"){
   this.fee.Refund =Math.round((amount * 40)/100)
   }
   else{
    this.fee.Refund=null;
   }
  }

  viewDocument(path){
    window.open(this.filesUrl+path.replace(/\s+/g, '%20').toLowerCase(), '_blank');
  }

 getFees(){
  this.loading = true;
  this.exhibitorService.getFees().subscribe(response => {    
   this.feeDetails = response.Data.getFees;
    this.loading = false;
  }, error => {
    this.loading = false;
  })
}

  uploadDocument($event){
    this.loading = true;
    this.document=($event.target.files[0]);
    var reader = new FileReader();
    const file = $event.target.files[0];
    reader.readAsDataURL(file);
    const formData :any= new FormData();
   formData.append('exhibitorPaymentId',1);
   formData.append('document', this.document);
      this.exhibitorService.uploadFinancialDocument(formData).subscribe(response => {
       this.snackBar.openSnackBar(response.Message, 'Close', 'green-snackbar');
       this.loading = false;
       this.document=null 
     }
     , error => {
       this.snackBar.openSnackBar(error, 'Close', 'red-snackbar');
       this.loading = false;

     });
  }

  deleteFee(id){
    this.loading = true;
    this.exhibitorService.deleteFee(id).subscribe(response => {
      this.loading = false;
      this.snackBar.openSnackBar(response.Message, 'Close', 'green-snackbar');
    }, error => {
      this.snackBar.openSnackBar(error.error.Message, 'Close', 'red-snackbar');
      this.loading = false;
  
    })
  }
}
