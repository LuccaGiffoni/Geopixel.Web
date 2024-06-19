using Geopixel.Web.Data.Models;
using Geopixel.Web.Data.Responses;

namespace Geopixel.Web.Data.Services.Interfaces;

public interface IService
{
    Task<Response<List<Campaign>>> GetCampaignsAsync(int pageNumber, int pageSize, string searchText);
    Task<Response<Campaign>> GetCampaignByIdAsync(string id); 
    Task<Response<bool>> CreateCampaignAsync(Campaign campaign);
    Task<Response<Campaign>> UpdateCampaignAsync(Campaign campaign);
    Task<Response<bool>> DeleteCampaignAsync(string id);
    Task<Response<Information>> GetInformationAsync();
}