using AAYHS.Core.DTOs.Response;
using AAYHS.Service.IService;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using AAYHS.Core.DTOs.Request;
using AAYHS.Repository.IRepository;
using System.IO;
using System.Linq;
using AAYHS.Core.Shared.Static;

namespace AAYHS.Service.Service
{
    public class ReportService : IReportService
    {
        #region readonly       
        private IMapper _mapper;
        private IReportRepository _reportRepository;
        #endregion

        #region private        
        private MainResponse _mainResponse;
        private IApplicationSettingRepository _applicationRepository;
        private IEmailSenderRepository _emailSenderRepository;
        #endregion

        public ReportService(IReportRepository reportRepository, IApplicationSettingRepository applicationRepository, 
                            IEmailSenderRepository emailSenderRepository, IMapper Mapper)
        {
            _reportRepository = reportRepository;
            _mapper = Mapper;
            _mainResponse = new MainResponse();
            _applicationRepository = applicationRepository;
            _emailSenderRepository = emailSenderRepository;
        }

        public MainResponse GetExhibitorRegistrationReport(int exhibitorId)
        {
            var getReport = _reportRepository.GetExhibitorRegistrationReport(exhibitorId);

            _mainResponse.GetExhibitorRegistrationReport = getReport;
            _mainResponse.Success = true;
            return _mainResponse;
        }

        public MainResponse GetExhibitorGroupInformationReport(int groupId)
        {
            var getReport = _reportRepository.GetExhibitorGroupInformationReport(groupId);

            _mainResponse.GetExhibitorGroupInformationReport = getReport;
            _mainResponse.Success = true;
            return _mainResponse;
        }

