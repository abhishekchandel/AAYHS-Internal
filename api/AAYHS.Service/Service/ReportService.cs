using AAYHS.Core.DTOs.Response;
using AAYHS.Service.IService;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using AAYHS.Core.DTOs.Request;
using AAYHS.Repository.IRepository;

namespace AAYHS.Service.Service
{
    public class ReportService: IReportService
    {
        #region readonly       
        private IMapper _mapper;
        private IReportRepository _reportRepository;
        #endregion

        #region private        
        private MainResponse _mainResponse;
        #endregion

        public ReportService(IMapper Mapper, IReportRepository reportRepository)
        {
            _mapper = Mapper;
            _reportRepository = reportRepository;
            _mainResponse = new MainResponse();
        }

        public MainResponse GetExhibitorRegistrationReport(int exhibitorId)
        {
            var getReport = _reportRepository.GetExhibitorRegistrationReport(exhibitorId);

            _mainResponse.GetExhibitorRegistrationReport = getReport;
            _mainResponse.Success = true;
            return _mainResponse;
        }

        public MainResponse GetProgramsReport(int classId)
        {
            var getProgramReport = _reportRepository.GetProgramsReport(classId);

            _mainResponse.GetProgramReport = getProgramReport;
            _mainResponse.Success = true;
            return _mainResponse;
        }

        public MainResponse GetPaddockReport(int classId)
        {
            var getPaddockReport = _reportRepository.GetPaddockReport(classId);

            _mainResponse.GetPaddockReport = getPaddockReport;
            _mainResponse.Success = true;
            return _mainResponse;
        }

        public MainResponse GetAllClassesEntries()
        {
            var getAllClasses = _reportRepository.GetAllClassesEntries();

            _mainResponse.GetAllClassesEntries = getAllClasses;
            _mainResponse.Success = true;
            return _mainResponse;
        }
    }
}
