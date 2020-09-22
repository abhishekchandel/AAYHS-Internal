using System;
using System.Collections.Generic;
using System.Text;

namespace AAYHS.Core.DTOs.Response
{
   public class SponsorResponse 
    {
        public int SponsorId { get; set; }
        public string SponsorName { get; set; }
        public string ContactName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public float AmountReceived { get; set; }
        public string Address { get; set; }
        public int ZipCodeId { get; set; }
        public int CityId { get; set; }
        public string CityName { get; set; }
        public int StateId { get; set; }
    }
    public class SponsorListResponse
    {
        public int TotalRecords { get; set; }
        public List<SponsorResponse> sponsorResponses { get; set; }
    }
    }
