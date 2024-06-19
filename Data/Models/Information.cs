using System.Text.Json.Serialization;

namespace Geopixel.Web.Data.Models;

[Serializable]
public class Information
{
    [JsonPropertyName("0")] public int Active { get; set; }
    [JsonPropertyName("1")] public int Scheduled { get; set; }
    [JsonPropertyName("2")] public int Finished { get; set; }
}