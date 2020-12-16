using AAYHS.Core.DTOs.Request;
using AAYHS.Core.DTOs.Response;
using AAYHS.Data.DBContext;
using AAYHS.Data.DBEntities;
using AAYHS.Repository.IRepository;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.IO;

namespace AAYHS.Repository.Repository
{
    public class ScanRepository : GenericRepository<Scans>, IScanRepository
    {
        #region readonly
        private readonly IMapper _Mapper;
        #endregion

        #region Private
        private MainResponse _mainResponse;
        #endregion

        #region public
        public AAYHSDBContext _objContext;
        private IGlobalCodeRepository _globalCodeRepository;
        #endregion

        public ScanRepository(AAYHSDBContext ObjContext, IMapper Mapper) : base(ObjContext)
        {
            _mainResponse = new MainResponse();
            _objContext = ObjContext;
            _Mapper = Mapper;
        }

        public GetAllScan GetAllScan(GetScanRequest getScanRequest)
        {
            GetAllScan getAllScan = new GetAllScan();
            IEnumerable<GetScan> data;

            var getYear = _objContext.YearlyMaintainence.Where(x => x.YearlyMaintainenceId == getScanRequest.YearlyMaintenanceId).FirstOrDefault();

            data = (from scan in _objContext.Scans
                    where scan.IsActive == true && scan.IsDeleted == false
                    && scan.DocumentType == getScanRequest.DocumentTypeId
                    && scan.CreatedDate.Value.Year == getYear.Years
                    select new GetScan
                    {
                        ScanId = scan.ScansId,
                        ExhibitorId = scan.ExhibitorId,
                        DocumentName = scan.DocumentPath != ""? Path.GetFileNameWithoutExtension(scan.DocumentPath):"",
                        DocumentPath = scan.DocumentPath
                    });

            if (data.Count()!=0)
            {
                getAllScan.getScans = data.ToList();
            }

            return getAllScan;
        }
    }
}
