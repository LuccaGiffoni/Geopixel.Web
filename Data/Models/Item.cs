using System.Text.Json.Serialization;

namespace Geopixel.Web.Data.Models;

public class Item(string description, int quantity)
{
    [JsonPropertyName("Description")] public string Description { get; set; } = description;
    [JsonPropertyName("InitialQuantity")] public int InitialQuantity { get; set; } = quantity;
    [JsonPropertyName("Quantity")] public int Quantity { get; set; } = quantity;
}