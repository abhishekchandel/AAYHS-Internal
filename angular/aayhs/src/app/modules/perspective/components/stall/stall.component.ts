import { Component, OnInit, Inject } from '@angular/core';
import { StallService } from '../../../../core/services/stall.service';
import { AssignStallModalComponent } from '../../../../shared/ui/modals/assign-stall-modal/assign-stall-modal.component'
import { MatDialogRef, MatDialogConfig, MatDialog, MAT_DIALOG_DATA } from '@angular/material/dialog';


@Component({
  selector: 'app-stall',
  templateUrl: './stall.component.html',
  styleUrls: ['./stall.component.scss']
})
export class StallComponent implements OnInit {
  loading = false;
  stallResponse: any
  chunkedData: any
  allAssignedStalls: any;
  groupAssignedStalls: any;
  tempDataArray: any = [];




  constructor(
    private stallService: StallService,
    public dialogRef: MatDialogRef<StallComponent>,
    public dialog: MatDialog, @Inject(MAT_DIALOG_DATA) public data: any) { }

  ngOnInit(): void {

    this.groupAssignedStalls = this.data;
    this.getAllAssignedStalls();
  }

  getAllAssignedStalls() {
    return new Promise((resolve, reject) => {
      this.loading = true;
      this.allAssignedStalls = null;
      this.stallService.getAllAssignedStalls().subscribe(response => {
        if (response.Data != null && response.Data.TotalRecords > 0) {

          this.allAssignedStalls = response.Data.stallResponses;
          if (this.allAssignedStalls != null && this.allAssignedStalls != undefined && this.allAssignedStalls.length > 0) {

            this.allAssignedStalls.forEach(data => {
              var s_id = String('stall_' + data.StallId);
              var element = document.getElementById(s_id);

              if (element != null && element != undefined) {
                if (this.groupAssignedStalls != null
                  && this.groupAssignedStalls != undefined
                  && this.groupAssignedStalls.length > 0) {

                  var assigendstall = this.groupAssignedStalls.filter((x) => { return x.StallId == data.StallId });
                  if (assigendstall != null && assigendstall.length > 0) {
                    element.classList.add("bookedgroupstall");
                  }
                  else {
                    element.classList.add("bookedstall");
                  }
                }
                else {
                  element.classList.add("bookedstall");
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
    if (this.groupAssignedStalls != null && this.groupAssignedStalls != undefined) {
      var check = this.groupAssignedStalls.filter((x) => { return x.StallId == stallId });
      var data: any;
      if (check != null && check != undefined && check.length > 0) {
        data = {
          SelectedStallId: stallId,
          Assigned: true,
          BookedByType: check[0].BookedByType,
          StallAssignmentId: check[0].StallAssignmentId,
          StallAssignmentTypeId: check[0].StallAssignmentTypeId,
        }
      }
      else {
        data = {
          SelectedStallId: stallId,
          Assigned: false,
          BookedByType: 'Group',
          StallAssignmentId: 0,
          StallAssignmentTypeId: 0,

        }
      }
    }
    else {
      data = {
        SelectedStallId: stallId,
        Assigned: false,
        BookedByType: 'Group',
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
      data: data
    };

    const dialogRef = this.dialog.open(AssignStallModalComponent, config);

    dialogRef.afterClosed().subscribe(dialogResult => {

      const result: any = dialogResult;
      if (result && result.submitted == true) {
        this.tempDataArray.push(result.data);
        var s_id = String('stall_' + result.data.SelectedStallId);
        var element = document.getElementById(s_id);
        if (result.data.Status == "Assigned") {
          if (element != null && element != undefined) {
            element.classList.add("bookedgroupstall");
          }
        }
        if (result.data.Status == "Unassigned") {
          if (element != null && element != undefined) {
            element.classList.add("unassignedgroupstall");
          }
        }
        if (result.data.Status == "Move") {
          if (element != null && element != undefined) {
            element.classList.add("unassignedgroupstall");
          }
          var movedstall_id = String('stall_' + result.data.StallMovedTo);
          var movedtoelement = document.getElementById(movedstall_id);

          if (movedtoelement != null && movedtoelement != undefined) {
            movedtoelement.classList.add("bookedgroupstall");
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
      data: this.tempDataArray
    });
  }
}
