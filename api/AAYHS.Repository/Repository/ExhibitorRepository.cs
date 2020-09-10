using AAYHS.Core.DTOs.Request;
using AAYHS.Core.DTOs.Response;
using AAYHS.Core.DTOs.Response.Common;
using AAYHS.Core.Enums;
using AAYHS.Data.DBContext;
using AAYHS.Data.DBEntities;
using AAYHS.Repository.IRepository;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AAYHS.Repository.Repository
{
    public class ExhibitorRepository : GenericRepository<Exhibitors>, IExhibitorRepository
    {
        #region readonly
        private readonly IMapper _Mapper;
        #endregion

        #region Private
        private MainResponse _mainResponse;
        #endregion

        #region public
        public AAYHSDBContext _context;
        private IGlobalCodeRepository _globalCodeRepository;
        #endregion

        public ExhibitorRepository(AAYHSDBContext ObjContext,IMapper Mapper) : base(ObjContext)
        {
            _mainResponse = new MainResponse();
            _context = ObjContext;         
            _Mapper = Mapper;
        }

        public ExhibitorListResponse GetAllExhibitors(BaseRecordFilterRequest filterRequest)
        {
            IEnumerable<ExhibitorResponse> exhibitorResponses = null;
            ExhibitorListResponse exhibitorListResponses = new ExhibitorListResponse();
            exhibitorResponses = (from exhibitor in _context.Exhibitors
                                  where exhibitor.IsActive == true && exhibitor.IsDeleted == false
                                  select new ExhibitorResponse
                                  {
                                      ExhibitorId = exhibitor.ExhibitorId,
                                      AddressId = exhibitor.AddressId,
                                      FirstName = exhibitor.FirstName,
                                      LastName = exhibitor.LastName,
                                      BackNumber = exhibitor.BackNumber,
                                      BirthYear = exhibitor.BirthYear,
                                      IsNSBAMember = exhibitor.IsNSBAMember,
                                      IsDoctorNote = exhibitor.IsDoctorNote,
                                      QTYProgram = exhibitor.QTYProgram,
                                      PrimaryEmail = exhibitor.PrimaryEmail,
                                      SecondaryEmail = exhibitor.SecondaryEmail,
                                      Phone = exhibitor.Phone,
                                  }).ToList();
            if (exhibitorResponses.Count() > 0)
            {
                if (filterRequest.SearchTerm!=null && filterRequest.SearchTerm!="")
                {
                    exhibitorResponses = exhibitorResponses.Where(x => Convert.ToString(x.ExhibitorId).Contains(filterRequest.SearchTerm) ||
                                         x.FirstName.ToLower().Contains(filterRequest.SearchTerm.ToLower()) || x.LastName.ToLower().Contains(filterRequest.SearchTerm.ToLower()) ||
                                         Convert.ToString(x.BirthYear).Contains(filterRequest.SearchTerm));
                }
                var propertyInfo = typeof(ExhibitorResponse).GetProperty(filterRequest.OrderBy);
                if (filterRequest.OrderByDescending == true)
                {
                    exhibitorResponses = exhibitorResponses.OrderByDescending(s => s.GetType().GetProperty(filterRequest.OrderBy).GetValue(s)).ToList();
                }
                else
                {
                    exhibitorResponses = exhibitorResponses.AsEnumerable().OrderBy(s => propertyInfo.GetValue(s, null)).ToList();
                }
                exhibitorListResponses.TotalRecords = exhibitorResponses.Count();
                if (filterRequest.AllRecords == true)
                {
                    exhibitorListResponses.exhibitorResponses = exhibitorResponses.ToList();
                }
                else
                {
                    exhibitorListResponses.exhibitorResponses = exhibitorResponses.Skip((filterRequest.Page - 1) * filterRequest.Limit).Take(filterRequest.Limit).ToList();
                }
            }
            
            return exhibitorListResponses;
        }

        public ExhibitorListResponse GetExhibitorById(int exhibitorId)
        {
            IEnumerable<ExhibitorResponse> exhibitorResponses = null;
            ExhibitorListResponse exhibitorListResponses = new ExhibitorListResponse();
            exhibitorResponses = (from exhibitor in _context.Exhibitors                                  
                                  join address in _context.Addresses on exhibitor.AddressId equals address.AddressId into address1
                                  from address2 in address1.DefaultIfEmpty()
                                  where exhibitor.IsActive == true && exhibitor.IsDeleted == false                                 
                                  && address2.IsActive==true && address2.IsDeleted==false
                                  && exhibitor.ExhibitorId== exhibitorId
                                  select new ExhibitorResponse 
                                  { 
                                    ExhibitorId=exhibitor.ExhibitorId,
                                    GroupId= exhibitor != null ? _context.GroupExhibitors.Where(x => x.ExhibitorId == exhibitorId && x.IsActive == true && x.IsDeleted == false).Select(y => y.GroupId).FirstOrDefault() : 0,                                    
                                    BackNumber=exhibitor.BackNumber,
                                    FirstName=exhibitor.FirstName,
                                    LastName=exhibitor.LastName,
                                    BirthYear=exhibitor.BirthYear,
                                    IsDoctorNote=exhibitor.IsDoctorNote,
                                    IsNSBAMember=exhibitor.IsNSBAMember,
                                    PrimaryEmail=exhibitor.PrimaryEmail,
                                    SecondaryEmail=exhibitor.SecondaryEmail,
                                    Phone=exhibitor.Phone,
                                    QTYProgram=exhibitor.QTYProgram,
                                    AddressId= address2 != null ? address2.AddressId:0,
                                    Address= address2 != null ? address2.Address:"",
                                    ZipCodeId= address2 != null ? address2.ZipCodeId:0,
                                    CityId= address2 != null ?address2.CityId:0,
                                    StateId = address2 != null ? _context.Cities.Where(x => x.CityId == address2.CityId).Select(y => y.StateId).FirstOrDefault() : 0,
                                  });
            if (exhibitorResponses.Count()!=0)
            {
                exhibitorListResponses.exhibitorResponses = exhibitorResponses.ToList();
                exhibitorListResponses.TotalRecords = exhibitorResponses.Count();
            }
            return exhibitorListResponses;
        }
       
        public ExhibitorHorsesResponse GetExhibitorHorses(int exhibitorId)
        {
            IEnumerable<ExhibitorHorses> exhibitorHorses = null;
            ExhibitorHorsesResponse exhibitorHorsesResponse = new ExhibitorHorsesResponse();

            exhibitorHorses = (from exhibitorHorse in _context.ExhibitorHorse
                               join horse in _context.Horses on exhibitorHorse.HorseId equals horse.HorseId                              
                               where exhibitorHorse.IsActive == true && exhibitorHorse.IsDeleted == false
                               && horse.IsActive == true && horse.IsDeleted == false                               
                               && exhibitorHorse.ExhibitorId == exhibitorId
                               select new ExhibitorHorses 
                               { 
                                 ExhibitorHorseId=exhibitorHorse.ExhibitorHorseId,
                                 HorseName=horse.Name,
                                 HorseType=_context.GlobalCodes.Where(x=>x.GlobalCodeId==horse.HorseTypeId).Select(y=>y.CodeName).First(),
                                 BackNumber= exhibitorHorse.BackNumber
                               });

            if (exhibitorHorses.Count()!=0)
            {
                exhibitorHorsesResponse.exhibitorHorses = exhibitorHorses.ToList();
                exhibitorHorsesResponse.TotalRecords = exhibitorHorses.Count();
            }
            return exhibitorHorsesResponse;
        }

        public GetAllClassesOfExhibitor GetAllClassesOfExhibitor(int exhibitorId)
        {
            IEnumerable<GetClassesOfExhibitor> getClassesOfExhibitor = null;
            GetAllClassesOfExhibitor getAllClassesOfExhibitor = new GetAllClassesOfExhibitor();

            getClassesOfExhibitor = (from exhibitorClass in _context.ExhibitorClass
                             join classes in _context.Classes on exhibitorClass.ClassId equals classes.ClassId
                             where exhibitorClass.IsActive == true && exhibitorClass.IsDeleted == false
                             && exhibitorClass.ExhibitorId == exhibitorId
                             select new GetClassesOfExhibitor
                             {
                               ExhibitorClassId = exhibitorClass.ExhibitorClassId,
                               ClassNumber =classes.ClassNumber,
                               Name=classes.Name,
                               AgeGroup=classes.AgeGroup,
                               Entries= classes != null ? _context.ExhibitorClass.Where(x => x.ClassId == classes.ClassId && x.IsActive == true && x.IsDeleted == false).Select(x => x.ExhibitorClassId).Count() : 0,
                               Scratch= exhibitorClass.IsScratch
                             });
            if (getClassesOfExhibitor.Count()>0)
            {
                getAllClassesOfExhibitor.getClassesOfExhibitors = getClassesOfExhibitor.ToList();
                getAllClassesOfExhibitor.TotalRecords = getClassesOfExhibitor.Count();
            }
            return getAllClassesOfExhibitor;
        }

        public GetAllSponsorsOfExhibitor GetAllSponsorsOfExhibitor(int exhibitorId)
        {
            IEnumerable<GetSponsorsOfExhibitor> getSponsorsOfExhibitors = null;
            GetAllSponsorsOfExhibitor getAllSponsorsOfExhibitor = new GetAllSponsorsOfExhibitor();

            getSponsorsOfExhibitors = (from sponsorExhibitor in _context.SponsorExhibitor
                                       join sponsor in _context.Sponsors on sponsorExhibitor.SponsorId equals sponsor.SponsorId
                                       join address in _context.Addresses on sponsor.AddressId equals address.AddressId
                                       join city in _context.Cities on address.CityId equals city.CityId
                                       join state in _context.States on city.StateId equals state.StateId                                    
                                       where sponsorExhibitor.IsActive==true && sponsorExhibitor.IsDeleted==false &&
                                       sponsor.IsActive==true && sponsor.IsDeleted==false &&
                                       sponsorExhibitor.ExhibitorId == exhibitorId
                                       select new GetSponsorsOfExhibitor 
                                       { 
                                         SponsorExhibitorId=sponsorExhibitor.SponsorExhibitorId,
                                         SponsorId=sponsor.SponsorId,
                                         Sponsor= sponsor.SponsorName,
                                         ContactName=sponsor.ContactName,
                                         Phone=sponsor.Phone,
                                         Address=address.Address,
                                         City=city.Name,
                                         State=state.Name,                                       
                                         Email =sponsor.Email,
                                         Amount=sponsor.AmountReceived,
                                         SponsorTypeId=sponsorExhibitor.SponsorTypeId,
                                         IdNumber = sponsorExhibitor.SponsorTypeId == (int)SponsorTypes.Class ? Convert.ToString(_context.Classes.Where(x => x.ClassId == Convert.ToInt32(sponsorExhibitor.TypeId)).Select(x => x.ClassNumber).FirstOrDefault())
                                       : (sponsorExhibitor.SponsorTypeId == (int)SponsorTypes.Add ? sponsorExhibitor.TypeId
                                       : Convert.ToString(0)),
                                       });
            if (getSponsorsOfExhibitors.Count()!=0)
            {
                getAllSponsorsOfExhibitor.getSponsorsOfExhibitors = getSponsorsOfExhibitors.ToList();
                getAllSponsorsOfExhibitor.TotalRecords = getSponsorsOfExhibitors.Count();
            }
            return getAllSponsorsOfExhibitor;
        }
     
        public GetSponsorForExhibitor GetSponsorDetail(int sponsorId)
        {
            IEnumerable<GetSponsorForExhibitor> data = null;
            GetSponsorForExhibitor getSponsorForExhibitor = new GetSponsorForExhibitor();

            data= (from sponsor in _context.Sponsors                  
                   join address in _context.Addresses on sponsor.AddressId equals address.AddressId
                   join city in _context.Cities on address.CityId equals city.CityId
                   join state in _context.States on city.StateId equals state.StateId
                   where sponsor.IsActive == true && sponsor.IsDeleted == false &&
                   sponsor.SponsorId == sponsorId
                   select new GetSponsorForExhibitor
                   {                      
                       SponsorId = sponsor.SponsorId,
                       SponsorName = sponsor.SponsorName,
                       ContactName = sponsor.ContactName,
                       Phone = sponsor.Phone,
                       Address = address.Address,
                       City = city.Name,
                       State = state.Name,
                       Email = sponsor.Email,
                       AmountReceived = sponsor.AmountReceived

                   });

            getSponsorForExhibitor = data.FirstOrDefault();
            return getSponsorForExhibitor;
        }

        public GetExhibitorFinancials GetExhibitorFinancials(int exhibitorId)
        {
            IEnumerable<ExhibitorMoneyReceived> data = null;
            GetExhibitorFinancials getExhibitorFinancials = new GetExhibitorFinancials();
           

            var stallCodes = (from gcc in _context.GlobalCodeCategories
                         join gc in _context.GlobalCodes on gcc.GlobalCodeCategoryId equals gc.CategoryId
                         where gcc.CategoryName == "StallType" && gc.IsDeleted == false && gc.IsActive == true
                         select new
                         {
                             gc.GlobalCodeId,
                             gc.CodeName

                         }).ToList();
            int horseStallTypeId = stallCodes.Where(x => x.CodeName == "HorseStall").Select(x => x.GlobalCodeId).FirstOrDefault();
            int tackStallTypeId= stallCodes.Where(x => x.CodeName == "TackStall").Select(x => x.GlobalCodeId).FirstOrDefault();

            var horseStall = _context.StallAssignment.Where(x => x.ExhibitorId == exhibitorId && x.StallAssignmentTypeId==horseStallTypeId 
                                                    && x.IsActive == true && x.IsDeleted == false).ToList();

            var tackStall= _context.StallAssignment.Where(x => x.ExhibitorId == exhibitorId && x.StallAssignmentTypeId == tackStallTypeId
                                                     && x.IsActive == true && x.IsDeleted == false).ToList();

            var classes = _context.ExhibitorClass.Where(x => x.ExhibitorId == exhibitorId
                                                     && x.IsActive == true && x.IsDeleted == false).ToList();

            int additionalPrograme = _context.Exhibitors.Where(x => x.ExhibitorId == exhibitorId && x.IsActive == true && x.IsDeleted == false
                                                        ).Select(x => x.QTYProgram).FirstOrDefault();

            var feeCodes = (from gcc in _context.GlobalCodeCategories
                              join gc in _context.GlobalCodes on gcc.GlobalCodeCategoryId equals gc.CategoryId
                              where gcc.CategoryName == "FeeType" && gc.IsDeleted == false && gc.IsActive == true
                              select new
                              {
                                  gc.GlobalCodeId,
                                  gc.CodeName

                              }).ToList();
            int horseStallFeeId = feeCodes.Where(x => x.CodeName == "Stall").Select(x => x.GlobalCodeId).FirstOrDefault();
            int tackStallFeeId= feeCodes.Where(x => x.CodeName == "Tack").Select(x => x.GlobalCodeId).FirstOrDefault();
            int additionalProgramsFeeId= feeCodes.Where(x => x.CodeName == "Additional Program").Select(x => x.GlobalCodeId).FirstOrDefault();
            int classEntryId= feeCodes.Where(x => x.CodeName == "Class Entry").Select(x => x.GlobalCodeId).FirstOrDefault();

            int yearlyMaintainenceId = _context.YearlyMaintainence.Where(x => x.Year.Year == DateTime.Now.Year && x.IsActive==true && 
                                               x.IsDeleted==false).Select(x => x.YearlyMaintainenceId).FirstOrDefault();

            decimal horseStallFee = _context.YearlyMaintainenceFee.Where(x => x.FeeTypeId == horseStallFeeId && x.YearlyMaintainenceId == yearlyMaintainenceId).Select
                              (x => x.Amount).FirstOrDefault();


            decimal tackStallFee= _context.YearlyMaintainenceFee.Where(x => x.FeeTypeId == tackStallFeeId && x.YearlyMaintainenceId == yearlyMaintainenceId).Select
                              (x => x.Amount).FirstOrDefault();

            decimal additionalProgramsFee= _context.YearlyMaintainenceFee.Where(x => x.FeeTypeId == additionalProgramsFeeId && x.YearlyMaintainenceId == yearlyMaintainenceId).Select
                              (x => x.Amount).FirstOrDefault();

            decimal classEntryFee = _context.YearlyMaintainenceFee.Where(x => x.FeeTypeId == classEntryId && x.YearlyMaintainenceId == yearlyMaintainenceId).Select
                              (x => x.Amount).FirstOrDefault();
            
            decimal horseStallAmount = horseStallFee * horseStall.Count();
            decimal tackStallAmount = tackStallFee * tackStall.Count();
            decimal additionalAmount = additionalProgramsFee * additionalPrograme;
            decimal classAmount = classEntryFee * classes.Count();

            string[] feetype = { "Stall", "Tack", "Additional Programs", "Class Entry" };
            decimal[] amount = { horseStallAmount, tackStallAmount, additionalAmount, classAmount };
            int[] qty = { horseStall.Count(), tackStall.Count(), additionalPrograme, classes.Count() };

            List<ExhibitorFeesBilled> exhibitorFeesBilleds = new List<ExhibitorFeesBilled>();
            for (int i = 0; i < feetype.Count(); i++)
            {
                if (qty[i]!=0)
                {
                    ExhibitorFeesBilled exhibitorFeesBilled = new ExhibitorFeesBilled();
                    exhibitorFeesBilled.Qty = qty[i];
                    exhibitorFeesBilled.FeeType = feetype[i];
                    exhibitorFeesBilled.Amount = amount[i];
                    exhibitorFeesBilleds.Add(exhibitorFeesBilled);
                }
                
            }
            
            getExhibitorFinancials.exhibitorFeesBilled = exhibitorFeesBilleds;
            getExhibitorFinancials.FeeBilledTotal = horseStallAmount + tackStallAmount + additionalAmount + classAmount;

            data = (from exhibitorpayment in _context.ExhibitorPaymentDetails
                    where exhibitorpayment.IsActive == true && exhibitorpayment.IsDeleted == false
                    && exhibitorpayment.ExhibitorId == exhibitorId
                    select new ExhibitorMoneyReceived
                    {
                        Date = exhibitorpayment.PayDate,
                        Amount = exhibitorpayment.Amount,
                    });

            getExhibitorFinancials.exhibitorMoneyReceived = data.ToList();
            getExhibitorFinancials.MoneyReceivedTotal = _context.ExhibitorPaymentDetails.Where(x => x.ExhibitorId == exhibitorId && x.IsActive == true && x.IsDeleted == false).Select(x => x.Amount).Sum();
            return getExhibitorFinancials;
        }

        public GetAllUploadedDocuments GetUploadedDocuments(int exhibitorId)
        {
            IEnumerable<GetUploadedDocuments> data = null;
            GetAllUploadedDocuments getAllUploadedDocuments = new GetAllUploadedDocuments();

            data = (from scan in _context.Scans
                    where scan.ExhibitorId == exhibitorId && scan.IsActive == true && scan.IsDeleted == false
                    select new GetUploadedDocuments
                    {
                        ScansId = scan.ScansId,
                        DocumentPath = scan.DocumentPath,
                        DocumentType = _context.GlobalCodes.Where(x => x.GlobalCodeId == scan.DocumentType && x.IsActive == true && x.IsDeleted == false).Select(x=>x.CodeName).FirstOrDefault()
                    });
            if (data.Count()>0)
            {
                getAllUploadedDocuments.getUploadedDocuments = data.ToList();

            }
            return getAllUploadedDocuments;
        }

        public GetAllFees GetAllFees()
        {
            IEnumerable<GetFees> data = null;
            GetAllFees getAllFees = new GetAllFees();

            int yearlyId = _context.YearlyMaintainence.Where(x => x.Year.Year == DateTime.Now.Year && x.IsActive == true && x.IsDeleted == false).Select(x => x.YearlyMaintainenceId).FirstOrDefault();

            data = (from yFee in _context.YearlyMaintainenceFee
                    where yFee.YearlyMaintainenceId == yearlyId && yFee.IsActive == true && yFee.IsDeleted == false
                    select new GetFees 
                    { 
                      FeeTypeId=yFee.FeeTypeId,
                      FeeType=_context.GlobalCodes.Where(x=>x.GlobalCodeId==yFee.FeeTypeId).Select(x=>x.CodeName).FirstOrDefault(),
                      Amount=yFee.Amount                    
                    });
            getAllFees.getFees = data.ToList();
            return getAllFees;
        }

       
    }
}