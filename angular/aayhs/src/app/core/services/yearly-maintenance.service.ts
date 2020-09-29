import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class YearlyMaintenanceService {

  api =environment.baseApiUrl;

  constructor(private http: HttpClient) { }

  getYearlyMaintenanceSummary(data){
    return this.http.post<any>(`${this.api}YearlyMaintenance/GetAllYearlyMaintenance`,data);
  }


  getNewRegisteredUsers(){
    return this.http.get<any>(`${this.api}YearlyMaintenance/GetAllUsers`);
  }

  verifyUser(data){
    return this.http.post<any>(`${this.api}YearlyMaintenance/ApprovedUser`,data);
  }

  deleteUser(id){
    return this.http.delete<any>(`${this.api}YearlyMaintenance/Deleteuser?userId=${id}`);
  }

  addYear(data){
    return this.http.post<any>(`${this.api}YearlyMaintenance/AddUpdateYearly`,data);
  }

  deleteYear(id){
    return this.http.delete<any>(`${this.api}YearlyMaintenance/DeleteYearly?yearlyMaintainenceId=${id}`);
  }

  getAdFees(id){
    return this.http.get<any>(`${this.api}YearlyMaintenance/GetAllAdFees?yearlyMaintenanceId=${id}`);
  }

  addAdFee(data){
    return this.http.post<any>(`${this.api}YearlyMaintenance/AddAdFee`,data);
  }

  deleteAdFee(id){
    return this.http.delete<any>(`${this.api}YearlyMaintenance/DeleteAdFee?yearlyMaintenanceFeeId=${id}`);
  }

  getYearlyMaintenanceById(id){
    debugger;
    return this.http.get<any>(`${this.api}YearlyMaintenance/GetYearlyMaintenanceById?yearlyMaintenanceId=${id}`);
  }

  deleteApprovedUser(id){
    return this.http.delete<any>(`${this.api}YearlyMaintenance/RemoveApprovedUser?userId=${id}`);
  }

  getApprovedUser(){
    return this.http.get<any>(`${this.api}YearlyMaintenance/GetAllUsersApproved`);
  }

  getRoles(){
    return this.http.get<any>(`${this.api}YearlyMaintenance/GetAllRoles`);

  }
}
