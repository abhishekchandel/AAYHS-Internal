export interface SponsorInformationViewModel
{
    SponsorName:string;
    ContactName:string;
    Phone:string;
    Email:string;
    Address:string;
    City:string;
    State:string;
    ZipCode:string;
    Amount:number;
    SponsorId:number;
    sponsorExhibitors:Array<SponsorExhibitors>;
    sponsorClasses:Array<SponsorClasses>
}

export interface SponsorExhibitors{
    ExhibitorId:number,
    ExhibitorName:string,
    SponsorType:string,
    IdNumber:string,
    BirthYear:number
}

export interface SponsorClasses{
    ClassNumber:string,
    ClassName:string,
    FromAge:number,
    ToAge:number,
    Exhibitor:string,
    HorseName:string
}
export interface SponserViewModel{
    SponsorId:number,
    SponsorName:string
}