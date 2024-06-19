using Geopixel.Web.Data.Enums;
using Geopixel.Web.Data.Models;
using Geopixel.Web.Data.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Geopixel.Web.Pages;

public partial class UpdateCampaign : ComponentBase
{
    private MudForm? Form { get; set; } = null!;
    private Campaign? Campaign { get; set; } = new();
    private DateRange DateRange { get; set; } = new (DateTime.Now.Date, DateTime.Now.AddDays(1).Date);

    [Parameter] public string CampaignId { get; set; } = string.Empty;

    [Inject] public IService CampaignService { get; set; } = null!;
    [Inject] public NavigationManager Navigation { get; set; } = null!;
    [Inject] public ISnackbar Snackbar { get; set; } = null!;

    protected override async Task OnParametersSetAsync()
    {
        try
        {
            var response = await CampaignService.GetCampaignByIdAsync(CampaignId);
            
            if (response.IsSuccess)
            {
                Campaign = response.Data;
                DateRange = new DateRange(Campaign.Start, Campaign.End);
            }
            else
            {
                Snackbar.Add($"Erro na leitura da campanha", Severity.Error);
            }
        }
        catch (Exception e)
        {
            Snackbar.Add($"Erro ao invocar método de leitura: {e.Message}", Severity.Error);
        }

    }
    
    private string ReturnItemDescription(int index)
    {
        if (Campaign == null) return "Item";
        
        switch (index)
        {
            case <= 5:
            {
                return string.IsNullOrEmpty(Campaign.Items[index].Description)
                    ? $"Item {index + 1}" : Campaign.Items[index].Description;   
            }
            case 6:
            {
                if (Campaign.Items.Count >= 6)
                {
                    return string.IsNullOrEmpty(Campaign.Items[index].Description)
                        ? $"Item {index + 1}" : Campaign.Items[index].Description;
                }

                return "Item 7";
            }
            case 7:
            {
                if (Campaign.Items.Count >= 6)
                {
                    return string.IsNullOrEmpty(Campaign.Items[index].Description)
                        ? $"Item {index + 1}" : Campaign.Items[index].Description;
                }

                return "Item 8";
            }
        }

        return string.Empty;
    }
    
    private async Task HandleValidSubmit()
    {
        if (Form == null)
        { 
            Snackbar.Add("O formulário de edição é nulo.", Severity.Error);
            return;
        }
        
        await Form.Validate();

        if (Form.IsValid)
        {
            Campaign.Start = DateRange.Start;
            Campaign.End = DateRange.End;

            if (Campaign.Start > DateTime.Now) Campaign.Status = CampaignStatus.Scheduled;
            if (Campaign.Start <= DateTime.Now && Campaign.End >= DateTime.Now) Campaign.Status = CampaignStatus.Active;
            if (Campaign.End < DateTime.Now) Campaign.Status = CampaignStatus.Finished;

            if (Campaign.Items[7].Description == string.Empty) Campaign.Items.RemoveAt(7);
            if (Campaign.Items[6].Description == string.Empty) Campaign.Items.RemoveAt(6);
            
            await CampaignService.UpdateCampaignAsync(Campaign);
            
            Snackbar.Add("Campanha atualizada!", Severity.Success);
            Navigation.NavigateTo("/Geopixel.Web/campaigns");
        }
    }
    
    private async Task Cancel()
    {
        if (Form == null)
        { 
            Snackbar.Add("O formulário de edição é nulo.", Severity.Error);
            return;
        }
        
        await Form.ResetAsync();
        Navigation.NavigateTo("/Geopixel.Web/campaigns");
    }
}