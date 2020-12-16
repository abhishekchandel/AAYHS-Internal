using AAYHS.Core.DTOs.Request;
using AAYHS.Core.DTOs.Response;
using AAYHS.Data.DBContext;
using AAYHS.Repository.IRepository;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using AAYHS.Data.DBEntities;
using System.Globalization;
using System.Text.RegularExpressions;
using AAYHS.Core.DTOs.Response.Common;

namespace AAYHS.Repository.Repository
{
    public class ReportRepository : IReportRepository
    {
        #region readonly
        private readonly AAYHSDBContext _ObjContext;
        private readonly IGlobalCodeRepository _globalCodeRepository;
        private readonly IYearlyMaintenanceRepository _yearlyMaintenanceRepository;
        private IMapper _Mapper;
        #endregion

        #region private 
        private MainResponse _MainResponse;
        #endregion

        public ReportRepository(AAYHSDBContext ObjContext, IGlobalCodeRepository globalCodeRepository,
                                IYearlyMaintenanceRepository yearlyMaintenanceRepository, IMapper Mapper)
        {
            _ObjContext = ObjContext;
            _globalCodeRepository = globalCodeRepository;
            _yearlyMaintenanceRepository = yearlyMaintenanceRepository;
            _Mapper = Mapper;
            _MainResponse = new MainResponse();
        }

