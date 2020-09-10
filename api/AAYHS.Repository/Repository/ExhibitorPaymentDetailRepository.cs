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
    public class ExhibitorPaymentDetailRepository:GenericRepository<ExhibitorPaymentDetail>, IExhibitorPaymentDetailRepository
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

        public ExhibitorPaymentDetailRepository(AAYHSDBContext ObjContext, IMapper Mapper) : base(ObjContext)
        {
            _mainResponse = new MainResponse();
            _objContext = ObjContext;
            _Mapper = Mapper;
        }
    }
}
