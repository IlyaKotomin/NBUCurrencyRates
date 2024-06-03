using System.Diagnostics.CodeAnalysis;

namespace NBUCurrencyRatesService.Configuration;

/// <summary>
///     Enumeration defining the output types for saving currency rates.
/// </summary>
[SuppressMessage("ReSharper", "InconsistentNaming")]
public enum OutputType
{
    /// <summary>
    ///     Output type for JSON format.
    /// </summary>
    json,

    /// <summary>
    ///     Output type for CSV format.
    /// </summary>
    csv,

    /// <summary>
    ///     Output type for XML format.
    /// </summary>
    xml
}