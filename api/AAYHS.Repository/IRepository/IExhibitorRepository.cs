using AAYHS.Core.DTOs.Request;
using AAYHS.Core.DTOs.Response;
using AAYHS.Data.DBEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace AAYHS.Repository.IRepository
{
   public interface IExhibitorRepository : IGenericRepository<Exhibitors>
    {
        ExhibitorListResponse GetAllExhibitors(BaseRecordFilterRequest filterRequest);
        ExhibitorListResponse GetExhibitorById(int exhibitorId);        
        ExhibitorHorsesResponse GetExhibitorHorses(int exhibitorId);
        GetAllClassesOfExhibitor GetAllClassesOfExhibitor(int exhibitorId);
        GetAllSponsorsOfExhibitor GetAllSponsorsOfExhibitor(int exhibitorId);
        GetSponsorForExhibitor GetSponsorDetail(int sponsorId);
        GetExhibitorFinancials GetExhibitorFinancials(int exhibitorId);
        GetAllUploadedDocuments GetUploadedDocuments(int exhibitorId);
   }
}
