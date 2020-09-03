import { Injectable } from '@angular/core';
import { BaseUrl } from '../../config/url-config';
import { HttpClient, HttpParams } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class ExhibitorService {

  api = BaseUrl.baseApiUrl;
  constructor(private http: HttpClient) { }

  // exhibitor info

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


  // exhibitor horses

    getExhibitorHorses(id:number){
    return this.http.get<any>(`${this.api}ExhibitorAPI/GetExhibitorHorses?exhibitorId=${id}`)
    }
  
    deleteExhibitorHorse(id:number){
      return this.http.delete<any>(`${this.api}ExhibitorAPI/DeleteExhibitorHorse?exhibitorHorseId=${id}`);
  
    }

    getAllHorses(id:number){
      return this.http.get<any>(`${this.api}ExhibitorAPI/GetAllHorses?exhibitorId=${id}`);
    }

    getHorseDetail(id:number){
      return this.http.get<any>(`${this.api}ExhibitorAPI/GetHorseDetail?horseId=${id}`);
    }

    addHorseToExhibitor(data){
      return this.http.post<any>(`${this.api}ExhibitorAPI/AddExhibitorHorse`,data);
    }


    // exhibitor classes
    
    getExhibitorClasses(id:number){
      return this.http.get<any>(`${this.api}ExhibitorAPI/GetAllClassesOfExhibitor?exhibitorId=${id}`)
    }

    deleteExhibitorClass(id:number){
      return this.http.delete<any>(`${this.api}ExhibitorAPI/RemoveExhibitorFromClass?exhibitorClassId=${id}`);
    }

    getAllClasses(id:number){
      return this.http.get<any>(`${this.api}ExhibitorAPI/GetAllClasses?exhibitorId=${id}`);
    }

    getClassDetail(classId,exhibitorId){
      let params = new HttpParams();
      params = params.append('classId', classId);
      params = params.append('exhibitorId',exhibitorId);

      return this.http.get<any>(`${this.api}ExhibitorAPI/GetClassDetail`, { params: params });
    }

    addExhibitorToClass(data){
      return this.http.post<any>(`${this.api}ExhibitorAPI/AddExhibitorToClass`,data);
    }


    //exhibitor sponsor
    getExhibitorSponsors(id:number){
      return this.http.get<any>(`${this.api}ExhibitorAPI/GetAllSponsorsOfExhibitor?exhibitorId=${id}`)
    }

    deleteExhibitorSponsor(id:number){
      return this.http.delete<any>(`${this.api}ExhibitorAPI/RemoveSponsorFromExhibitor?sponsorExhibitorId=${id}`);
    }

    getAllSponsors(id:number){
      return this.http.get<any>(`${this.api}ExhibitorAPI/GetAllSponsor?exhibitorId=${id}`);
    }

    addSponsorToExhibitor(data){
      return this.http.post<any>(`${this.api}ExhibitorAPI/AddSponsorForExhibitor`,data);
    }

    getSponsorInfo(id:number){
      return this.http.get<any>(`${this.api}ExhibitorAPI/GetSponsorDetailedInfo?sponsorId=${id}`);
    }

    updateScratch(data){
      return this.http.post<any>(`${this.api}ExhibitorAPI/UpdateScratch`,data);
    }
}
