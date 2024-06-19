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
        Campaign = await CampaignService.GetCampaignByIdAsync(CampaignId);

        if (Campaign != null)
        {
            DateRange = new DateRange(Campaign.Start, Campaign.End);
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
            await CampaignService.UpdateCampaignAsync(Campaign);
            
            Snackbar.Add("Campanha atualizada!", Severity.Success);
            Navigation.NavigateTo("/campaigns");
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
        Navigation.NavigateTo("/campaigns");
    }
}