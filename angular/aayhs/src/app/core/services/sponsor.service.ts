import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import{SponsorInformationViewModel} from '../models/sponsor-model'
import{SponsorViewModel} from '../models/sponsor-model'
import { BaseUrl } from '../../config/url-config';
import { Observable } from 'rxjs';


@Injectable({
  providedIn: 'root'
})
export class SponsorService {

  api = BaseUrl.baseApiUrl;

  constructor(private http: HttpClient) { }

  getSponsor(id:number){
  return this.http.get<SponsorInformationViewModel>(`${this.api}SponsorAPI/GetSponsorById?sponsorId=${id}`);
  }

  addSponsor(data){
    return this.http.post<any>(this.api +'SponsorAPI/AddSponsor',data);
  }

  getAllSponsers(data){
    
    return this.http.post<any>(`${this.api}SponsorAPI/GetAllSponsorsWithFilter`,data);
  }

  deleteSponsor(id:number){
    return this.http.delete<any>(`${this.api}SponsorAPI/DeleteSponsor?sponsorId=${id}`);
  }

  getCities(stateId:number){
    return this.http.post<any>(`${this.api}CommonAPI/GetCities?stateId=${stateId}`,"");
  }
  getAllStates(){
    return this.http.post<any>(`${this.api}CommonAPI/GetStates`,{});
  }
}
