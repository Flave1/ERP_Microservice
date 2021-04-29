using Deposit.Contracts.Response;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Deposit.Managers.Interface.temp
{
    public interface IFilesHandlerService
    { 
        Task<string> Save_ustomer_thumbs_Async(long cutomerid);
        Deposit_req_response SaveSingleFile(IFormFile file);
    }

}
