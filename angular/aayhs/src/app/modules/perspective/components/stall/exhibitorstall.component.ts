import { Component, OnInit, Inject } from '@angular/core';
import { StallService } from '../../../../core/services/stall.service';
import { AssignStallModalComponent } from '../../../../shared/ui/modals/assign-stall-modal/assign-stall-modal.component'
import { MatDialogRef, MatDialogConfig, MatDialog, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { MatSnackbarComponent } from '../../../../shared/ui/mat-snackbar/mat-snackbar.component';

@Component({
  selector: 'app-exhibitorstall',
  templateUrl: './exhibitorstall.component.html',
  styleUrls: ['./exhibitorstall.component.scss']
})
export class ExhibitorStallComponent implements OnInit {
  loading = false;
  stallResponse: any
  chunkedData: any
  allAssignedStalls: any = [];
  exhibitorAssignedStalls: any = [];
  newAssignedStalls: any = [];
  StallTypes: any = [];
  UnassignedStallNumbers: any = [];
  hoverStallId:any;
  hoverStallName:any;
  hoverBookedByType:any;
  hoverStallType:any;


  constructor(

    private stallService: StallService,
    private snackBar: MatSnackbarComponent,
    public dialogRef: MatDialogRef<ExhibitorStallComponent>,
    public dialog: MatDialog, @Inject(MAT_DIALOG_DATA) public data: any) { }

  ngOnInit(): void {

    if (this.data != null && this.data != undefined) {

      this.exhibitorAssignedStalls = this.data.exhibitorStallAssignment != null
        && this.data.exhibitorStallAssignment != undefined ? this.data.exhibitorStallAssignment : [];

      this.StallTypes = this.data.StallTypes != null
        && this.data.StallTypes != undefined ? this.data.StallTypes : [];
    }
    this.UnassignedStallNumbers = this.data.unassignedStallNumbers
    this.getAllAssignedStalls();

  }

  getAllAssignedStalls() {
    this.loading = true;
    return new Promise((resolve, reject) => {
     
      this.allAssignedStalls = [];
      this.stallService.getAllAssignedStalls().subscribe(response => {
        if (response.Data != null && response.Data.TotalRecords > 0) {
          this.allAssignedStalls = response.Data.stallResponses;

        }


        if (this.exhibitorAssignedStalls != null) {
          this.exhibitorAssignedStalls.forEach(exhibitorstall => {
            var stall = this.allAssignedStalls.filter((x) => { return x.StallId == exhibitorstall.StallId });
            if (stall == null || stall.length <= 0) {
              this.allAssignedStalls.push(exhibitorstall);
            }
          });
        }

        this.UnassignedStallNumbers.forEach(ust => {

          this.allAssignedStalls = this.allAssignedStalls.filter(x => x.StallId != ust);
        });


        if (this.allAssignedStalls != null && this.allAssignedStalls.length > 0) {

          this.allAssignedStalls.forEach(data => {
            var s_id = String('stall_' + data.StallId);
            var element = document.getElementById(s_id);

            if (element != null && element != undefined) {
              if (this.exhibitorAssignedStalls.length > 0) {
               
                var assigendstall = this.exhibitorAssignedStalls.filter((x) => { return x.StallId == data.StallId });
                if (assigendstall != null && assigendstall.length > 0) {
                  element.classList.add("bookedexhibitorstall");
                  element.classList.remove("bookedstall");
                  element.classList.remove("clstackstall");
                  element.classList.remove("unassignedexhibitorstall");

                  element.addEventListener('mouseover', () => this.ShowStallDetail(data.StallId));
                }
                else {
                  element.classList.add("bookedstall");
                  element.classList.remove("bookedexhibitorstall");
                  element.classList.remove("clstackstall");
                  element.classList.remove("unassignedexhibitorstall");

                  element.addEventListener('mouseover', () => this.ShowStallDetail(data.StallId));
                }
              }
              else {
                element.classList.add("bookedstall");
                element.classList.remove("bookedexhibitorstall");
                element.classList.remove("clstackstall");
                element.classList.remove("unassignedexhibitorstall");

                element.addEventListener('mouseover', () => this.ShowStallDetail(data.StallId))
              }
            }
          });
        }
        this.loading = false;

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

    var checkInexhibitorassigned = this.exhibitorAssignedStalls.filter((x) => { return x.StallId == stallId });
    var checkInAllassigned = this.allAssignedStalls.filter((x) => { return x.StallId == stallId });

    if (checkInAllassigned != null && checkInAllassigned != undefined && checkInAllassigned.length > 0
      && (checkInexhibitorassigned == null || checkInexhibitorassigned == undefined || checkInexhibitorassigned <= 0)) {
      return;
    }



    var temparray = [];
    if (this.exhibitorAssignedStalls != null && this.exhibitorAssignedStalls != undefined
      && this.exhibitorAssignedStalls.length > 0) {
      this.exhibitorAssignedStalls.forEach(dt => {
        temparray.push(dt);
      });
    }

    if (this.newAssignedStalls != null && this.newAssignedStalls != undefined && this.newAssignedStalls.length > 0) {
      this.newAssignedStalls.forEach(dt => {
        temparray.push(dt);
      });
    }




    if (temparray != null && temparray != undefined && temparray.length > 0) {
      var check = temparray.filter((x) => { return x.StallId == stallId });
      var data: any;
      if (check != null && check != undefined && check.length > 0) {
        data = {
          SelectedStallId: stallId,
          Assigned: true,
          StallAssignmentId: check[0].StallAssignmentId,
          StallAssignmentTypeId: check[0].StallAssignmentTypeId,
          StallAssignmentDate: check[0].StallAssignmentDate,
          BookedByName: check[0].BookedByName
        }
      }
      else {
        data = {
          SelectedStallId: stallId,
          Assigned: false,

          StallAssignmentId: 0,
          StallAssignmentTypeId: 0,
          StallAssignmentDate: new Date(),
          BookedByName: ''
        }
      }
    }
    else {
      data = {
        SelectedStallId: stallId,
        Assigned: false,
        StallAssignmentId: 0,
        StallAssignmentTypeId: 0,
        StallAssignmentDate: new Date(),
        BookedByName: ''
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

          var newExhibitorStallData = {
            StallAssignmentId: result.data.StallAssignmentId,
            StallId: result.data.SelectedStallId,
            StallAssignmentTypeId: result.data.StallAssignmentTypeId,
            StallAssignmentDate: result.data.StallAssignmentDate,
            GroupId: 0,
            ExhibitorId: 0,
            BookedByName: ""
          }

          if (this.newAssignedStalls != null && this.newAssignedStalls != undefined && this.newAssignedStalls.length > 0) {
            var exist = this.newAssignedStalls.filter(x => x.StallId == result.data.SelectedStallId);
            if (exist == null || exist == undefined || exist.length <= 0) {

              this.newAssignedStalls.push(newExhibitorStallData);

            }
          }
          else {
            this.newAssignedStalls.push(newExhibitorStallData);
          }



          if (this.UnassignedStallNumbers != null && this.UnassignedStallNumbers.length > 0) {

            this.UnassignedStallNumbers = this.UnassignedStallNumbers.filter(x => x != result.data.SelectedStallId);

          }

          if (element != null && element != undefined) {
            element.classList.add("bookedexhibitorstall");
            element.classList.remove("bookedstall");
            element.classList.remove("unassignedexhibitorstall");
            element.classList.remove("clstackstall");

             }


        }



        if (result.data.Status == "Unassign") {

          if (this.newAssignedStalls != null && this.newAssignedStalls.length > 0) {
            this.newAssignedStalls = this.newAssignedStalls.filter(x => x.StallId != result.data.SelectedStallId);
          }

          if (this.exhibitorAssignedStalls != null && this.exhibitorAssignedStalls.length > 0) {
            this.exhibitorAssignedStalls = this.exhibitorAssignedStalls.filter(x => x.StallId != result.data.SelectedStallId);
          }
          if (this.allAssignedStalls != null && this.allAssignedStalls.length > 0) {
            this.allAssignedStalls = this.allAssignedStalls.filter(x => x.StallId != result.data.SelectedStallId);
          }
          this.UnassignedStallNumbers.push(result.data.SelectedStallId);

          if (element != null && element != undefined) {
            element.classList.add("unassignedexhibitorstall");
            element.classList.remove("bookedexhibitorstall");
            element.classList.remove("clstackstall");
            element.classList.remove("bookedstall");

          }
        }



        if (result.data.Status == "Move") {

          if (this.allAssignedStalls != null && this.allAssignedStalls != undefined
            && this.allAssignedStalls.length > 0) {
            var stall = this.allAssignedStalls.filter(x => x.StallId == result.data.StallMovedTo);
            if (stall != null && stall != undefined && stall.length > 0) {
              var error = "Already assigned";
              this.snackBar.openSnackBar(error, 'Close', 'red-snackbar');
              return
            }
          }


          if (this.newAssignedStalls != null && this.newAssignedStalls.length > 0) {
            this.newAssignedStalls = this.newAssignedStalls.filter(x => x.StallId != result.data.SelectedStallId);
          }

          if (this.exhibitorAssignedStalls != null && this.exhibitorAssignedStalls.length > 0) {
            this.exhibitorAssignedStalls = this.exhibitorAssignedStalls.filter(x => x.StallId != result.data.SelectedStallId);
          }
          if (this.allAssignedStalls != null && this.allAssignedStalls.length > 0) {
            this.allAssignedStalls = this.allAssignedStalls.filter(x => x.StallId != result.data.SelectedStallId);
          }

          var newExhibitorStallData = {
            StallAssignmentId: result.data.StallAssignmentId,
            StallId: result.data.StallMovedTo,
            StallAssignmentTypeId: result.data.StallAssignmentTypeId,
            StallAssignmentDate: result.data.StallAssignmentDate,
            GroupId: 0,
            ExhibitorId: 0,
            BookedByName: ""
          }

          if (this.newAssignedStalls != null && this.newAssignedStalls != undefined && this.newAssignedStalls.length > 0) {
            var exist = this.newAssignedStalls.filter(x => x.StallId == result.data.StallMovedTo);
            if (exist == null || exist == undefined || exist.length <= 0) {
              this.newAssignedStalls.push(newExhibitorStallData);
            }
          }
          else {
            this.newAssignedStalls.push(newExhibitorStallData);
          }
          this.UnassignedStallNumbers.push(result.data.SelectedStallId);
          if (this.UnassignedStallNumbers != null && this.UnassignedStallNumbers.length > 0) {
            this.UnassignedStallNumbers = this.UnassignedStallNumbers.filter(x => x != result.data.StallMovedTo);
          }



          if (element != null && element != undefined) {
            element.classList.add("unassignedexhibitorstall");
            element.classList.remove("bookedexhibitorstall");
            element.classList.remove("clstackstall");
            element.classList.remove("bookedstall");
          }
          var movedstall_id = String('stall_' + result.data.StallMovedTo);
          var movedtoelement = document.getElementById(movedstall_id);

          if (movedtoelement != null && movedtoelement != undefined) {
            movedtoelement.classList.add("bookedexhibitorstall");
            movedtoelement.classList.remove("unassignedexhibitorstall");
            movedtoelement.classList.remove("clstackstall");
            movedtoelement.classList.remove("bookedstall");
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
    if (this.newAssignedStalls != null && this.newAssignedStalls.length > 0) {
      this.newAssignedStalls.forEach(dt => {
        this.exhibitorAssignedStalls.push(dt);
      });
    }


    this.dialogRef.close({
      submitted: true,
      data: { exhibitorAssignedStalls: this.exhibitorAssignedStalls, unassignedStallNumbers: this.UnassignedStallNumbers }
    });
  }

  
  changeTab() {
    if (this.allAssignedStalls != null && this.allAssignedStalls != undefined && this.allAssignedStalls.length > 0) {
      this.allAssignedStalls.forEach(data => {
        var s_id = String('stall_' + data.StallId);
        var element = document.getElementById(s_id);

        if (element != null && element != undefined) {
          element.classList.add("bookedstall");
          element.classList.remove("bookedexhibitorstall");
          element.classList.remove("clstackstall");
          element.classList.remove("unassignedexhibitorstall");
          element.addEventListener('mouseover', () => this.ShowStallDetail(data.StallId));
        }
      });
    }

    if (this.exhibitorAssignedStalls != null && this.exhibitorAssignedStalls != undefined && this.exhibitorAssignedStalls.length > 0) {
      this.exhibitorAssignedStalls.forEach(data => {
        var s_id = String('stall_' + data.StallId);
        var element = document.getElementById(s_id);

        if (element != null && element != undefined) {
          element.classList.add("bookedexhibitorstall");
          element.classList.remove("bookedstall");
          element.classList.remove("clstackstall");
          element.classList.remove("unassignedexhibitorstall");
          element.addEventListener('mouseover', () => this.ShowStallDetail(data.StallId));
        }
      });
    }


    if (this.newAssignedStalls != null && this.newAssignedStalls != undefined && this.newAssignedStalls.length > 0) {
      this.newAssignedStalls.forEach(data => {
        var s_id = String('stall_' + data.StallId);
        var element = document.getElementById(s_id);

        if (element != null && element != undefined) {
          element.classList.add("bookedexhibitorstall");
          element.classList.remove("bookedstall");
          element.classList.remove("clstackstall");
          element.classList.remove("unassignedexhibitorstall");
          element.addEventListener('mouseover', () => this.ShowStallDetail(data.StallId));
        }
      });
    }
  }


  ShowStallDetail(val) {
    var checkInAllassigned = this.allAssignedStalls.filter((x) => { return x.StallId == val });
    
    if (checkInAllassigned != null && checkInAllassigned != undefined && checkInAllassigned.length > 0)
    {
      document.getElementById("hoverbox").style.display = "block";
      this.hoverStallId=checkInAllassigned[0].StallId;
      this.hoverStallName=checkInAllassigned[0].BookedByName;
      this.hoverBookedByType=checkInAllassigned[0].BookedByType;
      var type=this.StallTypes.filter((x) => { return x.GlobalCodeId == checkInAllassigned[0].StallAssignmentTypeId });
      this.hoverStallType=type[0].CodeName;

    }
    return false;
  }
  closehoverbox(){
    document.getElementById("hoverbox").style.display = "none";
  }

}
