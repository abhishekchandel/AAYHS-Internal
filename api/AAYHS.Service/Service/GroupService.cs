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
using System.Text.RegularExpressions;

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
        private IZipCodeRepository _zipRepository;
        private IZipCodeRepository2 _zipRepository2;
        private ICityRepository _cityRepository;
        private IStallAssignmentRepository _stallAssignmentRepository;
        private IAddressRepository _AddressRepository;
        private readonly IGroupExhibitorRepository _groupExhibitorRepository;
        private readonly IGroupFinancialRepository _groupFinancialRepository;
        #endregion

        public GroupService(IGroupRepository GroupRepository, IStallAssignmentRepository stallAssignmentRepository, IAddressRepository AddressRepository,IGroupExhibitorRepository groupExhibitorRepository
                          ,IGroupFinancialRepository groupFinancialRepository, IMapper Mapper
            
            , IZipCodeRepository zipRepository, IZipCodeRepository2 zipRepository2, ICityRepository cityRepository)
        {
            _GroupRepository = GroupRepository;
            _AddressRepository = AddressRepository;
            _stallAssignmentRepository = stallAssignmentRepository;
            _groupExhibitorRepository = groupExhibitorRepository;
            _groupFinancialRepository = groupFinancialRepository;
            _Mapper = Mapper;
            _mainResponse = new MainResponse();
            newIdResponse = new BaseResponse();
            _zipRepository = zipRepository;
            _zipRepository2 = zipRepository2;
            _cityRepository = cityRepository;
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
                    ZipCodeId = request.ZipCodeId,
                    CreatedDate = DateTime.Now,
                    IsActive = true,
                    IsDeleted = false,
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
                    CreatedDate = DateTime.Now,
                    IsActive = true,
                    IsDeleted = false,
                };
                var Data= _GroupRepository.Add(Group);
                if (Data != null && Data.GroupId > 0 && request.groupStallAssignmentRequests != null && request.groupStallAssignmentRequests.Count > 0)
                {
                    StallAssignment stallAssignment;
                    foreach (var item in request.groupStallAssignmentRequests)
                    {
                        stallAssignment = new StallAssignment();
                        stallAssignment.StallId = item.SelectedStallId;
                        stallAssignment.StallAssignmentTypeId = item.StallAssignmentTypeId;
                        stallAssignment.GroupId = Data.GroupId;
                        stallAssignment.ExhibitorId = 0;
                        stallAssignment.BookedByType = "Group";
                        stallAssignment.IsActive = true;
                        stallAssignment.IsDeleted = false;
                        stallAssignment.CreatedDate = DateTime.Now;
                        stallAssignment.Date = item.StallAssignmentDate;
                        _stallAssignmentRepository.Add(stallAssignment);

                    }
                }
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
                        address.ZipCodeId = request.ZipCodeId;
                        address.ModifiedDate = DateTime.Now;
                        _AddressRepository.Update(address);
                    }
                     var stalls = _stallAssignmentRepository.RemoveAllGroupAssignedStalls(Group.GroupId);

                    if (request.groupStallAssignmentRequests != null && request.groupStallAssignmentRequests.Count > 0)
                    {

                        foreach (var assignment in request.groupStallAssignmentRequests)
                        {
                            StallAssignment stallAssignment = new StallAssignment();
                            stallAssignment.StallId = assignment.SelectedStallId;
                            stallAssignment.StallAssignmentTypeId = assignment.StallAssignmentTypeId;
                            stallAssignment.GroupId = Group.GroupId;
                            stallAssignment.ExhibitorId = 0;
                            stallAssignment.BookedByType = "Group";
                            stallAssignment.IsActive = true;
                            stallAssignment.IsDeleted = false;
                            stallAssignment.CreatedDate = DateTime.Now;
                            stallAssignment.Date = assignment.StallAssignmentDate;
                            _stallAssignmentRepository.Add(stallAssignment);
                        }
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
                _stallAssignmentRepository.RemoveAllGroupAssignedStalls(Group.GroupId);
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
           // AddZipCodes();
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

        public MainResponse GetGroupExhibitors(int GroupId)
        {
            var groupExhibitors = _GroupRepository.GetGroupExhibitors(GroupId);

            if (groupExhibitors.getGroupExhibitors!=null && groupExhibitors.TotalRecords!= 0)
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
                groupExhibitor.IsActive = false;
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
        
        public MainResponse AddUpdateGroupFinancials(AddGroupFinancialRequest addGroupFinancialRequest,string actionBy)
        {
            if (addGroupFinancialRequest.GroupFinancialId==0)
            {
                var groupFinancial = new GroupFinancials
                {
                    GroupId = addGroupFinancialRequest.GroupId,
                    Date = DateTime.Now,
                    FeeTypeId = addGroupFinancialRequest.FeeTypeId,
                    TimeFrameId = addGroupFinancialRequest.TimeFrameId,
                    Amount = addGroupFinancialRequest.Amount,
                    CreatedBy = actionBy,
                    CreatedDate = DateTime.Now,
                    IsActive=true,
                    IsDeleted=false
                };

                _groupFinancialRepository.Add(groupFinancial);

                _mainResponse.Success = true;
                _mainResponse.Message = Constants.GROUP_FINANCIAL_ADDED;
            }
            else
            {
                var groupFinancial = _groupFinancialRepository.GetSingle(x => x.GroupFinancialId == addGroupFinancialRequest.GroupFinancialId
                                     && x.IsActive == true && x.IsDeleted == false);

                if (groupFinancial!=null)
                {
                    groupFinancial.GroupId = addGroupFinancialRequest.GroupId;
                    groupFinancial.FeeTypeId = addGroupFinancialRequest.FeeTypeId;
                    groupFinancial.TimeFrameId = addGroupFinancialRequest.TimeFrameId;
                    groupFinancial.Amount = addGroupFinancialRequest.Amount;
                    groupFinancial.ModifiedBy = actionBy;
                    groupFinancial.ModifiedDate = DateTime.Now;
                    _groupFinancialRepository.Update(groupFinancial);

                    _mainResponse.Success = true;
                    _mainResponse.Message = Constants.GROUP_FINANCIAL_UPDATED;

                }
                else
                {
                    _mainResponse.Success = false;
                    _mainResponse.Message = Constants.NO_RECORD_FOUND;
                }
            }
            return _mainResponse;
        }

        public MainResponse UpdateGroupFinancialsAmount(UpdateGroupFinancialAmountRequest request)
        {
                var groupFinancial = _groupFinancialRepository.GetSingle(x => x.GroupFinancialId == request.GroupFinancialId
                                     && x.IsActive == true && x.IsDeleted == false);
                if (groupFinancial != null)
                {
                    groupFinancial.Amount = request.Amount;
                    groupFinancial.ModifiedDate = DateTime.Now;
                    _groupFinancialRepository.Update(groupFinancial);
                    _mainResponse.Success = true;
                    _mainResponse.Message = Constants.GROUP_FINANCIAL_UPDATED;
                }
                else
                {
                    _mainResponse.Success = false;
                    _mainResponse.Message = Constants.NO_RECORD_EXIST_WITH_ID;
                }
            
            return _mainResponse;
        }

        public MainResponse DeleteGroupFinancials(int groupFinancialId,string actionBy)
        {
            var deleteFinancial = _groupFinancialRepository.GetSingle(x => x.GroupFinancialId == groupFinancialId && x.IsActive == true && x.IsDeleted == false);

            if (deleteFinancial!=null)
            {
                deleteFinancial.IsDeleted = true;
                deleteFinancial.IsActive = false;
                deleteFinancial.DeletedBy = actionBy;
                deleteFinancial.DeletedDate = DateTime.Now;

                _groupFinancialRepository.Update(deleteFinancial);

                _mainResponse.Success = true;
                _mainResponse.Message = Constants.GROUP_FINANCIAL_DELETED;
            }
            else
            {
                _mainResponse.Success = false;
                _mainResponse.Message = Constants.NO_RECORD_FOUND;
            }
            return _mainResponse; 
        }

        public MainResponse GetAllGroupFinancials(int GroupId)
        {
            var financials = _GroupRepository.GetAllGroupFinancials(GroupId);
            if (financials!=null && financials.getGroupFinacials.Count()>0)
            {
                _mainResponse.GetAllGroupFinacials = financials;
                _mainResponse.GetAllGroupFinacials.TotalRecords = financials.getGroupFinacials.Count();
                _mainResponse.Success = true;
               
            }
            else
            {
                _mainResponse.Success = false;
                _mainResponse.Message = Constants.NO_RECORD_FOUND;
            }
            return _mainResponse;
        }

        public MainResponse GetGroupStatement(int GroupId)
        {
            var groupStatement = _GroupRepository.GetGroupStatement(GroupId);
            _mainResponse.GetGroupStatement = groupStatement;
            _mainResponse.Success = true;
            return _mainResponse;
        }
        public void AddZipCodes()
        {
            var zip2list = _zipRepository2.GetAll();
            foreach(var code2 in zip2list)
            {
                try
                {
                    var city = _cityRepository.GetSingle(x => x.Name == code2.City);
                    if (city != null)
                    {
                        ZipCodes code = new ZipCodes();
                        code.CityId = city.CityId;
                        code.Number = Convert.ToInt32(code2.ZipCode);
                        code.IsActive = true;
                        code.IsDeleted = false;
                        code.CreatedDate = DateTime.Now;
                        _zipRepository.Add(code);
                    }
                    else
                    {

                    }
                }
                catch
                {
                    continue;
                }
            }
        }
   }
}
