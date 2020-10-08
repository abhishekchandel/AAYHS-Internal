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

        public GetExhibitorRegistrationReport GetExhibitorRegistrationReport(int exhibitorId)
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
                                         where exhibtor.ExhibitorId == exhibitorId
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
                                   where horseExhibitor.ExhibitorId== exhibitorId
                                   && horseExhibitor.IsDeleted==false
                                    select new HorseDetail 
                                    { 
                                      HorseName=horse.Name,
                                      BackNumber= horseExhibitor.BackNumber,
                                      ExhibitorId= exhibitorId,
                                      NSBAIndicator=horse.NSBAIndicator
                                    }).ToList()
                    }) ;

            getExhibitorRegistrationReport = data.FirstOrDefault();

            var stallCodes = (from gcc in _ObjContext.GlobalCodeCategories
                              join gc in _ObjContext.GlobalCodes on gcc.GlobalCodeCategoryId equals gc.CategoryId
                              where gcc.CategoryName == "StallType" && gc.IsDeleted == false && gc.IsActive == true
                              select new
                              {
                                  gc.GlobalCodeId,
                                  gc.CodeName,
                                  gc.IsDeleted

                              }).ToList();
            int horseStallTypeId = stallCodes.Where(x => x.CodeName == "HorseStall"&& x.IsDeleted==false).Select(x => x.GlobalCodeId).FirstOrDefault();
            int tackStallTypeId = stallCodes.Where(x => x.CodeName == "TackStall" && x.IsDeleted == false).Select(x => x.GlobalCodeId).FirstOrDefault();

            var preHorseStall = _ObjContext.StallAssignment.Where(x => x.ExhibitorId == exhibitorId && x.StallAssignmentTypeId == horseStallTypeId &&
                                                     x.Date.Date < yearlyMainId.PreEntryCutOffDate.Date
                                                   && x.IsActive == true && x.IsDeleted == false).ToList();

            var preTackStall = _ObjContext.StallAssignment.Where(x => x.ExhibitorId == exhibitorId && x.StallAssignmentTypeId == tackStallTypeId
                                                       && x.Date.Date < yearlyMainId.PreEntryCutOffDate.Date
                                                      && x.IsActive == true && x.IsDeleted == false).ToList();

            var preClasses = _ObjContext.ExhibitorClass.Where(x => x.ExhibitorId == exhibitorId
                                                     && x.Date.Date < yearlyMainId.PreEntryCutOffDate.Date
                                                    && x.IsActive == true && x.IsDeleted == false).ToList();

            var postHorseStall = _ObjContext.StallAssignment.Where(x => x.ExhibitorId == exhibitorId && x.StallAssignmentTypeId == horseStallTypeId &&
                                                                x.Date.Date > yearlyMainId.PreEntryCutOffDate.Date
                                                      && x.IsActive == true && x.IsDeleted == false).ToList();


            var postTackStall = _ObjContext.StallAssignment.Where(x => x.ExhibitorId == exhibitorId && x.StallAssignmentTypeId == tackStallTypeId
                                                        && x.Date.Date > yearlyMainId.PreEntryCutOffDate.Date
                                                       && x.IsActive == true && x.IsDeleted == false).ToList();

            var postClasses = _ObjContext.ExhibitorClass.Where(x => x.ExhibitorId == exhibitorId
                                                        && x.Date.Date > yearlyMainId.PreEntryCutOffDate.Date
                                                       && x.IsActive == true && x.IsDeleted == false).ToList();

            var feeCodes = (from gcc in _ObjContext.GlobalCodeCategories
                            join gc in _ObjContext.GlobalCodes on gcc.GlobalCodeCategoryId equals gc.CategoryId
                            where gcc.CategoryName == "FeeType" && gc.IsDeleted == false && gc.IsActive == true
                            select new
                            {
                                gc.GlobalCodeId,
                                gc.CodeName,
                                gc.IsDeleted

                            }).ToList();

            int horseStallFeeId = feeCodes.Where(x => x.CodeName == "Stall" && x.IsDeleted == false).Select(x => x.GlobalCodeId).FirstOrDefault();
            int tackStallFeeId = feeCodes.Where(x => x.CodeName == "Tack" && x.IsDeleted == false).Select(x => x.GlobalCodeId).FirstOrDefault();
            int classEntryId = feeCodes.Where(x => x.CodeName == "Class Entry" && x.IsDeleted == false).Select(x => x.GlobalCodeId).FirstOrDefault();


            var horseStallFee = _ObjContext.YearlyMaintainenceFee.Where(x => x.FeeTypeId == horseStallFeeId && x.YearlyMaintainenceId == yearlyMainId.YearlyMaintainenceId).FirstOrDefault();

            var tackStallFee = _ObjContext.YearlyMaintainenceFee.Where(x => x.FeeTypeId == tackStallFeeId && x.YearlyMaintainenceId == yearlyMainId.YearlyMaintainenceId).FirstOrDefault();

            var classEntryFee = _ObjContext.YearlyMaintainenceFee.Where(x => x.FeeTypeId == classEntryId && x.YearlyMaintainenceId == yearlyMainId.YearlyMaintainenceId).FirstOrDefault();

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
            exhibitorId && x.IsActive == true && x.IsDeleted == false).Select(x => x.AmountPaid).Sum();

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

        public GetProgramReport GetProgramsReport(int classId)
        {
            IEnumerable<GetProgramReport> data;
            GetProgramReport getProgramReport = new GetProgramReport();

            data = (from classes in _ObjContext.Classes
                    where classes.IsActive == true && classes.IsDeleted == false
                    && classes.ClassId == classId
                    select new GetProgramReport
                    {
                        ClassNumber = classes.ClassNumber,
                        ClassName = classes.Name,
                        Age = classes.AgeGroup,

                        sponsorInfo = (from sponsorsClass in _ObjContext.ClassSponsor
                                       join sponsor in _ObjContext.Sponsors on sponsorsClass.SponsorId equals sponsor.SponsorId
                                       join address in _ObjContext.Addresses on sponsor.AddressId equals address.AddressId
                                       join city in _ObjContext.Cities on address.CityId equals city.CityId
                                       join state in _ObjContext.States on city.StateId equals state.StateId
                                       join zipcode in _ObjContext.ZipCodes2 on address.ZipCodeId equals zipcode.ZipCodeId
                                       where sponsorsClass.IsActive == true && sponsorsClass.IsDeleted == false
                                       && sponsor.IsActive == true && sponsor.IsDeleted == false
                                       && sponsorsClass.ClassId == classId
                                       orderby sponsorsClass.CreatedDate
                                       select new SponsorInfo
                                       {
                                           SponsorName = sponsor.SponsorName,
                                           City = city.Name,
                                           StateZipcode = state.Code + ", " + zipcode.ZipCode

                                       }).Take(4).ToList(),

                        classInfo = (from exhibitorClass in _ObjContext.ExhibitorClass
                                     join horse in _ObjContext.Horses on exhibitorClass.HorseId equals horse.HorseId
                                     join exhibitor in _ObjContext.Exhibitors on exhibitorClass.ExhibitorId equals exhibitor.ExhibitorId
                                     join address in _ObjContext.Addresses on exhibitor.AddressId equals address.AddressId
                                     join city in _ObjContext.Cities on address.CityId equals city.CityId
                                     join state in _ObjContext.States on city.StateId equals state.StateId
                                     join zipcode in _ObjContext.ZipCodes2 on address.ZipCodeId equals zipcode.ZipCodeId
                                     where exhibitorClass.IsActive == true && exhibitorClass.IsDeleted == false
                                     && horse.IsDeleted == false && exhibitor.IsDeleted == false
                                     && exhibitorClass.ClassId==classId
                                     select new ClassInfo 
                                     { 
                                       BackNumber= exhibitor.BackNumber,
                                       HorseName=horse.Name,
                                       ExhibitorName=exhibitor.FirstName+" "+exhibitor.LastName,
                                       City=city.Name,
                                       StateZipcode=state.Code+", "+zipcode.ZipCode
                                     }).ToList()

                    }) ;

            getProgramReport = data.FirstOrDefault();
            return getProgramReport;
        }

        public GetPaddockReport GetPaddockReport(int classId)
        {
            IEnumerable<GetPaddockReport> data;
            GetPaddockReport getPaddockReport = new GetPaddockReport();

            data = (from classes in _ObjContext.Classes
                    where classes.IsActive == true && classes.IsDeleted == false
                    && classes.ClassId == classId
                    select new GetPaddockReport
                    { 
                      ClassNumber=classes.ClassNumber,
                      ClassName=classes.Name,
                      Age=classes.AgeGroup,

                      classDetails=(from exhibitorClass in _ObjContext.ExhibitorClass
                                     join horse in _ObjContext.Horses on exhibitorClass.HorseId equals horse.HorseId
                                     join exhibitor in _ObjContext.Exhibitors on exhibitorClass.ExhibitorId equals exhibitor.ExhibitorId
                                     join address in _ObjContext.Addresses on exhibitor.AddressId equals address.AddressId
                                     join city in _ObjContext.Cities on address.CityId equals city.CityId
                                     join state in _ObjContext.States on city.StateId equals state.StateId
                                     join zipcode in _ObjContext.ZipCodes2 on address.ZipCodeId equals zipcode.ZipCodeId
                                     where exhibitorClass.IsActive == true && exhibitorClass.IsDeleted == false
                                     && horse.IsDeleted == false && exhibitor.IsDeleted == false
                                     && exhibitorClass.ClassId == classId
                                    select new ClassDetail 
                                    { 
                                      BackNumber=exhibitor.BackNumber,
                                      Scratch=exhibitorClass.IsScratch,
                                      NSBA=exhibitor.IsNSBAMember,
                                      HorseName=horse.Name,
                                      ExhibitorName=exhibitor.FirstName+" "+exhibitor.LastName,
                                      City=city.Name,
                                      StateZipcode=state.Code+", "+zipcode.ZipCode,
                                      Split=_ObjContext.ClassSplits.Where(x=>x.ClassId==exhibitorClass.ClassId).Select(x=>x.SplitNumber).FirstOrDefault()
                                    }).ToList()
                    });

            getPaddockReport = data.FirstOrDefault();
            return getPaddockReport;
        }
    }
}
