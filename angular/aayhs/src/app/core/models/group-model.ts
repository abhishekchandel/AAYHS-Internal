export interface GroupInformationViewModel
{
    GroupName:string;
    ContactName:string;
    Phone:string;
    Email:string;
    Address:string;
    CityId:number;
    StateId:number;
    ZipCode:string;
    AmountReceived :number;
    GroupId:number;

}
export interface BaseResponse{
    Success:boolean,
    Message:string
}