import { Component, OnInit } from '@angular/core';
import 'jspdf-autotable';
import { UserOptions } from 'jspdf-autotable';
import * as jsPDF from 'jspdf';
interface jsPDFWithPlugin extends jsPDF {
  autoTable: (options: UserOptions) => jsPDF;
}
import {ReportService} from 'src/app/core/services/report.service'

@Component({
  selector: 'app-reports',
  templateUrl: './reports.component.html',
  styleUrls: ['./reports.component.scss']
})
export class ReportsComponent implements OnInit {
  loading = false;
 programReport:any;
  constructor(private reportService: ReportService) { }

  ngOnInit(): void {
  }

}
