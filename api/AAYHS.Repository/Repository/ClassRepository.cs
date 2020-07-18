using AAYHS.Core.DTOs.Request;
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
                  join ec in _ObjContext.ExhibitorClass on c.ClassId equals ec.ClassId into ecc
                  where c.IsDeleted==false
                  select new ClassResponse
                  { 
                      ClassId=c.ClassId,
                      ClassNumber=c.ClassNumber,
                      Name=c.Name,
                      FromAge=c.FromAge,
                      ToAge=c.ToAge,
                      Entries= ecc.Where(x=>x.ClassId==c.ClassId).Count(),
                    
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
        public async Task<MainResponse> CreateClass(AddClassRequest addClassRequest)
        {
            if (addClassRequest.ClassId != 0)
            {
                Classes classes = new Classes();
                classes.SponsorId = addClassRequest.SponsorId;
                classes.ClassNumber = addClassRequest.ClassNumber;
                classes.Name = addClassRequest.Name;
                classes.Location = addClassRequest.Location;
                classes.FromAge = addClassRequest.FromAge;
                classes.ToAge = addClassRequest.ToAge;
                classes.IsActive = true;
                classes.CreatedBy = addClassRequest.CreatedBy;
                classes.CreatedDate = DateTime.Now;
                _ObjContext.Classes.Add(classes);
                await _ObjContext.SaveChangesAsync();


                int classId = classes.ClassId;
                ScheduleDates scheduleDates = new ScheduleDates();
                scheduleDates.ClassId = classId;
                scheduleDates.Date = addClassRequest.ScheduleDate;
                scheduleDates.Time = addClassRequest.ScheduleTime;
                scheduleDates.IsActive = true;
                scheduleDates.CreatedBy = addClassRequest.CreatedBy;
                scheduleDates.CreatedDate = DateTime.Now;
                _ObjContext.ScheduleDates.Add(scheduleDates);
                await _ObjContext.SaveChangesAsync();

                _mainResponse.Success = true;
                
            }
            return _mainResponse;
        }
    }
}
