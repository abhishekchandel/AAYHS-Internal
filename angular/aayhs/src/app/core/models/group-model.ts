export interface GroupInformationViewModel
{
    GroupName:string;
    ContactName:string;
    Phone:string;
    Email:string;
    Address:string;
    CityId:number;
    StateId:number;
    ZipCodeId:number;
    AmountReceived :any;
    GroupId:number;
    groupStallAssignmentRequests:any;
}
export interface BaseResponse{
    Success:boolean,
    Message:string
}