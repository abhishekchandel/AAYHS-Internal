using AAYHS.Core.DTOs.Request;
using AAYHS.Core.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AAYHS.Service.IService
{
   public interface IExhibitorService
    {
        MainResponse AddUpdateExhibitor(ExhibitorRequest request,string actionBy);
        MainResponse GetAllExhibitors(BaseRecordFilterRequest filterRequest);
        MainResponse GetExhibitorById(int exhibitorId);
        MainResponse DeleteExhibitor(int exhibitorId, string actionBy);     
        MainResponse GetExhibitorHorses(int exhibitorId);
        MainResponse DeleteExhibitorHorse(int exhibitorHorseId, string actionBy);
        MainResponse GetAllHorses(int exhibitorId);
        MainResponse GetHorseDetail(int horseId);
        MainResponse AddExhibitorHorse(AddExhibitorHorseRequest addExhibitorHorseRequest, string actionBy);
        MainResponse GetAllClassesOfExhibitor(int exhibitorId);
        MainResponse RemoveExhibitorFromClass(int exhibitorClassId, string actionBy);
        MainResponse GetAllClasses(int exhibitorId);
        MainResponse GetClassDetail(int classId, int exhibitorId);
        MainResponse UpdateScratch(UpdateScratch updateScratch, string actionBy);
        MainResponse AddExhibitorToClass(AddExhibitorToClass addExhibitorToClass, string actionBy);
        MainResponse GetAllSponsorsOfExhibitor(int exhibitorId);
        MainResponse RemoveSponsorFromExhibitor(int sponsorExhibitorId, string actionBy);
        MainResponse GetAllSponsor(int exhibitorId);
        MainResponse GetSponsorDetail(int sponsorId);
        MainResponse AddSponsorForExhibitor(AddSponsorForExhibitor addSponsorForExhibitor, string actionBy);
        MainResponse GetExhibitorFinancials(int exhibitorId);
        MainResponse UploadDocumentFile(DocumentUploadRequest documentUploadRequest, string actionBy);
        MainResponse GetUploadedDocuments(int exhibitorId);
        MainResponse GetFees();
        MainResponse RemoveExhibitorTransaction(int exhibitorPaymentId, string actionBy);
   }
}
