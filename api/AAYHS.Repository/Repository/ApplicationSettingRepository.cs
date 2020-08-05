using AAYHS.Core.DTOs.Response;
using AAYHS.Data.DBContext;
using AAYHS.Data.DBEntities;
using AAYHS.Repository.IRepository;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace AAYHS.Repository.Repository
{
    public class ApplicationSettingRepository : GenericRepository<ApplicationSettings>, IApplicationSettingRepository
    {
        #region readonly
        private readonly IMapper _Mapper;
        private readonly AAYHSDBContext _ObjContext;
        #endregion

        #region Private
        private MainResponse _MainResponse;
        #endregion

        public ApplicationSettingRepository(AAYHSDBContext ObjContext, IMapper Mapper) : base(ObjContext)
        {
            _ObjContext = ObjContext;
            _Mapper = Mapper;
            _MainResponse = new MainResponse();
        }
    }
}
