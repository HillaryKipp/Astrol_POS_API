namespace AstrolPOSAPI.Application.Interfaces.Services
{
    public interface INoSeriesService
    {
        /// <summary>
        /// Generates the next number in a series and increments the counter
        /// </summary>
        /// <param name="seriesCode">The code of the NoSeries entity (e.g., "COMPANY", "EMPLOYEE")</param>
        /// <param name="cancellationToken"></param>
        /// <returns>The formatted next number (e.g., "COMP-001")</returns>
        Task<string> GenerateNextNumberAsync(string seriesCode, CancellationToken cancellationToken = default);
    }
}
