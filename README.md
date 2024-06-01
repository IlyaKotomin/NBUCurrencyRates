# NBU Currency Rates Service

---

Windows service for working with exchange rates of the National Bank of Ukraine written in .NET Core 8.0 | C# 12

---

## CLI Commands (Powershell):

---

### Install
```bash 
sc create "NBU Rates Service" binpath="path/to/NBURatesService.exe"
```

### Uninstall
```bash
sc delete "NBU Rates Service"
```


### Set autostart
```Bash
sc config "NBU Rates Service" start= auto
```

---


## Configuration
The configuration file `config.js` is located in the folder the executable file `NBURatesService.exe` and looks like this:

```json
{
  //In milliseconds
  "FetchFrequency": 5000,

  //The output will be in the folder with the executable file if the field is empty
  "OutputPath": "",

  //The output file name will be Rates if the field is empty
  "OutputFileName": "",
  
  //The output file type name will be json if the field is empty 
  //              (You can use this types: xml, json, csv)
  "FileType": ""          
}
```
The program configuration is reloaded before each data fetch, so after changing the configuration file, all settings are automatically applied without restarting the service.

Thanks to this, the file can be overwritten in other programs and dynamically change saving settings.

---


## Dependencies

---


### OS And Framework:
    Windows
    .NET Core 8.0

### NuGet Packages:
- They are used to correctly start the service on a Windows system.
  - `Microsoft.Extensions.Hosting`
  - `Microsoft.Extensions.Hosting.WindowsServices`


- Json serialization and deserialization for API Models:
  - `Newtonsoft.Json`

---


## Control codes?
This project does NOT use control codes to control the service, since this is outdated (`ServiceBase.OnCustomCommand(Int32)`) and is not an appropriate method of application communication.

In place, the configuration is updated itself before loading data from the API
without using codes. (Since the information in the API is rarely updated, auto-updating the config does not require many resources)

`PS:
using WindowsServiceLifetime is a bad practice due to the 
fact that the source code of this class is poorly written. 
ServiceBase is only suitable for .NET Framework, 
which is an outdated technology so my choise is BackgraoundService`

---


## Troubleshooting
### How can I find out what error is stopping the service?
Use `Event Viewer` (Windows App)!
There you can also see all the service 
logs. I recommend adding 
"Custom View" to view logs only for this service

### Frequent problems from logs:
- Error! Can not get data from https://bank.gov.ua/NBUStatService/v1/statdirectory/exchange?json
  - Solution: Check your internet connection
