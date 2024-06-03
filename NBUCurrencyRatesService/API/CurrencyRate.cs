using Newtonsoft.Json;

namespace NBUCurrencyRatesService.API;

/// <summary>
///     Model representing a currency rate retrieved from the NBU (National Bank of Ukraine) API.
/// </summary>
public class CurrencyRate
{
    /// <summary>
    ///     Numeric code of the currency.
    /// </summary>
    // ReSharper disable StringLiteralTypo
    [JsonProperty("r030")]
    public int R030 { get; set; }

    /// <summary>
    ///     Full name of the currency.
    /// </summary>
    [JsonProperty("txt")]
    public string? FullName { get; set; }


    /// <summary>
    ///     Alphabetic currency code.
    /// </summary>
    [JsonProperty("cc")]
    public string? CurrencyCode { get; set; }

    /// <summary>
    ///     Exchange rate of the currency.
    /// </summary>
    [JsonProperty("rate")]
    public float Rate { get; set; }

    /// <summary>
    ///     Date of the exchange rate.
    /// </summary>
    [JsonProperty("exchangedate")]
    public string? ExchangeDate { get; set; }
    // ReSharper restore StringLiteralTypo
}