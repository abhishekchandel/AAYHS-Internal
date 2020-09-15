using AAYHS.Core.DTOs.Response;
using AAYHS.Core.DTOs.Response.Common;
using AAYHS.Core.Shared.Static;
using AAYHS.Repository.IRepository;
using AAYHS.Service.IService;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AAYHS.Service.Service
{
    public class GlobalCodeService: IGlobalCodeService
    {
        #region readonly
        private readonly IGlobalCodeRepository _globalCodeRepository;
        private IStateRepository _stateRepository;
        private ICityRepository _cityRepository;
        private IZipCodeRepository _zipCodeRepository;
        private readonly IMapper _mapper;
        #endregion

        #region Private
        private MainResponse _mainResponse;
        #endregion

        public GlobalCodeService(IGlobalCodeRepository GlobalCodeRepository, IStateRepository stateRepository,ICityRepository cityRepository, IZipCodeRepository zipCodeRepository
                                 , IMapper Mapper)
        {
            _globalCodeRepository = GlobalCodeRepository;
            _stateRepository = stateRepository;
            _cityRepository = cityRepository;
            _zipCodeRepository = zipCodeRepository;
            _mapper = Mapper;
            _mainResponse = new MainResponse();
        }

        public MainResponse GetGlobalCode(string categoryName)
        {
            var globalCodeResponse =  _globalCodeRepository.GetCodes(categoryName);
            if (globalCodeResponse.totalRecords != 0)
            {

                _mainResponse.Success = true;
                _mainResponse.GlobalCodeMainResponse = globalCodeResponse;
            }
            else
            {
                _mainResponse.Message = Constants.NO_RECORD_FOUND;
                _mainResponse.Success = false;
            }
            return _mainResponse;
        }       
        public MainResponse GetAllStates()
        {
            var states = _stateRepository.GetAll(x => x.IsDeleted == false && x.IsActive == true).OrderBy(x => x.Name);
            var stateResponse = _mapper.Map<List<State>>(states);
            StateResponse response = new StateResponse();
            response.State = stateResponse;
            _mainResponse.StateResponse = response;
            _mainResponse.Success = true;
            return _mainResponse;

        }
        public MainResponse GetAllCities(int StateId)
        {
            var cities = _cityRepository.GetAll(x => x.StateId == StateId && x.IsDeleted == false && x.IsActive == true).OrderBy(x => x.Name);
            var cityResponse = _mapper.Map<List<City>>(cities);
            CityResponse city = new CityResponse();
            city.City = cityResponse;
            _mainResponse.CityResponse = city;
            _mainResponse.Success = true;
            return _mainResponse;
        }
        public MainResponse GetAllZipCodes(int CityId)
        {
            var zipCodes = _zipCodeRepository.GetAll(x => x.CityId == CityId && x.IsDeleted == false && x.IsActive == true).OrderBy(x => x.Number);
            var zipCodeResponse = _mapper.Map<List<ZipCode>>(zipCodes);
            ZipCodeResponse response = new ZipCodeResponse();
            response.ZipCode = zipCodeResponse;
            _mainResponse.ZipCodeResponse = response;
            _mainResponse.Success = true;
            return _mainResponse;
        }

    }
}
