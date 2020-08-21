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
    public class ExhibitorHorseRepository:GenericRepository<ExhibitorHorse>,IExhibitorHorseRepository
    {
        #region readonly
        private readonly AAYHSDBContext _objContext;
        private IMapper _Mapper;
        #endregion

        #region private 
        private MainResponse _mainResponse;
        #endregion

        public ExhibitorHorseRepository(AAYHSDBContext ObjContext, IMapper Mapper) : base(ObjContext)
        {
            _mainResponse = new MainResponse();
            _objContext = ObjContext;
            _Mapper = Mapper;
        }

    }
}
