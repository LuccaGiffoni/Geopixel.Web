using Geopixel.Web.Data.Models;
using Geopixel.Web.Data.Services.Interfaces;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Geopixel.Web.Data.Pages;

public partial class CreateCampaign : ComponentBase
{
    private MudForm? Form { get; set; } = null!;
    private Campaign Campaign { get; set; } = new();
    private DateRange DateRange { get; set; } = null!;
        
    [Inject] public IService CampaignService { get; set; } = null!;
    [Inject] public NavigationManager Navigation { get; set; } = null!;
    [Inject] public ISnackbar Snackbar { get; set; } = null!;

    private async Task HandleValidSubmit()
    {
        try
        {
            Campaign.Start = DateRange.Start;
            Campaign.End = DateRange.End;
            
            Campaign.CampaignId = Guid.NewGuid().ToString();

            if (Campaign.Items[7].Description == string.Empty) Campaign.Items.RemoveAt(7);
            if (Campaign.Items[6].Description == string.Empty) Campaign.Items.RemoveAt(6);
            
            await CampaignService.CreateCampaignAsync(Campaign);
            Snackbar.Add("Campanha criada com sucesso!", Severity.Success);
            
            Navigation.NavigateTo("/campaigns");
        }
        catch (Exception ex)
        {
            Snackbar.Add("Erro ao criar a campanha: " + ex.Message, Severity.Error);
        }
    }
    
    private string ReturnItemDescription(int index)
    {
        return string.IsNullOrEmpty(Campaign.Items[index].Description)
            ? $"Item {index + 1}" : Campaign.Items[index].Description;
    }
    
    private async Task Cancel()
    {
        if (Form == null)
        { 
            Snackbar.Add("O formulário de criação é nulo.", Severity.Error);
            return;
        }
        
        await Form.ResetAsync();
        Navigation.NavigateTo("/campaigns");
    }
}