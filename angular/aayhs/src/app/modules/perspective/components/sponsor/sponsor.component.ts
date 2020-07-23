import { Component, OnInit } from '@angular/core';
import{SponsorInformationViewModel} from '../../../../core/models/sponsor-model';
import{SponsorService} from '../../../../core/services/sponsor.service';
import {  ConfirmDialogComponent,ConfirmDialogModel} from '../../../../shared/ui/modals/confirmation-modal/confirm-dialog/confirm-dialog.component';
import { MatDialog } from '@angular/material/dialog';


export interface PeriodicElement {
  SponsorId: number;
  Sponsor: string;
}

export interface SponsorExhibitors {
  ExhibitorId: number;
  ExhibitorName: string;
  SponsorType: string;
  IdNumber: string;
  BirthYear: number;

}

export interface SponsorClass {
  ClassNumber: string;
  ClassName: string;
  AgeGroup: string;
  Exhibitor: string;
  HorseName: string;

}

const ELEMENT_DATA: PeriodicElement[] = [
  {SponsorId: 1, Sponsor: 'Abigail Roberts'},
  {SponsorId: 2, Sponsor: 'Apple Bee'},
  {SponsorId: 3, Sponsor: 'Miller Apple Hill'},
  {SponsorId: 4, Sponsor: 'Summit Eq Vet Service'},
];

const data1:SponsorExhibitors[] =[
  {ExhibitorId:2536,ExhibitorName:'Kristyn Monoroe',SponsorType:'Cooler',IdNumber:'',BirthYear:2011},
  {ExhibitorId:2537,ExhibitorName:'Kristyn Monoroe',SponsorType:'Add',IdNumber:'307',BirthYear:2011},
  {ExhibitorId:7421,ExhibitorName:'Margaret Hamilton',SponsorType:'Class',IdNumber:'92E',BirthYear:2007},

]

const data2:SponsorClass[] =[
  {ClassNumber:'10W',ClassName:'Poles Horse',AgeGroup:'12',Exhibitor:'',HorseName:'2011'},
  {ClassNumber:'92E',ClassName:'Western Showmanship',AgeGroup:'13',Exhibitor:'307',HorseName:'2011'},
  {ClassNumber:'31',ClassName:'Barrels Horse',AgeGroup:'13-15',Exhibitor:'92E',HorseName:'2007'},

]






@Component({
  selector: 'app-sponsor',
  templateUrl: './sponsor.component.html',
  styleUrls: ['./sponsor.component.scss']
})
export class SponsorComponent implements OnInit {
  sponsersDisplayedColumns: string[] = ['SponsorId', 'Sponsor', 'Remove'];
  sponsersExhibitorsDisplayedColumns: string[] = ['ExhibitorId', 'ExhibitorName', 'SponsorType','IdNumber','BirthYear','Remove'];
  sponsersClassesDisplayedColumns: string[] = ['ClassNumber', 'ClassName', 'AgeGroup','Exhibitor','HorseName','Remove'];

  columnsToDisplay: string[] = this.sponsersDisplayedColumns.slice();
  data: PeriodicElement[] = ELEMENT_DATA;
  data12: SponsorExhibitors[]= data1;
  data13: SponsorClass[]= data2;

  result: string = '';

  loading = false;
  sponsorInfo:SponsorInformationViewModel = {
    SponsorName: null,
    ContactName: null,
    Phone: null,
    Email: null,
    Address: null,
    City: null,
    State: null,
    ZipCode: null,
    Amount: null,
    SponsorId: null,
    sponsorExhibitors: null,
    sponsorClasses: null
  }

  sponsors:SponsorInformationViewModel[];

  constructor(private sponsorService: SponsorService,public dialog: MatDialog) { }
  ngOnInit(): void {
  }


  getSponsorDetails=(id:number)=> {
  this.sponsorService.getSponsor(id).subscribe(sponsor =>{
  },error=>{
  }
  )}

addSponsor=(sponsor)=>{

}

getAllDetails(){

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
    // this.removeCartObit(data.ObituaryId, this.result)
  });

}

confirmRemoveClass(index, data): void {
  debugger;
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



}
