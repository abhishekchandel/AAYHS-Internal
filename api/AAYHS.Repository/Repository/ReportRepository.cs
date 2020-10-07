using AAYHS.Core.DTOs.Request;
using AAYHS.Core.DTOs.Response;
using AAYHS.Data.DBContext;
using AAYHS.Repository.IRepository;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace AAYHS.Repository.Repository
{
    public class ReportRepository: IReportRepository
    {
        #region readonly
        private readonly AAYHSDBContext _ObjContext;
        private IMapper _Mapper;
        #endregion

        #region private 
        private MainResponse _MainResponse;
        #endregion

        public ReportRepository(AAYHSDBContext ObjContext, IMapper Mapper)
        {
            _ObjContext = ObjContext;
            _Mapper = Mapper;
            _MainResponse = new MainResponse();
        }

        public GetExhibitorRegistrationReport GetExhibitorRegistrationReport(RegistrationReportRequest registrationReportRequest)
        {
            IEnumerable<GetExhibitorRegistrationReport> data;
            GetExhibitorRegistrationReport getExhibitorRegistrationReport = new GetExhibitorRegistrationReport();

           var yearlyMainId = _ObjContext.YearlyMaintainence.Where(x => x.Years == DateTime.Now.Year && x.IsActive == true 
            && x.IsDeleted == false).FirstOrDefault();

            data = (from contactInfo in _ObjContext.AAYHSContact
                    join address in _ObjContext.AAYHSContactAddresses on contactInfo.ExhibitorConfirmationEntriesAddressId equals
                    address.AAYHSContactAddressId
                    join states in _ObjContext.States on address.StateId equals states.StateId
                    where contactInfo.YearlyMaintainenceId == yearlyMainId.YearlyMaintainenceId
                    && contactInfo.IsActive == true && contactInfo.IsDeleted == false
                    select new GetExhibitorRegistrationReport
                    {
                        Email1 = contactInfo.Email1,
                        Address = address.Address,
                        CityName = address.City,
                        StateZipcode = states.Code + ", " + address.ZipCode,
                        Phone1 = contactInfo.Phone1,

                        exhibitorInfo = (from exhibtor in _ObjContext.Exhibitors
                                         join address in _ObjContext.Addresses on exhibtor.AddressId equals address.AddressId
                                         join city in _ObjContext.Cities on address.CityId equals city.CityId
                                         join state in _ObjContext.States on city.StateId equals state.StateId
                                         join zipcode in _ObjContext.ZipCodes2 on address.ZipCodeId equals zipcode.ZipCodeId
                                         where exhibtor.ExhibitorId == registrationReportRequest.ExhibitorId
                                         select new ExhibitorInfo 
                                         { 
                                           ExhibitorName=exhibtor.FirstName+" "+exhibtor.LastName,
                                           Address=address.Address,
                                           CityName=city.Name,
                                           StateZipcode=state.Code+", "+zipcode.ZipCode,
                                           Email=exhibtor.PrimaryEmail,
                                           Phone=exhibtor.Phone                                        
                                         }).FirstOrDefault(),

                      horseDetail=(from horseExhibitor in _ObjContext.ExhibitorHorse
                                   join horse in _ObjContext.Horses on horseExhibitor.HorseId equals horse.HorseId
                                   where horseExhibitor.ExhibitorId== registrationReportRequest.ExhibitorId
                                   && horseExhibitor.HorseId== registrationReportRequest.HorseId
                                    select new HorseDetail 
                                    { 
                                      HorseName=horse.Name,
                                      BackNumber= horseExhibitor.BackNumber,
                                      ExhibitorId= registrationReportRequest.ExhibitorId,
                                      NSBAIndicator=horse.NSBAIndicator
                                    }).FirstOrDefault()
                    }) ;

            getExhibitorRegistrationReport = data.FirstOrDefault();

            var stallCodes = (from gcc in _ObjContext.GlobalCodeCategories
                              join gc in _ObjContext.GlobalCodes on gcc.GlobalCodeCategoryId equals gc.CategoryId
                              where gcc.CategoryName == "StallType" && gc.IsDeleted == false && gc.IsActive == true
                              select new
                              {
                                  gc.GlobalCodeId,
                                  gc.CodeName

                              }).ToList();
            int horseStallTypeId = stallCodes.Where(x => x.CodeName == "HorseStall").Select(x => x.GlobalCodeId).FirstOrDefault();
            int tackStallTypeId = stallCodes.Where(x => x.CodeName == "TackStall").Select(x => x.GlobalCodeId).FirstOrDefault();

            var preHorseStall = _ObjContext.StallAssignment.Where(x => x.ExhibitorId == registrationReportRequest.ExhibitorId && x.StallAssignmentTypeId == horseStallTypeId &&
                                                     x.Date.Date < yearlyMainId.PreEntryCutOffDate.Date
                                                   && x.IsActive == true && x.IsDeleted == false).ToList();

            var preTackStall = _ObjContext.StallAssignment.Where(x => x.ExhibitorId == registrationReportRequest.ExhibitorId && x.StallAssignmentTypeId == tackStallTypeId
                                                       && x.Date.Date < yearlyMainId.PreEntryCutOffDate.Date
                                                      && x.IsActive == true && x.IsDeleted == false).ToList();


            var postHorseStall = _ObjContext.StallAssignment.Where(x => x.ExhibitorId == registrationReportRequest.ExhibitorId && x.StallAssignmentTypeId == horseStallTypeId &&
                                                                x.Date.Date > yearlyMainId.PreEntryCutOffDate.Date
                                                      && x.IsActive == true && x.IsDeleted == false).ToList();


            var postTackStall = _ObjContext.StallAssignment.Where(x => x.ExhibitorId == registrationReportRequest.ExhibitorId && x.StallAssignmentTypeId == tackStallTypeId
                                                        && x.Date.Date > yearlyMainId.PreEntryCutOffDate.Date
                                                       && x.IsActive == true && x.IsDeleted == false).ToList();

            var feeCodes = (from gcc in _ObjContext.GlobalCodeCategories
                            join gc in _ObjContext.GlobalCodes on gcc.GlobalCodeCategoryId equals gc.CategoryId
                            where gcc.CategoryName == "FeeType" && gc.IsDeleted == false && gc.IsActive == true
                            select new
                            {
                                gc.GlobalCodeId,
                                gc.CodeName

                            }).ToList();

            int horseStallFeeId = feeCodes.Where(x => x.CodeName == "Stall").Select(x => x.GlobalCodeId).FirstOrDefault();
            int tackStallFeeId = feeCodes.Where(x => x.CodeName == "Tack").Select(x => x.GlobalCodeId).FirstOrDefault();

            var horseStallFee = _ObjContext.YearlyMaintainenceFee.Where(x => x.FeeTypeId == horseStallFeeId && x.YearlyMaintainenceId == yearlyMainId.YearlyMaintainenceId).FirstOrDefault();


            var tackStallFee = _ObjContext.YearlyMaintainenceFee.Where(x => x.FeeTypeId == tackStallFeeId && x.YearlyMaintainenceId == yearlyMainId.YearlyMaintainenceId).FirstOrDefault();
            decimal preHorseStallAmount = 0;
            if (horseStallFee != null)
            {
                preHorseStallAmount = horseStallFee.PreEntryFee * preHorseStall.Count();
            }

            decimal preTackStallAmount = 0;

            if (tackStallFee != null)
            {
                preTackStallAmount = tackStallFee.PreEntryFee * preTackStall.Count();
            }

            decimal postHorseStallAmount = 0;
            if (horseStallFee != null)
            {
                postHorseStallAmount = horseStallFee.PostEntryFee * postHorseStall.Count();
            }


            decimal postTackStallAmount = 0;
            if (tackStallFee != null)
            {
                postTackStallAmount = tackStallFee.PostEntryFee * postTackStall.Count();
            }

            decimal horseStallAmount = preHorseStallAmount + postHorseStallAmount;
            decimal tackStallAmount = preTackStallAmount + postTackStallAmount;

            int horseStall = preHorseStall.Count() + postHorseStall.Count();
            int tackStall = preTackStall.Count() + postTackStall.Count();

            FinancialsDetail financialsDetail = new FinancialsDetail();

            financialsDetail.HorseStallQty = horseStall;
            financialsDetail.HorseStallAmount = horseStallAmount;

            financialsDetail.TackStallQty = tackStall;
            financialsDetail.TackStallAmount = tackStallAmount;
            financialsDetail.AmountDue = horseStallAmount + tackStallAmount;
            financialsDetail.ReceivedAmount= _ObjContext.ExhibitorPaymentDetails.Where(x => x.ExhibitorId ==
            registrationReportRequest.ExhibitorId && x.IsActive == true && x.IsDeleted == false).Select(x => x.AmountPaid).Sum();

            decimal overPayment = (financialsDetail.ReceivedAmount)-(horseStallAmount + tackStallAmount);
            if (overPayment < 0)
            {
                overPayment = 0;
            }
            financialsDetail.Overpayment = overPayment;
            decimal balance = (horseStallAmount + tackStallAmount) - (financialsDetail.ReceivedAmount);
            if (balance < 0)
            {
                balance = 0;
            }
            financialsDetail.BalanceDue = balance;

            getExhibitorRegistrationReport.financialsDetail = financialsDetail;
            return getExhibitorRegistrationReport;
        }
    }
}
