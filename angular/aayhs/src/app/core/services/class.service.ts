import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import{SponsorViewModel} from '../models/sponsor-model'
import { BaseUrl } from '../../config/url-config';
@Injectable({
  providedIn: 'root'
})
export class ClassService {

  api = BaseUrl.baseApiUrl;

  constructor(private http: HttpClient) { }


  getAllClasses(data){
    debugger;
    return this.http.post<any>(`${this.api}ClassAPI/GetAllClasses`,data);
  }

  deleteSponsor(id:number){
    return this.http.delete<any>(`${this.api}ClassAPI/RemoveClass?sponsorId=${id}`);
  }

  getClassById(id:number){
    return this.http.get<any>(`${this.api}ClassAPI/GetClass?classId=${id}`);
  }

  getClassExhibitors(id:number){
    return this.http.get<any>(`${this.api}ClassAPI/GetClassExhibitors?classId=${id}`);
  }

  createUpdateClass(data){
    return this.http.post<any>(`${this.api}ClassAPI/CreateUpdateClass`,data);
  }

  getClassEnteries(id:number){
    return this.http.get<any>(`${this.api}ClassAPI/GetClassEnteries?classId=${id}`);
  }

  deleteClassExhibitor(id:number){
    return this.http.delete<any>(`${this.api}ClassAPI/DeleteClassExhibitor?exhibitorClassId=${id}`);
  }

  deleteClass(id:number){
    return this.http.delete<any>(`${this.api}ClassAPI/RemoveClassExhibitor?classId=${id}`);
  }

  createUpdateSplitClass(data){
    return this.http.post<any>(`${this.api}ClassAPI/AddUpdateSplitClass`,data);
  }

  getbackNumber(id:number){
    return this.http.get<any>(`${this.api}ClassAPI/GetBackNumberForAllExhibitor?classId=${id}`);
  }

  getExhibitorReults(id:number){
    return this.http.get<any>(`${this.api}ClassAPI/GetResultExhibitorDetails?classId=${id}`);
  }

  addClassResult(data){
    return this.http.post<any>(`${this.api}ClassAPI/AddClassResult`,data);
  }

}
