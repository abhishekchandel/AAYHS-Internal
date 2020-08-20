import { Component, OnInit } from '@angular/core';
import { GroupService } from '../../../../core/services/group.service';
import {AssignStallModalComponent} from '../../../../shared/ui/modals/assign-stall-modal/assign-stall-modal.component'
import { MatDialogRef,MatDialogConfig,MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-stall',
  templateUrl: './stall.component.html',
  styleUrls: ['./stall.component.scss']
})
export class StallComponent implements OnInit {
  loading = false;
  stallResponse:any
  chunkedData :any

  constructor(private groupService: GroupService,public dialogRef: MatDialogRef<StallComponent>, public dialog: MatDialog) { }

  ngOnInit(): void {
  }

 

  chunk(arr,size){
    var newArr = [];
    for (var i=0; i<arr.length; i+=size) {
      newArr.push(arr.slice(i, i+size));
    }
    return newArr; 
  }

  assignStall() {
    var data = {
      
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
