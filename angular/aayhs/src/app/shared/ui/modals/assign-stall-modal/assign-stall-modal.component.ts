import { Component, OnInit,Inject  } from '@angular/core';
import { MatDialogRef,MatDialogConfig,MatDialog  , MAT_DIALOG_DATA} from '@angular/material/dialog';
import { GroupService } from '../../../../core/services/group.service';
import { MatSnackbarComponent } from '../../../../shared/ui/mat-snackbar/mat-snackbar.component';

@Component({
  selector: 'app-assign-stall-modal',
  templateUrl: './assign-stall-modal.component.html',
  styleUrls: ['./assign-stall-modal.component.scss']
})
export class AssignStallModalComponent implements OnInit {
  showAssign:boolean;
  showMove:boolean=false;
  assignmentType:""
  dataToReturn:any;
  stallTypes:any;
  StallAssignmentTypeId:number;
  StallMovedTo:number=null;
  StallNumber:number;
  AssignedToName:string;
  constructor(
    private groupService: GroupService,
    private snackBar: MatSnackbarComponent,
    public dialogRef: MatDialogRef<AssignStallModalComponent>,
    public dialog: MatDialog,@Inject(MAT_DIALOG_DATA) public data: any) { }

  ngOnInit(): void {
    this.StallNumber=this.data.modalData.SelectedStallId;
    this.AssignedToName=this.data.modalData.AssignedToName;
    this.showAssign=this.data.modalData.Assigned;
    this.stallTypes=this.data.StallTypes;
    if(this.StallNumber==2051 || this.StallNumber==2045 || this.StallNumber==2132 || this.StallNumber==2138)
    {
    this.setStallType(this.stallTypes[1].GlobalCodeId);
    }
    else{
      if(this.data.modalData.StallAssignmentTypeId>0)
      {
        this.setStallType(this.data.modalData.StallAssignmentTypeId);
      }
      else{
      this.setStallType(this.stallTypes[0].GlobalCodeId);
      }
    }
  // this.getAllStallTypes();

  }

  assignStall(){
    
    this.dataToReturn={
      SelectedStallId:this.data.modalData.SelectedStallId,
      Status:"Assign",
      StallAssignmentId: this.data.modalData.StallAssignmentId,
      StallAssignmentTypeId: this.StallAssignmentTypeId,
      StallMovedTo: 0,
    }
    this.dialogRef.close({
      submitted: true,
      data: this.dataToReturn
    });
  }


  unAssignStall(){
    
    this.dataToReturn={
      SelectedStallId:this.data.modalData.SelectedStallId,
      Status:"Unassign",
      StallAssignmentId: this.data.modalData.StallAssignmentId,
      StallAssignmentTypeId: this.StallAssignmentTypeId,
      StallMovedTo: 0,
    }
    this.dialogRef.close({
      submitted: true,
      data: this.dataToReturn
    });
  }


  moveStall(){
    if(this.StallMovedTo==null || this.StallMovedTo==undefined)
    {
      var error="Stall number is required field";
      this.snackBar.openSnackBar(error, 'Close', 'red-snackbar');
      return
    }
    if(this.StallMovedTo <=0 || (this.StallMovedTo >1001 && this.StallMovedTo < 2027) || this.StallMovedTo > 2195 )
    {
      var error="Invalid Stall number";
      this.snackBar.openSnackBar(error, 'Close', 'red-snackbar');
      return
    }

    else{
    this.dataToReturn={
      SelectedStallId:this.data.modalData.SelectedStallId,
      Status:"Move",
      StallAssignmentId: this.data.modalData.StallAssignmentId,
      StallAssignmentTypeId: this.StallAssignmentTypeId,
      StallMovedTo: this.StallMovedTo,
    }
    this.dialogRef.close({
      submitted: true,
      data: this.dataToReturn
    });
  }
}


  toggleMove(check:boolean){
    this.showMove=check;
  }

  onDismiss(): void {
    this.dialogRef.close({
      submitted: false,
      data: null
    });
  }

  

  setStallType(id){
    
    this.StallAssignmentTypeId=Number(id);
  }

  setMoveToStall(id){
    
    this.StallMovedTo=Number(id);
  }
}
