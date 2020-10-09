import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ReportService {
  api =environment.baseApiUrl;
  constructor(private http: HttpClient) { }

  getProgramSheet(id:number){
    return this.http.get<any>(`${this.api}Report/GetProgramsReport?classId=${id}`);
  }
  getExhibitorRegistrationReport(exhibitorId:number){
    return this.http.get<any>(`${this.api}Report/GetExhibitorRegistrationReport?exhibitorIdst=${exhibitorId}`);
  }
}
