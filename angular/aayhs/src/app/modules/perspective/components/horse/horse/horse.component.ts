import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-horse',
  templateUrl: './horse.component.html',
  styleUrls: ['./horse.component.scss']
})
export class HorseComponent implements OnInit {
  sortColumn:string="";
  reverseSort : boolean = false
  constructor() { }

  ngOnInit(): void {
  }
  sortData(column){
    this.reverseSort=(this.sortColumn===column)?!this.reverseSort:false
    this.sortColumn=column
  }
  
  getSort(column){
  
    if(this.sortColumn===column)
    {
    return this.reverseSort ? 'arrow-down'
    : 'arrow-up';
    }
  }
}
