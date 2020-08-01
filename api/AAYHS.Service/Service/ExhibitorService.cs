using AAYHS.Core.DTOs.Request;
using AAYHS.Core.DTOs.Response;
using AAYHS.Core.Shared.Static;
using AAYHS.Data.DBEntities;
using AAYHS.Repository.IRepository;
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
  public  class ExhibitorService: IExhibitorService
    {
        #region readonly
        private readonly IMapper _Mapper;
        #endregion

        #region private
        private MainResponse _mainResponse;
        private IExhibitorRepository _ExhibitorRepository;
        #endregion

        public ExhibitorService(IExhibitorRepository ExhibitorRepository, IMapper Mapper)
        {
            _ExhibitorRepository = ExhibitorRepository;
            _Mapper = Mapper;
            _mainResponse = new MainResponse();
        }

        public MainResponse AddUpdateExhibitor(ExhibitorRequest request)
        {
            if (request.ExhibitorId <= 0)
            {
                var Exhibitor = _Mapper.Map<Exhibitors>(request);
                _ExhibitorRepository.Add(Exhibitor);
                _mainResponse.Message = Constants.RECORD_ADDED_SUCCESS;
                _mainResponse.Success = true;
            }
            else
            {
                var Exhibitor = _Mapper.Map<Exhibitors>(request);
                Exhibitor.ModifiedDate = DateTime.Now;
                _ExhibitorRepository.Update(Exhibitor);
                _mainResponse.Message = Constants.RECORD_UPDATE_SUCCESS;
                _mainResponse.Success = true;
            }
            return _mainResponse;
        }

        public MainResponse GetAllExhibitorsWithFilter(BaseRecordFilterRequest request)
        {
            _mainResponse = _ExhibitorRepository.GetAllExhibitorsWithFilters(request);
            if (_mainResponse.ExhibitorListResponse.exhibitorResponses != null && _mainResponse.ExhibitorListResponse.exhibitorResponses.Count > 0)
            {
                _mainResponse.ExhibitorListResponse.TotalRecords = _mainResponse.ExhibitorListResponse.exhibitorResponses.Count();
                _mainResponse.Message = Constants.RECORD_FOUND;
                _mainResponse.Success = true;
            }
            else
            {
                _mainResponse.Message = Constants.NO_RECORD_FOUND;
                _mainResponse.Success = false;
            }
            return _mainResponse;
        }

        public MainResponse GetAllExhibitors()
        {
            _mainResponse = _ExhibitorRepository.GetAllExhibitors();
            if (_mainResponse.ExhibitorListResponse.exhibitorResponses != null && _mainResponse.ExhibitorListResponse.exhibitorResponses.Count > 0)
            {
                _mainResponse.ExhibitorListResponse.TotalRecords = _mainResponse.ExhibitorListResponse.exhibitorResponses.Count();
                _mainResponse.Message = Constants.RECORD_FOUND;
                _mainResponse.Success = true;
            }
            else
            {
                _mainResponse.Message = Constants.NO_RECORD_FOUND;
                _mainResponse.Success = false;
            }
            return _mainResponse;
        }

        public MainResponse GetExhibitorById(GetExhibitorRequest request)
        {
            var data = _ExhibitorRepository.GetSingle(x => x.ExhibitorId == request.ExhibitorId && x.IsActive == true && x.IsDeleted == false);
            if (data != null && data.ExhibitorId > 0)
            {
                _mainResponse.ExhibitorResponse = _Mapper.Map<ExhibitorResponse>(data);
                _mainResponse.Message = Constants.RECORD_FOUND;
                _mainResponse.Success = true;
            }
            else
            {
                _mainResponse.Message = Constants.NO_RECORD_FOUND;
                _mainResponse.Success = false;
            }

            return _mainResponse;
        }

        public MainResponse DeleteExhibitor(GetExhibitorRequest request)
        {
            var Exhibitor = _ExhibitorRepository.GetSingle(x => x.ExhibitorId == request.ExhibitorId);
            if (Exhibitor != null && Exhibitor.ExhibitorId>0)
            {
                Exhibitor.IsDeleted = true;
                Exhibitor.IsActive = false;
                Exhibitor.DeletedDate = DateTime.Now;
                _ExhibitorRepository.Update(Exhibitor);
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
    }
}