        public GetExhibitorRegistrationReport GetExhibitorRegistrationReport(int exhibitorId)
        {
            GetExhibitorRegistrationReport getExhibitorRegistrationReport = new GetExhibitorRegistrationReport();

            var yearlyMainId = _ObjContext.YearlyMaintainence.Where(x => x.Years == DateTime.Now.Year && x.IsActive == true
             && x.IsDeleted == false).FirstOrDefault();


            string showdates = "";
            string startDateFullMonthName = yearlyMainId.ShowStartDate.ToString("MMMM");
            string endDateFullMonthName = yearlyMainId.ShowEndDate.ToString("MMMM");

            showdates = Convert.ToString(startDateFullMonthName + " " + yearlyMainId.ShowStartDate.Day + " - " + endDateFullMonthName + " " +
                yearlyMainId.ShowEndDate.Day + "," + yearlyMainId.Years);


            var stallCodes = (from gcc in _ObjContext.GlobalCodeCategories
                              join gc in _ObjContext.GlobalCodes on gcc.GlobalCodeCategoryId equals gc.CategoryId
                              where gcc.CategoryName == "StallType" && gc.IsDeleted == false && gc.IsActive == true
                              select new
                              {
                                  gc.GlobalCodeId,
                                  gc.CodeName,
                                  gc.IsDeleted

                              }).ToList();
            int horseStallTypeId = stallCodes.Where(x => x.CodeName == "HorseStall" && x.IsDeleted == false).Select(x => x.GlobalCodeId).FirstOrDefault();
            int tackStallTypeId = stallCodes.Where(x => x.CodeName == "TackStall" && x.IsDeleted == false).Select(x => x.GlobalCodeId).FirstOrDefault();


            var data = (from exhibtor in _ObjContext.Exhibitors
                        join address in _ObjContext.Addresses on exhibtor.AddressId equals address.AddressId                     
                        join state in _ObjContext.States on address.StateId equals state.StateId
                        where exhibtor.ExhibitorId == exhibitorId
                        select new ExhibitorInfo
                        {
                            ExhibitorName = exhibtor.FirstName + " " + exhibtor.LastName,
                            Address = address.Address,
                            CityName = address.City,
                            StateZipcode = state.Code + "  " + address.ZipCode,
                            Email = exhibtor.PrimaryEmail,
                            Phone = Regex.Replace(exhibtor.Phone, @"(\d{3})(\d{3})(\d{4})", "($1)-$2-$3"),


                        }).FirstOrDefault();

            if (data != null)
            {
                getExhibitorRegistrationReport.getExhibitorInfo = data;
            }
            else
            {

                getExhibitorRegistrationReport.getExhibitorInfo = new ExhibitorInfo();
            }


            var horseClassDetails = (from classExhibitor in _ObjContext.ExhibitorClass
                                     join clases in _ObjContext.Classes on classExhibitor.ClassId equals clases.ClassId
                                     where classExhibitor.ExhibitorId == exhibitorId
                                     && classExhibitor.IsDeleted == false && clases.IsDeleted == false
                                     select new HorseClassDetail
                                     {
                                         HorseName = _ObjContext.Horses.Where(x => x.HorseId == classExhibitor.HorseId && x.IsDeleted == false).Select(x => x.Name).FirstOrDefault(),
                                         BackNumber = _ObjContext.ExhibitorHorse.Where(x => x.HorseId == classExhibitor.HorseId && x.IsDeleted == false).Select(x => x.BackNumber).FirstOrDefault(),
                                         ClassNumber = clases.ClassNumber,
                                         ClassName = clases.Name
                                     }).ToList();

            getExhibitorRegistrationReport.horseClassDetails = horseClassDetails;

            IEnumerable<GetAAYHSContactInfo> getAAYHSContactInfo;

            getAAYHSContactInfo = (from contactInfo in _ObjContext.AAYHSContact
                                   join address in _ObjContext.AAYHSContactAddresses on contactInfo.ExhibitorConfirmationEntriesAddressId equals
                                   address.AAYHSContactAddressId
                                   join states in _ObjContext.States on address.StateId equals states.StateId
                                   where contactInfo.YearlyMaintainenceId == yearlyMainId.YearlyMaintainenceId
                                   && contactInfo.IsActive == true && contactInfo.IsDeleted == false
                                   select new GetAAYHSContactInfo
                                   {
                                       Email1 = address.Email,
                                       Address = address.Address,
                                       CityName = address.City,
                                       StateZipcode = states.Code + "  " + address.ZipCode,
                                       Phone1 = Regex.Replace(address.Phone, @"(\d{3})(\d{3})(\d{4})", "($1)-$2-$3"),

                                       ShowDates = showdates
                                   });



            getExhibitorRegistrationReport.getAAYHSContactInfo = getAAYHSContactInfo.FirstOrDefault();

            var stallAndTack = (from exhibitor in _ObjContext.Exhibitors
                                where exhibitor.IsDeleted == false
                                && exhibitor.ExhibitorId == exhibitorId
                                select new StallAndTackStallNumber
                                {
                                    ExhibitorId = exhibitorId,
                                    ExhibitorBirthYear = exhibitor.BirthYear,
                                    horseStalls = (from horseStall in _ObjContext.StallAssignment
                                                   where horseStall.IsDeleted == false
                                                   && horseStall.StallAssignmentTypeId == horseStallTypeId
                                                   && horseStall.ExhibitorId == exhibitorId
                                                   select new HorseStall
                                                   {
                                                       HorseStallNumber = horseStall != null ? horseStall.StallId : 0
                                                   }).ToList(),
                                    tackStalls = (from tackStall in _ObjContext.StallAssignment
                                                  where tackStall.IsDeleted == false
                                                  && tackStall.StallAssignmentTypeId == tackStallTypeId
                                                  && tackStall.ExhibitorId == exhibitorId
                                                  select new TackStall
                                                  {
                                                      TackStallNumber = tackStall != null ? tackStall.StallId : 0
                                                  }).ToList()

                                });
            StallAndTackStallNumber stallAndTackStallNumber = new StallAndTackStallNumber();
            stallAndTackStallNumber = stallAndTack.FirstOrDefault();
            getExhibitorRegistrationReport.stallAndTackStallNumber = stallAndTackStallNumber;

            var preHorseStall = _ObjContext.StallAssignment.Where(x => x.ExhibitorId == exhibitorId && x.StallAssignmentTypeId == horseStallTypeId &&
                                                     x.Date.Date <= yearlyMainId.PreEntryCutOffDate.Date
                                                   && x.IsActive == true && x.IsDeleted == false).ToList();

            var preTackStall = _ObjContext.StallAssignment.Where(x => x.ExhibitorId == exhibitorId && x.StallAssignmentTypeId == tackStallTypeId
                                                       && x.Date.Date <= yearlyMainId.PreEntryCutOffDate.Date
                                                      && x.IsActive == true && x.IsDeleted == false).ToList();

            var preClasses = _ObjContext.ExhibitorClass.Where(x => x.ExhibitorId == exhibitorId
                                                     && x.Date.Date <= yearlyMainId.PreEntryCutOffDate.Date
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


            var allFees = _ObjContext.YearlyMaintainenceFee.Where(x => x.FeeType == "GeneralFee" && x.IsDeleted == false).ToList();

            var horseStallFee = allFees.Where(x => x.FeeName == "Stall"
                                 && x.YearlyMaintainenceId == yearlyMainId.YearlyMaintainenceId && x.IsDeleted == false).ToList();


            var tackStallFee = allFees.Where(x => x.FeeName == "Tack" &&
                                 x.YearlyMaintainenceId == yearlyMainId.YearlyMaintainenceId && x.IsDeleted == false).ToList();

            var classEntryFee = allFees.Where(x => x.FeeName == "Class Entry"
                                && x.YearlyMaintainenceId == yearlyMainId.YearlyMaintainenceId && x.IsDeleted == false).ToList();

            decimal preHorseStallAmount = 0;

            if (horseStallFee != null)
            {
                decimal preHoseStallFee = horseStallFee.Where(x => x.TimeFrame == "Pre").Select(x => x.Amount).FirstOrDefault();
                preHorseStallAmount = preHoseStallFee * preHorseStall.Count();
            }

            decimal preTackStallAmount = 0;

            if (tackStallFee != null)
            {
                decimal preTackStallFee = tackStallFee.Where(x => x.TimeFrame == "Pre").Select(x => x.Amount).FirstOrDefault();
                preTackStallAmount = preTackStallFee * preTackStall.Count();
            }

            decimal preClassAmount = 0;

            if (classEntryFee != null)
            {
                decimal preClassStallFee = classEntryFee.Where(x => x.TimeFrame == "Pre").Select(x => x.Amount).FirstOrDefault();
                preClassAmount = preClassStallFee * preClasses.Count();
            }

            decimal postHorseStallAmount = 0;
            if (horseStallFee != null)
            {
                decimal postHoseStallFee = horseStallFee.Where(x => x.TimeFrame == "Post").Select(x => x.Amount).FirstOrDefault();
                postHorseStallAmount = postHoseStallFee * postHorseStall.Count();
            }


            decimal postTackStallAmount = 0;
            if (tackStallFee != null)
            {
                decimal postTackStallFee = tackStallFee.Where(x => x.TimeFrame == "Post").Select(x => x.Amount).FirstOrDefault();
                postTackStallAmount = postTackStallFee * postTackStall.Count();
            }

            decimal postClassAmount = 0;
            if (classEntryFee != null)
            {
                decimal postClassStallFee = classEntryFee.Where(x => x.TimeFrame == "Post").Select(x => x.Amount).FirstOrDefault();
                postClassAmount = postClassStallFee * postClasses.Count();
            }



            decimal horseStallAmount = preHorseStallAmount + postHorseStallAmount;
            decimal tackStallAmount = preTackStallAmount + postTackStallAmount;
            decimal classAmount = preClassAmount + postClassAmount;

            int horseStall = preHorseStall.Count() + postHorseStall.Count();
            int tackStall = preTackStall.Count() + postTackStall.Count();
            int classes = preClasses.Count + postClasses.Count();

            FinancialsDetail financialsDetail = new FinancialsDetail();

            financialsDetail.HorseStallQty = horseStall;
            financialsDetail.HorseStallAmount = horseStallAmount;

            financialsDetail.TackStallQty = tackStall;
            financialsDetail.TackStallAmount = tackStallAmount;

            financialsDetail.ClassQty = classes;
            financialsDetail.ClassAmount = classAmount;

           

            var administrationFee = allFees.Where(x => x.FeeName == "Administration"
                                  && x.YearlyMaintainenceId == yearlyMainId.YearlyMaintainenceId && x.IsDeleted == false).FirstOrDefault();
            decimal administrationAmount = 0;
            int numberOfHorses = _ObjContext.ExhibitorHorse.Where(x => x.ExhibitorId == exhibitorId && x.IsDeleted == false).Count();
            if (administrationFee != null)
            {
                administrationAmount = numberOfHorses * administrationFee.Amount;
            }

            financialsDetail.AdministrationQty = numberOfHorses;
            financialsDetail.AdministrationAmount = administrationAmount;

            var additionalProgramsfee = allFees.Where(x => x.FeeName == "Additional Programs"
                                 && x.YearlyMaintainenceId == yearlyMainId.YearlyMaintainenceId && x.IsDeleted == false).FirstOrDefault();
            var additionalPrograme = _ObjContext.Exhibitors.Where(x => x.ExhibitorId == exhibitorId && x.IsActive == true && x.IsDeleted == false
                                                   ).Select(x => x.QTYProgram).FirstOrDefault();
            decimal AdditionalProgramsAmount = 0;
            if (additionalProgramsfee != null && additionalPrograme != null)
            {
                AdditionalProgramsAmount = additionalProgramsfee.Amount * additionalPrograme;
            }

            financialsDetail.AdditionalProgramQty = additionalPrograme;
            financialsDetail.AdditionalProgramAmount = AdditionalProgramsAmount;

            financialsDetail.AmountDue = horseStallAmount + tackStallAmount + classAmount + administrationAmount + AdditionalProgramsAmount;

            var selectedexhibitor = (from exibr in _ObjContext.Exhibitors
                                     where exibr.ExhibitorId == exhibitorId && exibr.IsActive == true
                                     select exibr).FirstOrDefault();

            if (selectedexhibitor != null && selectedexhibitor.IsNSBAMember == true)
            {

                var allNSBSExhibitorClasses = (from exhibitoClasses in _ObjContext.ExhibitorClass
                                               join cls in _ObjContext.Classes on exhibitoClasses.ClassId equals cls.ClassId
                                               where exhibitoClasses.ExhibitorId == exhibitorId
                                               && cls.IsNSBAMember == true
                                               && exhibitoClasses.IsDeleted == false
                                               && cls.IsDeleted == false
                                               select new { exhibitoClasses.ClassId, exhibitoClasses.Date }).Distinct().ToList();

                var preNSBAClasses = allNSBSExhibitorClasses.Where(x => x.Date.Date <= yearlyMainId.PreEntryCutOffDate.Date).ToList();

                var postNSBAClasses = allNSBSExhibitorClasses.Where(x => x.Date.Date > yearlyMainId.PreEntryCutOffDate.Date).ToList();

                var allNSBAfee = allFees.Where(x => x.FeeName == "NSBA Entry"
                                     && x.YearlyMaintainenceId == yearlyMainId.YearlyMaintainenceId && x.IsDeleted == false).ToList();

                var preNSBAfee = allNSBAfee.Where(x => x.TimeFrame == "Pre").FirstOrDefault();
                var postNSBAfee = allNSBAfee.Where(x => x.TimeFrame == "Post").FirstOrDefault();
                decimal preNSBAamount = 0;
                decimal PostNSBAamount = 0;

                if (preNSBAfee != null)
                {
                    preNSBAamount = Convert.ToDecimal(preNSBAClasses.Count() * preNSBAfee.Amount);
                }
                if (postNSBAfee != null)
                {
                    PostNSBAamount = Convert.ToDecimal(postNSBAClasses.Count() * postNSBAfee.Amount);
                }
                var totalNSBAamount = preNSBAamount + PostNSBAamount;
                var totalNSBAClasses = allNSBSExhibitorClasses.Count();

                financialsDetail.NSBAClassQty = totalNSBAClasses;
                financialsDetail.NSBAClassAmount = totalNSBAamount;

                financialsDetail.AmountDue = financialsDetail.AmountDue + totalNSBAamount;

            }


            int prehorseStallFeeId = horseStallFee.Where(x => x.TimeFrame == "Pre" && x.IsDeleted == false).Select(x => x.YearlyMaintainenceFeeId).FirstOrDefault();
            int posthorseStallFeeId = horseStallFee.Where(x => x.TimeFrame == "Post" && x.IsDeleted == false).Select(x => x.YearlyMaintainenceFeeId).FirstOrDefault();
            int pretackStallFeeId = tackStallFee.Where(x => x.TimeFrame == "Pre" && x.IsDeleted == false).Select(x => x.YearlyMaintainenceFeeId).FirstOrDefault();
            int posttackStallFeeId = tackStallFee.Where(x => x.TimeFrame == "Post" && x.IsDeleted == false).Select(x => x.YearlyMaintainenceFeeId).FirstOrDefault();
            int preclassStallFeeId = classEntryFee.Where(x => x.TimeFrame == "Pre" && x.IsDeleted == false).Select(x => x.YearlyMaintainenceFeeId).FirstOrDefault();
            int postclassStallFeeId = classEntryFee.Where(x => x.TimeFrame == "Post" && x.IsDeleted == false).Select(x => x.YearlyMaintainenceFeeId).FirstOrDefault();

            int preAdministrationFeeId = allFees.Where(x => x.FeeName == "Administration" && x.YearlyMaintainenceId == yearlyMainId.YearlyMaintainenceId
            && x.TimeFrame == "Pre" && x.IsDeleted == false).Select(x => x.YearlyMaintainenceFeeId).FirstOrDefault();

            int postAdministrationFeeId = allFees.Where(x => x.FeeName == "Administration" && x.YearlyMaintainenceId == yearlyMainId.YearlyMaintainenceId 
            && x.TimeFrame == "Post" && x.IsDeleted == false).Select(x => x.YearlyMaintainenceFeeId).FirstOrDefault();
           
            int additionalProgrameFeeId = allFees.Where(x => x.FeeName == "Additional Programs" && x.YearlyMaintainenceId == yearlyMainId.YearlyMaintainenceId && x.IsDeleted == false).Select(x => x.YearlyMaintainenceFeeId).FirstOrDefault();

            int preNSBAFeeId = allFees.Where(x => x.FeeName == "NSBA Entry" && x.YearlyMaintainenceId == yearlyMainId.YearlyMaintainenceId 
            && x.TimeFrame == "Pre" && x.IsDeleted == false).Select(x => x.YearlyMaintainenceFeeId).FirstOrDefault();

            int postNSBAFeeId = allFees.Where(x => x.FeeName == "NSBA Entry" && x.YearlyMaintainenceId == yearlyMainId.YearlyMaintainenceId 
            && x.TimeFrame == "Post" && x.IsDeleted == false).Select(x => x.YearlyMaintainenceFeeId).FirstOrDefault();


            financialsDetail.ReceivedAmount = _ObjContext.ExhibitorPaymentDetails.Where(x => x.ExhibitorId ==
             exhibitorId && x.IsActive == true &&
               (x.FeeTypeId == prehorseStallFeeId
             || x.FeeTypeId == posthorseStallFeeId

             || x.FeeTypeId == pretackStallFeeId
             || x.FeeTypeId == posttackStallFeeId

             || x.FeeTypeId == preclassStallFeeId
             || x.FeeTypeId == postclassStallFeeId

             || x.FeeTypeId == preAdministrationFeeId
             || x.FeeTypeId == postAdministrationFeeId

             || x.FeeTypeId == additionalProgrameFeeId

              || x.FeeTypeId == preNSBAFeeId
             || x.FeeTypeId == postNSBAFeeId
             )
             && x.IsDeleted == false).Select(x => x.AmountPaid).Sum();

            decimal overPayment = (financialsDetail.ReceivedAmount) - (horseStallAmount 
                + tackStallAmount 
                + classAmount
                + administrationAmount
                + AdditionalProgramsAmount
                + financialsDetail.NSBAClassAmount);

            if (overPayment < 0)
            {
                overPayment = 0;
            }
            financialsDetail.Overpayment = overPayment;
            decimal balance = (horseStallAmount 
                + tackStallAmount 
                + classAmount 
                + administrationAmount 
                + AdditionalProgramsAmount
                + financialsDetail.NSBAClassAmount) - (financialsDetail.ReceivedAmount);
            if (balance < 0)
            {
                balance = 0;
            }
            financialsDetail.BalanceDue = balance;

            getExhibitorRegistrationReport.financialsDetail = financialsDetail;
            return getExhibitorRegistrationReport;
        }

        public GetExhibitorGroupInformationReport GetExhibitorGroupInformationReport(int groupId)
        {
           
            GetExhibitorGroupInformationReport getExhibitorGroupInformationReport = new GetExhibitorGroupInformationReport();

            var yearlyMainId = _ObjContext.YearlyMaintainence.Where(x => x.Years == DateTime.Now.Year && x.IsActive == true
             && x.IsDeleted == false).FirstOrDefault();

            var stallCodes = (from gcc in _ObjContext.GlobalCodeCategories
                              join gc in _ObjContext.GlobalCodes on gcc.GlobalCodeCategoryId equals gc.CategoryId
                              where gcc.CategoryName == "StallType" && gc.IsDeleted == false && gc.IsActive == true
                              select new
                              {
                                  gc.GlobalCodeId,
                                  gc.CodeName,
                                  gc.IsDeleted

                              }).ToList();
            int horseStallTypeId = stallCodes.Where(x => x.CodeName == "HorseStall" && x.IsDeleted == false).Select(x => x.GlobalCodeId).FirstOrDefault();
            int tackStallTypeId = stallCodes.Where(x => x.CodeName == "TackStall" && x.IsDeleted == false).Select(x => x.GlobalCodeId).FirstOrDefault();


           var data = (from groups in _ObjContext.Groups
                    join address in _ObjContext.Addresses on groups.AddressId equals address.AddressId     
                    join state in _ObjContext.States on address.StateId equals state.StateId
                    where groups.GroupId == groupId
                    select new GroupInfo
                    {
                        GroupName = groups.GroupName,
                        Address = address.Address,
                        CityName = address.City,
                        StateZipcode = state.Code + "  " + address.ZipCode,
                        Email = groups.Email,
                        Phone = Regex.Replace(groups.Phone, @"(\d{3})(\d{3})(\d{4})", "($1)-$2-$3"),
                    
                    }).FirstOrDefault();

            if (data != null)
            {
                getExhibitorGroupInformationReport.getGroupInfo = data;
            }
            else
            {
                getExhibitorGroupInformationReport.getGroupInfo = new GroupInfo();
            }
         

            var exhibitorDetails = (from groupExhibitors in _ObjContext.GroupExhibitors
                                    join exhibitors in _ObjContext.Exhibitors 
                                    on groupExhibitors.ExhibitorId equals exhibitors.ExhibitorId
                                    where groupExhibitors.GroupId == groupId &&
                                    groupExhibitors.IsDeleted == false && exhibitors.IsDeleted == false
                                    select new GetGroupExhibitors
                                    {
                                        GroupExhibitorId = groupExhibitors.GroupExhibitorId,
                                        ExhibitorId = groupExhibitors.ExhibitorId,
                                        ExhibitorName = exhibitors.FirstName + " " + exhibitors.LastName,
                                        BirthYear = exhibitors.BirthYear,

                                    }).ToList();

          
                if (exhibitorDetails != null && exhibitorDetails.Count > 0)
            {

                getExhibitorGroupInformationReport.exhibitorDetails = exhibitorDetails;
            }
            else {
                List<GetGroupExhibitors> exhibitorlist = new List<GetGroupExhibitors>();
                getExhibitorGroupInformationReport.exhibitorDetails= exhibitorlist;
            }


            GetAAYHSContactInfo getAAYHSContactInfo = new GetAAYHSContactInfo();

            getAAYHSContactInfo = (from contactInfo in _ObjContext.AAYHSContact
                                   join address in _ObjContext.AAYHSContactAddresses on contactInfo.ExhibitorConfirmationEntriesAddressId equals
                                   address.AAYHSContactAddressId
                                   join states in _ObjContext.States on address.StateId equals states.StateId
                                   where contactInfo.YearlyMaintainenceId == yearlyMainId.YearlyMaintainenceId
                                   && contactInfo.IsActive == true && contactInfo.IsDeleted == false
                                   select new GetAAYHSContactInfo
                                   {
                                       Email1 = address.Email,
                                       Address = address.Address,
                                       CityName = address.City,
                                       StateZipcode = states.Code + "  " + address.ZipCode,
                                       Phone1 = Regex.Replace(address.Phone, @"(\d{3})(\d{3})(\d{4})", "($1)-$2-$3"),
                                      
                                   }).FirstOrDefault();

            if(getAAYHSContactInfo!=null)
            {
            getExhibitorGroupInformationReport.getAAYHSContactInfo = getAAYHSContactInfo;
            }
            else
            {
                getExhibitorGroupInformationReport.getAAYHSContactInfo = new GetAAYHSContactInfo();
            }


            var stallAndTack = (from groups in _ObjContext.Groups
                                where groups.IsDeleted == false
                                && groups.GroupId == groupId
                                select new StallAndTackStallNumber
                                {
                                  
                                    horseStalls = (from horseStall in _ObjContext.StallAssignment
                                                   where horseStall.IsDeleted == false
                                                   && horseStall.StallAssignmentTypeId == horseStallTypeId
                                                   && horseStall.GroupId == groupId
                                                   select new HorseStall
                                                   {
                                                       HorseStallNumber = horseStall != null ? horseStall.StallId : 0
                                                   }).ToList(),

                                    tackStalls = (from tackStall in _ObjContext.StallAssignment
                                                  where tackStall.IsDeleted == false
                                                  && tackStall.StallAssignmentTypeId == tackStallTypeId
                                                  && tackStall.GroupId == groupId
                                                  select new TackStall
                                                  {
                                                      TackStallNumber = tackStall != null ? tackStall.StallId : 0
                                                  }).ToList()

                                });
            StallAndTackStallNumber stallAndTackStallNumber = new StallAndTackStallNumber();
            if (stallAndTack.FirstOrDefault() != null)
            {
            stallAndTackStallNumber = stallAndTack.FirstOrDefault();
            }
            getExhibitorGroupInformationReport.stallAndTackStallNumber = stallAndTackStallNumber;


            var preHorseStall = _ObjContext.StallAssignment.Where(x => x.GroupId == groupId && x.StallAssignmentTypeId == horseStallTypeId &&
                                                     x.Date.Date <= yearlyMainId.PreEntryCutOffDate.Date
                                                   && x.IsActive == true && x.IsDeleted == false).ToList();

            var preTackStall = _ObjContext.StallAssignment.Where(x => x.GroupId == groupId && x.StallAssignmentTypeId == tackStallTypeId
                                                       && x.Date.Date <= yearlyMainId.PreEntryCutOffDate.Date
                                                      && x.IsActive == true && x.IsDeleted == false).ToList();

            var postHorseStall = _ObjContext.StallAssignment.Where(x => x.GroupId == groupId && x.StallAssignmentTypeId == horseStallTypeId &&
                                                                x.Date.Date > yearlyMainId.PreEntryCutOffDate.Date
                                                      && x.IsActive == true && x.IsDeleted == false).ToList();


            var postTackStall = _ObjContext.StallAssignment.Where(x => x.GroupId == groupId && x.StallAssignmentTypeId == tackStallTypeId
                                                        && x.Date.Date > yearlyMainId.PreEntryCutOffDate.Date
                                                       && x.IsActive == true && x.IsDeleted == false).ToList();

            
            var allFees = _ObjContext.YearlyMaintainenceFee.Where(x => x.FeeType == "GeneralFee" && x.IsDeleted == false).ToList();

            var horseStallFee = allFees.Where(x => x.FeeName == "Stall"
                                 && x.YearlyMaintainenceId == yearlyMainId.YearlyMaintainenceId && x.IsDeleted == false).ToList();


            var tackStallFee = allFees.Where(x => x.FeeName == "Tack" &&
                                 x.YearlyMaintainenceId == yearlyMainId.YearlyMaintainenceId && x.IsDeleted == false).ToList();

            decimal preHorseStallAmount = 0;

            if (horseStallFee != null)
            {
                decimal preHoseStallFee = horseStallFee.Where(x => x.TimeFrame == "Pre").Select(x => x.Amount).FirstOrDefault();
                preHorseStallAmount = preHoseStallFee * preHorseStall.Count();
            }

            decimal preTackStallAmount = 0;

            if (tackStallFee != null)
            {
                decimal preTackStallFee = tackStallFee.Where(x => x.TimeFrame == "Pre").Select(x => x.Amount).FirstOrDefault();
                preTackStallAmount = preTackStallFee * preTackStall.Count();
            }

            decimal postHorseStallAmount = 0;
            if (horseStallFee != null)
            {
                decimal postHoseStallFee = horseStallFee.Where(x => x.TimeFrame == "Post").Select(x => x.Amount).FirstOrDefault();
                postHorseStallAmount = postHoseStallFee * postHorseStall.Count();
            }
            
            decimal postTackStallAmount = 0;
            if (tackStallFee != null)
            {
                decimal postTackStallFee = tackStallFee.Where(x => x.TimeFrame == "Post").Select(x => x.Amount).FirstOrDefault();
                postTackStallAmount = postTackStallFee * postTackStall.Count();
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


            int prehorseStallFeeId = horseStallFee.Where(x => x.TimeFrame == "Pre" && x.IsDeleted == false).Select(x => x.YearlyMaintainenceFeeId).FirstOrDefault();
            int posthorseStallFeeId = horseStallFee.Where(x => x.TimeFrame == "Post" && x.IsDeleted == false).Select(x => x.YearlyMaintainenceFeeId).FirstOrDefault();
            int pretackStallFeeId = tackStallFee.Where(x => x.TimeFrame == "Pre" && x.IsDeleted == false).Select(x => x.YearlyMaintainenceFeeId).FirstOrDefault();
            int posttackStallFeeId = tackStallFee.Where(x => x.TimeFrame == "Post" && x.IsDeleted == false).Select(x => x.YearlyMaintainenceFeeId).FirstOrDefault();

            financialsDetail.ReceivedAmount = Convert.ToDecimal(_ObjContext.GroupFinancials.Where(x => x.GroupId ==
              groupId && x.IsActive == true && 
              (x.FeeTypeId == prehorseStallFeeId
              || x.FeeTypeId == posthorseStallFeeId 
              || x.FeeTypeId == pretackStallFeeId
              || x.FeeTypeId == posttackStallFeeId)

              && x.IsDeleted == false).Select(x => x.Amount).Sum());

            decimal overPayment = (financialsDetail.ReceivedAmount) - (horseStallAmount + tackStallAmount

                );
            if (overPayment < 0)
            {
                overPayment = 0;
            }
            financialsDetail.Overpayment = overPayment;
            decimal balance = (horseStallAmount + tackStallAmount

                ) - (financialsDetail.ReceivedAmount);
            if (balance < 0)
            {
                balance = 0;
            }
            financialsDetail.BalanceDue = balance;

            getExhibitorGroupInformationReport.financialsDetail = financialsDetail;

            return getExhibitorGroupInformationReport;
        }

        public GetExhibitorGroupInformationReportForAllGroups GetExhibitorGroupInformationReportForAllGroups()
        {

            GetExhibitorGroupInformationReportForAllGroups getExhibitorGroupInformationReportForAllGroups = new GetExhibitorGroupInformationReportForAllGroups();

            var yearlyMainId = _ObjContext.YearlyMaintainence.Where(x => x.Years == DateTime.Now.Year && x.IsActive == true
             && x.IsDeleted == false).FirstOrDefault();

            var stallCodes = (from gcc in _ObjContext.GlobalCodeCategories
                              join gc in _ObjContext.GlobalCodes on gcc.GlobalCodeCategoryId equals gc.CategoryId
                              where gcc.CategoryName == "StallType" && gc.IsDeleted == false && gc.IsActive == true
                              select new
                              {
                                  gc.GlobalCodeId,
                                  gc.CodeName,
                                  gc.IsDeleted

                              }).ToList();
            int horseStallTypeId = stallCodes.Where(x => x.CodeName == "HorseStall" && x.IsDeleted == false).Select(x => x.GlobalCodeId).FirstOrDefault();
            int tackStallTypeId = stallCodes.Where(x => x.CodeName == "TackStall" && x.IsDeleted == false).Select(x => x.GlobalCodeId).FirstOrDefault();


            var data = (from groups in _ObjContext.Groups
                        join address in _ObjContext.Addresses on groups.AddressId equals address.AddressId
                        join state in _ObjContext.States on address.StateId equals state.StateId
                        where groups.IsActive==true
                        select new GroupInfo
                        {
                            GroupId= groups.GroupId,
                            GroupName = groups.GroupName,
                            Address = address.Address,
                            CityName = address.City,
                            StateZipcode = state.Code + "  " + address.ZipCode,
                            Email = groups.Email,
                            Phone = Regex.Replace(groups.Phone, @"(\d{3})(\d{3})(\d{4})", "($1)-$2-$3"),
                           

                        }).ToList();

            List<GetExhibitorGroupInformationReport> list = new List<GetExhibitorGroupInformationReport>();

            foreach(var groupitem in data)
            {
                GetExhibitorGroupInformationReport getExhibitorGroupInformationReport = new GetExhibitorGroupInformationReport();
                    getExhibitorGroupInformationReport.getGroupInfo = groupitem;


                var exhibitorDetails = (from groupExhibitors in _ObjContext.GroupExhibitors
                                        join exhibitors in _ObjContext.Exhibitors
                                        on groupExhibitors.ExhibitorId equals exhibitors.ExhibitorId
                                        where groupExhibitors.GroupId == groupitem.GroupId &&
                                        groupExhibitors.IsDeleted == false && exhibitors.IsDeleted == false
                                        select new GetGroupExhibitors
                                        {
                                            GroupExhibitorId = groupExhibitors.GroupExhibitorId,
                                            ExhibitorId = groupExhibitors.ExhibitorId,
                                            ExhibitorName = exhibitors.FirstName + " " + exhibitors.LastName,
                                            BirthYear = exhibitors.BirthYear,

                                        }).ToList();

                if (exhibitorDetails != null && exhibitorDetails.Count > 0)
                {
                    getExhibitorGroupInformationReport.exhibitorDetails = exhibitorDetails;
                }
                else
                {
                    getExhibitorGroupInformationReport.exhibitorDetails = new List<GetGroupExhibitors>();
                }


                var stallAndTack = (from groups in _ObjContext.Groups
                                    where groups.IsDeleted == false
                                    && groups.GroupId == groupitem.GroupId
                                    select new StallAndTackStallNumber
                                    {

                                        horseStalls = (from horseStall in _ObjContext.StallAssignment
                                                       where horseStall.IsDeleted == false
                                                       && horseStall.StallAssignmentTypeId == horseStallTypeId
                                                       && horseStall.GroupId == groupitem.GroupId
                                                       select new HorseStall
                                                       {
                                                           HorseStallNumber = horseStall != null ? horseStall.StallId : 0
                                                       }).ToList(),

                                        tackStalls = (from tackStall in _ObjContext.StallAssignment
                                                      where tackStall.IsDeleted == false
                                                      && tackStall.StallAssignmentTypeId == tackStallTypeId
                                                      && tackStall.GroupId == groupitem.GroupId
                                                      select new TackStall
                                                      {
                                                          TackStallNumber = tackStall != null ? tackStall.StallId : 0
                                                      }).ToList()

                                    });
                StallAndTackStallNumber stallAndTackStallNumber = new StallAndTackStallNumber();
                if (stallAndTack.FirstOrDefault() != null)
                {
                    stallAndTackStallNumber = stallAndTack.FirstOrDefault();
                }
                getExhibitorGroupInformationReport.stallAndTackStallNumber = stallAndTackStallNumber;
                var preHorseStall = _ObjContext.StallAssignment.Where(x => x.GroupId == groupitem.GroupId && x.StallAssignmentTypeId == horseStallTypeId &&
                                                         x.Date.Date <= yearlyMainId.PreEntryCutOffDate.Date
                                                       && x.IsActive == true && x.IsDeleted == false).ToList();
                var preTackStall = _ObjContext.StallAssignment.Where(x => x.GroupId == groupitem.GroupId && x.StallAssignmentTypeId == tackStallTypeId
                                                           && x.Date.Date <= yearlyMainId.PreEntryCutOffDate.Date
                                                          && x.IsActive == true && x.IsDeleted == false).ToList();
                var postHorseStall = _ObjContext.StallAssignment.Where(x => x.GroupId == groupitem.GroupId && x.StallAssignmentTypeId == horseStallTypeId &&
                                                                    x.Date.Date > yearlyMainId.PreEntryCutOffDate.Date
                                                          && x.IsActive == true && x.IsDeleted == false).ToList();
                var postTackStall = _ObjContext.StallAssignment.Where(x => x.GroupId == groupitem.GroupId && x.StallAssignmentTypeId == tackStallTypeId
                                                            && x.Date.Date > yearlyMainId.PreEntryCutOffDate.Date
                                                           && x.IsActive == true && x.IsDeleted == false).ToList();

                var allFees = _ObjContext.YearlyMaintainenceFee.Where(x => x.FeeType == "GeneralFee" && x.IsDeleted == false).ToList();

                var horseStallFee = allFees.Where(x => x.FeeName == "Stall"
                                     && x.YearlyMaintainenceId == yearlyMainId.YearlyMaintainenceId && x.IsDeleted == false).ToList();


                var tackStallFee = allFees.Where(x => x.FeeName == "Tack" &&
                                     x.YearlyMaintainenceId == yearlyMainId.YearlyMaintainenceId && x.IsDeleted == false).ToList();

                decimal preHorseStallAmount = 0;

                if (horseStallFee != null)
                {
                    decimal preHoseStallFee = horseStallFee.Where(x => x.TimeFrame == "Pre").Select(x => x.Amount).FirstOrDefault();
                    preHorseStallAmount = preHoseStallFee * preHorseStall.Count();
                }

                decimal preTackStallAmount = 0;

                if (tackStallFee != null)
                {
                    decimal preTackStallFee = tackStallFee.Where(x => x.TimeFrame == "Pre").Select(x => x.Amount).FirstOrDefault();
                    preTackStallAmount = preTackStallFee * preTackStall.Count();
                }

                decimal postHorseStallAmount = 0;
                if (horseStallFee != null)
                {
                    decimal postHoseStallFee = horseStallFee.Where(x => x.TimeFrame == "Post").Select(x => x.Amount).FirstOrDefault();
                    postHorseStallAmount = postHoseStallFee * postHorseStall.Count();
                }

                decimal postTackStallAmount = 0;
                if (tackStallFee != null)
                {
                    decimal postTackStallFee = tackStallFee.Where(x => x.TimeFrame == "Post").Select(x => x.Amount).FirstOrDefault();
                    postTackStallAmount = postTackStallFee * postTackStall.Count();
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

                int prehorseStallFeeId = horseStallFee.Where(x => x.TimeFrame == "Pre" && x.IsDeleted == false).Select(x => x.YearlyMaintainenceFeeId).FirstOrDefault();
                int posthorseStallFeeId = horseStallFee.Where(x => x.TimeFrame == "Post" && x.IsDeleted == false).Select(x => x.YearlyMaintainenceFeeId).FirstOrDefault();
                int pretackStallFeeId = tackStallFee.Where(x => x.TimeFrame == "Pre" && x.IsDeleted == false).Select(x => x.YearlyMaintainenceFeeId).FirstOrDefault();
                int posttackStallFeeId = tackStallFee.Where(x => x.TimeFrame == "Post" && x.IsDeleted == false).Select(x => x.YearlyMaintainenceFeeId).FirstOrDefault();



                financialsDetail.ReceivedAmount = Convert.ToDecimal(_ObjContext.GroupFinancials.Where(x => x.GroupId ==
                  groupitem.GroupId && x.IsActive == true && 
                  (x.FeeTypeId == prehorseStallFeeId
                  || x.FeeTypeId == posthorseStallFeeId
                  || x.FeeTypeId == pretackStallFeeId
                  || x.FeeTypeId == posttackStallFeeId)
                  && x.IsDeleted == false).Select(x => x.Amount).Sum());

                decimal overPayment = (financialsDetail.ReceivedAmount) - (horseStallAmount + tackStallAmount);
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
                getExhibitorGroupInformationReport.financialsDetail = financialsDetail;
                list.Add(getExhibitorGroupInformationReport);
            }

            GetAAYHSContactInfo getAAYHSContactInfo = new GetAAYHSContactInfo();

            getAAYHSContactInfo = (from contactInfo in _ObjContext.AAYHSContact
                                   join address in _ObjContext.AAYHSContactAddresses on contactInfo.ExhibitorConfirmationEntriesAddressId equals
                                   address.AAYHSContactAddressId
                                   join states in _ObjContext.States on address.StateId equals states.StateId
                                   where contactInfo.YearlyMaintainenceId == yearlyMainId.YearlyMaintainenceId
                                   && contactInfo.IsActive == true && contactInfo.IsDeleted == false
                                   select new GetAAYHSContactInfo
                                   {
                                       Email1 = address.Email,
                                       Address = address.Address,
                                       CityName = address.City,
                                       StateZipcode = states.Code + "  " + address.ZipCode,
                                       Phone1 = Regex.Replace(address.Phone, @"(\d{3})(\d{3})(\d{4})", "($1)-$2-$3"),
                                      
                                   }).FirstOrDefault();

            if (getAAYHSContactInfo != null)
            {
                getExhibitorGroupInformationReportForAllGroups.getAAYHSContactInfo = getAAYHSContactInfo;
            }
            else
            {
                getExhibitorGroupInformationReportForAllGroups.getAAYHSContactInfo = new GetAAYHSContactInfo();
               
            }
            getExhibitorGroupInformationReportForAllGroups.getExhibitorGroupInformationReports = list;
            return getExhibitorGroupInformationReportForAllGroups;
        }
        
        public GetExhibitorSponsorConfirmationReport GetExhibitorSponsorConfirmation(HorseExhibitorsRequest request)
        {

            GetExhibitorSponsorConfirmationReport getExhibitorSponsorConfirmationReport = new GetExhibitorSponsorConfirmationReport();
            var yearlyMainId = _ObjContext.YearlyMaintainence.Where(x => x.Years == DateTime.Now.Year && x.IsActive == true
             && x.IsDeleted == false).FirstOrDefault();

            GetAAYHSContactInfo getAAYHSContactInfo = new GetAAYHSContactInfo();

            getAAYHSContactInfo = (from contactInfo in _ObjContext.AAYHSContact
                                   join address in _ObjContext.AAYHSContactAddresses on contactInfo.ExhibitorSponsorConfirmationAddressId equals
                                   address.AAYHSContactAddressId
                                   join states in _ObjContext.States on address.StateId equals states.StateId
                                   where contactInfo.YearlyMaintainenceId == yearlyMainId.YearlyMaintainenceId
                                   && contactInfo.IsActive == true && contactInfo.IsDeleted == false
                                   select new GetAAYHSContactInfo
                                   {
                                       Email1 = address.Email,
                                       Address = address.Address,
                                       CityName = address.City,
                                       StateZipcode = states.Code + "  " + address.ZipCode,
                                       Phone1 = Regex.Replace(address.Phone, @"(\d{3})(\d{3})(\d{4})", "($1)-$2-$3"),
                                   }).FirstOrDefault();

            if (getAAYHSContactInfo != null)
            {
                getExhibitorSponsorConfirmationReport.getAAYHSContactInfo = getAAYHSContactInfo;
            }
            else
            {
                getExhibitorSponsorConfirmationReport.getAAYHSContactInfo = new GetAAYHSContactInfo(); 
            }

            ExhibitorInfo exhibitorInfo = new ExhibitorInfo();
            exhibitorInfo = (from exhibitor in _ObjContext.Exhibitors
                             join address in _ObjContext.Addresses on exhibitor.AddressId equals address.AddressId
                             join state in _ObjContext.States on address.StateId equals state.StateId
                             where exhibitor.ExhibitorId == request.ExhibitorId && exhibitor.IsActive==true
                             select new ExhibitorInfo
                             {
                                 ExhibitorId = exhibitor.ExhibitorId,
                                 ExhibitorName = exhibitor.FirstName + " " + exhibitor.LastName,
                                 Address = address != null ? address.Address : "",
                                 CityName = address != null ? address.City : "",
                                 StateZipcode = (state != null ? state.Code : "") + "  " + address != null ? address.ZipCode : "",
                                 Email = exhibitor.PrimaryEmail,
                                 Phone = exhibitor.Phone != null ? Regex.Replace(exhibitor.Phone, @"(\d{3})(\d{3})(\d{4})", "($1)-$2-$3") : "",

                             }).FirstOrDefault();
            if(exhibitorInfo!=null)
            {
                getExhibitorSponsorConfirmationReport.exhibitorinfo = exhibitorInfo;
            }
            else
            {
                getExhibitorSponsorConfirmationReport.exhibitorinfo = new ExhibitorInfo();
            }



            List<HorseInfo> horseInfo = new List<HorseInfo>();

            horseInfo = (from exhibitorHorse in _ObjContext.ExhibitorHorse
                         join horse in _ObjContext.Horses on exhibitorHorse.HorseId equals horse.HorseId
                         where exhibitorHorse.IsActive == true 
                         && horse.IsActive == true
                         && exhibitorHorse.ExhibitorId == getExhibitorSponsorConfirmationReport.exhibitorinfo.ExhibitorId
                         select new HorseInfo
                         {
                             ExhibitorHorseId = exhibitorHorse.ExhibitorHorseId,
                             HorseId = exhibitorHorse.HorseId,
                             HorseName = horse.Name,
                             HorseTypeId = horse.HorseTypeId,
                             HorseTypeName = _ObjContext.GlobalCodes.Where(x => x.GlobalCodeId == horse.HorseTypeId).Select(y => y.CodeName).FirstOrDefault(),

                        

                             sponsordetail = (from sponsorExhibitor in _ObjContext.SponsorExhibitor
                                              join sponsor in _ObjContext.Sponsors
                                              on sponsorExhibitor.SponsorId equals sponsor.SponsorId
                                              where sponsorExhibitor.IsActive
                                              && sponsor.IsActive
                                           && sponsorExhibitor.ExhibitorId == request.ExhibitorId
                                           && sponsorExhibitor.HorseId == exhibitorHorse.HorseId
                                              select new Sponsordetail
                                              {
                                                  SponsorName = sponsor.SponsorName,
                                                  SponsorId = sponsorExhibitor.SponsorId,
                                                  Amount = _ObjContext.SponsorExhibitor.Where(x => x.SponsorId == sponsorExhibitor.SponsorId
                                                  && x.HorseId == sponsorExhibitor.HorseId
                                                  && x.ExhibitorId == sponsorExhibitor.ExhibitorId && x.IsActive).Select(x => x.SponsorAmount).Sum()

                                              }).ToList(),




                             SponsorAmountTotal = _ObjContext.SponsorExhibitor.Where(x => x.IsActive == true
                             && x.ExhibitorId == request.ExhibitorId
                             && x.HorseId == exhibitorHorse.HorseId).Select(x => x.SponsorAmount).Sum()

                         }).ToList();

            foreach (var item in horseInfo)
            {
                item.sponsordetail = item.sponsordetail.GroupBy(x => x.SponsorId).Select(y => y.First()).OrderByDescending(x => x.SponsorId).ToList();
            }


                getExhibitorSponsorConfirmationReport.horseinfo = horseInfo;
     
                var ReportText = (from yst in _ObjContext.YearlyStatementText
                        where yst.YearlyMaintenanceId == yearlyMainId.YearlyMaintainenceId && yst.StatementNumber == Convert.ToString(3)
                        select yst.StatementText).FirstOrDefault();

            getExhibitorSponsorConfirmationReport.ReportText = ReportText!=null? ReportText:"";

            return getExhibitorSponsorConfirmationReport;
        }

        public GetExhibitorSponsorConfirmationReportForAllExhibitors GetExhibitorSponsorConfirmationReportForAllExhibitors()
        {
            GetExhibitorSponsorConfirmationReportForAllExhibitors getExhibitorSponsorConfirmationReportForAllExhibitors = new GetExhibitorSponsorConfirmationReportForAllExhibitors();


            var yearlyMainId = _ObjContext.YearlyMaintainence.Where(x => x.Years == DateTime.Now.Year && x.IsActive == true
             && x.IsDeleted == false).FirstOrDefault();

            GetAAYHSContactInfo getAAYHSContactInfo = new GetAAYHSContactInfo();

            getAAYHSContactInfo = (from contactInfo in _ObjContext.AAYHSContact
                                   join address in _ObjContext.AAYHSContactAddresses on contactInfo.ExhibitorSponsorConfirmationAddressId equals
                                   address.AAYHSContactAddressId
                                   join states in _ObjContext.States on address.StateId equals states.StateId
                                   where contactInfo.YearlyMaintainenceId == yearlyMainId.YearlyMaintainenceId
                                   && contactInfo.IsActive == true && contactInfo.IsDeleted == false
                                   select new GetAAYHSContactInfo
                                   {
                                       Email1 = address.Email,
                                       Address = address.Address,
                                       CityName = address.City,
                                       StateZipcode = states.Code + "  " + address.ZipCode,
                                       Phone1 = Regex.Replace(address.Phone, @"(\d{3})(\d{3})(\d{4})", "($1)-$2-$3"),
                                       
                                   }).FirstOrDefault();

            if (getAAYHSContactInfo != null)
            {
                getExhibitorSponsorConfirmationReportForAllExhibitors.getAAYHSContactInfo = getAAYHSContactInfo;
            }
            else
            {
                getExhibitorSponsorConfirmationReportForAllExhibitors.getAAYHSContactInfo = new GetAAYHSContactInfo();
            }



            List<ExhibitorInfo> exhibitorlist = new List<ExhibitorInfo>();
            exhibitorlist = (from exhibitor in _ObjContext.Exhibitors
                             join address in _ObjContext.Addresses on exhibitor.AddressId equals address.AddressId
                             join state in _ObjContext.States on address.StateId equals state.StateId
                             where exhibitor.IsActive == true
                             select new ExhibitorInfo
                             {
                                 ExhibitorId = exhibitor.ExhibitorId,
                                 ExhibitorName = exhibitor.FirstName + " " + exhibitor.LastName,
                                 Address = address!=null? address.Address:"",
                                 CityName = address != null ? address.City:"",
                                 StateZipcode = (state!=null? state.Code:"") + "  " + address != null? address.ZipCode:"",
                                 Email = exhibitor.PrimaryEmail,
                                 Phone = exhibitor.Phone!=null? Regex.Replace(exhibitor.Phone, @"(\d{3})(\d{3})(\d{4})", "($1)-$2-$3"):"",
                                 
                             }).ToList();

            List<GetExhibitorSponsorConfirmationReport> list = new List<GetExhibitorSponsorConfirmationReport>();
            if(exhibitorlist!=null && exhibitorlist.Count>0)
            {
                foreach(var exhibitor in exhibitorlist)
                {
                    GetExhibitorSponsorConfirmationReport getExhibitorSponsorConfirmationReport = new GetExhibitorSponsorConfirmationReport();
                    getExhibitorSponsorConfirmationReport.exhibitorinfo = exhibitor;

                    List<HorseInfo> horseInfo = new List<HorseInfo>();
                    horseInfo = (from exhibitorHorse in _ObjContext.ExhibitorHorse
                                 join horse in _ObjContext.Horses on exhibitorHorse.HorseId equals horse.HorseId
                                 where exhibitorHorse.IsActive == true
                                 && horse.IsActive == true
                                 && exhibitorHorse.ExhibitorId == getExhibitorSponsorConfirmationReport.exhibitorinfo.ExhibitorId
                                 select new HorseInfo
                                 {
                                     ExhibitorHorseId = exhibitorHorse.ExhibitorHorseId,
                                     HorseId = exhibitorHorse.HorseId,
                                     HorseName = horse.Name,
                                     HorseTypeId = horse.HorseTypeId,
                                     HorseTypeName = _ObjContext.GlobalCodes.Where(x => x.GlobalCodeId == horse.HorseTypeId).Select(y => y.CodeName).FirstOrDefault(),

                                     sponsordetail = (from sponsorExhibitor in _ObjContext.SponsorExhibitor
                                                      join sponsor in _ObjContext.Sponsors on sponsorExhibitor.SponsorId equals sponsor.SponsorId

                                                      where sponsorExhibitor.IsActive
                                                      && sponsor.IsActive
                                                      && sponsorExhibitor.ExhibitorId == exhibitor.ExhibitorId
                                                      && sponsorExhibitor.HorseId == exhibitorHorse.HorseId
                                                      select new Sponsordetail
                                                      {
                                                          SponsorName = sponsor.SponsorName,
                                                          SponsorId = sponsorExhibitor.SponsorId,
                                                          Amount = _ObjContext.SponsorExhibitor.Where(x => x.SponsorId == sponsorExhibitor.SponsorId
                                                          && x.HorseId == sponsorExhibitor.HorseId
                                                          && x.ExhibitorId == sponsorExhibitor.ExhibitorId && x.IsActive).Select(x => x.SponsorAmount).Sum()

                                                      }).ToList(),

                                     SponsorAmountTotal = _ObjContext.SponsorExhibitor.Where(x => x.IsActive == true
                                     && x.ExhibitorId == exhibitor.ExhibitorId
                                     && x.HorseId == exhibitorHorse.HorseId).Select(x => x.SponsorAmount).Sum()

                                 }).ToList();

                    foreach (var item in horseInfo)
                    {
                        item.sponsordetail = item.sponsordetail.GroupBy(x => x.SponsorId).Select(y => y.First()).OrderByDescending(x => x.SponsorId).ToList();
                    }

                    getExhibitorSponsorConfirmationReport.horseinfo = horseInfo;

                    list.Add(getExhibitorSponsorConfirmationReport);

                }
               
            }
            getExhibitorSponsorConfirmationReportForAllExhibitors.getExhibitorSponsorConfirmationReports = list;
            var ReportText = (from yst in _ObjContext.YearlyStatementText
                              where yst.YearlyMaintenanceId == yearlyMainId.YearlyMaintainenceId && yst.StatementNumber == Convert.ToString(3)
                              select yst.StatementText).FirstOrDefault();

            getExhibitorSponsorConfirmationReportForAllExhibitors.ReportText =  ReportText != null ? ReportText : "";

            return getExhibitorSponsorConfirmationReportForAllExhibitors;
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
                                       join state in _ObjContext.States on address.StateId equals state.StateId
                                       where sponsorsClass.IsActive == true && sponsorsClass.IsDeleted == false
                                       && sponsor.IsActive == true && sponsor.IsDeleted == false
                                       && sponsorsClass.ClassId == classId
                                       orderby sponsorsClass.CreatedDate
                                       select new SponsorInfo
                                       {
                                           SponsorName = sponsor.SponsorName,
                                           City = address.City,
                                           StateZipcode = state.Code

                                       }).Take(4).ToList(),

                        classInfo = (from exhibitorClass in _ObjContext.ExhibitorClass
                                     join horse in _ObjContext.Horses on exhibitorClass.HorseId equals horse.HorseId
                                     join exhibitor in _ObjContext.Exhibitors on exhibitorClass.ExhibitorId equals exhibitor.ExhibitorId
                                     join address in _ObjContext.Addresses on exhibitor.AddressId equals address.AddressId
                                     join state in _ObjContext.States on address.StateId equals state.StateId
                                     where exhibitorClass.IsActive == true && exhibitorClass.IsDeleted == false
                                     && horse.IsDeleted == false && exhibitor.IsDeleted == false
                                     && exhibitorClass.ClassId == classId
                                     select new ClassInfo
                                     {
                                         BackNumber = _ObjContext.ExhibitorHorse.Where(x => x.ExhibitorId == exhibitorClass.ExhibitorId &&
                                         x.HorseId == exhibitorClass.HorseId && x.IsDeleted == false).Select(x => x.BackNumber).FirstOrDefault(),

                                         NSBA = Convert.ToString(exhibitor.IsNSBAMember == true ? "N" : "-"),
                                         HorseName = horse.Name,
                                         ExhibitorName = exhibitor.FirstName + " " + exhibitor.LastName,
                                         CityState = address.City + ", " + state.Code

                                     }).ToList()

                    });

            getProgramReport = data.FirstOrDefault();
            return getProgramReport;
        }

