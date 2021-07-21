using CovidCases.Helpers;

namespace CovidCases.Models
{
    public enum ExportType
    {
        [StringValue(".xml")]
        XML,
        [StringValue(".json")]
        JSON,
        [StringValue(".csv")]
        CSV
    }
}
