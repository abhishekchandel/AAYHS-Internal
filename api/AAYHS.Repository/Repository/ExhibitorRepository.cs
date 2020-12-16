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
            int groupId = _context.GroupExhibitors.Where(x => x.ExhibitorId == exhibitorId && x.IsActive == true && x.IsDeleted == false).Select(x=>x.GroupId).FirstOrDefault();

            IEnumerable<ExhibitorResponse> exhibitorResponses = null;
            ExhibitorListResponse exhibitorListResponses = new ExhibitorListResponse();
            exhibitorResponses = (from exhibitor in _context.Exhibitors
                                  join address in _context.Addresses on exhibitor.AddressId equals address.AddressId into address1
                                  from address2 in address1.DefaultIfEmpty()
                                  where exhibitor.IsActive == true && exhibitor.IsDeleted == false
                                  && address2.IsActive == true && address2.IsDeleted == false
                                  && exhibitor.ExhibitorId == exhibitorId
                                  select new ExhibitorResponse
                                  {
                                      ExhibitorId = exhibitor.ExhibitorId,
                                      GroupId = _context.Groups.Where(x => x.GroupId == groupId && x.IsActive == true && x.IsDeleted == false).Select(x => x.GroupId).FirstOrDefault(),
                                      GroupName = _context.Groups.Where(x => x.GroupId == groupId && x.IsActive == true && x.IsDeleted == false).Select(x => x.GroupName).FirstOrDefault(),
                                      BackNumber = exhibitor.BackNumber,
                                      FirstName = exhibitor.FirstName,
                                      LastName = exhibitor.LastName,
                                      BirthYear = exhibitor.BirthYear,
                                      IsDoctorNote = exhibitor.IsDoctorNote,
                                      IsNSBAMember = exhibitor.IsNSBAMember,
                                      PrimaryEmail = exhibitor.PrimaryEmail,
                                      SecondaryEmail = exhibitor.SecondaryEmail,
                                      Phone = exhibitor.Phone,
                                      QTYProgram = exhibitor.QTYProgram,
                                      AddressId = address2 != null ? address2.AddressId : 0,
                                      Address = address2 != null ? address2.Address : "",
                                      ZipCode = address2 != null ? address2.ZipCode : "",
                                      City = address2 != null ? address2.City:"",
                                      StateId = address2 != null ? address2.StateId : 0,

                                      exhibitorStallAssignmentResponses = (from stallassign in _context.StallAssignment
                                                                       where stallassign.ExhibitorId == exhibitor.ExhibitorId
                                                                       && stallassign.IsActive == true
                                                                       && stallassign.IsDeleted == false
                                                                       select new ExhibitorStallAssignmentResponse
                                                                       {
                                                                           StallAssignmentId = stallassign.StallAssignmentId,
                                                                           StallId = stallassign.StallId,
                                                                           StallAssignmentTypeId = stallassign.StallAssignmentTypeId,
                                                                           GroupId = stallassign.GroupId,
                                                                           ExhibitorId = stallassign.ExhibitorId,
                                                                           BookedByType = stallassign.BookedByType,
                                                                           BookedByName = exhibitor.FirstName+' '+exhibitor.LastName,
                                                                           StallAssignmentDate = stallassign.Date
                                                                       }).ToList()
                                  }); ;
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
                                 HorseId=exhibitorHorse.HorseId,
                                 HorseName=horse.Name,
                                 HorseType=_context.GlobalCodes.Where(x=>x.GlobalCodeId==horse.HorseTypeId).Select(y=>y.CodeName).First(),
                                 BackNumber= exhibitorHorse.BackNumber,
                                 Date=Convert.ToString(exhibitorHorse.Date)
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
                             join horses in _context.Horses on exhibitorClass.HorseId equals horses.HorseId into horses1
                             from horse2 in horses1.DefaultIfEmpty()
                             where exhibitorClass.IsActive == true && exhibitorClass.IsDeleted == false
                             && horse2.IsActive==true && horse2.IsDeleted==false
                             && exhibitorClass.ExhibitorId == exhibitorId
                             select new GetClassesOfExhibitor
                             {
                               ExhibitorClassId = exhibitorClass.ExhibitorClassId,
                               ClassId=classes.ClassId,
                               ClassNumber =classes.ClassNumber,
                               Name=classes.Name,
                               HorseId=horse2.HorseId,
                               HorseName=horse2.Name,
                               AgeGroup=classes.AgeGroup,
                               Entries= classes != null ? _context.ExhibitorClass.Where(x => x.ClassId == classes.ClassId && x.IsActive == true && x.IsDeleted == false).Select(x => x.ExhibitorClassId).Count() : 0,
                               Scratch= exhibitorClass.IsScratch,
                               Date=Convert.ToString(exhibitorClass.Date)
                              
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
            List<GetSponsorsOfExhibitor> getSponsorsOfExhibitors1 = new List<GetSponsorsOfExhibitor>();
            GetAllSponsorsOfExhibitor getAllSponsorsOfExhibitor = new GetAllSponsorsOfExhibitor();
            var sponsorTypes = (from gcc in _context.GlobalCodeCategories
                                join gc in _context.GlobalCodes on gcc.GlobalCodeCategoryId equals gc.CategoryId
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
                 adSponsorTypeId = sponsorTypes.Where(x => x.CodeName == "Ad").Select(x=>x.GlobalCodeId).FirstOrDefault();
                 classSponsorTypeId = sponsorTypes.Where(x => x.CodeName == "Class").Select(x => x.GlobalCodeId).FirstOrDefault();
            }


            getSponsorsOfExhibitors = (from sponsorExhibitor in _context.SponsorExhibitor
                                       join sponsor in _context.Sponsors on sponsorExhibitor.SponsorId equals sponsor.SponsorId
                                       join address in _context.Addresses on sponsor.AddressId equals address.AddressId into address1
                                       from address2 in address1.DefaultIfEmpty()
                                       //join city in _context.Cities on address.CityId equals city.CityId
                                       join state in _context.States on address2.StateId equals state.StateId into state1
                                       from state2 in state1.DefaultIfEmpty()
                                       //join zipcode in _context.ZipCodes2 on address.ZipCodeId equals zipcode.ZipCodeId into zipcode1
                                       //from zipcode2 in zipcode1.DefaultIfEmpty()
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
                                         Address = address2 != null ? address2.Address:"",
                                         City = address2 != null ? address2.City:"",
                                         State= state2!=null? state2.Name:"", 
                                         Zipcode= address2 != null? Convert.ToInt32(address2.ZipCode):0,
                                         Email =sponsor.Email,
                                         SponsorAmount=sponsorExhibitor.SponsorAmount,
                                         Amount= sponsor!=null?Convert.ToDecimal( sponsor.AmountReceived):0,
                                         AmountPaid= _context.SponsorExhibitor.Where(x => x.SponsorId == sponsor.SponsorId && x.IsDeleted == false).Select(x => x.SponsorAmount).Sum(),
                                         Balance= (Convert.ToDecimal(sponsor.AmountReceived)- _context.SponsorExhibitor.Where(x => x.SponsorId == sponsor.SponsorId && x.IsDeleted == false).Select(x => x.SponsorAmount).Sum()),
                                         SponsorTypeId =sponsorExhibitor.SponsorTypeId,
                                         HorseId=sponsorExhibitor.HorseId,
                                          HorseName =(from horse in _context.Horses where horse.HorseId==sponsorExhibitor.HorseId select horse.Name).FirstOrDefault(),
                                           SponsorTypeName = (from code in _context.GlobalCodes where code.GlobalCodeId == sponsorExhibitor.SponsorTypeId select code.CodeName).FirstOrDefault(),
                                         AdTypeName = (from fee in _context.YearlyMaintainenceFee where fee.YearlyMaintainenceFeeId == sponsorExhibitor.AdTypeId select fee.FeeName).FirstOrDefault(),
                                         IdNumber = sponsorExhibitor.SponsorTypeId == Convert.ToInt32(classSponsorTypeId) ? Convert.ToString(_context.Classes.Where(x => x.ClassId == Convert.ToInt32(sponsorExhibitor.TypeId)).Select(x => x.ClassNumber).FirstOrDefault())
                                       : (sponsorExhibitor.SponsorTypeId == Convert.ToInt32(adSponsorTypeId) ? sponsorExhibitor.TypeId
                                       : Convert.ToString(0)),
                                       });
            if (getSponsorsOfExhibitors.Count()!=0)
            {
                foreach (var item in getSponsorsOfExhibitors)
                {
                    if (item.Balance<=0)
                    {
                        item.Balance = 0;

                    }
                    getSponsorsOfExhibitors1.Add(item);
                }
                getAllSponsorsOfExhibitor.getSponsorsOfExhibitors = getSponsorsOfExhibitors1;
                getAllSponsorsOfExhibitor.TotalRecords = getSponsorsOfExhibitors.Count();
            }
            return getAllSponsorsOfExhibitor;
        }
     
        public GetSponsorForExhibitor GetSponsorDetail(int sponsorId)
        {
            IEnumerable<GetSponsorForExhibitor> data = null;
            GetSponsorForExhibitor getSponsorForExhibitor = new GetSponsorForExhibitor();

            data= (from sponsor in _context.Sponsors                  
                   join address in _context.Addresses on sponsor.AddressId equals address.AddressId into address1
                   from address2 in address1.DefaultIfEmpty()
                   join state in _context.States on address2.StateId equals state.StateId into state1
                   from state2 in state1.DefaultIfEmpty()
                   where sponsor.IsActive == true && sponsor.IsDeleted == false &&
                   sponsor.SponsorId == sponsorId
                   select new GetSponsorForExhibitor
                   {                      
                       SponsorId = sponsor.SponsorId,
                       SponsorName = sponsor.SponsorName,
                       ContactName = sponsor.ContactName,
                       Phone = sponsor.Phone,
                       Address = address2 != null ? address2.Address:"",
                       City = address2 != null ? address2.City:"",
                       State = state2!=null? state2.Name:"",
                       Email = sponsor.Email,
                       Amount = sponsor != null ? Convert.ToDecimal(sponsor.AmountReceived) : 0,    
                       AmountPaid= _context.SponsorExhibitor.Where(x => x.SponsorId == sponsor.SponsorId && x.IsDeleted==false).Select(x => x.SponsorAmount).Sum(),
                       Balance = (Convert.ToDecimal(sponsor.AmountReceived) - _context.SponsorExhibitor.Where(x => x.SponsorId == sponsor.SponsorId && x.IsDeleted == false).Select(x => x.SponsorAmount).Sum()),
                       Zipcode= address2 != null?Convert.ToInt32(address2.ZipCode):0
                   });
           
            getSponsorForExhibitor = data.FirstOrDefault();
            if (getSponsorForExhibitor!=null)
            {
                if (getSponsorForExhibitor.Balance <= 0)
                {
                    getSponsorForExhibitor.Balance = 0;
                }
            }
           
            return getSponsorForExhibitor;
        }

        public GetExhibitorFinancials GetExhibitorFinancials(int exhibitorId)
        {
            IEnumerable<ExhibitorMoneyReceived> data = null;
            GetExhibitorFinancials getExhibitorFinancials = new GetExhibitorFinancials();

            var yearlyMaintainence = _context.YearlyMaintainence.Where(x => x.Years == DateTime.Now.Year && x.IsActive == true &&
                                               x.IsDeleted == false).FirstOrDefault();
            if (yearlyMaintainence!=null)
            {
                var stallCodes = (from gcc in _context.GlobalCodeCategories
                                  join gc in _context.GlobalCodes on gcc.GlobalCodeCategoryId equals gc.CategoryId
                                  where gcc.CategoryName == "StallType" && gc.IsDeleted == false && gc.IsActive == true
                                  select new
                                  {
                                      gc.GlobalCodeId,
                                      gc.CodeName,
                                      gc.IsDeleted

                                  }).ToList();

                int horseStallTypeId = stallCodes!=null? stallCodes.Where(x => x.CodeName == "HorseStall" && x.IsDeleted == false).
                                       Select(x => x.GlobalCodeId).FirstOrDefault():0;
                int tackStallTypeId = stallCodes != null ? stallCodes.Where(x => x.CodeName == "TackStall" && x.IsDeleted == false).
                                      Select(x => x.GlobalCodeId).FirstOrDefault():0;


                var preHorseStall = _context.StallAssignment.Where(x => x.ExhibitorId == exhibitorId && x.StallAssignmentTypeId == horseStallTypeId &&
                                                          x.Date.Date <= yearlyMaintainence.PreEntryCutOffDate.Date
                                                        && x.IsActive == true && x.IsDeleted == false).ToList();

                var preTackStall = _context.StallAssignment.Where(x => x.ExhibitorId == exhibitorId && x.StallAssignmentTypeId == tackStallTypeId
                                                           && x.Date.Date <= yearlyMaintainence.PreEntryCutOffDate.Date
                                                          && x.IsActive == true && x.IsDeleted == false).ToList();

                var preClasses = _context.ExhibitorClass.Where(x => x.ExhibitorId == exhibitorId
                                                            && x.Date.Date <= yearlyMaintainence.PreEntryCutOffDate.Date
                                                           && x.IsActive == true && x.IsDeleted == false).ToList();

                
                int additionalPrograme = _context.Exhibitors.Where(x => x.ExhibitorId == exhibitorId && x.IsActive == true && x.IsDeleted == false
                                                            ).Select(x => x.QTYProgram).FirstOrDefault();


                var postHorseStall = _context.StallAssignment.Where(x => x.ExhibitorId == exhibitorId && x.StallAssignmentTypeId == horseStallTypeId &&
                                                                    x.Date.Date > yearlyMaintainence.PreEntryCutOffDate.Date
                                                          && x.IsActive == true && x.IsDeleted == false).ToList();


                var postTackStall = _context.StallAssignment.Where(x => x.ExhibitorId == exhibitorId && x.StallAssignmentTypeId == tackStallTypeId
                                                            && x.Date.Date > yearlyMaintainence.PreEntryCutOffDate.Date
                                                           && x.IsActive == true && x.IsDeleted == false).ToList();

                var postClasses = _context.ExhibitorClass.Where(x => x.ExhibitorId == exhibitorId
                                                            && x.Date.Date > yearlyMaintainence.PreEntryCutOffDate.Date
                                                           && x.IsActive == true && x.IsDeleted == false).ToList();             

                var allFees = _context.YearlyMaintainenceFee.Where(x => x.FeeType== "GeneralFee" && x.IsDeleted == false).ToList();

                var horseStallFee = allFees.Where(x => x.FeeName == "Stall"
                                     && x.YearlyMaintainenceId == yearlyMaintainence.YearlyMaintainenceId && x.IsDeleted==false).ToList();


                var tackStallFee = allFees.Where(x => x.FeeName == "Tack" && 
                                     x.YearlyMaintainenceId == yearlyMaintainence.YearlyMaintainenceId && x.IsDeleted == false).ToList();

                decimal additionalProgramsFee = allFees.Where(x => x.FeeName == "Additional Programs" 
                                                && x.YearlyMaintainenceId == yearlyMaintainence.YearlyMaintainenceId && x.IsDeleted == false).Select
                                               (x => x.Amount).FirstOrDefault();

                var classEntryFee = allFees.Where(x => x.FeeName == "Class Entry" 
                                    && x.YearlyMaintainenceId == yearlyMaintainence.YearlyMaintainenceId && x.IsDeleted == false).ToList();

                var nsbaClassEntryFee=allFees.Where(x=>x.FeeName== "NSBA Entry" && x.YearlyMaintainenceId == yearlyMaintainence.YearlyMaintainenceId
                                      && x.IsDeleted == false).ToList();

                var administrationFee = allFees.Where(x => x.FeeName == "Administration" && x.YearlyMaintainenceId == yearlyMaintainence.YearlyMaintainenceId
                                       && x.IsDeleted == false).ToList();

                var checkNSBAExhibitor = _context.Exhibitors.Where(x => x.ExhibitorId == exhibitorId).FirstOrDefault();

                var allNSBAClasses = _context.Classes.Where(x => x.IsNSBAMember == true && x.IsDeleted == false).ToList();
                decimal preClassEntryAmount = 0;
                decimal postClassEntryAmount = 0;

                int preNSBAClasses = 0;
                int postNSBAClasses = 0;
                if (allNSBAClasses.Count()!=0)
                {                   
                    if (checkNSBAExhibitor.IsNSBAMember == true)
                    {
                        if (preClasses.Count != 0)
                        {
                            var preNSBA = (from all in allNSBAClasses
                                           where preClasses.Any(x => x.ClassId == all.ClassId
                                           )
                                           select new { all.ClassId });
                            preNSBAClasses = preNSBA.Count();
                        }
                        if (postClasses.Count != 0)
                        {
                            var postNSBA = (from all in allNSBAClasses
                                           where postClasses.Any(x => x.ClassId == all.ClassId
                                           )
                                           select new { all.ClassId });
                            postNSBAClasses = postNSBA.Count();
                        }
                        if (nsbaClassEntryFee != null)
                        {
                            decimal preNSBAClassFee = nsbaClassEntryFee.Where(x => x.TimeFrame == "Pre").Select(x => x.Amount).FirstOrDefault();
                            decimal postNSBAClassFee = nsbaClassEntryFee.Where(x => x.TimeFrame == "Post").Select(x => x.Amount).FirstOrDefault();
                            preClassEntryAmount = preNSBAClassFee * preNSBAClasses;
                            postClassEntryAmount = postNSBAClassFee * postNSBAClasses;
                        }

                    }
                }               

                decimal preHorseStallAmount = 0;
                if (horseStallFee.Count != 0)
                {
                    decimal preHoseStallFee = horseStallFee.Where(x => x.TimeFrame == "Pre").Select(x=>x.Amount).FirstOrDefault();
                    preHorseStallAmount = preHoseStallFee * preHorseStall.Count();
                }

                decimal preTackStallAmount = 0;
                if (tackStallFee.Count != 0)
                {
                    decimal preTackStallFee = tackStallFee.Where(x => x.TimeFrame == "Pre").Select(x => x.Amount).FirstOrDefault();
                    preTackStallAmount = preTackStallFee * preTackStall.Count();
                }

                decimal preClassAmount = 0;
                if (classEntryFee.Count != 0)
                {
                    decimal preClassFee = classEntryFee.Where(x => x.TimeFrame == "Pre").Select(x => x.Amount).FirstOrDefault();
                    preClassAmount = preClassFee * preClasses.Count();
                }

                decimal preAdministrationAmount = 0;
                int numberOfHorses = _context.ExhibitorHorse.Where(x => x.ExhibitorId == exhibitorId && x.IsDeleted == false).Count();
                if (administrationFee.Count != 0)
                {
                    decimal preAdministrationFee=administrationFee.Where(x => x.TimeFrame == "Pre").Select(x => x.Amount).FirstOrDefault();                   
                    preAdministrationAmount = numberOfHorses * preAdministrationFee;
                }

                decimal additionalAmount = 0;

                additionalAmount = additionalProgramsFee * additionalPrograme;

                decimal postHorseStallAmount = 0;
                if (horseStallFee.Count != 0)
                {
                    decimal postHoseStallFee = horseStallFee.Where(x => x.TimeFrame == "Post").Select(x => x.Amount).FirstOrDefault();
                    postHorseStallAmount = postHoseStallFee * postHorseStall.Count();
                }


                decimal postTackStallAmount = 0;
                if (tackStallFee.Count != 0)
                {
                    decimal postTackStallFee = tackStallFee.Where(x => x.TimeFrame == "Post").Select(x => x.Amount).FirstOrDefault();
                    postTackStallAmount = postTackStallFee * postTackStall.Count();
                }

                decimal postClassAmount = 0;
                if (classEntryFee.Count != 0)
                {
                    decimal postClassStallFee = classEntryFee.Where(x => x.TimeFrame == "Post").Select(x => x.Amount).FirstOrDefault();
                    postClassAmount = postClassStallFee * postClasses.Count();
                }
              
                decimal horseStallAmount = preHorseStallAmount + postHorseStallAmount;
                decimal tackStallAmount = preTackStallAmount + postTackStallAmount;
                decimal classAmount = preClassAmount + postClassAmount;
                decimal nsbaClassesAmount = preClassEntryAmount + postClassEntryAmount;

                int horseStall = preHorseStall.Count() + postHorseStall.Count();
                int tackStall = preTackStall.Count() + postTackStall.Count();
                int classes = preClasses.Count() + postClasses.Count();
                int nsbaClasses = preNSBAClasses + postNSBAClasses;

                string[] feetype = { "Stall", "Tack", "Additional Programs","Class Entry", "NSBA Entry", "Administration" };
                decimal[] amount = { horseStallAmount, tackStallAmount, additionalAmount, classAmount,nsbaClassesAmount, preAdministrationAmount };
                int[] qty = { horseStall, tackStall, additionalPrograme, classes, nsbaClasses, numberOfHorses };

                List<ExhibitorFeesBilled> exhibitorFeesBilleds = new List<ExhibitorFeesBilled>();
                for (int i = 0; i < feetype.Count(); i++)
                {
                    if (qty[i] != 0)
                    {
                        ExhibitorFeesBilled exhibitorFeesBilled = new ExhibitorFeesBilled();
                        exhibitorFeesBilled.FeeTypeId = allFees.Where(x => x.FeeName == feetype[i] && x.YearlyMaintainenceId ==
                        yearlyMaintainence.YearlyMaintainenceId && (x.TimeFrame=="Pre"||x.TimeFrame=="")).
                        Select(x => x.YearlyMaintainenceFeeId).FirstOrDefault();
                        exhibitorFeesBilled.Qty = qty[i];
                        exhibitorFeesBilled.FeeType = feetype[i];
                        exhibitorFeesBilled.Amount = amount[i];
                        exhibitorFeesBilleds.Add(exhibitorFeesBilled);
                    }

                }

                getExhibitorFinancials.exhibitorFeesBilled = exhibitorFeesBilleds;
                getExhibitorFinancials.FeeBilledTotal = horseStallAmount + tackStallAmount + additionalAmount + classAmount+ nsbaClassesAmount+ preAdministrationAmount;

                data = (from exhibitorpayment in _context.ExhibitorPaymentDetails
                        where exhibitorpayment.IsActive == true && exhibitorpayment.IsDeleted == false
                        && exhibitorpayment.ExhibitorId == exhibitorId 
                        select new ExhibitorMoneyReceived
                        {
                            Date = exhibitorpayment.PayDate,
                            Amount = exhibitorpayment.AmountPaid,
                        });

                getExhibitorFinancials.exhibitorMoneyReceived = data.ToList();
                getExhibitorFinancials.MoneyReceivedTotal = _context.ExhibitorPaymentDetails.Where(x => x.ExhibitorId == exhibitorId && x.IsDeleted == false).Select(x => x.AmountPaid).Sum();

                getExhibitorFinancials.Outstanding = (horseStallAmount + tackStallAmount + additionalAmount + classAmount + nsbaClassesAmount + preAdministrationAmount) - (getExhibitorFinancials.MoneyReceivedTotal);
                decimal overPayment = (getExhibitorFinancials.MoneyReceivedTotal) - (horseStallAmount + tackStallAmount + additionalAmount + classAmount + nsbaClassesAmount + preAdministrationAmount);
                if (overPayment < 0)
                {
                    overPayment = 0;
                }
                if (getExhibitorFinancials.Outstanding < 0)
                {
                    getExhibitorFinancials.Outstanding = 0;
                }
                getExhibitorFinancials.OverPayment = overPayment;
                getExhibitorFinancials.Refunds = _context.ExhibitorPaymentDetail.Where(x => x.ExhibitorId == exhibitorId && x.IsDeleted == false).Select(x => x.RefundAmount).Sum();
            }
            
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
            string timeFrameType;
            IEnumerable<GetFees> data = null;
            GetAllFees getAllFees = new GetAllFees();

            var yearlyId = _context.YearlyMaintainence.Where(x => x.Years == DateTime.Now.Year && x.IsActive == true && x.IsDeleted == false).FirstOrDefault();
            if (yearlyId!=null)
            {
                if (DateTime.Now.Date <= yearlyId.PreEntryCutOffDate.Date)
                {
                    timeFrameType = "Pre";
                }
                else
                {
                    timeFrameType = "Post";
                }
            }
            else
            {
                return getAllFees;
            }
          
            data = (from yearlyFee in _context.YearlyMaintainenceFee
                    where yearlyFee.IsActive == true && yearlyFee.IsDeleted == false
                     && yearlyFee.YearlyMaintainenceId == yearlyId.YearlyMaintainenceId
                     && yearlyFee.FeeType== "GeneralFee"
                    select new GetFees
                    {
                        FeeTypeId=yearlyFee.YearlyMaintainenceFeeId,
                        FeeName=yearlyFee.FeeName,
                        TimeFrameType= yearlyFee.TimeFrame,
                        Amount=yearlyFee.Amount
                    });

            if (data.Count()!=0)
            {
                getAllFees.DefaultTimeFrame = timeFrameType;
                getAllFees.getFees = data.ToList();
            }
            
            return getAllFees;
        }

        public GetAllExhibitorTransactions GetAllExhibitorTransactions(int exhibitorId)
        {
            IEnumerable<GetExhibitorTransactions> data = null;
            GetAllExhibitorTransactions getAllExhibitorTransactions = new GetAllExhibitorTransactions();

            var exhibitorSponsor =_context.SponsorExhibitor.Where(x => x.ExhibitorId == exhibitorId && x.IsActive == true && x.IsDeleted == false);

            data = (from exhibitorPaymentDetail in _context.ExhibitorPaymentDetail
                    where exhibitorPaymentDetail.ExhibitorId == exhibitorId &&
                    exhibitorPaymentDetail.IsActive == true && exhibitorPaymentDetail.IsDeleted == false
                    select new GetExhibitorTransactions 
                    { 
                      ExhibitorPaymentDetailId=exhibitorPaymentDetail.ExhibitorPaymentId,
                      PayDate=exhibitorPaymentDetail.PayDate,
                      TypeOfFee=_context.YearlyMaintainenceFee.Where(x=>x.YearlyMaintainenceFeeId==exhibitorPaymentDetail.FeeTypeId).Select(x=>x.FeeName).FirstOrDefault(),
                      TimeFrameType= exhibitorPaymentDetail.TimeFrameType,
                      Amount =exhibitorPaymentDetail.Amount,
                      AmountPaid=exhibitorPaymentDetail.AmountPaid,
                      RefundAmount=exhibitorPaymentDetail.RefundAmount,
                      DocumentPath=exhibitorPaymentDetail.DocumentPath
                    });

            getAllExhibitorTransactions.getExhibitorTransactions = data.ToList();
            if (exhibitorSponsor.Count()!=0)
            {
                getAllExhibitorTransactions.IsRefund = true;
            }
            else
            {
                getAllExhibitorTransactions.IsRefund = false;
            }
            return getAllExhibitorTransactions;
        }

        public GetAllExhibitorTransactions GetFinancialViewDetail(ViewDetailRequest viewDetailRequest)
        {
            GetAllExhibitorTransactions getAllExhibitorTransactions = new GetAllExhibitorTransactions();
            IEnumerable<GetExhibitorTransactions> data = null;
            int yealryMaintId = _context.YearlyMaintainence.Where(x => x.Years == DateTime.Now.Year && x.IsDeleted == false).Select(x => x.YearlyMaintainenceId).FirstOrDefault();
            var fee = _context.YearlyMaintainenceFee.Where(x => x.YearlyMaintainenceFeeId == viewDetailRequest.FeeTypeId).FirstOrDefault();
            var allFee = _context.YearlyMaintainenceFee.Where(x => x.FeeName ==(fee!=null? fee.FeeName:"") && x.YearlyMaintainenceId==yealryMaintId
                          && x.IsDeleted == false).ToList();
            int[] feeId= new int[allFee.Count];

            for (int i = 0; i <= feeId.Length-1; i++)
            {
                feeId[i] = allFee[i].YearlyMaintainenceFeeId;
            }

            data = (from exhibitorPaymentDetail in _context.ExhibitorPaymentDetail
                    where exhibitorPaymentDetail.ExhibitorId == viewDetailRequest.ExhibitorId &&
                    (exhibitorPaymentDetail.FeeTypeId ==  (feeId.Length!=0?feeId[0]:0)
                    || exhibitorPaymentDetail.FeeTypeId == (feeId.Length==2? feeId[1]:0))
                    && exhibitorPaymentDetail.IsDeleted == false
                    select new GetExhibitorTransactions
                    {
                        ExhibitorPaymentDetailId = exhibitorPaymentDetail.ExhibitorPaymentId,
                        PayDate = exhibitorPaymentDetail.PayDate,
                        TypeOfFee = _context.YearlyMaintainenceFee.Where(x => x.YearlyMaintainenceFeeId == exhibitorPaymentDetail.FeeTypeId).Select(x => x.FeeName).FirstOrDefault(),
                        TimeFrameType = exhibitorPaymentDetail.TimeFrameType,
                        Amount = exhibitorPaymentDetail.Amount,
                        AmountPaid = exhibitorPaymentDetail.AmountPaid,
                        RefundAmount = exhibitorPaymentDetail.RefundAmount
                    }).ToList();

            getAllExhibitorTransactions.getExhibitorTransactions = data.ToList();
            return getAllExhibitorTransactions;

        }

        public ExhibitorAllSponsorAmount ExhibitorAllSponsorAmount(int exhibitorId)
        {
            IEnumerable<ExhibitorSponsor> data;
            ExhibitorAllSponsorAmount exhibitorAllSponsorAmount = new ExhibitorAllSponsorAmount();

            data = (from exhibitorSponor in _context.SponsorExhibitor
                    join sponsor in _context.Sponsors on exhibitorSponor.SponsorId equals sponsor.SponsorId
                    join horse in _context.Horses on exhibitorSponor.HorseId equals horse.HorseId into horse1
                    from horse2 in horse1.DefaultIfEmpty()
                    where exhibitorSponor.ExhibitorId == exhibitorId && exhibitorSponor.IsActive == true && 
                    exhibitorSponor.IsDeleted == false
                    && sponsor.IsDeleted == false && horse2.IsDeleted == false
                    orderby horse2.Name ascending
                    select new ExhibitorSponsor
                    {
                        SponsorId=exhibitorSponor.SponsorId,
                        HorseName= horse2!=null? horse2.Name:"",
                        SponsorName=sponsor.SponsorName,
                        Amount=exhibitorSponor.SponsorAmount
                    });

            if (data.Count()!=0)
            {
                exhibitorAllSponsorAmount.exhibitorSponsors = data.ToList();
            }

            return exhibitorAllSponsorAmount;
        }
    }
}