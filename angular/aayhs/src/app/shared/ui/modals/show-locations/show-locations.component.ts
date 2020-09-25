import { Component, OnInit,Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
@Component({
  selector: 'app-show-locations',
  templateUrl: './show-locations.component.html',
  styleUrls: ['./show-locations.component.scss']
})
export class ShowLocationsComponent implements OnInit {

  constructor(public dialogRef: MatDialogRef<ShowLocationsComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any) {
    
  }

  ngOnInit(): void {
  }

  onDismiss(): void {
    // Close the dialog, return false
    this.dialogRef.close(false);
  }

}