        public GetProgramReportOfAllClasses GetProgramReportOfAllClasses()
        {
            List<GetProgramReport> getProgramReport = new List<GetProgramReport>();
            GetProgramReportOfAllClasses getProgramReportOfAllClasses = new GetProgramReportOfAllClasses();

            getProgramReport = (from classes in _ObjContext.Classes
                                where classes.IsActive == true && classes.IsDeleted == false
                                select new GetProgramReport
                                {
                                    ClassNumber = classes.ClassNumber,
                                    ClassName = classes.Name,
                                    Age = classes.AgeGroup,

                                    sponsorInfo = (from sponsorsClass in _ObjContext.ClassSponsor
                                                   join sponsor in _ObjContext.Sponsors on sponsorsClass.SponsorId equals sponsor.SponsorId
                                                   join address in _ObjContext.Addresses on sponsor.AddressId equals address.AddressId
                                                   join state in _ObjContext.States on address.StateId equals state.StateId
                                                   where sponsorsClass.IsActive == true && sponsorsClass.IsDeleted == false
                                                   && sponsor.IsActive == true && sponsor.IsDeleted == false
                                                   && sponsorsClass.ClassId == classes.ClassId
                                                   orderby sponsorsClass.CreatedDate
                                                   select new SponsorInfo
                                                   {
                                                       SponsorName = sponsor.SponsorName,
                                                       City = address.City,
                                                       StateZipcode = state.Code

                                                   }).Take(4).ToList(),

                                    classInfo = (from exhibitorClass in _ObjContext.ExhibitorClass
                                                 join horse in _ObjContext.Horses on exhibitorClass.HorseId equals horse.HorseId
                                                 join exhibitor in _ObjContext.Exhibitors on exhibitorClass.ExhibitorId equals exhibitor.ExhibitorId
                                                 join address in _ObjContext.Addresses on exhibitor.AddressId equals address.AddressId
                                                 join state in _ObjContext.States on address.StateId equals state.StateId
                                                 where exhibitorClass.IsActive == true && exhibitorClass.IsDeleted == false
                                                 && horse.IsDeleted == false && exhibitor.IsDeleted == false
                                                 && exhibitorClass.ClassId == classes.ClassId
                                                 select new ClassInfo
                                                 {
                                                     BackNumber = _ObjContext.ExhibitorHorse.Where(x => x.ExhibitorId == exhibitorClass.ExhibitorId &&
                                                     x.HorseId == exhibitorClass.HorseId && x.IsDeleted == false).Select(x => x.BackNumber).FirstOrDefault(),

                                                     NSBA = Convert.ToString(exhibitor.IsNSBAMember == true ? "N" : "-"),
                                                     HorseName = horse.Name,
                                                     ExhibitorName = exhibitor.FirstName + " " + exhibitor.LastName,
                                                     CityState = address.City + ", " + state.Code
                                                 }).ToList()

                                }).ToList();

            getProgramReportOfAllClasses.getProgramReport = getProgramReport;
            return getProgramReportOfAllClasses;
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
                                     join state in _ObjContext.States on address.StateId equals state.StateId                                    
                                     where exhibitorClass.IsActive == true && exhibitorClass.IsDeleted == false
                                     && horse.IsDeleted == false && exhibitor.IsDeleted == false
                                     && exhibitorClass.ClassId == classId
                                    select new ClassDetail 
                                    { 
                                      BackNumber= _ObjContext.ExhibitorHorse.Where(x=>x.ExhibitorId==exhibitorClass.ExhibitorId && 
                                      x.HorseId==exhibitorClass.HorseId && x.IsDeleted==false).Select(x=>x.BackNumber).FirstOrDefault(),

                                      Scratch=Convert.ToString( exhibitorClass.IsScratch==true?"S":"-"),
                                      NSBA= Convert.ToString(exhibitor.IsNSBAMember==true?"N":"-"),
                                      HorseName=horse.Name,
                                      ExhibitorName=exhibitor.FirstName+" "+exhibitor.LastName,
                                      CityState= address.City+", "+state.Code,
                                      Split=_ObjContext.ClassSplits.Where(x=>x.ClassId==exhibitorClass.ClassId).Select(x=>x.SplitNumber).FirstOrDefault()
                                    }).ToList()
                    });           
            getPaddockReport = data.FirstOrDefault();
            return getPaddockReport;
        }

        public GetPaddockReportOfAllClasses GetPaddockReportOfAllClasses()
        {            
            List<GetPaddockReport> getPaddockReport = new List<GetPaddockReport>();
            GetPaddockReportOfAllClasses getPaddockReportOfAllClasses = new GetPaddockReportOfAllClasses();

            getPaddockReport = (from classes in _ObjContext.Classes
                    where classes.IsActive == true && classes.IsDeleted == false
                    select new GetPaddockReport
                    {
                        ClassNumber = classes.ClassNumber,
                        ClassName = classes.Name,
                        Age = classes.AgeGroup,

                        classDetails = (from exhibitorClass in _ObjContext.ExhibitorClass
                                        join horse in _ObjContext.Horses on exhibitorClass.HorseId equals horse.HorseId
                                        join exhibitor in _ObjContext.Exhibitors on exhibitorClass.ExhibitorId equals exhibitor.ExhibitorId
                                        join address in _ObjContext.Addresses on exhibitor.AddressId equals address.AddressId
                                        join state in _ObjContext.States on address.StateId equals state.StateId                                      
                                        where exhibitorClass.IsActive == true && exhibitorClass.IsDeleted == false
                                        && horse.IsDeleted == false && exhibitor.IsDeleted == false
                                        && exhibitorClass.ClassId == classes.ClassId
                                        select new ClassDetail
                                        {
                                            BackNumber = _ObjContext.ExhibitorHorse.Where(x => x.ExhibitorId == exhibitorClass.ExhibitorId &&
                                             x.HorseId == exhibitorClass.HorseId && x.IsDeleted == false).Select(x => x.BackNumber).FirstOrDefault(),

                                            Scratch = Convert.ToString(exhibitorClass.IsScratch == true ? "S" : "-"),
                                            NSBA = Convert.ToString(exhibitor.IsNSBAMember == true ? "N" : "-"),
                                            HorseName = horse.Name,
                                            ExhibitorName = exhibitor.FirstName + " " + exhibitor.LastName,
                                            CityState = address.City + ", " + state.Code,
                                            Split = _ObjContext.ClassSplits.Where(x => x.ClassId == exhibitorClass.ClassId).Select(x => x.SplitNumber).FirstOrDefault()
                                        }).ToList()
                    }).ToList();
            getPaddockReportOfAllClasses.getPaddockReport = getPaddockReport;
            return getPaddockReportOfAllClasses;
        }

        public GetAllClassesEntries GetAllClassesEntries(int classId)
        {
            IEnumerable<GetClassEntriesCount> data;
            GetAllClassesEntries getAllClassesEntries = new GetAllClassesEntries();

            if (classId<0)
            {
                data = (from classes in _ObjContext.Classes
                        where classes.IsActive == true && classes.IsDeleted == false
                        orderby classes.ClassNumber
                        select new GetClassEntriesCount
                        {
                            ClassNumber = classes.ClassNumber,
                            ClassName = classes.Name,
                            ClassAgeGroup = classes.AgeGroup,
                            EntryTotal = _ObjContext.ExhibitorClass.Where(x => x.ClassId == classes.ClassId && x.IsScratch == false && x.IsActive == true && x.IsDeleted == false).Count()
                        });
            }
            else
            {
                data = (from classes in _ObjContext.Classes
                        where classes.IsActive == true && classes.IsDeleted == false
                        && classes.ClassId==classId
                        orderby classes.ClassNumber
                        select new GetClassEntriesCount
                        {
                            ClassNumber = classes.ClassNumber,
                            ClassName = classes.Name,
                            ClassAgeGroup = classes.AgeGroup,
                            EntryTotal = _ObjContext.ExhibitorClass.Where(x => x.ClassId == classes.ClassId && x.IsScratch == false && x.IsActive == true && x.IsDeleted == false).Count()
                        });
            }
            
            getAllClassesEntries.getClassEntriesCount = data.ToList();
            return getAllClassesEntries;
        }

        public GetClassResultReport GetClassResultReport()
        {

            GetClassResultReport getClassResultReports = new GetClassResultReport();
            List<GetClassesResult> getClassesResults = new List<GetClassesResult>();

            int id = _yearlyMaintenanceRepository.GetCategoryId("ClassHeaderType");

            var data = (from classes1 in _ObjContext.Classes
                        join header in _ObjContext.GlobalCodes on classes1.ClassHeaderId equals header.GlobalCodeId
                        where classes1.IsActive == true && classes1.IsDeleted == false
                        && header.CategoryId == id
                        select new GetClassesInfoAndResult
                        {
                            ClassHeader = header.CodeName,
                            ClassNumber = classes1.ClassNumber,
                            ClassName = classes1.Name,
                            AgeGroup = classes1.AgeGroup,
                            getClassesSponsors = (from sponsorsClass in _ObjContext.ClassSponsors
                                                  join sponsor in _ObjContext.Sponsors on sponsorsClass.SponsorId equals sponsor.SponsorId
                                                  where sponsorsClass.ClassId == classes1.ClassId
                                                  && sponsorsClass.IsActive == true && sponsorsClass.IsDeleted == false
                                                  && sponsor.IsActive == true && sponsor.IsDeleted == false
                                                  orderby sponsorsClass.CreatedDate
                                                  select new GetClassesSponsors
                                                  {
                                                      SponsorName = sponsor.SponsorName
                                                  }).Take(4).ToList(),
                            getClassResults = (from result in _ObjContext.Result
                                               join exhibitor in _ObjContext.Exhibitors on result.ExhibitorId equals exhibitor.ExhibitorId                                              
                                               join horse in _ObjContext.Horses on result.HorseId equals horse.HorseId into horse1
                                               from horse2 in horse1.DefaultIfEmpty()
                                               join exhibitorGroup in _ObjContext.GroupExhibitors on exhibitor.ExhibitorId equals exhibitorGroup.ExhibitorId into exhibitorGroup1
                                               from exhibitorGroup2 in exhibitorGroup1.DefaultIfEmpty()
                                               join groups in _ObjContext.Groups on exhibitorGroup2.GroupId equals groups.GroupId into groups1
                                               from groups2 in groups1.DefaultIfEmpty()
                                               where result.IsActive == true && result.IsDeleted == false && exhibitor.IsActive == true && exhibitor.IsDeleted == false
                                               && result.ClassId == classes1.ClassId
                                               orderby result.Placement
                                               select new GetClassResult
                                               {
                                                   Place = result.Placement,
                                                   BackNumber = _ObjContext.ExhibitorHorse.Where(x => x.ExhibitorId == result.ExhibitorId &&
                                                   x.HorseId == result.HorseId && x.IsDeleted == false).Select(x => x.BackNumber).FirstOrDefault(),
                                                   ExhibitorName = exhibitor.FirstName + " " + exhibitor.LastName,
                                                   HorseName = horse2 != null ? horse2.Name : "",
                                                   GroupName = groups2 != null ? groups2.GroupName : ""
                                               }).ToList()

                        }).ToList();

            if (data.Count!=0)
            {
                var allData = from data1 in data
                              group data1 by data1.ClassHeader into newData
                              orderby newData.Key
                              select newData;

                foreach (var item in allData)
                {
                    GetClassesResult getClassesResult = new GetClassesResult();
                    getClassesResult.ClassHeader = item.Key;
                    getClassesResult.getClassesInfoAndResult = item.ToList();
                    getClassesResults.Add(getClassesResult);
                }

                getClassResultReports.getClassesResult = getClassesResults.ToList();
            }
            
            return getClassResultReports;
        }

        public GetSingleClassResult GetSingleClassResult(int classId)
        {
            GetSingleClassResult getSingleClassResult = new GetSingleClassResult();

            var getClassResult = (from classes in _ObjContext.Classes
                                  join header in _ObjContext.GlobalCodes on classes.ClassHeaderId equals header.GlobalCodeId into header1
                                  from header2 in header1.DefaultIfEmpty()
                                  where classes.IsActive == true && classes.IsDeleted == false
                                  && classes.ClassId == classId
                                  select new GetSingleClassResult
                                  {
                                      ClassHeader = header2 != null ? header2.CodeName : "",
                                      ClassName = classes.Name,
                                      ClassNumber = classes.ClassNumber,
                                      AgeGroup = classes.AgeGroup,

                                      getClassSponsors = (from sponsorsClass in _ObjContext.ClassSponsors
                                                          join sponsor in _ObjContext.Sponsors on sponsorsClass.SponsorId equals sponsor.SponsorId
                                                          where sponsorsClass.ClassId == classes.ClassId
                                                          && sponsorsClass.IsActive == true && sponsorsClass.IsDeleted == false
                                                          && sponsor.IsActive == true && sponsor.IsDeleted == false
                                                          orderby sponsorsClass.CreatedDate
                                                          select new GetClassesSponsors
                                                          {
                                                              SponsorName = sponsor.SponsorName
                                                          }).Take(4).ToList(),
                                      getClassResults = (from result in _ObjContext.Result
                                                         join exhibitor in _ObjContext.Exhibitors on result.ExhibitorId equals exhibitor.ExhibitorId                                                      
                                                         join horse in _ObjContext.Horses on result.HorseId equals horse.HorseId into horse1
                                                         from horse2 in horse1.DefaultIfEmpty()
                                                         join exhibitorGroup in _ObjContext.GroupExhibitors on exhibitor.ExhibitorId equals exhibitorGroup.ExhibitorId into exhibitorGroup1
                                                         from exhibitorGroup2 in exhibitorGroup1.DefaultIfEmpty()
                                                         join groups in _ObjContext.Groups on exhibitorGroup2.GroupId equals groups.GroupId into groups1
                                                         from groups2 in groups1.DefaultIfEmpty()
                                                         where result.IsActive == true && result.IsDeleted == false && exhibitor.IsActive == true && exhibitor.IsDeleted == false
                                                         && result.ClassId == classes.ClassId
                                                         orderby result.Placement
                                                         select new GetClassResult
                                                         {
                                                             Place = result.Placement,
                                                             BackNumber = _ObjContext.ExhibitorHorse.Where(x => x.ExhibitorId == result.ExhibitorId &&
                                                             x.HorseId == result.HorseId && x.IsDeleted == false).Select(x => x.BackNumber).FirstOrDefault(),
                                                             ExhibitorName = exhibitor.FirstName + " " + exhibitor.LastName,
                                                             HorseName = horse2 != null ? horse2.Name : "",
                                                             GroupName = groups2 != null ? groups2.GroupName : ""
                                                         }).ToList()

                                  });
            getSingleClassResult = getClassResult.FirstOrDefault();
            return getSingleClassResult;
        }

        public GetExhibitorSponsorRefundStatement GetExhibitorSponsorRefundReport(int exhibitorId)
        {
            GetExhibitorSponsorRefundStatement getExhibitorSponsorRefundStatement = new GetExhibitorSponsorRefundStatement();

            var yearlyMainId = _ObjContext.YearlyMaintainence.Where(x => x.Years == DateTime.Now.Year && x.IsActive == true
                                && x.IsDeleted == false).FirstOrDefault();

            GetAAYHSContactInfo getAAYHSContactInfo = new GetAAYHSContactInfo();

            getAAYHSContactInfo = (from contactInfo in _ObjContext.AAYHSContact
                                   join address in _ObjContext.AAYHSContactAddresses on contactInfo.ExhibitorSponsorRefundStatementAddressId
                                   equals
                                   address.AAYHSContactAddressId
                                   join states in _ObjContext.States on address.StateId equals states.StateId
                                   where contactInfo.YearlyMaintainenceId == yearlyMainId.YearlyMaintainenceId
                                   && contactInfo.IsActive == true && contactInfo.IsDeleted == false
                                   select new GetAAYHSContactInfo
                                   {
                                       Email1 = address.Email,
                                       Address = address.Address,
                                       CityName = address.City,
                                       StateZipcode = states.Code + "  " + address.ZipCode,
                                       Phone1 = Regex.Replace(address.Phone, @"(\d{3})(\d{3})(\d{4})", "($1)-$2-$3"),
                                       
                                   }).FirstOrDefault();

            if (getAAYHSContactInfo != null)
            {
                getExhibitorSponsorRefundStatement.getAAYHSContactInfo = getAAYHSContactInfo;
            }
            else
            {
                getExhibitorSponsorRefundStatement.getAAYHSContactInfo = new GetAAYHSContactInfo();
            }

            ExhibitorInfo exhibitorInfo = new ExhibitorInfo();
            exhibitorInfo = (from exhibitor in _ObjContext.Exhibitors
                             join address in _ObjContext.Addresses on exhibitor.AddressId equals address.AddressId
                             join state in _ObjContext.States on address.StateId equals state.StateId
                             where exhibitor.ExhibitorId == exhibitorId && exhibitor.IsActive == true
                             select new ExhibitorInfo
                             {
                                 ExhibitorId = exhibitor.ExhibitorId,
                                 ExhibitorName = exhibitor.FirstName + " " + exhibitor.LastName,
                                 Address = address != null ? address.Address : "",
                                 CityName = address != null ? address.City : "",
                                 StateZipcode = (state != null ? state.Code : "") + "  " + address != null ? address.ZipCode : "",
                                 Email = exhibitor.PrimaryEmail,
                                 Phone = exhibitor.Phone != null ? Regex.Replace(exhibitor.Phone, @"(\d{3})(\d{3})(\d{4})", "($1)-$2-$3") : "",
                                 QtyProgram = exhibitor.QTYProgram
                             }).FirstOrDefault();
            if (exhibitorInfo != null)
            {
                getExhibitorSponsorRefundStatement.exhibitorInfo = exhibitorInfo;
            }
            else
            {
                getExhibitorSponsorRefundStatement.exhibitorInfo = new ExhibitorInfo();
            }

            List<HorsesSponsor> horsesSponsors = new List<HorsesSponsor>();

            horsesSponsors = (from sponsorExihbitor in _ObjContext.SponsorExhibitor
                              join horse in _ObjContext.Horses on sponsorExihbitor.HorseId equals horse.HorseId
                              where sponsorExihbitor.ExhibitorId == exhibitorId
                              && sponsorExihbitor.IsActive == true && sponsorExihbitor.IsDeleted == false
                              && horse.IsDeleted == false
                              select new HorsesSponsor
                              {
                                  ExhibitorId = exhibitorId,
                                  HorseId = sponsorExihbitor.HorseId,
                                  HorseName = horse.Name,
                                  horseSponsorInfos = (from sponsorExihbitor1 in _ObjContext.SponsorExhibitor
                                                       join sponsor in _ObjContext.Sponsors on sponsorExihbitor1.SponsorId equals
                                                       sponsor.SponsorId
                                                       where sponsorExihbitor1.HorseId == sponsorExihbitor.HorseId
                                                       && sponsorExihbitor1.ExhibitorId==exhibitorId
                                                       && sponsorExihbitor1.IsDeleted == false && sponsor.IsDeleted == false
                                                       select new HorseSponsorInfo
                                                       {
                                                           SponsorId = sponsorExihbitor1.SponsorId,
                                                           SponsorName = sponsor.SponsorName,
                                                           Amount = _ObjContext.SponsorExhibitor.Where(x => x.SponsorId == sponsorExihbitor1.SponsorId
                                                           && x.HorseId == sponsorExihbitor1.HorseId
                                                           && x.ExhibitorId == sponsorExihbitor1.ExhibitorId && x.IsDeleted==false).Select(x => x.SponsorAmount).Sum()
                                                       }).ToList(),

                                  TotalAmount = _ObjContext.SponsorExhibitor.Where(x => x.HorseId == sponsorExihbitor.HorseId
                                                && x.ExhibitorId == exhibitorId &&
                                                x.IsDeleted == false).Select(x => x.SponsorAmount).Sum()

                              }).ToList();

            foreach (var item in horsesSponsors)
            {
                item.horseSponsorInfos = item.horseSponsorInfos.GroupBy(x => x.SponsorId).Select(y => y.First()).OrderByDescending(x => x.SponsorId).ToList();
            }


            var allFees = _ObjContext.YearlyMaintainenceFee.Where(x => x.FeeType == "GeneralFee" && x.IsDeleted == false).ToList();

            var horseStallFee = allFees.Where(x => x.FeeName == "Stall"
                                     && x.YearlyMaintainenceId == yearlyMainId.YearlyMaintainenceId && x.IsDeleted == false).ToList();


            var tackStallFee = allFees.Where(x => x.FeeName == "Tack" &&
                                 x.YearlyMaintainenceId == yearlyMainId.YearlyMaintainenceId && x.IsDeleted == false).ToList();

            decimal additionalProgramsFee = allFees.Where(x => x.FeeName == "Additional Programs"
                                            && x.YearlyMaintainenceId == yearlyMainId.YearlyMaintainenceId && x.IsDeleted == false).Select
                                           (x => x.Amount).FirstOrDefault();

            var classEntryFee = allFees.Where(x => x.FeeName == "Class Entry"
                                && x.YearlyMaintainenceId == yearlyMainId.YearlyMaintainenceId && x.IsDeleted == false).ToList();

            decimal preHoseStallFee = 0;
            if (horseStallFee != null)
            {
                preHoseStallFee = horseStallFee.Where(x => x.TimeFrame == "Pre").Select(x => x.Amount).FirstOrDefault();

            }
            decimal preTackStallFee = 0;
            if (tackStallFee != null)
            {
                preTackStallFee = tackStallFee.Where(x => x.TimeFrame == "Pre").Select(x => x.Amount).FirstOrDefault();

            }
            decimal preClassStallFee = 0;
            if (classEntryFee != null)
            {
                preClassStallFee = classEntryFee.Where(x => x.TimeFrame == "Pre").Select(x => x.Amount).FirstOrDefault();

            }

            if (horsesSponsors.Count() != 0)
            {
                horsesSponsors = horsesSponsors.GroupBy(x => x.HorseName).Select(x => x.First()).ToList();

                foreach (var item in horsesSponsors)
                {
                    var allIncentive = _ObjContext.SponsorIncentives.Where(x => item.TotalAmount >= x.SponsorAmount);
                    RefundableCosts refundableCosts = new RefundableCosts();
                    ShowCosts showCosts = new ShowCosts();
                    if (allIncentive.Count() != 0)
                    {
                        var maxIncentive = allIncentive.Max(x => x.SponsorAmount);
                        var checkIncentive = allIncentive.FirstOrDefault(x => x.SponsorAmount == maxIncentive);
                        var getIncentive = _ObjContext.YearlyStatementText.Where(x => x.StatementNumber == "3a" &&
                                          x.Incentive == checkIncentive.Award && x.IsDeleted == false).FirstOrDefault();

                        if (getIncentive != null)
                        {
                            refundableCosts.Incentive = getIncentive.Incentive;
                            refundableCosts.IncentiveText = getIncentive.StatementText;

                            item.refundableCosts = refundableCosts;
                        }

                    }
                    var preClasses = _ObjContext.ExhibitorClass.Where(x => x.ExhibitorId == exhibitorId && x.HorseId == item.HorseId &&
                                     x.Date <= yearlyMainId.PreEntryCutOffDate && x.IsActive == true && x.IsDeleted == false).ToList();

                    showCosts.ExhibitorId = item.ExhibitorId;
                    showCosts.ClassFee = preClassStallFee * preClasses.Count();
                    showCosts.HorseStallFee = preHoseStallFee * 1;
                    showCosts.TackStallFee = preTackStallFee * 1;
                    showCosts.ProgramFee = additionalProgramsFee * (exhibitorInfo != null ? exhibitorInfo.QtyProgram : 0);

                    item.showCosts = showCosts;
                    item.TotalShowCost = showCosts.ClassFee + showCosts.HorseStallFee +
                                        showCosts.TackStallFee + showCosts.ProgramFee;
                }

                getExhibitorSponsorRefundStatement.horsesSponsors = horsesSponsors;
            }

            return getExhibitorSponsorRefundStatement;
        }

        public AllSponsorsYealry AllPatronSponsorsYearly()
        {
            var patron = _ObjContext.GlobalCodeCategories.Where(x => x.CategoryName == "SponsorTypes" && x.IsDeleted==false).FirstOrDefault();

            int patronId = _ObjContext.GlobalCodes.Where(x =>x.CategoryId==(patron!=null?patron.GlobalCodeCategoryId:0) &&
                           x.CodeName == "Patron" && x.IsDeleted == false).Select(x => x.GlobalCodeId).FirstOrDefault();

            IEnumerable<SponsorsYealry> data;
            AllSponsorsYealry allSponsorsYealry = new AllSponsorsYealry();

            data = (from sponsorExhibitor in _ObjContext.SponsorExhibitor
                    join sponsors in _ObjContext.Sponsors on sponsorExhibitor.SponsorId equals sponsors.SponsorId
                    where sponsorExhibitor.CreatedDate.Value.Year == DateTime.Now.Year
                    && sponsorExhibitor.SponsorTypeId == patronId
                    && sponsorExhibitor.IsActive == true && sponsorExhibitor.IsDeleted == false
                    && sponsors.IsActive == true && sponsors.IsDeleted == false
                    orderby sponsors.SponsorName ascending
                    select new SponsorsYealry
                    {
                        SponsorId = sponsors.SponsorId,
                        SponsorName = sponsors.SponsorName
                    }).Distinct();

            allSponsorsYealry.sponsors = data.ToList();
            return allSponsorsYealry;
        }

        public ShowSummaryReport ShowSummaryReport()
        {

            var yearlyMainId = _ObjContext.YearlyMaintainence.Where(x => x.Years == DateTime.Now.Year && x.IsActive == true
                               && x.IsDeleted == false).FirstOrDefault();

            ShowSummaryReport showSummaryReport = new ShowSummaryReport();

            showSummaryReport.TotalNumberOfExhibiitors = _ObjContext.Exhibitors.Where(x => x.IsActive == true && x.IsDeleted == false)
                                                            .Select(x => x.ExhibitorId).Count();

            if (yearlyMainId != null)
            {
                var allClasses = _ObjContext.Classes.Where(x => x.IsActive == true && x.IsDeleted == false).ToList();

                var allExhibitorClasses = _ObjContext.ExhibitorClass.Where(x => x.IsActive == true && x.IsDeleted == false).ToList();

                var preClasses = allExhibitorClasses.Where(x => x.Date.Date <= yearlyMainId.PreEntryCutOffDate.Date
                                                       && x.IsActive == true && x.IsDeleted == false).ToList();

                var postClasses = allExhibitorClasses.Where(x => x.Date.Date > yearlyMainId.PreEntryCutOffDate.Date
                                                          && x.IsActive == true && x.IsDeleted == false).ToList();


                NumberOfClasses numberOfClasses = new NumberOfClasses();

                numberOfClasses.TotalClasses = allClasses.Count();
                numberOfClasses.TotalEntries = allExhibitorClasses.Count();
                numberOfClasses.TotalPreEntries = preClasses.Count();
                numberOfClasses.TotalPostEntries = postClasses.Count();

                showSummaryReport.numberOfClasses = numberOfClasses;

                var allClassesNSBA = _ObjContext.Classes.Where(x => x.IsNSBAMember == true && x.IsActive == true
                                     && x.IsDeleted == false).ToList();

                var allNSBSExhibitorClasses = (from exhibitoClasses in _ObjContext.ExhibitorClass
                                               join classes in _ObjContext.Classes on exhibitoClasses.ClassId equals classes.ClassId
                                               where classes.IsNSBAMember == true && exhibitoClasses.IsDeleted == false
                                               && classes.IsDeleted == false
                                               select new { exhibitoClasses.ClassId, exhibitoClasses.Date }).ToList();

                var preNSBAClasses = allNSBSExhibitorClasses.Where(x => x.Date.Date <= yearlyMainId.PreEntryCutOffDate.Date).ToList();

                var postNSBAClasses = allNSBSExhibitorClasses.Where(x => x.Date.Date > yearlyMainId.PreEntryCutOffDate.Date).ToList();

                NumberOfNSBAClasses numberOfNSBAClasses = new NumberOfNSBAClasses();

                numberOfNSBAClasses.TotalNSBAClasses = allClassesNSBA.Count();
                numberOfNSBAClasses.TotalEntries = allNSBSExhibitorClasses.Count();
                numberOfNSBAClasses.TotalPreEntries = preNSBAClasses.Count();
                numberOfNSBAClasses.TotalPostEntries = postNSBAClasses.Count();

                showSummaryReport.numberOfNSBAClasses = numberOfNSBAClasses;

                var allFees = _ObjContext.YearlyMaintainenceFee.Where(x => x.FeeType == "GeneralFee" && x.IsDeleted == false).ToList();

                var horseStallFee = allFees.Where(x => x.FeeName == "Stall"
                                     && x.YearlyMaintainenceId == yearlyMainId.YearlyMaintainenceId && x.IsDeleted == false).ToList();

                int additionalProgramsFeeId = allFees.Where(x => x.FeeName == "Additional Programs"
                                                && x.YearlyMaintainenceId == yearlyMainId.YearlyMaintainenceId && x.IsDeleted == false).Select
                                               (x => x.YearlyMaintainenceFeeId).FirstOrDefault();

                var tackStallFee = allFees.Where(x => x.FeeName == "Tack" &&
                                     x.YearlyMaintainenceId == yearlyMainId.YearlyMaintainenceId && x.IsDeleted == false).ToList();

                var classEntryFee = allFees.Where(x => x.FeeName == "Class Entry"
                                    && x.YearlyMaintainenceId == yearlyMainId.YearlyMaintainenceId && x.IsDeleted == false).ToList();

                var administrationFee = allFees.Where(x => x.FeeName == "Administration"
                                    && x.YearlyMaintainenceId == yearlyMainId.YearlyMaintainenceId && x.IsDeleted == false).ToList();

                var nsbaFee = allFees.Where(x => x.FeeName == "NSBA Entry"
                                    && x.YearlyMaintainenceId == yearlyMainId.YearlyMaintainenceId && x.IsDeleted == false).ToList();

                int prehorseStallFeeId = horseStallFee.Where(x => x.TimeFrame == "Pre" && x.IsDeleted == false).Select(x => x.YearlyMaintainenceFeeId).FirstOrDefault();
                int posthorseStallFeeId = horseStallFee.Where(x => x.TimeFrame == "Post" && x.IsDeleted == false).Select(x => x.YearlyMaintainenceFeeId).FirstOrDefault();
                int pretackStallFeeId = tackStallFee.Where(x => x.TimeFrame == "Pre" && x.IsDeleted == false).Select(x => x.YearlyMaintainenceFeeId).FirstOrDefault();
                int posttackStallFeeId = tackStallFee.Where(x => x.TimeFrame == "Post" && x.IsDeleted == false).Select(x => x.YearlyMaintainenceFeeId).FirstOrDefault();
                int preclassEntryFeeId = classEntryFee.Where(x => x.TimeFrame == "Pre" && x.IsDeleted == false).Select(x => x.YearlyMaintainenceFeeId).FirstOrDefault();
                int postclassEntryFeeId = classEntryFee.Where(x => x.TimeFrame == "Post" && x.IsDeleted == false).Select(x => x.YearlyMaintainenceFeeId).FirstOrDefault();
                int preadministrationFee = administrationFee.Where(x => x.TimeFrame == "Pre" && x.IsDeleted == false).Select(x => x.YearlyMaintainenceFeeId).FirstOrDefault();
                int postadministrationFee= administrationFee.Where(x => x.TimeFrame == "Post" && x.IsDeleted == false).Select(x => x.YearlyMaintainenceFeeId).FirstOrDefault();
                int prensbaFee = nsbaFee.Where(x => x.TimeFrame == "Pre" && x.IsDeleted == false).Select(x => x.YearlyMaintainenceFeeId).FirstOrDefault();
                int postnsbaFee = nsbaFee.Where(x => x.TimeFrame == "Post" && x.IsDeleted == false).Select(x => x.YearlyMaintainenceFeeId).FirstOrDefault();


                var allPaidFees = _ObjContext.ExhibitorPaymentDetail.Where(x => x.IsActive == true && x.IsDeleted == false);

                decimal adminstrationsFee = allPaidFees.Where(x => x.FeeTypeId == preadministrationFee|| x.FeeTypeId == postadministrationFee).Select(x => x.AmountPaid).Sum();

                decimal horseStallPaidFee = allPaidFees.Where(x => x.FeeTypeId == prehorseStallFeeId || x.FeeTypeId == posthorseStallFeeId).Select(x => x.AmountPaid).Sum();

                decimal tackStallPaidFee = allPaidFees.Where(x => x.FeeTypeId == pretackStallFeeId || x.FeeTypeId == posttackStallFeeId).Select(x => x.AmountPaid).Sum();

                decimal additionalProgramsFee = allPaidFees.Where(x => x.FeeTypeId == additionalProgramsFeeId).Select(x => x.AmountPaid).Sum();

                decimal classEntryPaidFee = allPaidFees.Where(x => x.FeeTypeId == preclassEntryFeeId || x.FeeTypeId == postclassEntryFeeId).Select(x => x.AmountPaid).Sum();

                decimal nsbaEntryPaidFee= allPaidFees.Where(x => x.FeeTypeId == prensbaFee || x.FeeTypeId == postnsbaFee).Select(x => x.AmountPaid).Sum();

                var allGroupFee = _ObjContext.GroupFinancials.Where(x => x.IsActive == true && x.IsDeleted == false);

                decimal adminstrationsGroupFee = Convert.ToDecimal(allGroupFee.Where(x => x.FeeTypeId == preadministrationFee || x.FeeTypeId == postadministrationFee).Select(x => x.Amount).Sum());

                decimal horseStallGroupFee = Convert.ToDecimal(allGroupFee.Where(x => x.FeeTypeId == prehorseStallFeeId || x.FeeTypeId == posthorseStallFeeId).Select(x => x.Amount).Sum());

                decimal tackStallGroupFee = Convert.ToDecimal(allGroupFee.Where(x => x.FeeTypeId == pretackStallFeeId || x.FeeTypeId == posttackStallFeeId).Select(x => x.Amount).Sum());

                decimal additionalGroupProgramsFee = Convert.ToDecimal(allGroupFee.Where(x => x.FeeTypeId == additionalProgramsFeeId).Select(x => x.Amount).Sum());

                decimal classEntryGroupFee = Convert.ToDecimal(allGroupFee.Where(x => x.FeeTypeId == preclassEntryFeeId || x.FeeTypeId == postclassEntryFeeId).Select(x => x.Amount).Sum());

                decimal nsbaEntryGroupFee = Convert.ToDecimal(allGroupFee.Where(x => x.FeeTypeId == prensbaFee || x.FeeTypeId == postnsbaFee).Select(x => x.Amount).Sum());

                GeneralFees generalFees = new GeneralFees();

                generalFees.AdminstrationsFee = (adminstrationsFee + adminstrationsGroupFee);
                generalFees.BoxStallFee = (horseStallPaidFee + horseStallGroupFee);
                generalFees.TackStallFee = (tackStallPaidFee + tackStallGroupFee);
                generalFees.ClassEntryFee = (classEntryPaidFee + classEntryGroupFee);
                generalFees.ProgramFee = (additionalProgramsFee + additionalGroupProgramsFee);
                generalFees.NsbaEntryFee = (nsbaEntryPaidFee + nsbaEntryGroupFee);
                generalFees.TotalFees = generalFees.AdminstrationsFee + generalFees.BoxStallFee + generalFees.TackStallFee +
                                        generalFees.ClassEntryFee + generalFees.ProgramFee + generalFees.NsbaEntryFee;

                showSummaryReport.generalFees = generalFees;

                NumberOfStall numberOfStall = new NumberOfStall();

                var allStallAssigned = _ObjContext.StallAssignment.ToList();

                var permanentStall = allStallAssigned.Count() != 0 ? allStallAssigned.Where(x => x.StallId <= 1012) : null;
                var portableStall = allStallAssigned.Count() != 0 ? allStallAssigned.Where(x => x.StallId >= 2020) : null;

                numberOfStall.TotalPermanentStalls = permanentStall != null ? permanentStall.Count() : 0;
                numberOfStall.TotalPortableStalls = portableStall != null ? portableStall.Count() : 0;
                numberOfStall.TotalStalls = allStallAssigned.Count() != 0 ? allStallAssigned.Count() : 0;

                showSummaryReport.numberOfStall = numberOfStall;
            }

            return showSummaryReport;
        }

        public GetExhibitorsSponsorRefundReport GetExhibitorsSponsorRefundReport()
        {
            GetExhibitorsSponsorRefundReport getExhibitorsSponsorRefundReport = new GetExhibitorsSponsorRefundReport();

            var yearlyMainId = _ObjContext.YearlyMaintainence.Where(x => x.Years == DateTime.Now.Year && x.IsActive == true
                                && x.IsDeleted == false).FirstOrDefault();

            GetAAYHSContactInfo getAAYHSContactInfo = new GetAAYHSContactInfo();

            getAAYHSContactInfo = (from contactInfo in _ObjContext.AAYHSContact
                                   join address in _ObjContext.AAYHSContactAddresses on contactInfo.ExhibitorSponsorRefundStatementAddressId
                                   equals
                                   address.AAYHSContactAddressId
                                   join states in _ObjContext.States on address.StateId equals states.StateId
                                   where contactInfo.YearlyMaintainenceId == yearlyMainId.YearlyMaintainenceId
                                   && contactInfo.IsActive == true && contactInfo.IsDeleted == false
                                   select new GetAAYHSContactInfo
                                   {
                                       Email1 = address.Email,
                                       Address = address.Address,
                                       CityName = address.City,
                                       StateZipcode = states.Code + "  " + address.ZipCode,
                                       Phone1 = Regex.Replace(address.Phone, @"(\d{3})(\d{3})(\d{4})", "($1)-$2-$3"),
                                       
                                   }).FirstOrDefault();

            if (getAAYHSContactInfo != null)
            {
                getExhibitorsSponsorRefundReport.getAAYHSContactInfo = getAAYHSContactInfo;
            }
            else
            {
                getExhibitorsSponsorRefundReport.getAAYHSContactInfo = new GetAAYHSContactInfo();
            }

            List<ExhibitorInfo> exhibitorInfo = new List<ExhibitorInfo>();
            exhibitorInfo = (from exhibitor in _ObjContext.Exhibitors
                             join address in _ObjContext.Addresses on exhibitor.AddressId equals address.AddressId
                             join state in _ObjContext.States on address.StateId equals state.StateId
                             where exhibitor.IsActive == true && exhibitor.IsDeleted == false
                             select new ExhibitorInfo
                             {
                                 ExhibitorId = exhibitor.ExhibitorId,
                                 ExhibitorName = exhibitor.FirstName + " " + exhibitor.LastName,
                                 Address = address != null ? address.Address : "",
                                 CityName = address != null ? address.City : "",
                                 StateZipcode = (state != null ? state.Code : "") + "  " + address != null ? address.ZipCode : "",
                                 Email = exhibitor.PrimaryEmail,
                                 Phone = exhibitor.Phone != null ? Regex.Replace(exhibitor.Phone, @"(\d{3})(\d{3})(\d{4})", "($1)-$2-$3") : "",
                                 QtyProgram = exhibitor.QTYProgram
                             }).ToList();

            List<ExhibitorsHorseAndSponsors> exhibitorsHorseAndSponsors = new List<ExhibitorsHorseAndSponsors>();
            List<HorsesSponsor> horsesSponsors = new List<HorsesSponsor>();

            horsesSponsors = (from sponsorExihbitor in _ObjContext.SponsorExhibitor
                              join horse in _ObjContext.Horses on sponsorExihbitor.HorseId equals horse.HorseId
                              where sponsorExihbitor.IsActive == true && sponsorExihbitor.IsDeleted == false
                              && horse.IsDeleted == false
                              select new HorsesSponsor
                              {
                                  ExhibitorId = sponsorExihbitor.ExhibitorId,
                                  HorseId = sponsorExihbitor.HorseId,
                                  HorseName = horse.Name,
                                  horseSponsorInfos = (from sponsorExihbitor1 in _ObjContext.SponsorExhibitor
                                                       join sponsor in _ObjContext.Sponsors on sponsorExihbitor1.SponsorId equals
                                                       sponsor.SponsorId
                                                       where sponsorExihbitor1.HorseId == sponsorExihbitor.HorseId
                                                       && sponsorExihbitor1.ExhibitorId == sponsorExihbitor.ExhibitorId
                                                       && sponsorExihbitor1.IsDeleted == false && sponsor.IsDeleted == false
                                                       select new HorseSponsorInfo
                                                       {
                                                           SponsorId = sponsorExihbitor1.SponsorId,
                                                           SponsorName = sponsor.SponsorName,
                                                           Amount = _ObjContext.SponsorExhibitor.Where(x => x.SponsorId == sponsorExihbitor1.SponsorId
                                                           && x.HorseId == sponsorExihbitor1.HorseId
                                                           && x.ExhibitorId == sponsorExihbitor1.ExhibitorId && x.IsDeleted == false).Select(x => x.SponsorAmount).Sum()

                                                       }).ToList(),

                                  TotalAmount = _ObjContext.SponsorExhibitor.Where(x => x.HorseId == sponsorExihbitor.HorseId
                                                && x.ExhibitorId == sponsorExihbitor.ExhibitorId &&
                                                x.IsDeleted == false).Select(x => x.SponsorAmount).Sum()

                              }).ToList();

            foreach (var item in horsesSponsors)
            {
                item.horseSponsorInfos = item.horseSponsorInfos.GroupBy(x => x.SponsorId).Select(y => y.First()).OrderByDescending(x => x.SponsorId).ToList();
            }

            var allFees = _ObjContext.YearlyMaintainenceFee.Where(x => x.FeeType == "GeneralFee" && x.IsDeleted == false).ToList();

            var horseStallFee = allFees.Where(x => x.FeeName == "Stall"
                                    && x.YearlyMaintainenceId == yearlyMainId.YearlyMaintainenceId && x.IsDeleted == false).ToList();


            var tackStallFee = allFees.Where(x => x.FeeName == "Tack" &&
                                 x.YearlyMaintainenceId == yearlyMainId.YearlyMaintainenceId && x.IsDeleted == false).ToList();

            decimal additionalProgramsFee = allFees.Where(x => x.FeeName == "Additional Programs"
                                            && x.YearlyMaintainenceId == yearlyMainId.YearlyMaintainenceId && x.IsDeleted == false).Select
                                           (x => x.Amount).FirstOrDefault();

            var classEntryFee = allFees.Where(x => x.FeeName == "Class Entry"
                                && x.YearlyMaintainenceId == yearlyMainId.YearlyMaintainenceId && x.IsDeleted == false).ToList();

            decimal preHoseStallFee = 0;
            if (horseStallFee != null)
            {
                preHoseStallFee = horseStallFee.Where(x => x.TimeFrame == "Pre").Select(x => x.Amount).FirstOrDefault();

            }
            decimal preTackStallFee = 0;
            if (tackStallFee != null)
            {
                preTackStallFee = tackStallFee.Where(x => x.TimeFrame == "Pre").Select(x => x.Amount).FirstOrDefault();

            }
            decimal preClassStallFee = 0;
            if (classEntryFee != null)
            {
                preClassStallFee = classEntryFee.Where(x => x.TimeFrame == "Pre").Select(x => x.Amount).FirstOrDefault();

            }

            foreach (var exhibitor1 in exhibitorInfo)
            {
                ExhibitorsHorseAndSponsors exhibitorsHorse = new ExhibitorsHorseAndSponsors();
                exhibitorsHorse.exhibitorInfo = exhibitor1;

                if (horsesSponsors.Count() != 0)
                {
                    var exhibitorHorseSponsor = horsesSponsors.Where(x => x.ExhibitorId == exhibitor1.ExhibitorId).ToList();
                    exhibitorHorseSponsor = exhibitorHorseSponsor.GroupBy(x => x.HorseName).Select(x => x.First()).ToList();

                    foreach (var item in exhibitorHorseSponsor)
                    {
                        var allIncentive = _ObjContext.SponsorIncentives.Where(x => item.TotalAmount >= x.SponsorAmount);
                        RefundableCosts refundableCosts = new RefundableCosts();
                        ShowCosts showCosts = new ShowCosts();
                        if (allIncentive.Count() != 0)
                        {
                            var maxIncentive = allIncentive.Max(x => x.SponsorAmount);
                            var checkIncentive = allIncentive.FirstOrDefault(x => x.SponsorAmount == maxIncentive);
                            var getIncentive = _ObjContext.YearlyStatementText.Where(x => x.StatementNumber == "3a" &&
                                              x.Incentive == checkIncentive.Award && x.IsDeleted == false).FirstOrDefault();

                            if (getIncentive != null)
                            {
                                refundableCosts.Incentive = getIncentive.Incentive;
                                refundableCosts.IncentiveText = getIncentive.StatementText;

                                item.refundableCosts = refundableCosts;
                            }

                        }
                        var preClasses = _ObjContext.ExhibitorClass.Where(x => x.ExhibitorId == exhibitor1.ExhibitorId && x.HorseId == item.HorseId &&
                                         x.Date <= yearlyMainId.PreEntryCutOffDate && x.IsActive == true && x.IsDeleted == false).ToList();

                        showCosts.ExhibitorId = exhibitor1.ExhibitorId;
                        showCosts.ClassFee = preClassStallFee * preClasses.Count();
                        showCosts.HorseStallFee = preHoseStallFee * 1;
                        showCosts.TackStallFee = preTackStallFee * 1;
                        showCosts.ProgramFee = additionalProgramsFee * (exhibitor1 != null ? exhibitor1.QtyProgram : 0);

                        item.showCosts = showCosts;
                        item.TotalShowCost = showCosts.ClassFee + showCosts.HorseStallFee +
                                            showCosts.TackStallFee + showCosts.ProgramFee;
                    }
                    exhibitorsHorse.horsesSponsors = exhibitorHorseSponsor;

                }
                exhibitorsHorseAndSponsors.Add(exhibitorsHorse);

            }
            getExhibitorsSponsorRefundReport.exhibitorsHorseAndSponsors = exhibitorsHorseAndSponsors;
            return getExhibitorsSponsorRefundReport;
        }

        public GetAdministrativeReport GetAdministrativeReport()
        {

            GetAdministrativeReport getAdministrativeReport = new GetAdministrativeReport();

            var yearlyMaintId = _ObjContext.YearlyMaintainence.Where(x => x.Years == DateTime.Now.Year && x.IsDeleted == false).FirstOrDefault();

            if (yearlyMaintId != null)
            {
                GetFeeCategories getFeeCategories = new GetFeeCategories();

                List<GetAdFee> getAdFees = new List<GetAdFee>();

                getAdFees = (from fees in _ObjContext.YearlyMaintainenceFee
                             where fees.IsDeleted == false
                             && fees.YearlyMaintainenceId == yearlyMaintId.YearlyMaintainenceId
                             && fees.FeeType == "AdFee"
                             select new GetAdFee
                             {
                                 YearlyMaintainenceFeeId = fees.YearlyMaintainenceFeeId,
                                 FeeName = fees.FeeName,
                                 Amount = fees.Amount,
                                 Active = fees.IsActive
                             }).ToList();

                getFeeCategories.getAdFees = getAdFees;

                List<GetClassCategories> getClassCategories = new List<GetClassCategories>();

                getClassCategories = (from globalCategory in _ObjContext.GlobalCodeCategories
                                      join globalCode in _ObjContext.GlobalCodes on globalCategory.GlobalCodeCategoryId equals globalCode.CategoryId
                                      where globalCode.IsActive == true && globalCode.IsDeleted == false
                                      && globalCategory.CategoryName == "ClassHeaderType"
                                      select new GetClassCategories
                                      {
                                          CategoryId = globalCode.GlobalCodeId,
                                          CategoryName = globalCode.CodeName

                                      }).ToList();

                getFeeCategories.getClassCategories = getClassCategories;

                int feeCategoryId = _ObjContext.GlobalCodeCategories.Where(x => x.CategoryName == "FeeType").Select(x => x.GlobalCodeCategoryId).FirstOrDefault();

                List<GetGeneralFee> getGeneralFees = new List<GetGeneralFee>();

                getGeneralFees = (from yearlyFee in _ObjContext.YearlyMaintainenceFee
                                  where yearlyFee.IsDeleted == false
                                  && yearlyFee.YearlyMaintainenceId == yearlyMaintId.YearlyMaintainenceId
                                  && yearlyFee.FeeType == "GeneralFee"
                                  select new GetGeneralFee
                                  {
                                      YearlyMaintenanceFeeId = yearlyFee.YearlyMaintainenceFeeId,
                                      FeeName = yearlyFee.FeeName,
                                      TimeFrame = yearlyFee.TimeFrame,
                                      Amount = yearlyFee.Amount,
                                      Active = yearlyFee.IsActive
                                  }).ToList();

                if (getGeneralFees.Count() != 0)
                {
                    getFeeCategories.getGeneralFees = getGeneralFees;
                }

                List<GetScratchRefund> getScratchRefunds = new List<GetScratchRefund>();

                getScratchRefunds = (from refund in _ObjContext.RefundDetail
                                     join feeType in _ObjContext.GlobalCodes on refund.FeeTypeId equals feeType.GlobalCodeId
                                     where refund.IsDeleted == false
                                     && refund.YearlyMaintenanceId == yearlyMaintId.YearlyMaintainenceId
                                     select new GetScratchRefund
                                     {
                                         RefundId = refund.RefundDetailId,
                                         DateAfter = refund.DateAfter,
                                         DateBefore = refund.DateBefore,
                                         RefundType = feeType.CodeName,
                                         Refund = refund.RefundPercentage,
                                         Active = refund.IsActive
                                     }).ToList();

                getFeeCategories.getScratchRefunds = getScratchRefunds;

                List<GetIncentiveRefund> getIncentiveRefunds = new List<GetIncentiveRefund>();

                getIncentiveRefunds = (from sponsorIncentive in _ObjContext.SponsorIncentives
                                       where sponsorIncentive.YearlyMaintenanceId == yearlyMaintId.YearlyMaintainenceId
                                       && sponsorIncentive.IsDeleted == false
                                       select new GetIncentiveRefund
                                       {
                                           SponsorIncentiveId = sponsorIncentive.SponsorIncentiveId,
                                           SponsorAmount = sponsorIncentive.SponsorAmount,
                                           Award = sponsorIncentive.Award

                                       }).ToList();

                getFeeCategories.getIncentiveRefunds = getIncentiveRefunds;

                GetStatement getStatement = new GetStatement();

                List<SponsorRefundStatement> sponsorRefundStatements = new List<SponsorRefundStatement>();

                sponsorRefundStatements = (from statement in _ObjContext.YearlyStatementText
                                           where statement.IsActive == true && statement.IsDeleted == false
                                           && statement.YearlyMaintenanceId == yearlyMaintId.YearlyMaintainenceId
                                           select new SponsorRefundStatement
                                           {
                                               YearlyStatementTextId = statement.YearlyStatementTextId,
                                               StatementName = statement.StatementName,
                                               StatementNumber = statement.StatementNumber,
                                               StatementText = statement.StatementText,
                                               Incentive = statement.Incentive

                                           }).ToList();

                getStatement.sponsorRefundStatements = sponsorRefundStatements;

                GetAAYHSInfo getAAYHSInfo = new GetAAYHSInfo();

                getAAYHSInfo = (from contactInfo in _ObjContext.AAYHSContact
                                where contactInfo.YearlyMaintainenceId == yearlyMaintId.YearlyMaintainenceId
                                select new GetAAYHSInfo
                                {
                                    AAYHSContactId = contactInfo.AAYHSContactId,
                                    Email1 = contactInfo.Email1,
                                    Email2 = contactInfo.Email2,
                                    Phone1 = Regex.Replace(contactInfo.Phone1, @"(\d{3})(\d{3})(\d{4})", "($1)-$2-$3"),
                                   
                                    Phone2 = Regex.Replace(contactInfo.Phone2, @"(\d{3})(\d{3})(\d{4})", "($1)-$2-$3"),
                                    ShowLocation = contactInfo.Location,
                                    Address = contactInfo.Address,
                                    City = contactInfo.City,
                                    State = contactInfo.State,
                                    ZipCode = contactInfo.ZipCode,
                                    exhibitorSponsorConfirmation = (from address in _ObjContext.AAYHSContactAddresses
                                                                    join state in _ObjContext.States on address.StateId equals state.StateId into state1
                                                                    from state2 in state1.DefaultIfEmpty()
                                                                    where contactInfo.ExhibitorSponsorConfirmationAddressId == address.AAYHSContactAddressId
                                                                    && address.IsDeleted == false
                                                                    select new ExhibitorSponsorConfirmation
                                                                    {
                                                                        Address = address.Address,
                                                                        City = address.City,
                                                                        State = state2 != null ? state2.Name : "",
                                                                        ZipCode = address.ZipCode
                                                                    }).FirstOrDefault(),

                                    exhibitorRefundStatement = (from address in _ObjContext.AAYHSContactAddresses
                                                                join state in _ObjContext.States on address.StateId equals state.StateId into state1
                                                                from state2 in state1.DefaultIfEmpty()
                                                                where contactInfo.ExhibitorSponsorRefundStatementAddressId == address.AAYHSContactAddressId
                                                                && address.IsDeleted == false
                                                                select new ExhibitorRefundStatement
                                                                {
                                                                    Address = address.Address,
                                                                    City = address.City,
                                                                    State = state2 != null ? state2.Name : "",
                                                                    ZipCode = address.ZipCode
                                                                }).FirstOrDefault(),

                                    confirmationEntriesAndStalls = (from address in _ObjContext.AAYHSContactAddresses
                                                                    join state in _ObjContext.States on address.StateId equals state.StateId into state1
                                                                    from state2 in state1.DefaultIfEmpty()
                                                                    where contactInfo.ExhibitorConfirmationEntriesAddressId == address.AAYHSContactAddressId
                                                                    && address.IsDeleted == false
                                                                    select new ConfirmationEntriesAndStalls
                                                                    {
                                                                        Address = address.Address,
                                                                        City = address.City,
                                                                        State = state2 != null ? state2.Name : "",
                                                                        ZipCode = address.ZipCode
                                                                    }).FirstOrDefault()
                                }).FirstOrDefault();


                getAdministrativeReport.getFeeCategories = getFeeCategories;
                getAdministrativeReport.getStatement = getStatement;
                getAdministrativeReport.getAAYHSInfo = getAAYHSInfo;
            }

            return getAdministrativeReport;


        }

        public GetNSBAExhibitorFee GetNSBAExhibitorsFeeReport()
        {
            GetNSBAExhibitorFee getNSBAExhibitorFee = new GetNSBAExhibitorFee();
            IEnumerable<NSBAExhibitorFee> data;
            List<NSBAExhibitorFee> nSBAExhibitorFees = new List<NSBAExhibitorFee>();

            var yearlyMaintId = _ObjContext.YearlyMaintainence.Where(x => x.Years == DateTime.Now.Year && x.IsDeleted == false).FirstOrDefault();

            var allgeneralfee = _ObjContext.YearlyMaintainenceFee.Where(x => x.YearlyMaintainenceId == yearlyMaintId.YearlyMaintainenceId
                           && x.FeeType == "GeneralFee" && x.IsDeleted == false).ToList();

            decimal preNSBAEntryFee = allgeneralfee.Where(x => x.TimeFrame == "Pre" && x.FeeName == "NSBA Entry").Select(x => x.Amount).FirstOrDefault();
            decimal postNSBAEntryFee = allgeneralfee.Where(x => x.TimeFrame == "Post" && x.FeeName == "NSBA Entry").Select(x => x.Amount).FirstOrDefault();



            data = (from exhibitor in _ObjContext.Exhibitors
                    join exhibitorHorse in _ObjContext.ExhibitorHorse on exhibitor.ExhibitorId equals exhibitorHorse.ExhibitorId
                    join horse in _ObjContext.Horses on exhibitorHorse.HorseId equals horse.HorseId
                    where exhibitorHorse.IsDeleted == false && exhibitor.IsDeleted == false
                    && exhibitor.IsNSBAMember == true
                    orderby exhibitor.FirstName ascending
                    select new NSBAExhibitorFee
                    {
                        BackNumber = exhibitorHorse.BackNumber,
                        ExhibitorId = exhibitor.ExhibitorId,
                        Exhibitor = exhibitor.FirstName + " " + exhibitor.LastName,
                        HorseId = horse.HorseId,
                        Horse = horse.Name
                    }).OrderBy(x=>x.BackNumber).ToList();


            foreach (var item in data)
            {

                var nsbaExhibitorClasses = (from classes in _ObjContext.Classes
                                            join exhibitorclasses in _ObjContext.ExhibitorClass
                                            on classes.ClassId equals exhibitorclasses.ClassId
                                            where classes.IsNSBAMember == true
                                            && exhibitorclasses.ExhibitorId == item.ExhibitorId
                                            && exhibitorclasses.HorseId == item.HorseId
                                            && classes.IsDeleted == false
                                            && exhibitorclasses.IsDeleted == false

                                            select new
                                            {
                                                classes.ClassId,
                                                exhibitorclasses.ExhibitorId,
                                                exhibitorclasses.HorseId,
                                                exhibitorclasses.Date
                                            }).ToList();
                if (nsbaExhibitorClasses.Count > 0)
                {
                    var prensbafee = (preNSBAEntryFee) * (nsbaExhibitorClasses != null ? nsbaExhibitorClasses.Where(x => x.Date <= yearlyMaintId.PreEntryCutOffDate).Count() : 0);

                    var postnsbafee = (postNSBAEntryFee) * (nsbaExhibitorClasses != null ? nsbaExhibitorClasses.Where(x => x.Date > yearlyMaintId.PreEntryCutOffDate).Count() : 0);


                    item.PreEntryTotal = prensbafee;
                    item.PostEntryTotal = postnsbafee;

                    nSBAExhibitorFees.Add(item);
                }
            }


            getNSBAExhibitorFee.nSBAExhibitorFee = nSBAExhibitorFees;
            return getNSBAExhibitorFee;

        }

        public GetNSBAExhibitorFee GetNSBAandClassesExhibitorsFeeReport()
        {
            GetNSBAExhibitorFee getNSBAExhibitorFee = new GetNSBAExhibitorFee();
            IEnumerable<NSBAExhibitorFee> data;
            List<NSBAExhibitorFee> nSBAExhibitorFees = new List<NSBAExhibitorFee>();

            var yearlyMaintId = _ObjContext.YearlyMaintainence.Where(x => x.Years == DateTime.Now.Year && x.IsDeleted == false).FirstOrDefault();

            var allgeneralfee = _ObjContext.YearlyMaintainenceFee.Where(x => x.YearlyMaintainenceId == yearlyMaintId.YearlyMaintainenceId
                           && x.FeeType == "GeneralFee" && x.IsDeleted == false).ToList();

            decimal preNSBAEntryFee = allgeneralfee.Where(x => x.TimeFrame == "Pre" && x.FeeName == "NSBA Entry").Select(x => x.Amount).FirstOrDefault();
            decimal postNSBAEntryFee = allgeneralfee.Where(x => x.TimeFrame == "Post" && x.FeeName == "NSBA Entry").Select(x => x.Amount).FirstOrDefault();

            decimal preClassEntryFee = allgeneralfee.Where(x => x.TimeFrame == "Pre" && x.FeeName == "Class Entry").Select(x => x.Amount).FirstOrDefault();
            decimal postClassEntryFee = allgeneralfee.Where(x => x.TimeFrame == "Post" && x.FeeName == "Class Entry").Select(x => x.Amount).FirstOrDefault();

       

            data = (from exhibitor in _ObjContext.Exhibitors
                    join exhibitorHorse in _ObjContext.ExhibitorHorse on exhibitor.ExhibitorId equals exhibitorHorse.ExhibitorId
                    join horse in _ObjContext.Horses on exhibitorHorse.HorseId equals horse.HorseId
                    where exhibitorHorse.IsDeleted == false && exhibitor.IsDeleted == false
                    && exhibitor.IsNSBAMember == true
                    orderby exhibitor.FirstName ascending
                    select new NSBAExhibitorFee
                    {
                        BackNumber = exhibitorHorse.BackNumber,
                        ExhibitorId = exhibitor.ExhibitorId,
                        Exhibitor = exhibitor.FirstName + " " + exhibitor.LastName,
                        HorseId = horse.HorseId,
                        Horse = horse.Name
                    }).OrderBy(x => x.BackNumber).ToList();


            foreach (var item in data)
            {

                var nsbaExhibitorClasses = (from classes in _ObjContext.Classes
                                            join exhibitorclasses in _ObjContext.ExhibitorClass
                                            on classes.ClassId equals exhibitorclasses.ClassId
                                            where classes.IsNSBAMember == true
                                            && exhibitorclasses.ExhibitorId == item.ExhibitorId
                                            && exhibitorclasses.HorseId == item.HorseId
                                            && classes.IsDeleted == false
                                            && exhibitorclasses.IsDeleted == false

                                            select new
                                            {
                                                classes.ClassId,
                                                exhibitorclasses.ExhibitorId,
                                                exhibitorclasses.HorseId,
                                                exhibitorclasses.Date
                                            }).ToList();
                if (nsbaExhibitorClasses.Count > 0)
                {
                    var prensbafee = (preNSBAEntryFee) * (nsbaExhibitorClasses != null ? nsbaExhibitorClasses.Where(x => x.Date <= yearlyMaintId.PreEntryCutOffDate).Count() : 0);
                    var preclassfee = (preClassEntryFee) * (nsbaExhibitorClasses != null ? nsbaExhibitorClasses.Where(x => x.Date <= yearlyMaintId.PreEntryCutOffDate).Count() : 0);

                    var postnsbafee = (postNSBAEntryFee) * (nsbaExhibitorClasses != null ? nsbaExhibitorClasses.Where(x => x.Date > yearlyMaintId.PreEntryCutOffDate).Count() : 0);
                    var postclassfee = (postClassEntryFee) * (nsbaExhibitorClasses != null ? nsbaExhibitorClasses.Where(x => x.Date > yearlyMaintId.PreEntryCutOffDate).Count() : 0);


                    item.PreEntryTotal = prensbafee + preclassfee;
                    item.PostEntryTotal = postnsbafee + postclassfee;

                    nSBAExhibitorFees.Add(item);
                }
            }
          

            getNSBAExhibitorFee.nSBAExhibitorFee = nSBAExhibitorFees;
            return getNSBAExhibitorFee;

        }

        public GetClassResultReport GetNSBAClassesResultReport()
        {
            GetClassResultReport getClassResultReports = new GetClassResultReport();
            List<GetClassesResult> getClassesResults = new List<GetClassesResult>();

            var nsbaExhibitor = _ObjContext.Exhibitors.Where(x => x.IsNSBAMember == true && x.IsDeleted == false);
            int id = _yearlyMaintenanceRepository.GetCategoryId("ClassHeaderType");

            var data = (from classes1 in _ObjContext.Classes
                        join header in _ObjContext.GlobalCodes on classes1.ClassHeaderId equals header.GlobalCodeId
                        where classes1.IsActive == true && classes1.IsDeleted == false
                        && classes1.IsNSBAMember == true
                        && header.CategoryId == id
                        select new GetClassesInfoAndResult
                        {
                            ClassHeader = header.CodeName,
                            ClassNumber = classes1.ClassNumber,
                            ClassName = classes1.Name,
                            AgeGroup = classes1.AgeGroup,
                            getClassResults = (from result in _ObjContext.Result
                                               join exhibitor in nsbaExhibitor on result.ExhibitorId equals exhibitor.ExhibitorId
                                               join exhibitorsClass in _ObjContext.ExhibitorClass on exhibitor.ExhibitorId equals exhibitorsClass.ExhibitorId
                                               join horse in _ObjContext.Horses on exhibitorsClass.HorseId equals horse.HorseId into horse1
                                               from horse2 in horse1.DefaultIfEmpty()
                                               join exhibitorGroup in _ObjContext.GroupExhibitors on exhibitor.ExhibitorId equals exhibitorGroup.ExhibitorId into exhibitorGroup1
                                               from exhibitorGroup2 in exhibitorGroup1.DefaultIfEmpty()
                                               join groups in _ObjContext.Groups on exhibitorGroup2.GroupId equals groups.GroupId into groups1
                                               from groups2 in groups1.DefaultIfEmpty()
                                               where result.IsActive == true && result.IsDeleted == false && exhibitor.IsActive == true && exhibitor.IsDeleted == false
                                               && exhibitorsClass.IsDeleted == false && exhibitorsClass.ClassId == classes1.ClassId
                                               && result.ClassId == classes1.ClassId
                                               orderby result.Placement
                                               select new GetClassResult
                                               {
                                                   Place = result.Placement,
                                                   BackNumber = exhibitor.BackNumber,
                                                   ExhibitorName = exhibitor.FirstName + " " + exhibitor.LastName,
                                                   HorseName = horse2 != null ? horse2.Name : "",
                                                   GroupName = groups2 != null ? groups2.GroupName : ""
                                               }).ToList()

                        }).ToList();

            if (data.Count != 0)
            {

                var allData = from data1 in data
                              group data1 by data1.ClassHeader into newData
                              orderby newData.Key
                              select newData;

                foreach (var item in allData)
                {
                    GetClassesResult getClassesResult = new GetClassesResult();
                    getClassesResult.ClassHeader = item.Key;
                    getClassesResult.getClassesInfoAndResult = item.ToList();
                    getClassesResults.Add(getClassesResult);
                }

                getClassResultReports.getClassesResult = getClassesResults.ToList();
            }

            return getClassResultReports;
        }

        public GetNonExhibitorSponsor GetNonExhibitorSponsor(int sponsorId)
        {
            IEnumerable<GetNonExhibitorSponsor> data;
            GetNonExhibitorSponsor getNonExhibitorSponsor = new GetNonExhibitorSponsor();

            int sponsorTypeCategoryId = _ObjContext.GlobalCodeCategories.Where(x => x.CategoryName == "SponsorTypes" && x.IsDeleted == false).
                                         Select(x=>x.GlobalCodeCategoryId).FirstOrDefault();

            var sponsorTypes = (from gcc in _ObjContext.GlobalCodeCategories
                                join gc in _ObjContext.GlobalCodes on gcc.GlobalCodeCategoryId equals gc.CategoryId
                                where gcc.CategoryName == "SponsorTypes" && gc.IsDeleted == false && gc.IsActive == true
                                select new GlobalCodeResponse
                                {
                                    GlobalCodeId = gc.GlobalCodeId,
                                    CodeName = (gc.CodeName == null ? "" : gc.CodeName),
                                    Description = (gc.Description == null ? String.Empty : gc.Description),
                                    GlobalCodeCategory = gcc.CategoryName,
                                    CategoryId = gc.CategoryId,
                                }).ToList();

            var adSponsorTypeId = 0;
            var classSponsorTypeId = 0;
            if (sponsorTypes != null && sponsorTypes.Count > 0)
            {
                adSponsorTypeId = sponsorTypes.Where(x => x.CodeName == "Ad").Select(x => x.GlobalCodeId).FirstOrDefault();
                classSponsorTypeId = sponsorTypes.Where(x => x.CodeName == "Class").Select(x => x.GlobalCodeId).FirstOrDefault();
            }

            data = (from sponsor in _ObjContext.Sponsors
                    where sponsor.SponsorId == sponsorId
                    select new GetNonExhibitorSponsor
                    {
                        SponsorId = sponsor.SponsorId,
                        SponsorName = sponsor.SponsorName,
                        Total = sponsor != null ? Convert.ToDecimal(sponsor.AmountReceived) : 0,

                        nonExhibitorSponsorTypes = (from sponsorDistributions in _ObjContext.SponsorDistributions
                                                    where sponsorDistributions.SponsorId == sponsorId
                                                    && sponsorDistributions.IsDeleted == false
                                                    select new NonExhibitorSponsorType
                                                    {
                                                        SponsorType = _ObjContext.GlobalCodes.Where(x => x.GlobalCodeId == sponsorDistributions.SponsorTypeId &&
                                                          x.CategoryId == sponsorTypeCategoryId && x.IsDeleted == false).Select(x => x.CodeName).FirstOrDefault(),

                                                        Contribution = sponsorDistributions.TotalDistribute,
                                                        IDNumber = sponsorDistributions.SponsorTypeId == Convert.ToInt32(classSponsorTypeId) ? Convert.ToString(_ObjContext.Classes.Where(x => x.ClassId == Convert.ToInt32(sponsorDistributions.TypeId)).Select(x => x.ClassNumber).FirstOrDefault())
                                                        : (sponsorDistributions.SponsorTypeId == Convert.ToInt32(adSponsorTypeId) ? sponsorDistributions.TypeId
                                                        : Convert.ToString(0)),
                                                        AdSize = _ObjContext.YearlyMaintainenceFee.Where(x => x.YearlyMaintainenceFeeId == sponsorDistributions.AdTypeId
                                                                 && x.IsDeleted == false).Select(x => x.FeeName).FirstOrDefault()

                                                    }).ToList(),

                        TotalDistribution = _ObjContext.SponsorDistributions.Where(x => x.SponsorId == sponsor.SponsorId &&
                                                        x.IsDeleted == false).Select(x => x.TotalDistribute).Sum(),                        
                    });
          
            getNonExhibitorSponsor = data.FirstOrDefault();
            decimal remaining = getNonExhibitorSponsor.Total - ((_ObjContext.SponsorDistributions.Where(x => x.SponsorId == getNonExhibitorSponsor.SponsorId &&
                                                     x.IsDeleted == false).Select(x => x.TotalDistribute).Sum() + _ObjContext.SponsorExhibitor.
                                                   Where(x => x.SponsorId == getNonExhibitorSponsor.SponsorId && x.IsDeleted == false).Select(x => x.SponsorAmount).Sum()));
            if (remaining<0)
            {
                getNonExhibitorSponsor.Remaining = 0;
            }
            else
            {
                getNonExhibitorSponsor.Remaining = remaining;
            }
            return getNonExhibitorSponsor;
        }

        public GetAllNonExhibitorSponsors GetAllNonExhibitorSponsors()
        {
            GetAllNonExhibitorSponsors getAllNonExhibitorSponsors = new GetAllNonExhibitorSponsors();
            List<GetNonExhibitorSponsor> _getAllNonExhibitorSponsors = new List<GetNonExhibitorSponsor>();

            int sponsorTypeCategoryId = _ObjContext.GlobalCodeCategories.Where(x => x.CategoryName == "SponsorTypes" && x.IsDeleted == false).
                                        Select(x => x.GlobalCodeCategoryId).FirstOrDefault();

            var sponsorTypes = (from gcc in _ObjContext.GlobalCodeCategories
                                join gc in _ObjContext.GlobalCodes on gcc.GlobalCodeCategoryId equals gc.CategoryId
                                where gcc.CategoryName == "SponsorTypes" && gc.IsDeleted == false && gc.IsActive == true
                                select new GlobalCodeResponse
                                {
                                    GlobalCodeId = gc.GlobalCodeId,
                                    CodeName = (gc.CodeName == null ? "" : gc.CodeName),
                                    Description = (gc.Description == null ? String.Empty : gc.Description),
                                    GlobalCodeCategory = gcc.CategoryName,
                                    CategoryId = gc.CategoryId,
                                }).ToList();

            var adSponsorTypeId = 0;
            var classSponsorTypeId = 0;
            if (sponsorTypes != null && sponsorTypes.Count > 0)
            {
                adSponsorTypeId = sponsorTypes.Where(x => x.CodeName == "Ad").Select(x => x.GlobalCodeId).FirstOrDefault();
                classSponsorTypeId = sponsorTypes.Where(x => x.CodeName == "Class").Select(x => x.GlobalCodeId).FirstOrDefault();
            }

            var allNonExhibitorSponsors = (from sponsor in _ObjContext.Sponsors
                                           where sponsor.IsDeleted == false
                                           orderby sponsor.SponsorName
                                           select new GetNonExhibitorSponsor
                                           {
                                               SponsorId=sponsor.SponsorId,
                                               SponsorName = sponsor.SponsorName,
                                               Total = sponsor != null ? Convert.ToDecimal(sponsor.AmountReceived) : 0,

                                               nonExhibitorSponsorTypes = (from sponsorDistributions in _ObjContext.SponsorDistributions
                                                                           where sponsorDistributions.SponsorId == sponsor.SponsorId
                                                                           && sponsorDistributions.IsDeleted == false
                                                                           select new NonExhibitorSponsorType
                                                                           {
                                                                               SponsorType = _ObjContext.GlobalCodes.Where(x => x.GlobalCodeId == sponsorDistributions.SponsorTypeId &&
                                                                                 x.CategoryId == sponsorTypeCategoryId && x.IsDeleted == false).Select(x => x.CodeName).FirstOrDefault(),

                                                                               Contribution = sponsorDistributions.TotalDistribute,
                                                                               IDNumber = sponsorDistributions.SponsorTypeId == Convert.ToInt32(classSponsorTypeId) ? Convert.ToString(_ObjContext.Classes.Where(x => x.ClassId == Convert.ToInt32(sponsorDistributions.TypeId)).Select(x => x.ClassNumber).FirstOrDefault())
                                                                             : (sponsorDistributions.SponsorTypeId == Convert.ToInt32(adSponsorTypeId) ? sponsorDistributions.TypeId
                                                                             : Convert.ToString(0)),
                                                                               AdSize = _ObjContext.YearlyMaintainenceFee.Where(x => x.YearlyMaintainenceFeeId == sponsorDistributions.AdTypeId
                                                                                        && x.IsDeleted == false).Select(x => x.FeeName).FirstOrDefault(),

                                                                           }).ToList(),

                                               TotalDistribution = _ObjContext.SponsorDistributions.Where(x => x.SponsorId == sponsor.SponsorId &&
                                                        x.IsDeleted == false).Select(x => x.TotalDistribute).Sum(),

                                           }).ToList();

            var allsponsors = allNonExhibitorSponsors.Where(x=>x.nonExhibitorSponsorTypes.Count()>0).ToList();

            var allSponsorDistributions = _ObjContext.SponsorDistributions.Where(x => x.IsDeleted == false);

            var allExhibitorSponsors = _ObjContext.SponsorExhibitor.Where(x => x.IsDeleted == false);

            foreach (var sponsor1 in allsponsors)
            {
                GetNonExhibitorSponsor getNonExhibitorSponsor = new GetNonExhibitorSponsor();
                sponsor1.Remaining=sponsor1.Total- ((allSponsorDistributions.Where(x => x.SponsorId == sponsor1.SponsorId
                                                   ).Select(x => x.TotalDistribute).Sum() + allExhibitorSponsors.
                                                   Where(x => x.SponsorId == sponsor1.SponsorId && x.IsDeleted == false).Select(x => x.SponsorAmount).Sum()));
                if (sponsor1.Remaining<0)
                {
                    sponsor1.Remaining = 0;
                }
                getNonExhibitorSponsor = sponsor1;
                _getAllNonExhibitorSponsors.Add(getNonExhibitorSponsor);
            }
            getAllNonExhibitorSponsors.getNonExhibitorSponsors = _getAllNonExhibitorSponsors;
            return getAllNonExhibitorSponsors;
        }

        public ExhibiorAdsSponsorReportListResponse GetExhibiorAdsSponsorReport()
        {
            ExhibiorAdsSponsorReportListResponse list = new ExhibiorAdsSponsorReportListResponse();
            List<ExhibiorAdsSponsorReportResponse> responsdata = new List<ExhibiorAdsSponsorReportResponse>();


            var sponsorTypes = (from gcc in _ObjContext.GlobalCodeCategories
                                join gc in _ObjContext.GlobalCodes on gcc.GlobalCodeCategoryId equals gc.CategoryId
                                where gcc.CategoryName == "SponsorTypes" && gc.IsDeleted == false && gc.IsActive == true
                                select new GlobalCodeResponse
                                {
                                    GlobalCodeId = gc.GlobalCodeId,
                                    CodeName = (gc.CodeName == null ? "" : gc.CodeName),
                                    Description = (gc.Description == null ? String.Empty : gc.Description),
                                    GlobalCodeCategory = gcc.CategoryName,
                                    CategoryId = gc.CategoryId,
                                }).ToList();
            var adSponsorTypeId = 0;
          
            if (sponsorTypes != null && sponsorTypes.Count > 0)
            {
                adSponsorTypeId = sponsorTypes.Where(x => x.CodeName == "Ad").Select(x => x.GlobalCodeId).FirstOrDefault();
            }

            responsdata = (from sponsorExhibitor in _ObjContext.SponsorExhibitor
                           join exhibitor in _ObjContext.Exhibitors on sponsorExhibitor.ExhibitorId equals exhibitor.ExhibitorId 
                           join horse in _ObjContext.Horses on sponsorExhibitor.HorseId equals horse.HorseId
                           join sponsor in _ObjContext.Sponsors on sponsorExhibitor.SponsorId equals sponsor.SponsorId
                           
                           where sponsorExhibitor.SponsorTypeId == adSponsorTypeId  
                           && sponsorExhibitor.IsActive == true
                           &&  exhibitor.IsActive == true
                           && horse.IsActive == true
                           && sponsor.IsActive == true
                           
                           select new ExhibiorAdsSponsorReportResponse
                           {
                               ExhibitorId = sponsorExhibitor.ExhibitorId,
                               ExhibitorName = exhibitor.FirstName + " " + exhibitor.LastName,
                               ExhibitorEmail = exhibitor.PrimaryEmail,
                               HorseId = sponsorExhibitor.HorseId,
                               HorseName = horse.Name,
                               BackNumber = Convert.ToString((from exhibitorhorse in _ObjContext.ExhibitorHorse 
                                             where exhibitorhorse.ExhibitorId== sponsorExhibitor.ExhibitorId 
                                             && exhibitorhorse.HorseId==sponsorExhibitor.HorseId
                                             select exhibitorhorse.BackNumber).FirstOrDefault()),
                               AdId = sponsorExhibitor.TypeId,
                               SponsorId = sponsor.SponsorId,
                               SponsorName = sponsor.SponsorName,
                               SponsorEmail = sponsor.Email,
                               Amount = sponsorExhibitor.SponsorAmount
                           }).ToList();


            list.exhibiorAdsSponsorReportResponses = responsdata;
            list.TotalAmount = responsdata.Select(x => x.Amount).Sum();
            list.TotalRecords = responsdata.Count();
            return list;
        }

        public NonExhibiorSummarySponsorDistributionsListResponse GetNonExhibiorSummarySponsorDistributionsReport()
        {

            NonExhibiorSummarySponsorDistributionsListResponse list = new NonExhibiorSummarySponsorDistributionsListResponse();
            List<NonExhibiorSummarySponsorDistributionsResponse> responsdata = new List<NonExhibiorSummarySponsorDistributionsResponse>();
                  
            responsdata = (from sponsordistribution in _ObjContext.SponsorDistributions
                           join sponsor in _ObjContext.Sponsors on sponsordistribution.SponsorId equals sponsor.SponsorId
                           where  sponsordistribution.IsActive == true && sponsor.IsActive == true
                           select new NonExhibiorSummarySponsorDistributionsResponse
                           {
                               SponsorId = sponsor.SponsorId,
                               SponsorName = sponsor.SponsorName,
                               AmountReceived = Convert.ToDecimal(sponsor.AmountReceived),
                               NonExhibitorContribution = _ObjContext.SponsorDistributions.Where(x => x.SponsorId == sponsordistribution.SponsorId
                                              && x.IsActive).Select(x => x.TotalDistribute).Sum(),

                               ExhibitorContribution = _ObjContext.SponsorExhibitor.Where(x => x.SponsorId == sponsordistribution.SponsorId 
                                              && x.IsActive).Select(x => x.SponsorAmount).Sum(),

                               Remaining = 0,
                           }).Distinct().OrderBy(x=>x.SponsorName).ToList();
            foreach(var item in responsdata)
            {
                item.Remaining = item.AmountReceived - (item.NonExhibitorContribution + item.ExhibitorContribution);
                if(item.Remaining<=0)
                {
                    item.Remaining = 0;
                }
            }


            list.TotalReceived = responsdata.Select(x => x.AmountReceived).Sum();
            list.TotalNonExhibitorContribution = responsdata.Select(x => x.NonExhibitorContribution).Sum();
            list.TotalExhibitorContribution = responsdata.Select(x => x.ExhibitorContribution).Sum();
            list.TotalRemaining = responsdata.Select(x => x.Remaining).Sum();

            list.nonExhibiorSummarySponsorDistributionsResponses = responsdata;
            list.TotalRecords = responsdata.Count();

            return list;
        }

        public GetAllExhibitorSponsoredAd GetAllExhibitorSponsoredAd()
        {
            GetAllExhibitorSponsoredAd getAllExhibitorSponsoredAd = new GetAllExhibitorSponsoredAd();

            int sponsorTypeCategoryId = _ObjContext.GlobalCodeCategories.Where(x => x.CategoryName == "SponsorTypes" && x.IsDeleted == false).
                                        Select(x => x.GlobalCodeCategoryId).FirstOrDefault();

            int sponsorTypeId = _ObjContext.GlobalCodes.Where(x => x.CodeName == "Ad" && x.CategoryId == sponsorTypeCategoryId && x.IsDeleted == false).Select
                              (x => x.GlobalCodeId).FirstOrDefault();

            var adSponsors = (from sponsorsExhibitor in _ObjContext.SponsorExhibitor
                              join exhibitor in _ObjContext.Exhibitors on sponsorsExhibitor.ExhibitorId equals exhibitor.ExhibitorId
                              join sponsor in _ObjContext.Sponsors on sponsorsExhibitor.SponsorId equals sponsor.SponsorId
                              where sponsorsExhibitor.SponsorTypeId == sponsorTypeId
                              && sponsorsExhibitor.IsDeleted == false && exhibitor.IsDeleted == false
                              && sponsor.IsDeleted == false
                              orderby sponsor.SponsorName
                              select new GetExhibitorSponsoredAd
                              {
                                  ExhibitorId = exhibitor.ExhibitorId,
                                  ExhibitorName = exhibitor.FirstName + " " + exhibitor.LastName,
                                  AdNumber = sponsorsExhibitor.TypeId,
                                  SponsorName = sponsor.SponsorName,
                                  Amount = sponsorsExhibitor.SponsorAmount

                              }).ToList();

            getAllExhibitorSponsoredAd.getExhibitorSponsoredAds = adSponsors;
            getAllExhibitorSponsoredAd.TotalAmount = adSponsors.Sum(x=>x.Amount);
            return getAllExhibitorSponsoredAd;
        }

        public GetAllNonExhibitorSponsorAd GetAllNonExhibitorSponsorAd()
        {
            GetAllNonExhibitorSponsorAd getAllNonExhibitorSponsorAd = new GetAllNonExhibitorSponsorAd();

            int sponsorTypeCategoryId = _ObjContext.GlobalCodeCategories.Where(x => x.CategoryName == "SponsorTypes" && x.IsDeleted == false).
                                      Select(x => x.GlobalCodeCategoryId).FirstOrDefault();

            int sponsorTypeId = _ObjContext.GlobalCodes.Where(x => x.CodeName == "Ad" && x.CategoryId == sponsorTypeCategoryId && x.IsDeleted == false).Select
                              (x => x.GlobalCodeId).FirstOrDefault();

            var adSponsors = (from sponsorDistributions in _ObjContext.SponsorDistributions
                              join sponsor in _ObjContext.Sponsors on sponsorDistributions.SponsorId equals sponsor.SponsorId
                              where sponsorDistributions.IsDeleted == false
                              && sponsorDistributions.SponsorTypeId == sponsorTypeId
                              && sponsor.IsDeleted==false
                              orderby sponsor.SponsorName
                              select new GetNonExhibitorSponsorAd
                              {
                                  SponsorId=sponsorDistributions.SponsorId,
                                  SponsorName=sponsor.SponsorName,
                                  AdId=sponsorDistributions.TypeId,
                                  Amount=sponsorDistributions.TotalDistribute

                              });
            
            getAllNonExhibitorSponsorAd.getNonExhibitorSponsorAds = adSponsors.ToList();
            getAllNonExhibitorSponsorAd.TotalAmount = adSponsors.Sum(x => x.Amount);
            return getAllNonExhibitorSponsorAd;
        }
        
    }
}
