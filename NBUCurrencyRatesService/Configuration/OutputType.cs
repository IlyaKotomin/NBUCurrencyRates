using System.Diagnostics.CodeAnalysis;

namespace NBUCurrencyRatesService.Configuration;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public enum OutputType
{
    json,
    csv,
    xml
}