import { Component, OnInit,Inject  } from '@angular/core';
import { MatDialogRef,MatDialogConfig,MatDialog  , MAT_DIALOG_DATA} from '@angular/material/dialog';
import { GroupService } from '../../../../core/services/group.service';

@Component({
  selector: 'app-assign-stall-modal',
  templateUrl: './assign-stall-modal.component.html',
  styleUrls: ['./assign-stall-modal.component.scss']
})
export class AssignStallModalComponent implements OnInit {
  showAssign:boolean;
  assignmentType:""
  dataToReturn:any;
  stallTypes:any;
  stallAssignmentTypeId:number;
  constructor(
    private groupService: GroupService,
    public dialogRef: MatDialogRef<AssignStallModalComponent>,
    public dialog: MatDialog,@Inject(MAT_DIALOG_DATA) public data: any) { }

  ngOnInit(): void {
    this.showAssign=this.data.assigned;
   this.getAllStallTypes();

  }

  assignStall(){
    this.dataToReturn={
      selectedStallId:this.data.selectedStallId,
      status:"Assign",
      BookedByType:'Group',
      ExhibitorId: 0,
      GroupId:this.data.GroupId,
      StallAssignmentId: this.data.StallAssignmentId,
      StallAssignmentTypeId: this.stallAssignmentTypeId,
      StallMovedTo: 0,
    }
  }


  unAssignStall(){
    this.dataToReturn={
      selectedStallId:this.data.selectedStallId,
      status:"Unssign",
      BookedByType:'Group',
      ExhibitorId: 0,
      GroupId:this.data.GroupId,
      StallAssignmentId: this.data.StallAssignmentId,
      StallAssignmentTypeId: this.stallAssignmentTypeId,
      StallMovedTo: 0,
    }
  }


  moveStall(){
    this.dataToReturn={
      selectedStallId:this.data.selectedStallId,
      status:"Move",
      BookedByType:'Group',
      ExhibitorId: 0,
      GroupId:this.data.GroupId,
      StallAssignmentId: this.data.StallAssignmentId,
      StallAssignmentTypeId: this.stallAssignmentTypeId,
      StallMovedTo: 0,
    }
  }
 

  onDismiss(): void {
    // Close the dialog, return false
    this.dialogRef.close(false);
  }

  getAllStallTypes() {
   
    this.stallTypes=null;
    this.groupService.getGlobalCodes('StallType').subscribe(response => {
      if(response.Data!=null && response.Data.totalRecords>0)
      {
     this.stallTypes = response.Data.globalCodeResponse;
     if(this.data.StallAssignmentTypeId>0)
     {
      this.setStallType(this.data.StallAssignmentTypeId);
      }
      else{
        this.setStallType(this.stallTypes[0].GlobalCodeId);
      }
    }
  }, error => {
     
  })
  }
  setStallType(id){
    this.stallAssignmentTypeId=Number(id);
  }
}
