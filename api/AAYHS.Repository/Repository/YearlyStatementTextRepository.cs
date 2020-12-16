using AAYHS.Core.DTOs.Response;
using AAYHS.Data.DBContext;
using AAYHS.Data.DBEntities;
using AAYHS.Repository.IRepository;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AAYHS.Repository.Repository
{
    public class YearlyStatementTextRepository:GenericRepository<YearlyStatementText>, IYearlyStatementTextRepository
    {
        #region readonly
        private readonly AAYHSDBContext _ObjContext;
        private IMapper _Mapper;
        #endregion

        #region private 
        private MainResponse _MainResponse;
        #endregion

        public YearlyStatementTextRepository(AAYHSDBContext ObjContext, IMapper Mapper) : base(ObjContext)
        {
            _ObjContext = ObjContext;
            _Mapper = Mapper;
            _MainResponse = new MainResponse();
        }

        public GetAllStatementText GetYearlyStatementText(int yearlyMaintenanceId)
        {
            GetAllStatementText getAllStatementText = new GetAllStatementText();
            IEnumerable<GetStatementText> data;

            data = (from statement in _ObjContext.YearlyStatementText
                    where statement.IsActive == true && statement.IsDeleted == false
                    && statement.YearlyMaintenanceId == yearlyMaintenanceId
                    select new GetStatementText
                    {
                        YearlyStatementTextId = statement.YearlyStatementTextId,
                        YearlyMaintenanceId = statement.YearlyMaintenanceId,
                        StatementName = statement.StatementName,
                        StatementNumber = statement.StatementNumber,
                        StatementText = statement.StatementText,
                        Incentive=statement.Incentive
                    });

            if (data.Count() != 0)
            {
                getAllStatementText.getStatementTexts = data.ToList();
            }

            return getAllStatementText;
        }
    }
}
