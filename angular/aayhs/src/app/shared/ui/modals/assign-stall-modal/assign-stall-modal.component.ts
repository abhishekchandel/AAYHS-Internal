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
  showMove:boolean=false;
  assignmentType:""
  dataToReturn:any;
  stallTypes:any;
  StallAssignmentTypeId:number;
  StallMovedTo:number=0;
  StallNumber:number;
  AssignedToName:string;
  constructor(
    private groupService: GroupService,
    public dialogRef: MatDialogRef<AssignStallModalComponent>,
    public dialog: MatDialog,@Inject(MAT_DIALOG_DATA) public data: any) { }

  ngOnInit(): void {
    this.StallNumber=this.data.SelectedStallId;
    this.AssignedToName=this.data.AssignedToName;
    this.showAssign=this.data.Assigned;
   this.getAllStallTypes();

  }

  assignStall(){
    
    this.dataToReturn={
      SelectedStallId:this.data.SelectedStallId,
      Status:"Assign",
      StallAssignmentId: this.data.StallAssignmentId,
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
      SelectedStallId:this.data.SelectedStallId,
      Status:"Unassign",
      StallAssignmentId: this.data.StallAssignmentId,
      StallAssignmentTypeId: this.StallAssignmentTypeId,
      StallMovedTo: 0,
    }
    this.dialogRef.close({
      submitted: true,
      data: this.dataToReturn
    });
  }


  moveStall(){
    
    this.dataToReturn={
      SelectedStallId:this.data.SelectedStallId,
      Status:"Move",
      StallAssignmentId: this.data.StallAssignmentId,
      StallAssignmentTypeId: this.StallAssignmentTypeId,
      StallMovedTo: this.StallMovedTo,
    }
    this.dialogRef.close({
      submitted: true,
      data: this.dataToReturn
    });
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
    
    this.StallAssignmentTypeId=Number(id);
  }
  setMoveToStall(id){
    
    this.StallMovedTo=Number(id);
  }
}
