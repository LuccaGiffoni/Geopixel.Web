using Geopixel.Web.Data.Models;

namespace Geopixel.Web.Data.Services.Interfaces;

public interface IService
{ 
    Task<List<Campaign>?> GetCampaignsAsync(); 
    Task<Campaign?> GetCampaignByIdAsync(string id); 
    Task CreateCampaignAsync(Campaign campaign);
    Task UpdateCampaignAsync(Campaign campaign);
    Task DeleteCampaignAsync(string id);
}