import { Component, OnInit, Inject } from '@angular/core';
import { StallService } from '../../../../core/services/stall.service';
import { AssignStallModalComponent } from '../../../../shared/ui/modals/assign-stall-modal/assign-stall-modal.component'
import { MatDialogRef, MatDialogConfig, MatDialog, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatSnackbarComponent } from '../../../../shared/ui/mat-snackbar/mat-snackbar.component';

@Component({
  selector: 'app-stall',
  templateUrl: './stall.component.html',
  styleUrls: ['./stall.component.scss']
})
export class StallComponent implements OnInit {
  loading = false;
  stallResponse: any
  chunkedData: any
  allAssignedStalls: any = [];
  groupAssignedStalls: any = [];
  StallTypes: any = [];




  constructor(
    private stallService: StallService,
    private snackBar: MatSnackbarComponent,
    public dialogRef: MatDialogRef<StallComponent>,
    public dialog: MatDialog, @Inject(MAT_DIALOG_DATA) public data: any) { }

  ngOnInit(): void {
    debugger
    if (this.data != null && this.data != undefined) {

      this.groupAssignedStalls = this.data.groupStallAssignment != null
        && this.data.groupStallAssignment != undefined ? this.data.groupStallAssignment : [];

      this.StallTypes = this.data.StallTypes != null
        && this.data.StallTypes != undefined ? this.data.StallTypes : [];
    }
    this.getAllAssignedStalls();
  }

  getAllAssignedStalls() {
    return new Promise((resolve, reject) => {
      this.loading = true;
      this.allAssignedStalls = null;
      this.stallService.getAllAssignedStalls().subscribe(response => {
        if (response.Data != null && response.Data.TotalRecords > 0) {
          debugger
          this.allAssignedStalls = response.Data.stallResponses;
          if (this.groupAssignedStalls != null) {
            this.groupAssignedStalls.forEach(groupstall => {
              var stall = this.allAssignedStalls.filter((x) => { return x.StallId == groupstall.StallId });
              if (stall == null || stall.length <= 0) {
                this.allAssignedStalls.push(groupstall);
              }
            });
          }


          if (this.allAssignedStalls != null && this.allAssignedStalls != undefined && this.allAssignedStalls.length > 0) {

            this.allAssignedStalls.forEach(data => {
              var s_id = String('stall_' + data.StallId);
              var element = document.getElementById(s_id);

              if (element != null && element != undefined) {
                if (this.groupAssignedStalls.length > 0) {

                  var assigendstall = this.groupAssignedStalls.filter((x) => { return x.StallId == data.StallId });
                  if (assigendstall != null && assigendstall.length > 0) {
                    element.classList.add("bookedgroupstall");
                    element.classList.remove("bookedstall");
                  }
                  else {
                    element.classList.add("bookedstall");
                    element.classList.remove("bookedgroupstall");

                  }
                }
                else {
                  element.classList.add("bookedstall");
                  element.classList.remove("bookedgroupstall");
                }
              }
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



  chunk(arr, size) {
    var newArr = [];
    for (var i = 0; i < arr.length; i += size) {
      newArr.push(arr.slice(i, i + size));
    }
    return newArr;
  }

  assignStall(stallId) {
    var checkassigned = this.groupAssignedStalls.filter((x) => { return x.StallId == stallId });

    if (checkassigned == null || checkassigned == undefined || checkassigned.length <= 0) {
      var checknonassigned = this.allAssignedStalls.filter((x) => { return x.StallId == stallId });
      if (checknonassigned != null && checknonassigned != undefined && checknonassigned.length > 0) {
        var nameandtype ='Already assigned to '+ checknonassigned[0].BookedByName + '--' + checknonassigned[0].BookedByType;
        this.snackBar.openSnackBar(nameandtype, 'Close', 'red-snackbar');
        return;
      }
    }




    if (this.groupAssignedStalls != null && this.groupAssignedStalls != undefined) {
      var check = this.groupAssignedStalls.filter((x) => { return x.StallId == stallId });
      var data: any;
      if (check != null && check != undefined && check.length > 0) {
        data = {
          SelectedStallId: stallId,
          Assigned: true,

          StallAssignmentId: check[0].StallAssignmentId,
          StallAssignmentTypeId: check[0].StallAssignmentTypeId,
          AssignedToName: check[0].GroupName
        }
      }
      else {
        data = {
          SelectedStallId: stallId,
          Assigned: false,

          StallAssignmentId: 0,
          StallAssignmentTypeId: 0,
          AssignedToName: ''
        }
      }
    }
    else {
      data = {
        SelectedStallId: stallId,
        Assigned: false,
        StallAssignmentId: 0,
        StallAssignmentTypeId: 0,
        AssignedToName: ''
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
      data: { modalData: data, StallTypes: this.StallTypes }
    };

    const dialogRef = this.dialog.open(AssignStallModalComponent, config);

    dialogRef.afterClosed().subscribe(dialogResult => {
      const result: any = dialogResult;
      if (result && result.submitted == true) {

        var s_id = String('stall_' + result.data.SelectedStallId);
        var element = document.getElementById(s_id);



        if (result.data.Status == "Assign") {

          var newGroupStallData = {
            StallAssignmentId: result.data.StallAssignmentId,
            StallId: result.data.SelectedStallId,
            StallAssignmentTypeId: result.data.StallAssignmentTypeId,
            GroupId: 0,
            ExhibitorId: 0,
            GroupName: ""
          }
          this.groupAssignedStalls.push(newGroupStallData);


          if (element != null && element != undefined) {
            element.classList.add("bookedgroupstall");
            element.classList.remove("unassignedgroupstall");

          }
        }



        if (result.data.Status == "Unassign") {

          if (this.groupAssignedStalls != null && this.groupAssignedStalls.length > 0) {
            this.groupAssignedStalls = this.groupAssignedStalls.filter(x => x.StallId != result.data.SelectedStallId);
          }

          if (element != null && element != undefined) {
            element.classList.add("unassignedgroupstall");
            element.classList.remove("bookedgroupstall");
          }
        }



        if (result.data.Status == "Move") {

          if (this.groupAssignedStalls != null && this.groupAssignedStalls.length > 0) {
            this.groupAssignedStalls = this.groupAssignedStalls.filter(x => x.StallId != result.data.SelectedStallId);
          }

          var newGroupStallData = {
            StallAssignmentId: result.data.StallAssignmentId,
            StallId: result.data.StallMovedTo,
            StallAssignmentTypeId: result.data.StallAssignmentTypeId,
            GroupId: 0,
            ExhibitorId: 0,
            GroupName: ""
          }
          this.groupAssignedStalls.push(newGroupStallData);


          if (element != null && element != undefined) {
            element.classList.add("unassignedgroupstall");
            element.classList.remove("bookedgroupstall");
          }
          var movedstall_id = String('stall_' + result.data.StallMovedTo);
          var movedtoelement = document.getElementById(movedstall_id);

          if (movedtoelement != null && movedtoelement != undefined) {
            movedtoelement.classList.add("bookedgroupstall");
            movedtoelement.classList.remove("unassignedgroupstall");
          }
        }
      }
    });
  }

  onDismiss(): void {
    this.dialogRef.close({
      submitted: false,
      data: null
    });
  }

  onSubmit(): void {
    this.dialogRef.close({
      submitted: true,
      data: this.groupAssignedStalls
    });
  }
}
