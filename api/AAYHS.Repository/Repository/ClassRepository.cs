﻿using AAYHS.Core.DTOs.Request;
using AAYHS.Core.DTOs.Response;
using AAYHS.Core.DTOs.Response.Common;
using AAYHS.Data.DBContext;
using AAYHS.Data.DBEntities;
using AAYHS.Repository.IRepository;
using AutoMapper;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AAYHS.Repository.Repository
{
    public class ClassRepository : GenericRepository<Classes>, IClassRepository
    {

        #region readonly
        private readonly AAYHSDBContext _ObjContext;
        private IMapper _Mapper;
        #endregion

        #region private 
        private MainResponse _mainResponse;
        #endregion

        public ClassRepository(AAYHSDBContext ObjContext, IMapper Mapper) :base(ObjContext)
        {
            _ObjContext = ObjContext;
            _Mapper = Mapper;
            _mainResponse = new MainResponse();
        }

        public  MainResponse GetAllClasses(ClassRequest classRequest)
        {
            IEnumerable<ClassResponse> data;
            GetAllClasses getAllClasses = new GetAllClasses();

            data=(from c in _ObjContext.Classes                  
                  where c.IsDeleted == false && c.IsActive==true                          
                  select new ClassResponse
                  { 
                      ClassId=c.ClassId,
                      ClassNumber=c.ClassNumber,
                      Name=c.Name,
                      Entries= _ObjContext.ExhibitorClass.Where(x=>x.ClassId==c.ClassId).Select(x=>x.ExhibitorClassId).Count(),
                      FromAge =c.FromAge,
                      ToAge=c.ToAge
                                 
                  });
            if (data.Count()!=0)
            {
                if (classRequest.OrderByDescending == true)
                {
                    data = data.OrderByDescending(x => x.GetType().GetProperty(classRequest.OrderBy).GetValue(x));
                }
                else
                {
                    data = data.OrderBy(x => x.GetType().GetProperty(classRequest.OrderBy).GetValue(x));
                }

                if (classRequest.AllRecords)
                {
                    getAllClasses.classResponses = data.ToList();
                }
                else
                {
                    getAllClasses.classResponses = data.Skip((classRequest.Page - 1) * classRequest.Limit).Take(classRequest.Limit).ToList();

                }
                _mainResponse.GetAllClasses = getAllClasses;
               
            }
            return _mainResponse;
        }
        public MainResponse GetClass(ClassRequest classRequest)
        {
            IEnumerable<GetClass> data;
            GetClass getClass = new GetClass();

            data = (from c in _ObjContext.Classes
                    join cs in _ObjContext.ClassSponsors on c.ClassId equals cs.ClassId
                    join s in _ObjContext.Sponsors on cs.SponsorId equals s.SponsorId
                    join sd in _ObjContext.ScheduleDates on c.ClassId equals sd.ClassId
                    where c.IsActive == true && c.IsDeleted == false && c.ClassId == classRequest.ClassId
                    select new GetClass 
                    {
                        ClassId=c.ClassId,
                        ClassNumber=c.ClassNumber,
                        Name=c.Name,
                        FromAge=c.FromAge,
                        ToAge=c.ToAge,
                        Location=c.Location,
                        SponsorName=s.FirstName+" "+s.LastName,
                        ScheduleDate=sd.Date,
                        SchedulTime=sd.Time
                    });
            if (data.Count()!=0)
            {
                 if (classRequest.OrderByDescending == true)
                {
                    data = data.OrderByDescending(x => x.GetType().GetProperty(classRequest.OrderBy).GetValue(x));
                }
                else
                {
                    data = data.OrderBy(x => x.GetType().GetProperty(classRequest.OrderBy).GetValue(x));
                }

                if (classRequest.AllRecords)
                {
                    getClass = data.FirstOrDefault();
                }
                else
                {
                    getClass = data.Skip((classRequest.Page - 1) * classRequest.Limit).Take(classRequest.Limit).FirstOrDefault();

                }
                _mainResponse.GetClass = getClass;
               
            }
            return _mainResponse;
        } 
        public async Task<MainResponse> CreateClass(AddClassRequest addClassRequest)
        {
            if (addClassRequest.ClassId == 0)
            {
              
                Classes classes = new Classes();
                classes.ClassNumber = addClassRequest.ClassNumber;
                classes.Name = addClassRequest.Name;
                classes.Location = addClassRequest.Location;
                classes.FromAge = addClassRequest.FromAge;
                classes.ToAge = addClassRequest.ToAge;
                classes.IsActive = true;
                classes.CreatedBy = addClassRequest.ActionBy;
                classes.CreatedDate = DateTime.Now;
                _ObjContext.Classes.Add(classes);
                await _ObjContext.SaveChangesAsync();
                int classId = classes.ClassId;

                ClassSponsor classSponsor = new ClassSponsor();
                classSponsor.SponsorId = addClassRequest.SponsorId;
                classSponsor.ClassId = classId;
                classSponsor.AgeFrom = addClassRequest.FromAge;
                classSponsor.AgeTo = addClassRequest.ToAge;
                classSponsor.IsActive = true;
                classSponsor.CreatedBy = addClassRequest.ActionBy;
                classSponsor.CreatedDate = DateTime.Now;
                _ObjContext.ClassSponsors.Add(classSponsor);
                await _ObjContext.SaveChangesAsync();

                
                ScheduleDates scheduleDates = new ScheduleDates();
                scheduleDates.ClassId = classId;
                scheduleDates.Date = addClassRequest.ScheduleDate;
                scheduleDates.Time = addClassRequest.ScheduleTime;
                scheduleDates.IsActive = true;
                scheduleDates.CreatedBy = addClassRequest.ActionBy;
                scheduleDates.CreatedDate = DateTime.Now;
                _ObjContext.ScheduleDates.Add(scheduleDates);
                await _ObjContext.SaveChangesAsync();

                _mainResponse.Success = true;

            }
           
            return _mainResponse;
        }
        public async Task<MainResponse> AddExhibitorToClass(AddClassExhibitor addClassExhibitor)
        {
            var addExhibitor = new ExhibitorClass
            {
                ExhibitorId = addClassExhibitor.ExhibitorId,
                ClassId = addClassExhibitor.ClassId,
                HorseId = addClassExhibitor.HorseId,
                IsActive = true,
                CreatedBy = addClassExhibitor.ActionBy,
                CreatedDate = DateTime.Now
            };
            _ObjContext.ExhibitorClass.Add(addExhibitor);
            await _ObjContext.SaveChangesAsync();

            _mainResponse.Success = true;
            return _mainResponse;
        }
        public MainResponse GetClassExhibitors(ClassRequest classRequest)
        {
            IEnumerable<GetClassExhibitor> data;
            GetAllClassExhibitor getAllClassExhibitor = new GetAllClassExhibitor();

            data = (from ec in _ObjContext.ExhibitorClass
                    join ex in _ObjContext.Exhibitors on ec.ExhibitorId equals ex.ExhibitorId
                    join h in _ObjContext.Horses on ec.HorseId equals h.HorseId
                    join epd in _ObjContext.ExhibitorPaymentDetails on ec.ExhibitorId equals epd.ExhibitorId
                    join f in _ObjContext.Fees on epd.FeeId equals f.FeeId
                    where ec.IsDeleted == false && ex.IsDeleted == false && ec.IsActive == true && ex.IsActive == true &&
                    ec.ClassId== classRequest.ClassId
                    select new GetClassExhibitor
                    {
                        ExhibitorClassId=ec.ExhibitorClassId,
                        ExhibitorNumber=ex.BackNumber,
                        ExhibitorName=ex.FirstName+" "+ex.LastName,
                        Horse=h.Name,
                        BirthYear=ex.BirthYear,
                        AmountPaid=epd.Amount,
                        AmountDue= ((int)(Convert.ToDecimal(f.FeeAmount)- epd.Amount))
                       
                    });
            if (data.Count() != 0)
            {
                if (classRequest.OrderByDescending == true)
                {   
                    data = data.OrderByDescending(x => x.GetType().GetProperty(classRequest.OrderBy).GetValue(x));
                }
                else
                {
                    data = data.OrderBy(x => x.GetType().GetProperty(classRequest.OrderBy).GetValue(x));
                }

                if (classRequest.AllRecords)
                {
                    getAllClassExhibitor.getClassExhibitors = data.ToList();
                }
                else
                {
                    getAllClassExhibitor.getClassExhibitors = data.Skip((classRequest.Page - 1) * classRequest.Limit).Take(classRequest.Limit).ToList();

                }
                _mainResponse.GetAllClassExhibitor = getAllClassExhibitor;

            }
            return _mainResponse;
        }
        public async Task<MainResponse> SplitClass(SplitRequest splitRequest)
        {
            var splitClass = new ClassSplits
            {
                ClassId = splitRequest.ClassId,
                SplitNumber = splitRequest.SplitNumber,
                Quantity = splitRequest.Quantity,
                IsActive = true,
                CreatedBy = splitRequest.ActionBy,
                CreatedDate = DateTime.Now
            };
            _ObjContext.ClassSplits.Add(splitClass);
            await _ObjContext.SaveChangesAsync();

            _mainResponse.Success = true;
            return _mainResponse;
        }
    }
}
