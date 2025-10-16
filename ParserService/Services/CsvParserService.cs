using CsvHelper;
using CsvHelper.Configuration;
using ParserService.Interfaces;
using ParserService.Models;
using System.Globalization;
using Grpc.Core;

namespace ParserService.Services;

public class CsvParserService : IParserService
{
    private readonly IWebHostEnvironment _hostingEnvironment;
    
    public CsvParserService(IWebHostEnvironment hostingEnvironment)
    {
        _hostingEnvironment = hostingEnvironment;
    }
    
    public async Task<List<TripRecord>> ParseCsvAsync(string filePath, CancellationToken cancellationToken)
    {
        if (!File.Exists(filePath))
            throw new RpcException(new Status(StatusCode.NotFound, $"The specified CSV file could not be found: {Path.GetFileName(filePath)}"));
        
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true,
            TrimOptions = TrimOptions.Trim,
            HeaderValidated = null,
            MissingFieldFound = null,
        };

        var uniqueKeys = new HashSet<string>();
        var uniqueRecords = new List<TripRecord>();
        var duplicateRecordsForFile = new List<dynamic>();

        using var reader = new StreamReader(filePath);
        using var csv = new CsvReader(reader, config);
        
        csv.Context.RegisterClassMap<TripRecordMap>();
        
        await foreach (var record in csv.GetRecordsAsync<TripRecord>(cancellationToken))
        {
            var key = $"{record.PickupDatetime:o}|{record.DropoffDatetime:o}|{record.PassengerCount}";
            
            if (!uniqueKeys.Add(key))
            {
                duplicateRecordsForFile.Add(csv.GetRecord<dynamic>());
                continue;
            }
            
            var easternZone = GetEasternTimeZone();
            record.PickupDatetime = TimeZoneInfo.ConvertTimeToUtc(record.PickupDatetime, easternZone);
            record.DropoffDatetime = TimeZoneInfo.ConvertTimeToUtc(record.DropoffDatetime, easternZone);
            
            uniqueRecords.Add(record);
        }
        
        if (duplicateRecordsForFile.Any())
        {
            string solutionRootPath = Directory.GetParent(_hostingEnvironment.ContentRootPath).FullName;
            
            string duplicatesFilePath = Path.Combine(solutionRootPath, "Data", "duplicates.csv");
            
            await WriteDuplicatesToFileAsync(duplicatesFilePath, duplicateRecordsForFile, cancellationToken);
        }

        return uniqueRecords;
    }
    
    private async Task WriteDuplicatesToFileAsync(string filePath, IEnumerable<dynamic> duplicates, CancellationToken cancellationToken)
    {
        await using var writer = new StreamWriter(filePath);
        await using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
        await csv.WriteRecordsAsync(duplicates, cancellationToken);
    }
    
    private TimeZoneInfo GetEasternTimeZone()
    {
        try { return TimeZoneInfo.FindSystemTimeZoneById("America/New_York"); }
        catch { return TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time"); }
    }
}