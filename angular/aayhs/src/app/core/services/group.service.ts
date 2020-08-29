import { Injectable } from '@angular/core';
import { BaseUrl } from '../../config/url-config';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
@Injectable({
  providedIn: 'root'
})
export class GroupService {
  api = BaseUrl.baseApiUrl;
  constructor(private http: HttpClient) { }


getAllGroups(data){
  return this.http.post<any>(`${this.api}GroupAPI/GetAllGroups`,data);
}

getGroup(id:number){
  return this.http.get<any>(`${this.api}GroupAPI/GetGroupById?GroupId=${id}`);
  }
 

addUpdateGroup(data){
    return this.http.post<any>(this.api +'GroupAPI/AddUpdateGroup',data);
  }
deleteGroup(id:number){
    return this.http.delete<any>(`${this.api}GroupAPI/DeleteGroup?GroupId=${id}`);
  }

getCities(stateId:number){
    return this.http.get<any>(`${this.api}CommonAPI/GetCities?stateId=${stateId}`);
  }

getZipCodes(cityId:number){
    return this.http.get<any>(`${this.api}CommonAPI/GetZipCodes?cityId=${cityId}`);
  }

getAllStates(){
    return this.http.get<any>(`${this.api}CommonAPI/GetStates`,{});
  }
getGroupExhibitors(id:number){
    return this.http.get<any>(`${this.api}GroupAPI/GetGroupExhibitors?GroupId=${id}`);
    }
deleteGroupExhibitors(id:number){
      return this.http.delete<any>(`${this.api}GroupAPI/DeleteGroupExhibitor?groupExhibitorId=${id}`);
    }
getAllGroupFinancials(id:number){
      return this.http.get<any>(`${this.api}GroupAPI/GetAllGroupFinancials?GroupId=${id}`);
}
addUpdateGroupFinancials(data){
  return this.http.post<any>(this.api +'GroupAPI/AddUpdateGroupFinancials',data);
}
UpdateGroupFinancialsAmount(data){
  return this.http.post<any>(this.api +'GroupAPI/UpdateGroupFinancialsAmount',data);
}

deleteGroupFinancials(id:number){
  return this.http.delete<any>(`${this.api}GroupAPI/DeleteGroupFinancials?groupFinancialId=${id}`);
}
getGlobalCodes(type:string){
  return this.http.get<any>(`${this.api}CommonAPI/GetGlobalCode?categoryName=${type}`);
}
}