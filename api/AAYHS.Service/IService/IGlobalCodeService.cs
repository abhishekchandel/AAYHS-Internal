using AAYHS.Core.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AAYHS.Service.IService
{
    public interface IGlobalCodeService
    {
        Task<MainResponse> GetHorseType();
    }
}