        public MainResponse GetExhibitorGroupInformationReportForAllGroups()
        {
            var getReport = _reportRepository.GetExhibitorGroupInformationReportForAllGroups();
            _mainResponse.GetExhibitorGroupInformationReportForAllGroups = getReport;
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

        public MainResponse GetProgramReportOfAllClasses()
        {
            var programReport = _reportRepository.GetProgramReportOfAllClasses();

            _mainResponse.GetProgramReportOfAllClasses = programReport;
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

        public MainResponse GetPaddockReportOfAllClasses()
        {
            var paddockReport = _reportRepository.GetPaddockReportOfAllClasses();

            _mainResponse.GetPaddockReportOfAllClasses = paddockReport;
            _mainResponse.Success = true;
            return _mainResponse;
        }

        public MainResponse GetAllClassesEntries(int classId)
        {
            var getAllClasses = _reportRepository.GetAllClassesEntries(classId);

            _mainResponse.GetAllClassesEntries = getAllClasses;
            _mainResponse.Success = true;
            return _mainResponse;
        }

        public GetClassResultReport GetClassResultReport()
        {
            var getClassResultReport = _reportRepository.GetClassResultReport();
            getClassResultReport.Success = true;
            return getClassResultReport;
        }

        public MainResponse GetSingleClassResult(int classId)
        {
            var getClassResult = _reportRepository.GetSingleClassResult(classId);

            _mainResponse.GetSingleClassResult = getClassResult;
            _mainResponse.Success = true;
            return _mainResponse;
        }

        public MainResponse GetExhibitorSponsorConfirmation(HorseExhibitorsRequest request)
        {

            var getResult = _reportRepository.GetExhibitorSponsorConfirmation(request);

            _mainResponse.GetExhibitorSponsorConfirmationReport = getResult;
            _mainResponse.Success = true;
            return _mainResponse;

        }

        public MainResponse GetExhibitorSponsorConfirmationReportForAllExhibitors()
        {
            var getResult = _reportRepository.GetExhibitorSponsorConfirmationReportForAllExhibitors();
            _mainResponse.GetExhibitorSponsorConfirmationReportForAllExhibitors = getResult;
            _mainResponse.Success = true;
            return _mainResponse;

        }

        public MainResponse GetExhibitorSponsorRefundReport(int exhibitorId)
        {
            var getExhibitorSponsorRefund = _reportRepository.GetExhibitorSponsorRefundReport(exhibitorId);
            _mainResponse.GetExhibitorSponsorRefundStatement = getExhibitorSponsorRefund;
            _mainResponse.Success = true;
            return _mainResponse;
        }

        public MainResponse SaveAndEmail(string fileBase64, string emailid)
        {
            try
            {
                _mainResponse.Message = "";
                var base64 = fileBase64.Split(",").Last();
                
                var base64firstdata = fileBase64.Split(",").First();
                var base64firstfiletype = base64firstdata.Split(";").First();
                var index = base64firstfiletype.IndexOf("/") + 1;
                var length = base64firstfiletype.Length;
                var type = base64firstfiletype.Substring(index, length-index);



                var filePath = SaveFileFromBase64(base64, type);
                string guid = Guid.NewGuid().ToString();

                //get email settings
                var settings = _applicationRepository.GetAll().FirstOrDefault();

                // Send Email with document
                EmailRequest email = new EmailRequest();
                email.To = emailid;
                email.SenderEmail = settings.CompanyEmail;
                email.CompanyEmail = settings.CompanyEmail;
                email.CompanyPassword = settings.CompanyPassword;
                email.Url = settings.ResetPasswordUrl;
                email.Token = guid;
                email.TemplateType = "Email With Document";

                _emailSenderRepository.SendEmailWithDocument(email, filePath);
                if (File.Exists(filePath))
                {
                    System.GC.Collect();
                    System.GC.WaitForPendingFinalizers();
                    File.Delete(filePath);
                }
                _mainResponse.Message = "Email Sent";
            }
            catch
            {
                _mainResponse.Message = "Some Error Occurred";
            }
            return _mainResponse;
        }
      
        private string SaveFileFromBase64(string file, string type)
        {
            string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            var basePath = Path.Combine(uploadsFolder, "Resources", "pdf");

            byte[] imageBytes = Convert.FromBase64String(file);
            MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
            string newFile = "";

            if (type.ToUpper() == "PNG")
            {
                newFile = Guid.NewGuid().ToString() + ".png";
            }
            else if (type.ToUpper() == "JPG" || type.ToUpper() == "JPEG")
            {
                newFile = Guid.NewGuid().ToString() + ".jpg";
            }
            else if (type.ToUpper() == "PLAIN")
            {
                newFile = Guid.NewGuid().ToString() + ".txt";
            }
            else
            {
                newFile = Guid.NewGuid().ToString() + "." + type;
            }

            var path = Path.Combine(basePath, newFile);
            bool exists = System.IO.Directory.Exists(basePath);
            if (!exists)
            {
                System.IO.Directory.CreateDirectory(basePath);
            }
            if (imageBytes.Length > 0)
            {
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    stream.Write(imageBytes, 0, imageBytes.Length);
                    stream.Flush();
                }
            }

            //path = path.Replace(basePath, "").Replace("\\", "/");
            return path;
        }

        public MainResponse AllPatronSponsorsYearly()
        {
            var allSponsors = _reportRepository.AllPatronSponsorsYearly();

            _mainResponse.AllSponsorsYealry = allSponsors;
            _mainResponse.Success = true;
            return _mainResponse;
        }

        public MainResponse ShowSummaryReport()
        {
            var summaryReport = _reportRepository.ShowSummaryReport();

            _mainResponse.ShowSummaryReport = summaryReport;
            _mainResponse.Success = true;
            return _mainResponse;
        }

        public MainResponse GetExhibitorsSponsorRefundReport()
        {
            var exhibitorsSponsorRefund = _reportRepository.GetExhibitorsSponsorRefundReport();

            _mainResponse.GetExhibitorsSponsorRefundReport = exhibitorsSponsorRefund;
            _mainResponse.Success = true;
            return _mainResponse;
        }

        public MainResponse GetAdministrativeReport()
        {
            var administrativeReport = _reportRepository.GetAdministrativeReport();

            _mainResponse.GetAdministrativeReport = administrativeReport;
            _mainResponse.Success = true;
            return _mainResponse;
        }

        public MainResponse GetNSBAExhibitorsFeeReport()
        {
            var report = _reportRepository.GetNSBAExhibitorsFeeReport();

            _mainResponse.GetNSBAExhibitorFee = report;
            _mainResponse.Success = true;
            return _mainResponse;
        }

        public MainResponse GetNSBAandClassesExhibitorsFeeReport()
        {
            var report = _reportRepository.GetNSBAandClassesExhibitorsFeeReport();

            _mainResponse.GetNSBAExhibitorFee = report;
            _mainResponse.Success = true;
            return _mainResponse;
        }

        public GetClassResultReport GetNSBAClassesResultReport()
        {
            var getNSBAClassResultReport = _reportRepository.GetNSBAClassesResultReport();
            getNSBAClassResultReport.Success = true;
            return getNSBAClassResultReport;

        }

        public MainResponse GetNonExhibitorSponsor(int sponsorId)
        {
            var nonExhibitorSponsor = _reportRepository.GetNonExhibitorSponsor(sponsorId);
            _mainResponse.Success = true;
            _mainResponse.GetNonExhibitorSponsor = nonExhibitorSponsor;
            return _mainResponse;
        }

        public MainResponse GetAllNonExhibitorSponsors()
        {
            var nonExhibitorSponsors = _reportRepository.GetAllNonExhibitorSponsors();
            _mainResponse.Success = true;
            _mainResponse.GetAllNonExhibitorSponsors = nonExhibitorSponsors;
            return _mainResponse;
        }

        public MainResponse GetExhibiorAdsSponsorReport()
        {
            var _report = _reportRepository.GetExhibiorAdsSponsorReport();
            _mainResponse.ExhibiorAdsSponsorReportListResponse = _report;
            _mainResponse.Success = true;
            return _mainResponse;
        }

        public MainResponse GetNonExhibiorSummarySponsorDistributionsReport()
        {
            var _report = _reportRepository.GetNonExhibiorSummarySponsorDistributionsReport();
            _mainResponse.NonExhibiorSummarySponsorDistributionsListResponse = _report;
            _mainResponse.Success = true;
            return _mainResponse;
        }

        public MainResponse GetAllExhibitorSponsoredAd()
        {
            var sponsors = _reportRepository.GetAllExhibitorSponsoredAd();
            _mainResponse.GetAllExhibitorSponsoredAd = sponsors;
            _mainResponse.Success = true;
            return _mainResponse;
        }

        public MainResponse GetAllNonExhibitorSponsorAd()
        {
            var sponsors = _reportRepository.GetAllNonExhibitorSponsorAd();
            _mainResponse.GetAllNonExhibitorSponsorAd = sponsors;
            _mainResponse.Success = true;
            return _mainResponse;
        }

    }
}