- Config error
  - Solution: Replace your config.json to new one (see #Configuration)

---

# Code Overview:

---

## Class Overview: `Config`

A configuration class for the NBU Currency Rates Service. The `Config` class is responsible for initializing and managing configuration settings used by the service, including fetching frequencies, output file types, file names, and paths.

### Namespace

`NBUCurrencyRatesService.Configuration`

### Purpose

The `Config` class handles the setup and management of configuration settings for the NBU Currency Rates Service. It reads configurations from a JSON file and provides mechanisms for reloading configurations dynamically.

### Constructor

```csharp
public Config(IConfiguration configuration, ILogger logger)
```

- **Parameters:**
  - `configuration`: An instance of `IConfiguration` to access configuration settings.
  - `logger`: An instance of `ILogger` to log information.

### Fields

- `private readonly ILogger _logger;`
- `private IConfiguration _configuration;`
- `public int FetchFrequency;`
- `public OutputType FileType;`
- `public string OutputFileName;`
- `public string OutputPath;`

### Methods

#### `Config(IConfiguration configuration, ILogger logger)`

Constructor that initializes the `Config` class with the provided `configuration` and `logger`. It calls the `Initialize` method to load configuration settings.

#### `void Reload()`

Reloads the configuration settings by reinitializing with the default configuration. Logs the reload action.

#### `static IConfiguration GetDefaultConfiguration()`

Builds and returns the default configuration from the `config.json` file located at the application's base directory.

#### `private void Initialize(IConfiguration configuration)`

Initializes the configuration settings from the provided `configuration` object. It sets the `FetchFrequency`, `OutputPath`, `OutputFileName`, and `FileType` fields.

### Configuration Settings

- **FetchFrequency**: The frequency (in ms) at which the service fetches currency rates.
- **FileType**: The type of output file (JSON, CSV, XML).
- **OutputFileName**: The name of the output file.
- **OutputPath**: The path where the output file will be saved.

### Example Configuration (`config.json`)

```json
{
  "FetchFrequency": "15000",
  "OutputFileName": "CurrencyRates",
  "OutputPath": "D:/Rates",
  "FileType": "JSON"
}
```

### Usage

1. Create an instance of the `Config` class by passing an `IConfiguration` object and an `ILogger` object.
2. Use the `Reload` method to reload the configuration settings if needed.

### Sample Code

```csharp
var configuration = Config.GetDefaultConfiguration();
var logger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger<Config>();
var config = new Config(configuration, logger);

Console.WriteLine($"Fetch Frequency: {config.FetchFrequency}");
Console.WriteLine($"Output File Type: {config.FileType}");
Console.WriteLine($"Output File Name: {config.OutputFileName}");
Console.WriteLine($"Output Path: {config.OutputPath}");
```

### Notes

- Ensure that the `config.json` file is present in the base directory of the application.
- The `FileType` field is parsed as an enumeration, ensure the value in the configuration file matches one of the `OutputType` enumeration values.
- Logging is enabled to track configuration reloads and other operations.

---

## Class Overview: `NBUGrabber`

A service class for fetching currency rates from the National Bank of Ukraine (NBU) API. The `NBUGrabber` class is responsible for making HTTP requests to the NBU API and deserializing the response into a list of currency rate objects.

### Namespace

`NBUCurrencyRatesService.API`

### Purpose

The `NBUGrabber` class handles the fetching of currency rates from the NBU API. It sends HTTP requests to the specified API endpoint, retrieves the data in JSON format, and deserializes it into a list of `CurrencyRate` objects.

### Constructor

```csharp
public NBUGrabber(ILogger logger)
```

- **Parameter:**
  - `logger`: An instance of `ILogger` to log information and errors.

### Fields

- `private const string ApiUrl = "https://bank.gov.ua/NBUStatService/v1/statdirectory/exchange?json";`

### Methods

#### `public async Task<List<CurrencyRate>?> FetchRates()`

Fetches the currency rates from the NBU API.

- **Returns:**
  - A `Task` that represents the asynchronous operation. The task result contains a list of `CurrencyRate` objects or `null` if an error occurs.

### Dependencies

- **Newtonsoft.Json**: Used for deserializing the JSON response from the API.

### Usage

1. Create an instance of the `NBUGrabber` class by passing an `ILogger` object.
2. Call the `FetchRates` method to fetch currency rates from the NBU API.

### Example Code

```csharp
var logger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger<NBUGrabber>();
var grabber = new NBUGrabber(logger);

var rates = await grabber.FetchRates();
if (rates != null)
{
    foreach (var rate in rates)
    {
        Console.WriteLine($"Currency: {rate.CurrencyCode}, Rate: {rate.Rate}");
    }
}
else
{
    Console.WriteLine("Failed to fetch currency rates.");
}
```

### Logging

- Logs an informational message when fetching data from the API.
- Logs an error message if an exception occurs during the fetch operation.

### Notes

- Ensure that the `CurrencyRate` class is defined to match the structure of the JSON response from the NBU API.
- The `FetchRates` method uses `HttpClient` to send HTTP requests. Make sure to handle the proper disposal of `HttpClient` if used in a different context.

### `CurrencyRate` Class

```csharp
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
```

### API URL

- The NBU API URL used to fetch currency rates is `https://bank.gov.ua/NBUStatService/v1/statdirectory/exchange?json`.

---

## Class Overview: `RatesWriter`

A service class for saving currency rates to various file formats. The `RatesWriter` class provides methods to save currency rates as JSON, CSV, or XML files based on the specified output type.

### Namespace

`NBUCurrencyRatesService.API`

### Purpose

The `RatesWriter` class is responsible for saving currency rates to files in different formats. It accepts a collection of currency rates, a file path, a file name, and the desired output type (JSON, CSV, or XML). Based on the output type, it writes the rates to the corresponding file format.

### Constructor

```csharp
public RatesWriter(ILogger logger)
```

- **Parameter:**
  - `logger`: An instance of `ILogger` to log information and errors.

### Methods

#### `public void Save(IEnumerable<CurrencyRate> rates, string path, string fileName, OutputType fileType)`

Saves the currency rates to a file based on the specified output type.

- **Parameters:**
  - `rates`: A collection of currency rates to be saved.
  - `path`: The directory path where the file will be saved.
  - `fileName`: The name of the file to be saved.
  - `fileType`: The output file type (JSON, CSV, or XML).

### Dependencies

- **Newtonsoft.Json**: Used for serializing/deserializing JSON data.
- **System.Xml.Serialization**: Used for serializing/deserializing XML data.

### Usage

1. Create an instance of the `RatesWriter` class by passing an `ILogger` object.
2. Call the `Save` method with the currency rates, file path, file name, and output type to save the rates to the desired file format.

### Example Code

```csharp
var logger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger<RatesWriter>();
var writer = new RatesWriter(logger);

var rates = GetCurrencyRates(); // Some magic to get Currency Rates (NBUGrabber for exampele)
var path = "/path/to/save";
var fileName = "CurrencyRates";

writer.Save(rates, path, fileName, OutputType.Json);
```

### Logging

- Logs an error if the provided path or file name is empty.
- Logs an informational message after successfully saving the rates to a file.

### Notes

- The `Save` method uses switch-case statements to determine the output file type and calls the corresponding private method (`SaveJson`, `SaveCsv`, or `SaveXml`) accordingly.

### File Formats

- **JSON**: Currency rates are saved as JSON objects in a `.json` file.
- **CSV**: Currency rates are saved as comma-separated values in a `.csv` file.
- **XML**: Currency rates are saved as XML elements in an `.xml` file.

### Example JSON Output

```json
[
  {
    "r030": 36,
    "txt": "Австралійський долар",
    "cc": "AUD",
    "rate": 26.9573,
    "exchangedate": "03.06.2024"
  },
  {
    "r030": 124,
    "txt": "Канадський долар",
    "cc": "CAD",
    "rate": 29.7151,
    "exchangedate": "03.06.2024"
  }
]
```

### Example CSV Output

```
R030,FullName,CurrencyCode,Rate,ExchangeDate
36,Австралійський долар,AUD,26.9573,03.06.2024
124,Канадський долар,CAD,29.7151,03.06.2024
156,Юань Женьміньбі,CNY,5.5984,03.06.2024
203,Чеська крона,CZK,1.7796,03.06.2024
208,Данська крона,DKK,5.8972,03.06.2024
344,Гонконгівський долар,HKD,5.1853,03.06.2024
```

### Example XML Output

```xml
<?xml version="1.0" encoding="utf-8"?>
<ArrayOfCurrencyRate xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
  <CurrencyRate>
    <R030>36</R030>
    <FullName>Австралійський долар</FullName>
    <CurrencyCode>AUD</CurrencyCode>
    <Rate>26.9573</Rate>
    <ExchangeDate>03.06.2024</ExchangeDate>
  </CurrencyRate>
  <CurrencyRate>
    <R030>124</R030>
    <FullName>Канадський долар</FullName>
    <CurrencyCode>CAD</CurrencyCode>
    <Rate>29.7151</Rate>
    <ExchangeDate>03.06.2024</ExchangeDate>
  </CurrencyRate>
</ArrayOfCurrencyRate>
```
