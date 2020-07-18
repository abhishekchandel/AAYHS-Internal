using AAYHS.Core.DTOs.Request;
using AAYHS.Core.DTOs.Response;
using AAYHS.Core.DTOs.Response.Common;
using AAYHS.Core.Shared.Static;
using AAYHS.Data.DBEntities;
using AAYHS.Repository.IRepository;
using AAYHS.Repository.Repository;
using AAYHS.Service.IService;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;

namespace AAYHS.Service.Service
{
    public class ClassService: IClassService
    {
        #region readonly
        private readonly IClassRepository _classRepository;
        private IMapper _mapper;
        #endregion

        #region private
        private MainResponse _mainResponse;
        #endregion

        public ClassService(IClassRepository classRepository, IMapper Mapper)
        {
            _classRepository = classRepository;
            _mapper = Mapper;
            _mainResponse = new MainResponse();
        }

        public MainResponse GetAllClasses(ClassRequest classRequest)
        {
            var allClasses = _classRepository.GetAllClasses(classRequest);
            if (allClasses.GetAllClasses.classResponses.Count!=0)
            {
                _mainResponse.GetAllClasses = allClasses.GetAllClasses;
                _mainResponse.Success = true;
            }
            else
            {
                _mainResponse.Message = Constants.NO_RECORD_FOUND;
                _mainResponse.Success = false;
            }
            return _mainResponse;
        }
        public async Task<MainResponse> CreateClass(AddClassRequest addClassRequest)
        {
            _mainResponse = await _classRepository.CreateClass(addClassRequest);
            if (_mainResponse.Success==true)
            {
                _mainResponse.Message = Constants.CLASS_CREATED;
            }
            return _mainResponse;
        }
    }
}
