namespace Storefront.Models;

public class SearchRequest
{
    /// <summary>
    /// ID локації.
    /// </summary>
    /// <example>109</example>
    public int PuLocationId { get; set; }
    
    /// <summary>
    /// Кількість запитів.
    /// </summary>
    /// <example>10</example>
    public int Count { get; set; }
}