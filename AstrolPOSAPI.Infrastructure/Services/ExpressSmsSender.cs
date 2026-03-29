using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using AstrolPOSAPI.Application.Interfaces.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace AstrolPOSAPI.Infrastructure.Services
{
    public class ExpressSmsSender : ISmsSender
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;
        private readonly ILogger<ExpressSmsSender> _logger;

        public ExpressSmsSender(IConfiguration configuration, IHttpClientFactory httpClientFactory, ILogger<ExpressSmsSender> logger)
        {
            _configuration = configuration;
            _httpClient = httpClientFactory.CreateClient("ExpressSMS");
            _logger = logger;
        }

        public async Task<SmsSendResponse> SendSmsAsync(string phoneNumber, string message)
        {
            var normalizedPhone = NormalizeKenyanPhoneNumber(phoneNumber);
            var apiKey = _configuration["ExpressSMS:ApiKey"];

            if (string.IsNullOrWhiteSpace(apiKey))
                throw new InvalidOperationException("ExpressSMS API key is not configured.");

            var payload = new
            {
                api_key = apiKey,
                message,
                phone = normalizedPhone
            };

            var jsonContent = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

            _logger.LogInformation("Sending SMS to {PhoneNumber}", normalizedPhone);

            var response = await _httpClient.PostAsync("api/sms/send", jsonContent);

            var responseContent = await response.Content.ReadAsStringAsync();
            var trimmedResponse = responseContent.Trim();

            _logger.LogInformation("API response: {Response}", responseContent);

            // Handle plain text responses first
            if (trimmedResponse.Equals("OK", StringComparison.OrdinalIgnoreCase) ||
                trimmedResponse.Equals("success", StringComparison.OrdinalIgnoreCase))
            {
                return new SmsSendResponse
                {
                    Success = "Success",
                    Message = "Message Sent Successfully",
                    Balance = null, // You might want to track balance separately
                    DeliveryCode = null
                };
            }

            if (!response.IsSuccessStatusCode)
            {
                try
                {
                    var error = JsonSerializer.Deserialize<ExpressSmsErrorResponse>(responseContent);
                    return new SmsSendResponse
                    {
                        Success = "Fail",
                        Message = error?.ErrorMessage ?? responseContent,
                        Balance = null,
                        DeliveryCode = null
                    };
                }
                catch (JsonException)
                {
                    return new SmsSendResponse
                    {
                        Success = "Fail",
                        Message = responseContent,
                        Balance = null,
                        DeliveryCode = null
                    };
                }
            }

            try
            {
                var success = JsonSerializer.Deserialize<ExpressSmsSuccessResponse>(responseContent);

                if (success == null)
                {
                    return new SmsSendResponse
                    {
                        Success = "Fail",
                        Message = "Empty response",
                        Balance = null,
                        DeliveryCode = null
                    };
                }

                if (!string.Equals(success.Success, "Success", StringComparison.OrdinalIgnoreCase))
                {
                    return new SmsSendResponse
                    {
                        Success = "Fail",
                        Message = success.Message ?? "Unknown error",
                        Balance = null,
                        DeliveryCode = null
                    };
                }

                // Return the complete response matching your desired format
                return new SmsSendResponse
                {
                    Success = "Success",
                    Message = success.Message ?? "Message Sent Successfully",
                    Balance = success.Balance,
                    DeliveryCode = success.DeliveryCode
                };
            }
            catch (JsonException ex)
            {
                _logger.LogWarning("Failed to deserialize JSON response: {Exception}. Raw response: {Response}", ex.Message, responseContent);

                return new SmsSendResponse
                {
                    Success = "Fail",
                    Message = $"Failed to parse response: {responseContent}",
                    Balance = null,
                    DeliveryCode = null
                };
            }
        }


        public async Task<(string Success, string Message, string Status)> CheckDeliveryStatusAsync(string deliveryCode)
        {
            var apiKey = _configuration["ExpressSMS:ApiKey"];
            if (string.IsNullOrWhiteSpace(apiKey))
                throw new InvalidOperationException("ExpressSMS API key is not configured.");

            var payload = new
            {
                api_key = apiKey,
                delivery_code = deliveryCode
            };

            var jsonContent = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

            _logger.LogInformation("Checking delivery status for {DeliveryCode}", deliveryCode);

            var response = await _httpClient.PostAsync("api/sms/status", jsonContent);
            var responseContent = await response.Content.ReadAsStringAsync();

            _logger.LogInformation("API response: {Response}", responseContent);

            if (!response.IsSuccessStatusCode)
            {
                try
                {
                    var error = JsonSerializer.Deserialize<ExpressSmsErrorResponse>(responseContent);
                    return ("Fail", error?.ErrorMessage ?? responseContent, "Unknown");
                }
                catch (JsonException)
                {
                    return ("Fail", responseContent, "Unknown");
                }
            }

            try
            {
                var status = JsonSerializer.Deserialize<ExpressSmsStatusResponse>(responseContent);
                return ("Success", "Status retrieved successfully", status?.DeliveryStatus ?? "Unknown");
            }
            catch (JsonException ex)
            {
                _logger.LogWarning("Failed to deserialize status response: {Exception}. Raw response: {Response}", ex.Message, responseContent);

                var trimmedResponse = responseContent.Trim();
                if (trimmedResponse.Equals("OK", StringComparison.OrdinalIgnoreCase) ||
                    trimmedResponse.StartsWith("Success", StringComparison.OrdinalIgnoreCase))
                {
                    return ("Success", "Status retrieved successfully", "Delivered");
                }

                return ("Fail", $"Failed to parse response: {responseContent}", "Unknown");
            }
        }

        private string NormalizeKenyanPhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                throw new ArgumentException("Phone number cannot be empty.");

            phoneNumber = phoneNumber.Trim();

            if (phoneNumber.StartsWith("+254"))
                return phoneNumber.Substring(1); // +254712345678 -> 254712345678

            if (phoneNumber.StartsWith("0"))
                return $"254{phoneNumber.Substring(1)}"; // 0712345678 -> 254712345678

            if (phoneNumber.StartsWith("254"))
                return phoneNumber;

            return phoneNumber; // Return as is if it doesn't match standard Kenyan formats, or throw
        }

        // DTOs

        private class ExpressSmsSuccessResponse
        {
            [JsonPropertyName("success")]
            public string? Success { get; set; }

            [JsonPropertyName("message")]
            public string? Message { get; set; }

            [JsonPropertyName("balance")]
            public int Balance { get; set; }

            [JsonPropertyName("delivery_code")]
            public JsonElement DeliveryCodeRaw { get; set; }

            [JsonIgnore]
            public string? DeliveryCode =>
                DeliveryCodeRaw.ValueKind == JsonValueKind.String
                    ? DeliveryCodeRaw.GetString()
                    : DeliveryCodeRaw.ValueKind == JsonValueKind.Number
                        ? DeliveryCodeRaw.GetInt64().ToString()
                        : null;
        }

        private class ExpressSmsErrorResponse
        {
            [JsonPropertyName("error")]
            public string? Error { get; set; }

            [JsonPropertyName("error_message")]
            public string? ErrorMessage { get; set; }
        }

        private class ExpressSmsStatusResponse
        {
            [JsonPropertyName("message_status")]
            public string? MessageStatus { get; set; }

            [JsonPropertyName("delivery_status")]
            public string? DeliveryStatus { get; set; }
        }

    }
}
