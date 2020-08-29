import { Injectable } from '@angular/core';
import { BaseUrl } from '../../config/url-config';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class ExhibitorService {

  api = BaseUrl.baseApiUrl;
  constructor(private http: HttpClient) { }

  getAllExhibitors(data){
    return this.http.post<any>(`${this.api}ExhibitorAPI/GetAllExhibitors`,data);
  }

  deleteExhibitor(id:number){
    return this.http.delete<any>(`${this.api}ExhibitorAPI/DeleteExhibitor?exhibitorId=${id}`);
  }

  getCities(stateId:number){
    return this.http.get<any>(`${this.api}CommonAPI/GetCities?stateId=${stateId}`);
  }
  getAllStates(){
    return this.http.get<any>(`${this.api}CommonAPI/GetStates`,{});
  }
  getZipCodes(cityId:number){
    return this.http.get<any>(`${this.api}CommonAPI/GetZipCodes?cityId=${cityId}`);
  }
  getGroups(){
    return this.http.get<any>(`${this.api}HorseAPI/GetGroups`);
  }

  getExhibitorById(id:number){
    return this.http.get<any>(`${this.api}ExhibitorAPI/GetExhibitorById?exhibitorId=${id}`);
  }
 
  createUpdateExhibitor(data){
    return this.http.post<any>(`${this.api}ExhibitorAPI/AddUpdateExhibitor`,data);
  }

  getExhibitorHorses(id:number){
    return this.http.get<any>(`${this.api}ExhibitorAPI/GetExhibitorHorses?exhibitorId=${id}`)
    }
  
    deleteExhibitorHorse(id:number){
      return this.http.delete<any>(`${this.api}ExhibitorAPI/DeleteExhibitorHorse?exhibitorId=${id}`);
  
    }
}
