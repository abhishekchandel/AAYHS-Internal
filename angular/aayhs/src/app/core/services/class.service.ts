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
}
