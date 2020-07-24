using AAYHS.Core.DTOs.Request;
using AAYHS.Core.DTOs.Response;
using AAYHS.Core.Shared.Static;
using AAYHS.Repository.IRepository;
using AAYHS.Service.IService;
using AutoMapper;
using System;
using System.Threading.Tasks;

namespace AAYHS.Service.Service
{
    public class ClassService : IClassService
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
            if (allClasses.GetAllClasses.classResponses.Count != 0)
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
            if (_mainResponse.Success == true)
            {
                _mainResponse.Message = Constants.CLASS_CREATED;
            }
            return _mainResponse;
        }
        public async Task<MainResponse> AddExhibitorToClass(AddClassExhibitor addClassExhibitor)
        {
            _mainResponse = await _classRepository.AddExhibitorToClass(addClassExhibitor);
            if (_mainResponse.Success == true)
            {
                _mainResponse.Message = Constants.CLASS_EXHIBITOR;
            }
            return _mainResponse;
        }

        public MainResponse GetClassExhibitors(ClassRequest classRequest)
        {
            _mainResponse = _classRepository.GetClassExhibitors(classRequest);
            return _mainResponse;
        }

        public async Task<MainResponse> RemoveClass(RemoveClass removeClass)
        {
            var _class = _classRepository.GetSingle(x => x.ClassId == removeClass.ClassId);
            if (_class != null)
            {
                _class.IsDeleted = true;
                _class.DeletedBy = removeClass.ActionBy;
                _class.DeletedDate = DateTime.Now;
                await _classRepository.UpdateAsync(_class);

                _mainResponse.Success = true;
                _mainResponse.Message = Constants.CLASS_REMOVED;
            }
            else
            {
                _mainResponse.Success = true;
                _mainResponse.Message = Constants.NO_RECORD_FOUND;
            }

            return _mainResponse;
        }

        public async Task<MainResponse> SplitClass(SplitRequest splitRequest)
        {
            _mainResponse = await _classRepository.SplitClass(splitRequest);
            if (_mainResponse.Success == true)
            {
                _mainResponse.Message = Constants.SPLIT_CREATED;
            }
            return _mainResponse;
        }
    }
}
