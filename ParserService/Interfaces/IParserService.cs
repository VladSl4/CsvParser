using ParserService.Models;

namespace ParserService.Interfaces;

public interface IParserService
{
    Task<List<TripRecord>> ParseCsvAsync(string filePath, CancellationToken cancellationToken);
}