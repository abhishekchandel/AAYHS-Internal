using AAYHS.Core.DTOs.Request;
using AAYHS.Core.DTOs.Response;
using AAYHS.Core.Shared.Static;
using AAYHS.Data.DBEntities;
using AAYHS.Repository.IRepository;
using AAYHS.Repository.Repository;
using AAYHS.Service.IService;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;


namespace AAYHS.Service.Service
{
   public class ClassSponsorService: IClassSponsorService
    {
        #region readonly
        private readonly IMapper _Mapper;
        #endregion

        #region private
        private MainResponse _MainResponse;
        private IClassSponsorRepository _ClassSponsorRepository;
       
        #endregion

        public ClassSponsorService(IClassSponsorRepository ClassSponsorRepository,  IMapper Mapper)
        {
            _ClassSponsorRepository = ClassSponsorRepository;
          
            _Mapper = Mapper;
            _MainResponse = new MainResponse();
        }

        public MainResponse AddClassSponsor(ClassSponsorRequest request)
        {
            var classsponsor = new ClassSponsors
            {
                SponsorId = request.SponsorId,
                ClassId = request.ClassId,
                AgeGroup = request.AgeGroup,
                
                CreatedDate = DateTime.Now
            };
            _ClassSponsorRepository.Add(classsponsor);
            _MainResponse.Message = Constants.RECORD_ADDED_SUCCESS;
            _MainResponse.Success = true;
            return _MainResponse;
        }

        public MainResponse UpdateClassSponsor(ClassSponsorRequest request)
        {
            var classsponsor = _ClassSponsorRepository.GetSingle(x => x.ClassSponsorId == request.ClassSponsorId);
                classsponsor.SponsorId = request.SponsorId;
                classsponsor.ClassId = request.ClassId;
                classsponsor.AgeGroup = request.AgeGroup;
                classsponsor.CreatedDate = DateTime.Now;
          
            _ClassSponsorRepository.Update(classsponsor);
            _MainResponse.Message = Constants.RECORD_UPDATE_SUCCESS;
            _MainResponse.Success = true;
            return _MainResponse;
        }

        public MainResponse GetClassSponsorbyId(GetClassSponsorRequest request)
        {
            var classsponsor = _ClassSponsorRepository.GetSingle(x => x.ClassSponsorId == request.ClassSponsorId);
            _MainResponse.ClassSponsorResponse = _Mapper.Map<ClassSponsorResponse>(classsponsor);
            _MainResponse.Message = Constants.RECORD_FOUND;
            _MainResponse.Success = true;
            return _MainResponse;
        }

        public MainResponse GetAllClassSponsor()
        {
            var classsponsor = _ClassSponsorRepository.GetAll(x => x.IsActive == true && x.IsDeleted==false);
            _MainResponse.ClassSponsorListResponse = _Mapper.Map<List<ClassSponsorResponse>>(classsponsor);
            _MainResponse.Message = Constants.RECORD_FOUND;
            _MainResponse.Success = true;
            return _MainResponse;
        }

        public MainResponse GetAllClassSponsorWithFilter(BaseRecordFilterRequest request)
        {
            var classsponsor = _ClassSponsorRepository.GetRecordsWithFilters(request.Page,request.Limit,request.OrderBy,request.OrderByDescending,request.AllRecords,x => x.IsActive == true && x.IsDeleted == false);
            _MainResponse.ClassSponsorListResponse = _Mapper.Map<List<ClassSponsorResponse>>(classsponsor);
            _MainResponse.Message = Constants.RECORD_FOUND;
            _MainResponse.Success = true;
            return _MainResponse;
        }
      
        public MainResponse DeleteClassSponsor(GetClassSponsorRequest request)
        {
            var classsponsor = _ClassSponsorRepository.GetSingle(x => x.ClassSponsorId == request.ClassSponsorId);
            if (classsponsor != null)
            {
                classsponsor.IsDeleted = true;
                classsponsor.DeletedDate = DateTime.Now;
                _ClassSponsorRepository.Update(classsponsor);
                _MainResponse.Message = Constants.RECORD_DELETE_SUCCESS;
                _MainResponse.Success = true;
            }
            else
            {
                _MainResponse.Message = Constants.NO_RECORD_Exist_WITH_ID;
                _MainResponse.Success = false;
            }
            return _MainResponse;
        }
    }
}
