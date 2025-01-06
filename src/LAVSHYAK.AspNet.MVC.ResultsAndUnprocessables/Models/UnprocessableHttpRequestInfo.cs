using System.Text.Json.Serialization;

namespace LAVSHYAK.AspNet.MVC.ResultsAndUnprocessables.Models;

public class UnprocessableHttpRequestInfo<TEnum> where TEnum : Enum
{
    [JsonPropertyName("code")]
    public required int Code { get; set; }
    
    [JsonPropertyName("name")]
    public required string Name { get; set; }
    
    [JsonPropertyName("enum")]
    public required TEnum Enum { get; set; }
    
    [JsonPropertyName("description")]
    public required string Description { get; set; }
}