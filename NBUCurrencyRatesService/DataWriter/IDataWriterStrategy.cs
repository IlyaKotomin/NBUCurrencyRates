using NBUCurrencyRatesService.API;

namespace NBUCurrencyRatesService.DataWriter;

public interface IDataWriterStrategy
{
    /// <summary>
    ///     Saves currency rates to a file in the .xml file.
    /// </summary>
    /// <param name="rates">Collection of currency rates to save.</param>
    /// <param name="path">Path where the output file will be saved.</param>
    /// <param name="fileName">Name of the output file.</param>
    public void Save(IEnumerable<CurrencyRate> rates, string path, string fileName);
}