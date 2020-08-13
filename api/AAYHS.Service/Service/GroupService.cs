using AAYHS.Core.DTOs.Request;
using AAYHS.Core.DTOs.Response;
using AAYHS.Core.Shared.Static;
using AAYHS.Data.DBEntities;
using AAYHS.Repository.IRepository;
using AAYHS.Service.IService;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AAYHS.Service.Service
{
   public class GroupService: IGroupService
    {
        #region readonly
        private readonly IMapper _Mapper;
        #endregion

        #region private
        private MainResponse _mainResponse;
        private BaseResponse newIdResponse;
        private IGroupRepository _GroupRepository;
        private IAddressRepository _AddressRepository;
        private readonly IGroupExhibitorRepository _groupExhibitorRepository;
        #endregion

        public GroupService(IGroupRepository GroupRepository, IAddressRepository AddressRepository,IGroupExhibitorRepository groupExhibitorRepository
                          , IMapper Mapper)
        {
            _GroupRepository = GroupRepository;
            _AddressRepository = AddressRepository;
            _groupExhibitorRepository = groupExhibitorRepository;
            _Mapper = Mapper;
            _mainResponse = new MainResponse();
            newIdResponse = new BaseResponse();
        }

        public MainResponse AddUpdateGroup(GroupRequest request)
        {
            if (request.GroupId == null || request.GroupId <= 0)
            {
                var checkexist = _GroupRepository.GetSingle(x => x.GroupName == request.GroupName
                && x.IsActive == true && x.IsDeleted == false);
                if (checkexist != null && checkexist.GroupId > 0)
                {
                    _mainResponse.Message = Constants.NAME_ALREADY_EXIST;
                    _mainResponse.Success = false;
                    return _mainResponse;
                }

                var addressEntity = new Addresses
                {
                    Address = request.Address,
                    CityId = request.CityId,
                    ZipCode = request.ZipCode,
                    CreatedDate = DateTime.Now
                };
                var address = _AddressRepository.Add(addressEntity);
                var Group = new Groups
                {
                    GroupName = request.GroupName,
                    ContactName = request.ContactName,
                    Phone = request.Phone,
                    Email = request.Email,
                    AmountReceived = request.AmountReceived,
                    AddressId = address != null ? address.AddressId : 0,
                    CreatedDate = DateTime.Now
                };
              var Data= _GroupRepository.Add(Group);
                _mainResponse.Message = Constants.RECORD_ADDED_SUCCESS;
                _mainResponse.Success = true;
                _mainResponse.NewId = Data.GroupId;
                newIdResponse.NewId = Data.GroupId;
                _mainResponse.BaseResponse = newIdResponse;
            }
            else
            {
                var Group = _GroupRepository.GetSingle(x => x.GroupId == request.GroupId && x.IsActive == true && x.IsDeleted == false);
                if (Group != null && Group.GroupId > 0)
                {
                    Group.GroupName = request.GroupName;
                    Group.ContactName = request.ContactName;
                    Group.Phone = request.Phone;
                    Group.Email = request.Email;
                    Group.AmountReceived = request.AmountReceived;
                    Group.ModifiedDate = DateTime.Now;
                    _GroupRepository.Update(Group);

                    var address = _AddressRepository.GetSingle(x => x.AddressId == Group.AddressId && x.IsActive==true && x.IsDeleted==false);
                    if (address != null && address.AddressId > 0)
                    {
                        address.Address = request.Address;
                        address.CityId = request.CityId;
                        address.ZipCode = request.ZipCode;
                        address.ModifiedDate = DateTime.Now;
                        _AddressRepository.Update(address);
                    }
                    _mainResponse.Message = Constants.RECORD_UPDATE_SUCCESS;
                    _mainResponse.Success = true;
                    _mainResponse.NewId = Convert.ToInt32(request.GroupId);
                    newIdResponse.NewId = Convert.ToInt32(request.GroupId);
                    _mainResponse.BaseResponse = newIdResponse;
                }
                else
                {
                    _mainResponse.Message = Constants.NO_RECORD_EXIST_WITH_ID;
                    _mainResponse.Success = false;
                }
            }
            return _mainResponse;
        }

        public MainResponse DeleteGroup(int GroupId)
        {
            var Group = _GroupRepository.GetSingle(x => x.GroupId == GroupId && x.IsActive == true && x.IsDeleted == false);
            if (Group != null && Group.GroupId > 0)
            {
                Group.IsDeleted = true;
                Group.IsActive = false;
                Group.DeletedDate = DateTime.Now;
                _GroupRepository.Update(Group);
                _mainResponse.Message = Constants.RECORD_DELETE_SUCCESS;
                _mainResponse.Success = true;
            }
            else
            {
                _mainResponse.Message = Constants.NO_RECORD_EXIST_WITH_ID;
                _mainResponse.Success = false;
            }
            return _mainResponse;
        }

        public MainResponse GetAllGroups(BaseRecordFilterRequest request)
        {

            _mainResponse = _GroupRepository.GetAllGroups(request);
            if (_mainResponse.GroupListResponse.groupResponses != null && _mainResponse.GroupListResponse.groupResponses.Count() > 0)
            {
                _mainResponse.Message = Constants.RECORD_FOUND;
                _mainResponse.Success = true;
            }
            else
            {
                _mainResponse.Message = Constants.NO_RECORD_FOUND;
                _mainResponse.Success = false;
            }
            return _mainResponse;
        }

        public MainResponse GetGroupById(int GroupId)
        {
            _mainResponse = _GroupRepository.GetGroupById(GroupId);
            if (_mainResponse.GroupResponse != null && _mainResponse.GroupResponse.GroupId > 0)
            {
                _mainResponse.Message = Constants.RECORD_FOUND;
                _mainResponse.Success = true;
            }
            else
            {
                _mainResponse.Message = Constants.NO_RECORD_FOUND;
                _mainResponse.Success = false;
            }
            return _mainResponse;
        }

        
        public MainResponse SearchGroup(SearchRequest searchRequest)
        {
            var search = _GroupRepository.SearchGroup(searchRequest);
            if (search != null && search.TotalRecords != 0)
            {
                _mainResponse.GroupListResponse = search;
                _mainResponse.Success = true;
                _mainResponse.GroupListResponse.TotalRecords = search.TotalRecords;
            }
            else
            {
                _mainResponse.Message = Constants.NO_RECORD_FOUND;
                _mainResponse.Success = false;
            }

            return _mainResponse;
        }

        public MainResponse GetGroupExhibitors(GroupExhibitorsRequest groupExhibitorsRequest)
        {
            var groupExhibitors = _GroupRepository.GetGroupExhibitors(groupExhibitorsRequest);

            if (groupExhibitors.getGroupExhibitors!=null && groupExhibitors.TotalRecords!=0)
            {
                _mainResponse.GetAllGroupExhibitors = groupExhibitors;
                _mainResponse.GetAllGroupExhibitors.TotalRecords = groupExhibitors.TotalRecords;
                _mainResponse.Success = true;
            }
            else
            {
                _mainResponse.Message = Constants.NO_RECORD_FOUND;
                _mainResponse.Success = false;
            }
            return _mainResponse;
        }

        public MainResponse DeleteGroupExhibitor(int groupExhibitorId,string actionBy)
        {
            var groupExhibitor = _groupExhibitorRepository.GetSingle(x =>x.GroupExhibitorId==groupExhibitorId && x.IsActive == true && x.IsDeleted == false);

            if (groupExhibitor!=null)
            {
                groupExhibitor.IsDeleted = true;
                groupExhibitor.ModifiedBy = actionBy;
                groupExhibitor.ModifiedDate = DateTime.Now;

                _groupExhibitorRepository.Update(groupExhibitor);

                _mainResponse.Success = true;
                _mainResponse.Message = Constants.GROUP_EXHIBITOR_DELETED;
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
