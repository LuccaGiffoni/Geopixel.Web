using System.Net.Http.Json;
using Geopixel.Web.Data.Models;
using Geopixel.Web.Data.Services.Interfaces;

namespace Geopixel.Web.Data.Services
{
    public class CampaignService(HttpClient httpClient) : IService
    {
        private const string ApiUrl = "https://us-east-1.aws.data.mongodb-api.com/app/geopixel-api-gkczdbu/endpoint/campaign";

        public async Task<List<Campaign>?> GetCampaignsAsync()
        {
            return await httpClient.GetFromJsonAsync<List<Campaign>>($"{ApiUrl}/read");
        }

        public async Task<Campaign?> GetCampaignByIdAsync(string id)
        { 
            var loadedCampaign = await httpClient.GetFromJsonAsync<Campaign>($"{ApiUrl}/readOne?id={id}");
            if (loadedCampaign == null) return null;
            
            switch (loadedCampaign.Items.Count)
            {
                case 6:
                {
                    var seven = new Item(string.Empty, 0);
                    loadedCampaign.Items.Add(seven);

                    var eight = new Item(string.Empty, 0);
                    loadedCampaign.Items.Add(eight);
                    break;
                }
                case 7:
                {
                    var eight = new Item(string.Empty, 0);
                    loadedCampaign.Items.Add(eight);
                    break;
                }
            }

            return loadedCampaign;
        }

        public async Task CreateCampaignAsync(Campaign campaign)
        {
            await httpClient.PostAsJsonAsync($"{ApiUrl}/create", campaign);
        }

        public async Task UpdateCampaignAsync(Campaign campaign)
        {
            await httpClient.PutAsJsonAsync($"{ApiUrl}/update?id={campaign.CampaignId}", campaign);
        }

        public async Task DeleteCampaignAsync(string id)
        {
            await httpClient.DeleteAsync($"{ApiUrl}/delete?id={id}");
        }
    }
}