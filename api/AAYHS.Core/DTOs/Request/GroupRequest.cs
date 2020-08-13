﻿using System;
using System.Collections.Generic;
using System.Text;

namespace AAYHS.Core.DTOs.Request
{
   public class GroupRequest
    {
        public int? GroupId { get; set; }
        public string GroupName { get; set; }
        public string ContactName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public float AmountReceived { get; set; }
        public string Address { get; set; }
        public string ZipCode { get; set; }
        public int CityId { get; set; }
        public int StateId { get; set; }

    }
   

    public class AddGroupFinancialRequest
    {
        public int GroupFinancialId { get; set; }
        public int GroupId { get; set; }
        public int FeeTypeId { get; set; }
        public int TimeFrameId { get; set; }
        public double Amount { get; set; }
    }
    public class GroupFinancialRequest : BaseRecordFilterRequest
    {
        public int GroupFinancialId { get; set; }
        public int GroupId { get; set; }
    }


}