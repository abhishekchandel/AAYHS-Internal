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
        private MainResponse _MainResponse;
        private IExhibitorRepository _ExhibitorRepository;
        #endregion

        public ExhibitorService(IExhibitorRepository ExhibitorRepository, IMapper Mapper)
        {
            _ExhibitorRepository = ExhibitorRepository;
            _Mapper = Mapper;
            _MainResponse = new MainResponse();
        }

        public MainResponse AddExhibitor(ExhibitorRequest request)
        {
            var Exhibitor = _Mapper.Map<Exhibitors>(request);
            _ExhibitorRepository.Add(Exhibitor);
            _MainResponse.Message = Constants.RECORD_ADDED_SUCCESS;
            _MainResponse.Success = true;
            return _MainResponse;
        }

        public MainResponse GetAllExhibitorsWithFilter(BaseRecordFilterRequest request)
        {
            var data = _ExhibitorRepository.GetRecordsWithFilters(request.Page, request.Limit, request.OrderBy, request.OrderByDescending, request.AllRecords, x => x.IsActive == true && x.IsDeleted == false);
            _MainResponse.TotalRecords = data.Count();
            if (_MainResponse.TotalRecords != 0)
            {
                _MainResponse.Data.ExhibitorListResponse = _Mapper.Map<List<ExhibitorResponse>>(data);
                _MainResponse.Message = Constants.RECORD_FOUND;
                _MainResponse.Success = true;
            }
            else
            {
                _MainResponse.Message = Constants.NO_RECORD_FOUND;
                _MainResponse.Success = false;
            }


            return _MainResponse;
        }

        public MainResponse GetAllExhibitors()
        {
            var data = _ExhibitorRepository.GetAll(x => x.IsActive == true && x.IsDeleted == false);
            _MainResponse.TotalRecords = data.Count();
            if (_MainResponse.TotalRecords != 0)
            {
                _MainResponse.Data.ExhibitorListResponse = _Mapper.Map<List<ExhibitorResponse>>(data); ;
                _MainResponse.Message = Constants.RECORD_FOUND;
                _MainResponse.Success = true;
            }
            else
            {
                _MainResponse.Message = Constants.NO_RECORD_FOUND;
                _MainResponse.Success = false;
            }
            return _MainResponse;
        }

        public MainResponse GetExhibitorById(GetExhibitorRequest request)
        {
            var data = _ExhibitorRepository.GetSingle(x => x.ExhibitorId == request.ExhibitorId && x.IsActive == true && x.IsDeleted == false);
            if (data != null)
            {
                _MainResponse.Data.ExhibitorResponse = _Mapper.Map<ExhibitorResponse>(data);
                _MainResponse.Message = Constants.RECORD_FOUND;
                _MainResponse.Success = true;
            }

            _MainResponse.Message = Constants.NO_RECORD_FOUND;
            _MainResponse.Success = false;

            return _MainResponse;
        }

        public MainResponse UpdateExhibitor(ExhibitorRequest request)
        {

            var Exhibitor = _Mapper.Map<Exhibitors>(request);
            Exhibitor.ModifiedDate = DateTime.Now;
            _ExhibitorRepository.Update(Exhibitor);
            _MainResponse.Message = Constants.RECORD_UPDATE_SUCCESS;
            _MainResponse.Success = true;
            return _MainResponse;
        }

        public MainResponse DeleteExhibitor(GetExhibitorRequest request)
        {
            var Exhibitor = _ExhibitorRepository.GetSingle(x => x.ExhibitorId == request.ExhibitorId);
            if (Exhibitor != null)
            {
                Exhibitor.IsDeleted = true;
                Exhibitor.DeletedDate = DateTime.Now;
                _ExhibitorRepository.Update(Exhibitor);
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
