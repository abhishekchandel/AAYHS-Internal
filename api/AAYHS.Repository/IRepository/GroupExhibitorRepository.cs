using AAYHS.Core.DTOs.Response;
using AAYHS.Data.DBContext;
using AAYHS.Data.DBEntities;
using AAYHS.Repository.Repository;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace AAYHS.Repository.IRepository
{
    public class GroupExhibitorRepository:GenericRepository<GroupExhibitors>,IGroupExhibitorRepository
    {
        #region readonly
        private readonly IMapper _Mapper;
        #endregion

        #region Private
        private MainResponse _MainResponse;
        #endregion

        #region public
        public AAYHSDBContext _ObjContext;
        #endregion

        public GroupExhibitorRepository(AAYHSDBContext ObjContext, IMapper Mapper) : base(ObjContext)
        {
            _MainResponse = new MainResponse();
            _ObjContext = ObjContext;
            _Mapper = Mapper;
        }


    }
}
