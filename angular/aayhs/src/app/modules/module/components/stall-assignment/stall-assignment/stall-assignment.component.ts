import { Component, OnInit, Inject } from '@angular/core';
import { StallService } from '../../../../../core/services/stall.service';

import { MatDialogRef, MatDialogConfig, MatDialog, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatSnackbarComponent } from '../../../../../shared/ui/mat-snackbar/mat-snackbar.component';


@Component({
  selector: 'app-stall-assignment',
  templateUrl: './stall-assignment.component.html',
  styleUrls: ['./stall-assignment.component.scss']
})
export class StallAssignmentComponent implements OnInit {
  loading = false;
  stallResponse: any
  allAssignedStalls: any = [];
  StallTypes: any = [];
  hoverStallId:any;
  hoverStallName:any;
  hoverBookedByType:any;
  hoverStallType:any;

  constructor( private stallService: StallService,
    private snackBar: MatSnackbarComponent,
   ) { }

  ngOnInit(): void {
  
  }
  

}
