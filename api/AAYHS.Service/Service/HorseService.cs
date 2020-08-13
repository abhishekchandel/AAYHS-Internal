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
        private readonly IGroupRepository _groupRepository;      
        #endregion

        #region private
        private MainResponse _mainResponse;
        #endregion

        public HorseService(IHorseRepository horseRepository,IStallAssignmentRepository stallAssignmentRepository,
                           ITackStallAssignmentRepository tackStallAssignmentRepository,IGroupRepository groupRepository, IMapper Mapper)
        {
            _horseRepository = horseRepository;
            _stallAssignmentRepository = stallAssignmentRepository;
            _tackStallAssignmentRepository = tackStallAssignmentRepository;
            _groupRepository = groupRepository;
            _mapper = Mapper;
            _mainResponse = new MainResponse();
        }

        public MainResponse GetAllHorses(HorseRequest horseRequest)
        {
                     
                var allHorses = _horseRepository.GetAllHorses(horseRequest);
                if (allHorses.horsesResponse != null && allHorses.TotalRecords != 0)
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
        public MainResponse GetHorse(int HorseId)
        {

            var horse = _horseRepository.GetSingle(x => x.HorseId == HorseId && x.IsActive == true && x.IsDeleted == false);
            if (horse != null)
            {
                _mainResponse.GetHorseById = _mapper.Map<GetHorseById>(horse);               
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
            var horse = _horseRepository.GetSingle(x => x.HorseId == HorseId && x.IsDeleted==false);

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
        public MainResponse AddUpdateHorse(HorseAddRequest horseAddRequest,string actionBy)
        {
            if (horseAddRequest.HorseId==0)
            {
                var horse = new Horses
                {
                    Name = horseAddRequest.Name,
                    HorseTypeId =horseAddRequest.HorseTypeId,
                    JumpHeightId=horseAddRequest.JumpHeightId,
                    GroupId=horseAddRequest.GroupId,
                    NSBAIndicator=horseAddRequest.NSBAIndicator,
                    IsActive=true,
                    CreatedBy= actionBy,
                    CreatedDate=DateTime.Now
                };
               var _horse= _horseRepository.Add(horse);

                _mainResponse.NewId = _horse.HorseId;
                _mainResponse.Message = Constants.HORSE_ADDED;
                _mainResponse.Success = true;
                
            }
            else
            {
                var horse = _horseRepository.GetSingle(x => x.HorseId == horseAddRequest.HorseId && x.IsActive==true && x.IsDeleted==false);
                if (horse != null)
                {
                    horse.Name = horseAddRequest.Name;
                    horse.HorseTypeId = horseAddRequest.HorseTypeId;
                    horse.GroupId = horseAddRequest.GroupId;
                    horse.JumpHeightId = horseAddRequest.JumpHeightId;
                    horse.NSBAIndicator = horseAddRequest.NSBAIndicator;
                    horse.ModifiedBy = actionBy;
                    horse.ModifiedDate = DateTime.Now;

                    _horseRepository.Update(horse);

                    _mainResponse.NewId = horseAddRequest.HorseId;
                    _mainResponse.Message = Constants.HORSE_UPDATED;
                    _mainResponse.Success = true;
                }
                else
                {
                    _mainResponse.Success = false;
                    _mainResponse.Message = Constants.NO_RECORD_FOUND;
                }              
               
            }
            return _mainResponse;
        }
        public MainResponse SearchHorse(SearchRequest searchRequest)
        {
            var allHorses = _horseRepository.SearchHorse(searchRequest);

            if (allHorses.horsesResponse != null && allHorses.TotalRecords != 0)
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
        public MainResponse LinkedExhibitors(HorseExhibitorRequest horseExhibitorRequest)
        {
            var exhibitors = _horseRepository.LinkedExhibitors(horseExhibitorRequest);

            if (exhibitors.getLinkedExhibitors!=null && exhibitors.TotalRecords!=0)
            {
                _mainResponse.GetAllLinkedExhibitors = exhibitors;
                _mainResponse.GetAllLinkedExhibitors.TotalRecords = exhibitors.TotalRecords;
                _mainResponse.Success = true;
            }
            else
            {
                _mainResponse.Message = Constants.NO_RECORD_FOUND;
                _mainResponse.Success = false;
            }
            return _mainResponse;
        }
        public MainResponse GetGroups()
        {
            var groups = _groupRepository.GetAll(x => x.IsActive == true && x.IsDeleted == false);

            if (groups.Count!=0)
            {
                var allGroups= _mapper.Map<List<GetGroup>>(groups);
                GetAllGroups getAllGroups = new GetAllGroups();
                getAllGroups.getGroups = allGroups;
                _mainResponse.GetAllGroups = getAllGroups;
                _mainResponse.Success = true;
            }
            else
            {
                _mainResponse.Success = false;
                _mainResponse.Message = Constants.NO_RECORD_FOUND;
            }
            return _mainResponse;
        }
    }
}
