using AAYHS.Core.DTOs.Request;
using AAYHS.Core.DTOs.Response;
using AAYHS.Core.DTOs.Response.Common;
using AAYHS.Core.Shared.Static;
using AAYHS.Data.DBEntities;
using AAYHS.Repository.IRepository;
using AAYHS.Repository.Repository;
using AAYHS.Service.IService;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace AAYHS.Service.Service
{
    public class HorseService : IHorseService
    {
       
        #region readonly     
        private IMapper _mapper;
        private readonly IHorseRepository _horseRepository;
        private readonly IStallAssignmentRepository _stallAssignmentRepository;
        private readonly ITackStallAssignmentRepository _tackStallAssignmentRepository;
        #endregion

        #region private
        private MainResponse _mainResponse;
        #endregion

        public HorseService(IHorseRepository horseRepository,IStallAssignmentRepository stallAssignmentRepository,
                           ITackStallAssignmentRepository tackStallAssignmentRepository, IMapper Mapper)
        {
            _horseRepository = horseRepository;
            _stallAssignmentRepository = stallAssignmentRepository;
            _tackStallAssignmentRepository = tackStallAssignmentRepository;
            _mapper = Mapper;
            _mainResponse = new MainResponse();
        }

        public MainResponse GetAllHorses(HorseRequest horseRequest)
        {
            var allHorses = _horseRepository.GetAllHorses(horseRequest);

            if (allHorses.horsesResponse!=null && allHorses.TotalRecords!=0)
            {
                _mainResponse.GetAllHorses = allHorses;
                _mainResponse.GetAllHorses.TotalRecords = allHorses.TotalRecords;
                _mainResponse.Success = true;
            }
            else
            {
                _mainResponse.Message = Constants.NO_RECORD_FOUND;
                _mainResponse.Success = false;
            }
            return _mainResponse;
        }
        public MainResponse RemoveHorse(int HorseId,string actionBy)
        {
            var horse = _horseRepository.GetSingle(x => x.HorseId == HorseId);

            if (horse!=null)
            {
                horse.IsDeleted = true;
                horse.DeletedBy = actionBy;
                horse.DeletedDate = DateTime.Now;
                _horseRepository.Update(horse);

                _mainResponse.Success = true;
                _mainResponse.Message = Constants.HORSE_REMOVED;
            }
            else          
            {
                _mainResponse.Success = false;
                _mainResponse.Message = Constants.NO_RECORD_FOUND;
            }
            return _mainResponse;
        }
        public MainResponse AddHorse(HorseAddRequest horseAddRequest,string actionBy)
        {
            if (horseAddRequest.HorseId!=0)
            {
                var horse = new Horses
                {
                    Name = horseAddRequest.Name,
                    HorseTypeId =horseAddRequest.HorseTypeId,
                    JumpHeight=horseAddRequest.JumpHeight,
                    GroupId=horseAddRequest.GroupId,
                    NSBAIndicator=horseAddRequest.NSBAIndicator,
                    IsActive=true,
                    CreatedBy= actionBy,
                    CreatedDate=DateTime.Now
                };
               var _horse= _horseRepository.Add(horse);

                var stall = new StallAssignment
                { 
                  StallId= horseAddRequest.StallId,
                  GroupId= horseAddRequest.GroupId,
                  HorseId= _horse.HorseId,
                  IsActive = true,
                  CreatedBy = actionBy,
                  CreatedDate = DateTime.Now
                };

                _stallAssignmentRepository.Add(stall);

                var tackStall = new TackStallAssignment
                {
                   TackStallId= horseAddRequest.TackStallId,
                   GroupId=horseAddRequest.GroupId,
                   HorseId= _horse.HorseId,
                   IsActive = true,
                   CreatedBy = actionBy,
                   CreatedDate = DateTime.Now
                };

                _tackStallAssignmentRepository.Add(tackStall);

                _mainResponse.Message = Constants.HORSE_ADDED;
                _mainResponse.Success = true;
                
            }
            else
            {

            }
            return _mainResponse;
        }
    }
}
