using Newtonsoft.Json;

namespace NBUCurrencyRatesService.API;

public class CurrencyRate
{
    // ReSharper disable StringLiteralTypo
    [JsonProperty("r030")] public int R030 { get; set; }
    [JsonProperty("txt")] public string? FullName { get; set; }
    [JsonProperty("cc")] public string? CurrencyCode { get; set; }
    [JsonProperty("rate")] public float Rate { get; set; }

    [JsonProperty("exchangedate")] public string? ExchangeDate { get; set; }
    // ReSharper restore StringLiteralTypo
}