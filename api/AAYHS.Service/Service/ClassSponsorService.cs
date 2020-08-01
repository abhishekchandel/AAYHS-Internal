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
        private MainResponse _mainResponse;
        private IClassSponsorRepository _ClassSponsorRepository;
       
        #endregion

        public ClassSponsorService(IClassSponsorRepository ClassSponsorRepository,  IMapper Mapper)
        {
            _ClassSponsorRepository = ClassSponsorRepository;
          
            _Mapper = Mapper;
            _mainResponse = new MainResponse();
        }

        public MainResponse AddUpdateClassSponsor(ClassSponsorRequest request)
        {
            if (request.ClassSponsorId <= 0)
            {
                var checkexist = _ClassSponsorRepository.GetSingle(x => x.ClassId == request.ClassId && x.SponsorId==request.SponsorId);
                if (checkexist != null && checkexist.ClassSponsorId > 0)
                {
                    _mainResponse.Message = Constants.RECORD_AlREADY_EXIST;
                    _mainResponse.Success = false;
                }
                else
                {
                    var classsponsor = new ClassSponsors
                    {
                        SponsorId = request.SponsorId,
                        ClassId = request.ClassId,
                        CreatedDate = DateTime.Now
                    };
                    _ClassSponsorRepository.Add(classsponsor);
                    _mainResponse.Message = Constants.RECORD_ADDED_SUCCESS;
                    _mainResponse.Success = true;
                }
            }
            else
            {
                var classsponsor = _ClassSponsorRepository.GetSingle(x => x.ClassSponsorId == request.ClassSponsorId);
                if (classsponsor != null && classsponsor.ClassSponsorId > 0)
                {
                    classsponsor.SponsorId = request.SponsorId;
                    classsponsor.ClassId = request.ClassId;
                    classsponsor.ModifiedDate = DateTime.Now;
                    _ClassSponsorRepository.Update(classsponsor);
                    _mainResponse.Message = Constants.RECORD_UPDATE_SUCCESS;
                    _mainResponse.Success = true;
                }
                else
                {
                    _mainResponse.Message = Constants.NO_RECORD_EXIST_WITH_ID;
                    _mainResponse.Success = false;
                }
            }
            return _mainResponse;
        }

        public MainResponse GetClassSponsorbyId(int ClassSponsorId)
        {
            _mainResponse = _ClassSponsorRepository.GetClassSponsorbyId(ClassSponsorId);
            if (_mainResponse.ClassSponsorResponse != null)
            {
                _mainResponse.Message = Constants.RECORD_FOUND;
                _mainResponse.Success = true;
            }
            else
            {
                _mainResponse.Message = Constants.NO_RECORD_EXIST_WITH_ID;
                _mainResponse.Success = false;
            }
            return _mainResponse;
        }

        public MainResponse GetAllClassSponsor()
        {
            _mainResponse = _ClassSponsorRepository.GetAllClassSponsor();
            if (_mainResponse.ClassSponsorListResponse.classSponsorResponses != null && _mainResponse.ClassSponsorListResponse.classSponsorResponses.Count > 0)
            {
                _mainResponse.ClassSponsorListResponse.TotalRecords = _mainResponse.ClassSponsorListResponse.classSponsorResponses.Count();
                _mainResponse.Message = Constants.RECORD_FOUND;
                _mainResponse.Success = true;
            }
            return _mainResponse;
        }

        public MainResponse GetAllClassSponsorWithFilter(BaseRecordFilterRequest request)
        {
            _mainResponse = _ClassSponsorRepository.GetAllClassSponsorWithFilters(request);
            if (_mainResponse.ClassSponsorListResponse.classSponsorResponses != null && _mainResponse.ClassSponsorListResponse.classSponsorResponses.Count > 0)
            {
                _mainResponse.ClassSponsorListResponse.TotalRecords = _mainResponse.ClassSponsorListResponse.classSponsorResponses.Count();
                   _mainResponse.Message = Constants.RECORD_FOUND;
                _mainResponse.Success = true;
            }
            return _mainResponse;
        }
      
        public MainResponse DeleteClassSponsor(int ClassSponsorId)
        {
            var classsponsor = _ClassSponsorRepository.GetSingle(x => x.ClassSponsorId == ClassSponsorId);
            if (classsponsor != null)
            {
                classsponsor.IsDeleted = true;
                classsponsor.DeletedDate = DateTime.Now;
                _ClassSponsorRepository.Update(classsponsor);
                _mainResponse.Message = Constants.RECORD_DELETE_SUCCESS;
                _mainResponse.Success = true;
            }
            else
            {
                _mainResponse.Message = Constants.NO_RECORD_EXIST_WITH_ID;
                _mainResponse.Success = false;
            }
            return _mainResponse;
        }

        public MainResponse GetSponsorClassesbySponsorId(int SponsorId)
        {
            _mainResponse = _ClassSponsorRepository.GetSponsorClassesbySponsorId(SponsorId);
            if (_mainResponse.SponsorClassesListResponse.sponsorClassesListResponses != null && _mainResponse.SponsorClassesListResponse.sponsorClassesListResponses.Count > 0)
            {
                _mainResponse.SponsorClassesListResponse.TotalRecords = _mainResponse.SponsorClassesListResponse.sponsorClassesListResponses.Count();
                   _mainResponse.Message = Constants.RECORD_FOUND;
                _mainResponse.Success = true;
            }
            return _mainResponse;
        }
    }
}
