using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace AAYHS.Core.DTOs.Request
{
   public class ExhibitorRequest
   {
        public int ExhibitorId { get; set; }
        public int GroupId { get; set; }
        public int AddressId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? BackNumber { get; set; }
        public int? BirthYear { get; set; }
        public bool IsNSBAMember { get; set; }
        public bool IsDoctorNote { get; set; }
        public int QTYProgram { get; set; }
        public string PrimaryEmail { get; set; }
        public string SecondaryEmail { get; set; }
        public string Phone { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public int StateId { get; set; }
        public string Address { get; set; }
        public DateTime Date { get; set; }
        public List<ExhibitorStallAssignmentRequest> exhibitorStallAssignmentRequests { get; set; }
    }

    public class ExhibitorStallAssignmentRequest
    {
        public int StallAssignmentId { get; set; }
        public int SelectedStallId { get; set; }
        public int StallAssignmentTypeId { get; set; }
        public DateTime StallAssignmentDate { get; set; }
    }

    public class AddExhibitorHorseRequest
   {
        public int ExhibitorId { get; set; }
        public int HorseId { get; set; }
        public int? BackNumber { get; set; }
        public string Date { get; set; }
    }
    public class AddExhibitorToClass
    {
        public int ExhibitorId { get; set; }
        public int ClassId { get; set; }
        public int HorseId { get; set; }
        public string Date { get; set; }
    }
    public class UpdateScratch
    {
        public int exhibitorClassId { get; set; }
        public bool IsScratch { get; set; }
    }
    public class AddSponsorForExhibitor
    {
        public int SponsorExhibitorId { get; set; }
        public int ExhibitorId { get; set; }
        public int SponsorId { get; set; }
        public int SponsorTypeId { get; set; }
        public string TypeId { get; set; }
        public int AdTypeId { get; set; }
        public int HorseId { get; set; }
        public decimal SponsorAmount { get; set; }
    }
    public class DocumentUploadRequest
    {
        public int Exhibitor { get; set; }
        public int DocumentType { get; set; }
        public List<IFormFile> Documents { get; set; }
    }
    public class FinancialDocumentRequest
    {
        public int ExhibitorPaymentId { get; set; }
        public IFormFile Document { get; set; }
    }
    public class DocumentDeleteRequest
    {
        public int ScanId { get; set; }
        public string Path { get; set; }
    }
    public class AddFinancialTransactionRequest
    {
        public int FeeTypeId { get; set; }
        public int ExhibitorId { get; set; }
        public decimal Amount { get; set; }
        public string TimeFrameType { get; set; }                 
        public string PayDate { get; set; }           
        public decimal AmountPaid { get; set; }
        public decimal RefundAmount { get; set; }
    }
    public class ViewDetailRequest
    {
        public int FeeTypeId { get; set; }
        public int ExhibitorId { get; set; }
    }
    public class EmailWithDocumentRequest
    {
        public string DocumentPath { get; set; }
        public string EmailId { get; set; }
    }
    public class UpdateFinancialRequest
    {
        public int ExhibitorPaymentId { get; set; }
        public decimal AmountReceived { get; set; }
        public decimal RefundAmount { get; set; }
    }
}