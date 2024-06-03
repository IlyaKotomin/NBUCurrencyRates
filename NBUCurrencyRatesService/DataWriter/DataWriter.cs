using NBUCurrencyRatesService.Configuration;
using NBUCurrencyRatesService.DataWriter;

namespace NBUCurrencyRatesService.API;

/// <summary>
///     Class responsible for writing currency rates to various file formats.
/// </summary>
/// <param name="logger">Logger instance for logging messages.</param>
public class DataWriter(ILogger logger)
{
    #region Methods

    /// <summary>
    ///     Saves currency rates to a file in the specified format.
    /// </summary>
    /// <param name="rates">Collection of currency rates to save.</param>
    /// <param name="path">Path where the output file will be saved.</param>
    /// <param name="fileName">Name of the output file.</param>
    /// <param name="fileType">Type of the output file format.</param>
    public void Save(IEnumerable<CurrencyRate> rates, string path, string fileName, OutputType fileType)
    {
        if (string.IsNullOrEmpty(path) || string.IsNullOrEmpty(fileName))
        {
            logger.LogError("Config exception!\nPlease, configure config.json in {appPath}", AppContext.BaseDirectory);
            return;
        }

        var dataWriterStrategy = CreateDataWriterStrategy(fileType);
        dataWriterStrategy.Save(rates, path, fileName);
    }
    
    /// <summary>
    /// Creates an instance of <see cref="IDataWriterStrategy"/> based on the specified <paramref name="fileType"/>.
    /// </summary>
    /// <param name="fileType">The type of output file to determine which strategy to create.</param>
    /// <returns>An instance of <see cref="IDataWriterStrategy"/> that corresponds to the specified <paramref name="fileType"/>.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="fileType"/> is not a recognized <see cref="OutputType"/>.</exception>
    private IDataWriterStrategy CreateDataWriterStrategy(OutputType fileType)
    {
        return fileType switch
        {
            OutputType.json => new JsonDataWriterStrategy(logger),
            OutputType.csv => new CsvDataWriterStrategy(logger),
            OutputType.xml => new XmlDataWriterStrategy(logger),
            _ => throw new ArgumentOutOfRangeException(nameof(fileType), fileType, null)
        };
    }

    #endregion
}