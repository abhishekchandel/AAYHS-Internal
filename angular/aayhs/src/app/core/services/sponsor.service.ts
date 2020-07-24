import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import{SponsorInformationViewModel} from '../models/sponsor-info'
import { BaseUrl } from '../../config/url-config';

@Injectable({
  providedIn: 'root'
})
export class SponsorService {

  api = BaseUrl.baseApiUrl;

  constructor(private http: HttpClient) { }

  getSponsor(id:number){
  return this.http.get<SponsorInformationViewModel>(`${this.api} +SponsorAPI/GetSponsorById?sponsorId=${id}`);
  }

  addSponsor(data){
    return this.http.post<any>(this.api +'SponsorAPI/AddSponser',data);
  }

  getAllSponsers(){
    return this.http.get<SponsorInformationViewModel[]>(this.api +'SponsorAPI/GetAllSponsors');
  }
}
