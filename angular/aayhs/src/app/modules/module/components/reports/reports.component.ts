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
    this.getProgramSheet(1);
  }


  savePDF(): void {
    let doc = new jsPDF("p", "mm", "a4") as jsPDFWithPlugin;
    doc.setFontSize(10);
    // doc.setFontType("bold");
    doc.text('Class Name :', 10, 10);
     doc.text(this.programReport.ClassName, 35, 10);

    doc.text('Class Number :', 10, 15);
     doc.text(this.programReport.ClassNumber, 35, 15)

    doc.text('Age Group :', 10, 20);
    doc.text(this.programReport.Age, 35, 20);

    var img = new Image()
    img.src = 'assets/images/logo.png'
    doc.addImage(img, 'png', 190, 5, 16, 20)


    doc.text('Sponsored By :', 100, 30);

      doc.line(0, 33, 300,33);

    doc.setLineWidth(5.0); 



     doc.text(this.programReport.sponsorInfo[0].SponsorName, 10, 40);
     doc.text(this.programReport.sponsorInfo[0].City + ' ' + this.programReport.sponsorInfo[0].StateZipcode, 10, 45);


     doc.text(this.programReport.sponsorInfo[1].SponsorName, 130, 40);
     doc.text(this.programReport.sponsorInfo[1].City + ' ' + this.programReport.sponsorInfo[1].StateZipcode, 130, 45);


     doc.text(this.programReport.sponsorInfo[2].SponsorName, 10, 55);
    doc.text(this.programReport.sponsorInfo[2].City + ' ' + this.programReport.sponsorInfo[2].StateZipcode, 10, 60);


    doc.text(this.programReport.sponsorInfo[3].SponsorName, 130, 55);
     doc.text(this.programReport.sponsorInfo[3].City + ' ' + this.programReport.sponsorInfo[3].StateZipcode, 130, 60);




    doc.autoTable({
       body: this.programReport.classInfo,
      columns:
        [
          { header: 'Back#', dataKey: 'BackNumber' },
          { header: 'NSBA', dataKey: 'NSBA' },
          { header: 'Horse', dataKey: 'HorseName' },
          { header: 'Exhibitor', dataKey: 'ExhibitorName' },
          { header: 'City/State', dataKey: 'City' },

        ],
      margin: { vertical: 35, horizontal: 10 },
      startY:70
    })

    doc.save('ProgramSheet.pdf');
  }

  getProgramSheet(id){
    return new Promise((resolve, reject) => {
      this.loading = true;
      this.reportService.getProgramSheet(id).subscribe(response => {
        this.programReport = response.Data;
        this.savePDF();
        this.loading = false;
      }, error => {
        this.loading = false;
  
      }
      )
      resolve();
    });
  }
  

}
