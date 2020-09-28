import { Component, OnInit,Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'app-class-category-modal',
  templateUrl: './class-category-modal.component.html',
  styleUrls: ['./class-category-modal.component.scss']
})
export class ClassCategoryModalComponent implements OnInit {
  name:null;
  
  constructor(public dialogRef: MatDialogRef<ClassCategoryModalComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any) {
    
  }

  ngOnInit(): void {
  }

  onDismiss(): void {
    // Close the dialog, return false
    this.dialogRef.close(false);
  }

  addClassCategory(){

  }
  
}
