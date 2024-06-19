using Geopixel.Web.Data.Models;
using Geopixel.Web.Data.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Geopixel.Web.Pages
{
    public partial class Campaigns : ComponentBase
    {
        private MudTable<Campaign>? table;
        // private Information Information = new();
        private List<Campaign>? CampaignsList = new();
        private int totalItems;
        private string searchString = "";
        private bool loading = true;

        [Inject] public IService CampaignService { get; set; } = null!;
        [Inject] public NavigationManager Navigation { get; set; } = null!;
        [Inject] public IDialogService Dialog { get; set; } = null!;
        [Inject] public ISnackbar Snackbar { get; set; } = null!;

        // protected override async Task OnInitializedAsync()
        // {
        //     await GetInformation();
        // }
        //
        // private async Task GetInformation()
        // {
        //     var response = await CampaignService.GetInformationAsync();
        //
        //     if (response.IsSuccess)
        //     {
        //         Information = response.Data;
        //     }
        //     else
        //     {
        //         Snackbar.Add($"Não foi possível ler as informações: {response.Message}", Severity.Error);
        //     }
        // }
        
        private async Task<TableData<Campaign>> ServerReload(TableState state)
        {
            loading = true;

            try
            {
                var response = await CampaignService.GetCampaignsAsync(state.Page + 1, state.PageSize, searchString);
                if (response.IsSuccess && response.Data != null)
                {
                    CampaignsList = response.Data;
                    totalItems = response.Data.Count;
                }
                else
                {
                    Snackbar.Add($"Não foi possível ler as campanhas: {response.Message}", Severity.Error);
                }
            }
            catch (Exception e)
            {
                Snackbar.Add($"Erro ao invocar o método de leitura: {e.Message}", Severity.Error);
            }
            finally
            {
                loading = false;
            }

            return new TableData<Campaign>
            {
                TotalItems = totalItems,
                Items = CampaignsList ?? new List<Campaign>()
            };
        }

        private void OnSearch(string text)
        {
            searchString = text;
            table?.ReloadServerData();
        }
        
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
                var response = await CampaignService.DeleteCampaignAsync(campaign.CampaignId);
                if (response.IsSuccess)
                {
                    await table!.ReloadServerData();
                }
                else
                {
                    Snackbar.Add($"Erro ao excluir a campanha: {response.Message}", Severity.Error);
                }
            }
        }

        private Color GetStatusColor(string status) => status switch
        {
            "Active" => Color.Info,
            "Scheduled" => Color.Warning,
            "Finished" => Color.Success,
            _ => Color.Default
        };
    }
}
