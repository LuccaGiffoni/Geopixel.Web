using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Geopixel.Web.Data.Enums;

namespace Geopixel.Web.Data.Models;

[Serializable]
public class Campaign
{
    #region Properties

    [JsonPropertyName("CampaignId")] public string CampaignId { get; set; }

    [Required(ErrorMessage = "O título é obrigatório."), MaxLength(50, ErrorMessage = "O título não pode ter mais de 50 caracteres")]
    [JsonPropertyName("Title")] public string Title { get; set; }
    
    [Required(ErrorMessage = "A descrição é obrigatória."), MaxLength(225, ErrorMessage = "A descrição não pode ter mais de 225 caracteres")]
    [JsonPropertyName("Description")] public string Description { get; set; }
    
    [Required(ErrorMessage = "A data de início é obrigatória")]
    [JsonPropertyName("Start")] public DateTime? Start { get; set; }
    
    [Required(ErrorMessage = "A data de fim é obrigatória")]
    [JsonPropertyName("End")] public DateTime? End { get; set; }
    
    [JsonPropertyName("Items")] public List<Item> Items { get; set; }
    
    [JsonPropertyName("Status")] public CampaignStatus Status { get; set; }

    #endregion

    #region Constructors

    public Campaign()
    {
        CampaignId = string.Empty;
        Title = string.Empty;
        Description = string.Empty;
        Start = DateTime.Today;
        End = DateTime.Today;
        Items = CreateVoidItems();
    }
    
    public Campaign(string title, string description, DateTime start, DateTime end, List<Item> items)
    {
        CampaignId = Guid.NewGuid().ToString();
        Title = title;
        Description = description;
        Start = start;
        End = end;
        Items = FormatItems(items);

        if (DateTime.Today > End || DateTime.Today < Start)
        {
            if (DateTime.Today > End) Status = CampaignStatus.Finished;
            else if (DateTime.Today < Start) Status = CampaignStatus.Scheduled;
        }
        else Status = CampaignStatus.Active;
    }

    #endregion

    #region Methods

    private static List<Item> CreateVoidItems()
    {
        var items = new List<Item>
        {
            new(string.Empty, 0),
            new(string.Empty, 0),
            new(string.Empty, 0),
            new(string.Empty, 0),
            new(string.Empty, 0),
            new(string.Empty, 0),
            new(string.Empty, 0),
            new(string.Empty, 0)
        };

        return items;
    }

    private List<Item> FormatItems(List<Item> receivedItems)
    {
        switch (receivedItems.Count)
        {
            case 6:
            {
                var seven = new Item(string.Empty, 0);
                receivedItems.Add(seven);
            
                var eight = new Item(string.Empty, 0);
                receivedItems.Add(eight);
                break;
            }
            case 7:
            {
                var eight = new Item(string.Empty, 0);
                receivedItems.Add(eight);
                break;
            }
        }

        return receivedItems;
    }

    #endregion
}