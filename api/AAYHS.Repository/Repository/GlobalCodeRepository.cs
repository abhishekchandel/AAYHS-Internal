﻿using AAYHS.Core.DTOs.Response;
using AAYHS.Core.DTOs.Response.Common;
using AAYHS.Data.DBContext;
using AAYHS.Data.DBEntities;
using AAYHS.Repository.IRepository;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AAYHS.Repository.Repository
{
    public class GlobalCodeRepository: GenericRepository<GlobalCodes>, IGlobalCodeRepository
    {
        #region readonly
        private readonly IMapper _Mapper;
        #endregion

        #region Private
        private MainResponse _mainResponse;
        #endregion

        #region public
        public AAYHSDBContext _ObjContext;
        #endregion

        public GlobalCodeRepository(AAYHSDBContext ObjContext, IMapper Mapper): base(ObjContext)
        {
            _mainResponse = new MainResponse();
            _ObjContext = ObjContext;
            _Mapper = Mapper;
        }

        public async Task<GlobalCodeMainResponse> GetCodes(string categoryName)
        {
            IQueryable<GlobalCodeResponse> data;
            GlobalCodeMainResponse globalCodeMainResponse = new GlobalCodeMainResponse();

            data = (from gcc in _ObjContext.GlobalCodeCategories
                    join gc in _ObjContext.GlobalCodes on gcc.GlobalCodeCategoryId equals gc.CategoryId
                    where gcc.CategoryName == categoryName && gc.IsDeleted == false && gc.IsActive == true
                    select new GlobalCodeResponse
                    {
                        GlobalCodeId = gc.GlobalCodeId,
                        CodeName = (gc.CodeName == null ? "" : gc.CodeName),
                        Description = (gc.Description == null ? String.Empty : gc.Description),
                        GlobalCodeCategory = gcc.CategoryName,
                        CategoryId = gc.CategoryId,
                    });
            globalCodeMainResponse.totalRecords = data.Count();
            globalCodeMainResponse.globalCodeResponse = await data.ToListAsync();
            return globalCodeMainResponse;
        }

    }
}
