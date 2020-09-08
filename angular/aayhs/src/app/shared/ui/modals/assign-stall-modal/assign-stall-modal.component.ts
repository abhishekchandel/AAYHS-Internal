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
    this.setStallType(this.stallTypes[0].GlobalCodeId);
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
    if(this.StallMovedTo==null || this.StallMovedTo==undefined || this.StallMovedTo<=0)
    {
      var error="Stall number is non negative required field";
      this.snackBar.openSnackBar(error, 'Close', 'red-snackbar');
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

  // getAllStallTypes() {
   
  //   this.stallTypes=null;
  //   this.groupService.getGlobalCodes('StallType').subscribe(response => {
  //     if(response.Data!=null && response.Data.totalRecords>0)
  //     {
  //    this.stallTypes = response.Data.globalCodeResponse;
  //    if(this.data.StallAssignmentTypeId>0)
  //    {
  //     this.setStallType(this.data.StallAssignmentTypeId);
  //     }
  //     else{
  //       this.setStallType(this.stallTypes[0].GlobalCodeId);
  //     }
  //   }
  // }, error => {
     
  // })
  // }

  setStallType(id){
    
    this.StallAssignmentTypeId=Number(id);
  }
  setMoveToStall(id){
    
    this.StallMovedTo=Number(id);
  }
}
