using System.Threading.Tasks;

namespace AstrolPOSAPI.Application.Interfaces.Infrastructure
{
    public interface ISmsSender
    {
        Task<SmsSendResponse> SendSmsAsync(string phoneNumber, string message);
        Task<(string Success, string Message, string Status)> CheckDeliveryStatusAsync(string deliveryCode);
    }

    public class SmsSendResponse
    {
        public required string Success { get; set; }
        public required string Message { get; set; }
        public int? Balance { get; set; }
        public string? DeliveryCode { get; set; }
    }
}
