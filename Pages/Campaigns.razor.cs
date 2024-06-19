using Geopixel.Web.Data.Models;
using Geopixel.Web.Data.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Geopixel.Web.Pages
{
    public partial class Campaigns : ComponentBase
    {
        private List<Campaign> CampaignsList = new();
        private bool loading = true;
        private string searchString = "";
        
        [Inject] public IService CampaignService { get; set; } = null!;
        [Inject] public NavigationManager Navigation { get; set; } = null!;
        [Inject] public IDialogService Dialog { get; set; } = null!;

        protected override async Task OnInitializedAsync()
        {
            await LoadCampaigns();
        }

        private async Task LoadCampaigns()
        {
            loading = true;
            CampaignsList = await CampaignService.GetCampaignsAsync();
            loading = false;
        }

        private bool FilterFunc(Campaign campaign) =>
            string.IsNullOrWhiteSpace(searchString) ||
            campaign.Title.Contains(searchString, StringComparison.OrdinalIgnoreCase) ||
            campaign.Description.Contains(searchString, StringComparison.OrdinalIgnoreCase);
        
        private void EditCampaign(Campaign campaign)
        {
            Navigation.NavigateTo($"/Geopixel.Web/campaigns/update/{campaign.CampaignId}");
        }

        private async Task DeleteCampaign(Campaign campaign)
        {
            var confirmed = await Dialog.ShowMessageBox(
                "Confirmação",
                "Tem certeza de que deseja excluir esta campanha?",
                yesText: "Sim", noText: "Não");

            if (confirmed == true)
            {
                await CampaignService.DeleteCampaignAsync(campaign.CampaignId);
                await LoadCampaigns();
            }
        }
    }
}