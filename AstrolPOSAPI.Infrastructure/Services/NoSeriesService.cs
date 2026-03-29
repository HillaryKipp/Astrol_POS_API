using AstrolPOSAPI.Application.Interfaces.Repositories;
using AstrolPOSAPI.Application.Interfaces.Services;
using AstrolPOSAPI.Domain.Entities.Core;
using Microsoft.EntityFrameworkCore;

namespace AstrolPOSAPI.Infrastructure.Services
{
    public class NoSeriesService : INoSeriesService
    {
        private readonly IUnitOfWork _unitOfWork;

        public NoSeriesService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<string> GenerateNextNumberAsync(string seriesCode, CancellationToken cancellationToken = default)
        {
            var allSeries = await _unitOfWork.Repository<NoSeries>().GetAllAsync();
            var noSeries = allSeries.FirstOrDefault(ns => ns.Code == seriesCode);

            if (noSeries == null)
            {
                throw new InvalidOperationException($"NoSeries with code '{seriesCode}' not found. Please create it first.");
            }

            // Parse current number or default to 0
            int currentNumber = 0;
            if (!string.IsNullOrEmpty(noSeries.CurrentNo))
            {
                int.TryParse(noSeries.CurrentNo, out currentNumber);
            }

            // Increment
            currentNumber++;

            // Format the number with leading zeros (assuming 3 digits minimum)
            string formattedNumber = currentNumber.ToString("D3");

            // Build the full code with prefix and suffix
            string generatedCode = $"{noSeries.Prefix ?? ""}{formattedNumber}{noSeries.Suffix ?? ""}";

            // Update the CurrentNo in the database
            noSeries.CurrentNo = currentNumber.ToString();
            await _unitOfWork.Repository<NoSeries>().UpdateAsync(noSeries);
            await _unitOfWork.Save(cancellationToken);

            return generatedCode;
        }
    }
}
