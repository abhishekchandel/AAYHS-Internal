using AAYHS.Core.DTOs.Request;
using AAYHS.Core.DTOs.Response;
using AAYHS.Core.DTOs.Response.Common;
using AAYHS.Data.DBContext;
using AAYHS.Data.DBEntities;
using AAYHS.Repository.IRepository;
using AutoMapper;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace AAYHS.Repository.Repository
{
    public class HorseRepository: GenericRepository<Horses>, IHorseRepository
    {

        #region readonly
        private readonly AAYHSDBContext _ObjContext;
        private IMapper _Mapper;
        #endregion

        #region private 
        private MainResponse _MainResponse;
        #endregion

        public HorseRepository(AAYHSDBContext ObjContext, IMapper Mapper) : base(ObjContext)
        {
            _ObjContext = ObjContext;
            _Mapper = Mapper;
            _MainResponse = new MainResponse();
        }
        public GetAllHorses GetAllHorses(HorseRequest horseRequest)
        {
            IEnumerable<HorseResponse> data;
            GetAllHorses getAllHorses = new GetAllHorses();

            data = (from horse in _ObjContext.Horses
                    join stall in _ObjContext.StallAssignment on horse.HorseId equals stall.HorseId into stall1
                    from stall2 in stall1.DefaultIfEmpty()
                    join tack in _ObjContext.TackStallAssignment on horse.HorseId equals tack.HorseId into tack1
                    from tack2 in tack1.DefaultIfEmpty()
                    where horse.IsActive == true && horse.IsDeleted == false
                    select new HorseResponse 
                    {
                        HorseId=horse.HorseId,
                        Name=horse.Name,
                        HorseType=_ObjContext.GlobalCodes.Where(x=>x.GlobalCodeId==horse.HorseTypeId).Select(x=>x.CodeName).FirstOrDefault(),
                        StallNumber=_ObjContext.Stall.Where(x=>x.StallId==stall2.StallId).Select(x=>x.StallNumber).FirstOrDefault(),
                        TackStallNumber=_ObjContext.TackStall.Where(x=>x.TackStallId==tack2.TackStallId).Select(x=>x.TackStallNumber).FirstOrDefault()
                    });

            if (data.Count() != 0)
            {
                if (horseRequest.OrderByDescending == true)
                {
                    data = data.OrderByDescending(x => x.GetType().GetProperty(horseRequest.OrderBy).GetValue(x));
                }
                else
                {
                    data = data.OrderBy(x => x.GetType().GetProperty(horseRequest.OrderBy).GetValue(x));
                }
                getAllHorses.TotalRecords = data.Count();
                if (horseRequest.AllRecords)
                {
                    getAllHorses.horsesResponse = data.ToList();
                }
                else
                {
                    getAllHorses.horsesResponse = data.Skip((horseRequest.Page - 1) * horseRequest.Limit).Take(horseRequest.Limit).ToList();

                }

            }
            return getAllHorses;
        }
    }
}
