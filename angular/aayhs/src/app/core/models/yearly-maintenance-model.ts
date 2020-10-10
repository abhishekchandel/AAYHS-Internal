export interface YearlyMaintenanceModel{
    ShowStartDate:string,
    ShowEndDate:string,
    PreEntryCutOffDate:string,
    SponcerCutOffDate:string,
    Year:number,
    YearlyMaintenanceId:number
}

export interface ContactInfo{
    Location:string,
    Email1:string,
    Email2:string,
    Phone1:string,
    Phone2:string,
    exhibitorSponsorAddress:string,
    exhibitorSponsorCity:string,
    exhibitorSponsorZip:string,
    exhibitorSponsorState:number,
    exhibitorRefundAddress:string,
    exhibitorRefundCity:string,
    exhibitorRefundZip:string,
    exhibitorRefundState:number,
    returnAddress:string,
    returnCity:string,
    returnZip:string,
    returnState:number
    AAYHSContactId:number
    yearlyMaintenanceId:number

}