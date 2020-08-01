export interface ClassInfoModel
{
    ClassHeader:string;
    ClassNumber:string;
    ClassName:string;
    Email:string;
    AgeGroup:string;
    ScheduleDate:Date
}

export interface ClassEnteries{
    Exhibitor:string,
    Horse:string,
    BirthYear:number,
    AmountPaid:number,
    AmountDue:number
}

export interface ClassResults{
    Result:string,
    BackNo:string,
    ExhibitorName:number,
    BirthYear:number,
    HorseName:string,
    AmountPaid:number,
    AmountDue:number
}

export interface ClassViewModel
{
    ClassNumber:string,
    ClassName:string,
   
    Enteries:number,
    AgeGroup:string
}