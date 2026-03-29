using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using AstrolPOSAPI.Application.Interfaces.Services;
using AstrolPOSAPI.Application.Models.Mpesa;
using AstrolPOSAPI.Shared.Configuration;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AstrolPOSAPI.Infrastructure.Services
{
    /// <summary>
    /// M-Pesa Daraja API service implementation
    /// </summary>
    public class MpesaService : IMpesaService
    {
        private readonly HttpClient _httpClient;
        private readonly MpesaSettings _settings;
        private readonly IMemoryCache _cache;
        private readonly ILogger<MpesaService> _logger;
        private const string TokenCacheKey = "MpesaAccessToken";

        public MpesaService(
            HttpClient httpClient,
            IOptions<MpesaSettings> settings,
            IMemoryCache cache,
            ILogger<MpesaService> logger)
        {
            _httpClient = httpClient;
            _settings = settings.Value;
            _cache = cache;
            _logger = logger;

            _httpClient.BaseAddress = new Uri(_settings.BaseUrl);
        }

        /// <inheritdoc />
        public async Task<MpesaResult> InitiateStkPushAsync(
            string phoneNumber,
            decimal amount,
            string accountReference,
            string transactionDesc)
        {
            try
            {
                var token = await GetAccessTokenAsync();
                if (string.IsNullOrEmpty(token))
                {
                    return new MpesaResult
                    {
                        Success = false,
                        Message = "Failed to obtain M-Pesa access token"
                    };
                }

                var formattedPhone = FormatPhoneNumber(phoneNumber);
                var timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
                var password = GeneratePassword(timestamp);

                var request = new StkPushRequest
                {
                    BusinessShortCode = _settings.Shortcode,
                    Password = password,
                    Timestamp = timestamp,
                    TransactionType = _settings.TransactionType,
                    Amount = (int)Math.Ceiling(amount), // M-Pesa requires integer amounts
                    PartyA = formattedPhone,
                    PartyB = _settings.Shortcode,
                    PhoneNumber = formattedPhone,
                    CallBackURL = _settings.CallbackUrl,
                    AccountReference = $"{_settings.AccountReferencePrefix}-{accountReference}",
                    TransactionDesc = transactionDesc.Length > 13
                        ? transactionDesc[..13]
                        : transactionDesc
                };

                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);

                var json = JsonSerializer.Serialize(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                _logger.LogInformation("Initiating STK Push for {Phone}, Amount: {Amount}",
                    formattedPhone, amount);

                var response = await _httpClient.PostAsync(
                    "/mpesa/stkpush/v1/processrequest", content);

                var responseContent = await response.Content.ReadAsStringAsync();
                _logger.LogDebug("STK Push Response: {Response}", responseContent);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("M-Pesa STK Push API returned error: {Status}", response.StatusCode);
                    return new MpesaResult
                    {
                        Success = false,
                        Message = $"M-Pesa API returned {response.StatusCode}. Please try again later or use manual reference."
                    };
                }

                var stkResponse = JsonSerializer.Deserialize<StkPushResponse>(responseContent);

                if (stkResponse == null)
                {
                    return new MpesaResult
                    {
                        Success = false,
                        Message = "Invalid response from M-Pesa API"
                    };
                }

                if (stkResponse.IsSuccess)
                {
                    _logger.LogInformation(
                        "STK Push initiated successfully. CheckoutRequestID: {CheckoutId}",
                        stkResponse.CheckoutRequestID);

                    return new MpesaResult
                    {
                        Success = true,
                        CheckoutRequestId = stkResponse.CheckoutRequestID,
                        MerchantRequestId = stkResponse.MerchantRequestID,
                        Message = stkResponse.CustomerMessage
                    };
                }

                _logger.LogWarning("STK Push failed: {Error}",
                    stkResponse.ErrorMessage ?? stkResponse.ResponseDescription);

                return new MpesaResult
                {
                    Success = false,
                    ErrorCode = stkResponse.ErrorCode ?? stkResponse.ResponseCode,
                    Message = stkResponse.ErrorMessage ?? stkResponse.ResponseDescription
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error initiating STK Push");
                return new MpesaResult
                {
                    Success = false,
                    Message = $"Error: {ex.Message}"
                };
            }
        }

        /// <inheritdoc />
        public async Task<MpesaResult> QueryStkStatusAsync(string checkoutRequestId)
        {
            try
            {
                var token = await GetAccessTokenAsync();
                if (string.IsNullOrEmpty(token))
                {
                    return new MpesaResult
                    {
                        Success = false,
                        Message = "Failed to obtain M-Pesa access token"
                    };
                }

                var timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
                var password = GeneratePassword(timestamp);

                var request = new StkQueryRequest
                {
                    BusinessShortCode = _settings.Shortcode,
                    Password = password,
                    Timestamp = timestamp,
                    CheckoutRequestID = checkoutRequestId
                };

                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);

                var json = JsonSerializer.Serialize(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(
                    "/mpesa/stkpushquery/v1/query", content);

                var responseContent = await response.Content.ReadAsStringAsync();
                _logger.LogDebug("STK Query Response: {Response}", responseContent);

                if (!response.IsSuccessStatusCode)
                {
                    return new MpesaResult
                    {
                        Success = false,
                        Message = $"M-Pesa API error: {response.StatusCode}"
                    };
                }

                var queryResponse = JsonSerializer.Deserialize<StkQueryResponse>(responseContent);

                if (queryResponse == null)
                {
                    return new MpesaResult
                    {
                        Success = false,
                        Message = "Invalid response from M-Pesa API"
                    };
                }

                return new MpesaResult
                {
                    Success = queryResponse.IsSuccess,
                    CheckoutRequestId = queryResponse.CheckoutRequestID,
                    MerchantRequestId = queryResponse.MerchantRequestID,
                    Message = queryResponse.ResultDesc
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error querying STK status");
                return new MpesaResult
                {
                    Success = false,
                    Message = $"Error: {ex.Message}"
                };
            }
        }

        /// <inheritdoc />
        public Task<MpesaResult> ProcessCallbackAsync(StkPushCallback callback)
        {
            try
            {
                var stkCallback = callback.Body?.StkCallback;
                if (stkCallback == null)
                {
                    return Task.FromResult(new MpesaResult
                    {
                        Success = false,
                        Message = "Invalid callback payload"
                    });
                }

                _logger.LogInformation(
                    "Processing M-Pesa callback. CheckoutRequestID: {CheckoutId}, ResultCode: {ResultCode}",
                    stkCallback.CheckoutRequestID, stkCallback.ResultCode);

                if (!stkCallback.IsSuccess)
                {
                    return Task.FromResult(new MpesaResult
                    {
                        Success = false,
                        CheckoutRequestId = stkCallback.CheckoutRequestID,
                        MerchantRequestId = stkCallback.MerchantRequestID,
                        Message = stkCallback.ResultDesc
                    });
                }

                // Extract metadata from successful callback
                var result = new MpesaResult
                {
                    Success = true,
                    CheckoutRequestId = stkCallback.CheckoutRequestID,
                    MerchantRequestId = stkCallback.MerchantRequestID,
                    Message = stkCallback.ResultDesc
                };

                if (stkCallback.CallbackMetadata?.Item != null)
                {
                    foreach (var item in stkCallback.CallbackMetadata.Item)
                    {
                        switch (item.Name)
                        {
                            case "MpesaReceiptNumber":
                                result.MpesaReceiptNumber = item.Value?.ToString();
                                break;
                            case "Amount":
                                if (decimal.TryParse(item.Value?.ToString(), out var amount))
                                    result.Amount = amount;
                                break;
                            case "PhoneNumber":
                                result.PhoneNumber = item.Value?.ToString();
                                break;
                            case "TransactionDate":
                                if (DateTime.TryParseExact(
                                    item.Value?.ToString(),
                                    "yyyyMMddHHmmss",
                                    null,
                                    System.Globalization.DateTimeStyles.None,
                                    out var date))
                                    result.TransactionDate = date;
                                break;
                        }
                    }
                }

                _logger.LogInformation(
                    "M-Pesa payment successful. Receipt: {Receipt}, Amount: {Amount}",
                    result.MpesaReceiptNumber, result.Amount);

                return Task.FromResult(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing M-Pesa callback");
                return Task.FromResult(new MpesaResult
                {
                    Success = false,
                    Message = $"Error processing callback: {ex.Message}"
                });
            }
        }

        /// <inheritdoc />
        public string FormatPhoneNumber(string phoneNumber)
        {
            // Remove all non-digit characters
            var digits = new string(phoneNumber.Where(char.IsDigit).ToArray());

            // Handle different formats
            if (digits.StartsWith("254") && digits.Length == 12)
            {
                return digits; // Already in correct format
            }
            else if (digits.StartsWith("0") && digits.Length == 10)
            {
                return "254" + digits[1..]; // 0722XXXXXX -> 254722XXXXXX
            }
            else if (digits.StartsWith("7") && digits.Length == 9)
            {
                return "254" + digits; // 722XXXXXX -> 254722XXXXXX
            }
            else if (digits.StartsWith("+254"))
            {
                return digits[1..]; // Remove +
            }

            return digits; // Return as-is if not recognized
        }

        /// <summary>
        /// Get OAuth access token from M-Pesa (with caching)
        /// </summary>
        private async Task<string?> GetAccessTokenAsync()
        {
            if (_cache.TryGetValue(TokenCacheKey, out string? cachedToken))
            {
                return cachedToken;
            }

            try
            {
                var credentials = Convert.ToBase64String(
                    Encoding.UTF8.GetBytes($"{_settings.ConsumerKey}:{_settings.ConsumerSecret}"));

                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Basic", credentials);

                var response = await _httpClient.GetAsync(
                    "/oauth/v1/generate?grant_type=client_credentials");

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("Failed to get M-Pesa access token. Status: {Status}",
                        response.StatusCode);
                    return null;
                }

                var content = await response.Content.ReadAsStringAsync();
                var authResponse = JsonSerializer.Deserialize<MpesaAuthResponse>(content);

                if (authResponse == null || string.IsNullOrEmpty(authResponse.AccessToken))
                {
                    _logger.LogError("Invalid auth response from M-Pesa");
                    return null;
                }

                // Cache token for slightly less than expiry time
                var expiresIn = int.TryParse(authResponse.ExpiresIn, out var seconds)
                    ? seconds - 60
                    : 3540; // Default 59 minutes

                _cache.Set(TokenCacheKey, authResponse.AccessToken,
                    TimeSpan.FromSeconds(expiresIn));

                _logger.LogDebug("M-Pesa access token obtained and cached");
                return authResponse.AccessToken;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obtaining M-Pesa access token");
                return null;
            }
        }

        /// <summary>
        /// Generate password for STK Push (Base64 of Shortcode + Passkey + Timestamp)
        /// </summary>
        private string GeneratePassword(string timestamp)
        {
            var rawPassword = $"{_settings.Shortcode}{_settings.Passkey}{timestamp}";
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(rawPassword));
        }

        /// <inheritdoc />
        public async Task<MpesaResult> RegisterC2BUrlsAsync(string shortCode, string confirmationUrl, string validationUrl)
        {
            try
            {
                var token = await GetAccessTokenAsync();
                if (string.IsNullOrEmpty(token))
                {
                    return new MpesaResult { Success = false, Message = "Failed to obtain access token" };
                }

                var request = new C2BRegisterUrlRequest
                {
                    ShortCode = shortCode,
                    ResponseType = "Completed",
                    ConfirmationURL = confirmationUrl,
                    ValidationURL = validationUrl
                };

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var json = JsonSerializer.Serialize(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync("/mpesa/c2b/v1/registerurl", content);
                var responseContent = await response.Content.ReadAsStringAsync();
                _logger.LogInformation("C2B Register URL Response: {Response}", responseContent);

                if (!response.IsSuccessStatusCode)
                {
                    return new MpesaResult { Success = false, Message = $"M-Pesa API error: {response.StatusCode}. {responseContent}" };
                }

                var c2bResponse = JsonSerializer.Deserialize<C2BRegisterUrlResponse>(responseContent);

                if (c2bResponse != null && !string.IsNullOrEmpty(c2bResponse.OriginatorConversationID))
                {
                    return new MpesaResult
                    {
                        Success = true,
                        Message = c2bResponse.ResponseDescription,
                        MerchantRequestId = c2bResponse.OriginatorConversationID
                    };
                }

                return new MpesaResult { Success = false, Message = responseContent };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error registering C2B URLs");
                return new MpesaResult { Success = false, Message = ex.Message };
            }
        }

        /// <inheritdoc />
        public Task<C2BValidationResponse> ValidateC2BPaymentAsync(C2BValidationRequest request)
        {
            // Perform any validation logic here (e.g., check if BillRefNumber exists)
            // For now, accept all payments
            _logger.LogInformation("Validating C2B Payment: {TransID}, Amount: {Amount}, BillRef: {BillRef}",
                request.TransID, request.TransAmount, request.BillRefNumber);

            return Task.FromResult(new C2BValidationResponse { ResultCode = "0", ResultDesc = "Accepted" });
        }

        /// <inheritdoc />
        public Task<MpesaResult> ProcessC2BConfirmationAsync(C2BConfirmationRequest request)
        {
            try
            {
                _logger.LogInformation("Processing C2B Confirmation: {TransID}, Amount: {Amount}, BillRef: {BillRef}",
                    request.TransID, request.TransAmount, request.BillRefNumber);

                if (decimal.TryParse(request.TransAmount, out var amount))
                {
                    var result = new MpesaResult
                    {
                        Success = true,
                        MpesaReceiptNumber = request.TransID,
                        Amount = amount,
                        PhoneNumber = request.MSISDN,
                        CheckoutRequestId = request.BillRefNumber // Use BillRef as the linker
                    };

                    // Try parsing transaction date if available
                    if (DateTime.TryParseExact(request.TransTime, "yyyyMMddHHmmss", null,
                        System.Globalization.DateTimeStyles.None, out var date))
                    {
                        result.TransactionDate = date;
                    }

                    return Task.FromResult(result);
                }

                return Task.FromResult(new MpesaResult { Success = false, Message = "Invalid amount format" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing C2B confirmation");
                return Task.FromResult(new MpesaResult { Success = false, Message = ex.Message });
            }
        }
    }
}
