using System.Net.Http.Json;
using System.Text.Json;
using Geopixel.Web.Data.Models;
using Geopixel.Web.Data.Responses;
using Geopixel.Web.Data.Services.Interfaces;

namespace Geopixel.Web.Data.Services
{
    public class CampaignService(HttpClient httpClient) : IService
    {
        private const string ApiUrl = "https://us-east-1.aws.data.mongodb-api.com/app/geopixel-api-gkczdbu/endpoint/campaign";

        public async Task<Response<List<Campaign>>> GetCampaignsAsync(int pageNumber, int pageSize, string searchText = "")
        {
            try
            {
                var campaigns = await httpClient.GetFromJsonAsync<List<Campaign>>($"{ApiUrl}/readPaged?pageNumber={pageNumber}&pageSize={pageSize}&searchString={searchText}");

                if (campaigns is not null)
                {
                    foreach (var campaign in campaigns)
                    {
                        switch (campaign.Items.Count)
                        {
                            case 6:
                                campaign.Items.Add(new Item("Item 7", 0));
                                campaign.Items.Add(new Item("Item 8", 0));
                                break;
                            case 7:
                                campaign.Items.Add(new Item("Item 8", 0));
                                break;
                        }
                    } 
                }
                
                return campaigns == null ? new Response<List<Campaign>>("Não há campanhas cadastradas!", [], false)
                    : new Response<List<Campaign>>("Campanhas retornadas com sucesso.", campaigns, true);
            }
            catch (Exception ex)
            {
                return new Response<List<Campaign>>( $"Erro ao ler as campanhas: {ex.Message}", [], false);
            }
        }

        public async Task<Response<Campaign>> GetCampaignByIdAsync(string id)
        {
            try
            {
                var campaign = await httpClient.GetFromJsonAsync<Campaign>($"{ApiUrl}/readOne?id={id}");

                if (campaign is not null)
                {
                    switch (campaign.Items.Count)
                    {
                        case 6:
                            campaign.Items.Add(new Item("Item 7", 0));
                            campaign.Items.Add(new Item("Item 8", 0));
                            break;
                        case 7:
                            campaign.Items.Add(new Item("Item 8", 0));
                            break;
                    }
                }
                
                return campaign == null ? new Response<Campaign>("Não há campanha com o ID enviado", new Campaign(), false)
                    : new Response<Campaign>("Campanha retornada com sucesso.", campaign, true);
            }
            catch (Exception ex)
            {
                return new Response<Campaign>($"Erro ao encontrar campanha: {ex.Message}", new Campaign(), false);
            }
        }

        public async Task<Response<bool>> CreateCampaignAsync(Campaign campaign)
        {
            try
            {
                var httpResponse = await httpClient.PostAsJsonAsync($"{ApiUrl}/create", campaign);
                if (!httpResponse.IsSuccessStatusCode) return new Response<bool>( "Erro ao criar a campanha", false, false);
                
                await httpResponse.Content.ReadFromJsonAsync<Campaign>();
                return new Response<bool>("Campanha criada com sucesso", true, true);
            }
            catch (Exception ex)
            {
                return new Response<bool>( $"Erro ao criar a campanha: {ex.Message}", false, false);
            }
        }

        public async Task<Response<Campaign>> UpdateCampaignAsync(Campaign campaign)
        {
            try
            {
                var httpResponse = await httpClient.PutAsJsonAsync($"{ApiUrl}/update?id={campaign.CampaignId}", campaign);
                if (!httpResponse.IsSuccessStatusCode) return new Response<Campaign>("Erro ao atualizar campanha", new Campaign(), false);
                
                var updatedCampaign = await httpResponse.Content.ReadFromJsonAsync<Campaign>();
                return updatedCampaign != null ? new Response<Campaign>("Campanha atualizada com sucesso", updatedCampaign, true)
                    : new Response<Campaign>("Erro ao atualizar a campanha", new Campaign(), false);
            }
            catch (Exception ex)
            {
                return new Response<Campaign>($"Erro ao atualizar a campanha: {ex.Message}", new Campaign(), false);
            }
        }

        public async Task<Response<bool>> DeleteCampaignAsync(string id)
        {
            try
            {
                var httpResponse = await httpClient.DeleteAsync($"{ApiUrl}/delete?id={id}");
                return httpResponse.IsSuccessStatusCode ? new Response<bool>("Campanha deletada com sucesso", true, true)
                    : new Response<bool>("Erro ao deletar a campanha", false, false);
            }
            catch (Exception ex)
            {
                return new Response<bool>($"Erro ao deletar a campanha: {ex.Message}", false, false);
            }
        }

        public async Task<Response<Information>> GetInformationAsync()
        {
            try
            {
                var data = await httpClient.GetStringAsync($"{ApiUrl}/getInfo");
                Console.WriteLine(data);
                var information = JsonSerializer.Deserialize<Information>(data);
                
                return new Response<Information>("Informações recuperadas!", information, true);
            }
            catch (Exception ex)
            {
                return new Response<Information>($"Erro ao ler as informações: {ex.Message}", new Information(), false);
            }
        }
    }
}
