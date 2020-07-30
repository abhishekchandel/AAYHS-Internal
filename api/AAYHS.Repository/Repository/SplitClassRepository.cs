﻿using AAYHS.Core.DTOs.Request;
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
    public class SplitClassRepository:GenericRepository<ClassSplits>,ISplitClassRepository
    {
        #region readonly
        private readonly IMapper _Mapper;
        #endregion

        #region Private
        private MainResponse _MainResponse;
        #endregion

        #region public
        public AAYHSDBContext _context;
        #endregion

        public SplitClassRepository(AAYHSDBContext ObjContext, IMapper Mapper) : base(ObjContext)
        {
             _MainResponse = new MainResponse();
             _context = ObjContext;
             _Mapper = Mapper;
        }
        public void DeleteSplitsByClassId(List<SplitRequest> splitRequest)
        {
            foreach (var request in splitRequest)
            {
                var split = _context.ClassSplits.Where(x => x.ClassId == request.ClassId).FirstOrDefault();
                if (split != null && split.ClassSplitId > 0)
                {
                    _context.ClassSplits.Remove(split);
                    _context.SaveChanges();
                }

            }

        }
    }
}
