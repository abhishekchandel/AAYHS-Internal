import { Component, OnInit,Inject } from '@angular/core';
import { StallService } from '../../../../core/services/stall.service';
import {AssignStallModalComponent} from '../../../../shared/ui/modals/assign-stall-modal/assign-stall-modal.component'
import { MatDialogRef,MatDialogConfig,MatDialog , MAT_DIALOG_DATA} from '@angular/material/dialog';


@Component({
  selector: 'app-stall',
  templateUrl: './stall.component.html',
  styleUrls: ['./stall.component.scss']
})
export class StallComponent implements OnInit {
  loading = false;
  stallResponse:any
  chunkedData :any
  allAssignedStalls:any;
  groupAssignedStalls:any;

  constructor(
    private stallService: StallService,
    public dialogRef: MatDialogRef<StallComponent>,
    public dialog: MatDialog,@Inject(MAT_DIALOG_DATA) public data: any) { }

  ngOnInit(): void {
  
    this.groupAssignedStalls=this.data;
    this.getAllAssignedStalls();
  }

  getAllAssignedStalls() {
    return new Promise((resolve, reject) => {
    this.loading = true;
    this.allAssignedStalls=null;
    this.stallService.getAllAssignedStalls().subscribe(response => {
      if(response.Data!=null && response.Data.TotalRecords>0)
      {

     this.allAssignedStalls = response.Data.stallResponses;
     if(this.allAssignedStalls!=null && this.allAssignedStalls!=undefined && this.allAssignedStalls.length>0)
     {

     this.allAssignedStalls.forEach(data => {
     var id=String('stall_'+ data.StallId);
      var element = document.getElementById(id);
      debugger
      element.classList.add("bookedstall");
    });
    }

    this.loading = false;
    }
      
    }, error => {
      this.loading = false;
    })
    resolve();
  });
}

 

  chunk(arr,size){
    var newArr = [];
    for (var i=0; i<arr.length; i+=size) {
      newArr.push(arr.slice(i, i+size));
    }
    return newArr; 
  }

  assignStall(stallId) {
    debugger
    var check=this.groupAssignedStalls.filter((x) => { return x.StallId ==   stallId});
    var data:any;
if(check!=null && check !=undefined)
{
data={
  selectedStallId:stallId,
  assigned:true,
  BookedByType:check.BookedByType,
  ExhibitorId: check.ExhibitorId,
  GroupId: check.GroupId,
  StallAssignmentId: check.StallAssignmentId,
  StallAssignmentTypeId: check.StallAssignmentTypeId,
 }
}
else{
  data={
    selectedStallId:stallId,
    assigned:false,
    BookedByType:'Group',
    ExhibitorId: 0,
    GroupId: 0,
    StallAssignmentId: 0,
    StallAssignmentTypeId: 0,

  }
}

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
    data:data
  };

    const dialogRef = this.dialog.open(AssignStallModalComponent, config);

    dialogRef.afterClosed().subscribe(dialogResult => {
      const result: any = dialogResult;
      if (result && result.submitted == true) {
       
      }
    });
  }

  onDismiss(): void {
    // Close the dialog, return false
    this.dialogRef.close(false);
  }
}
