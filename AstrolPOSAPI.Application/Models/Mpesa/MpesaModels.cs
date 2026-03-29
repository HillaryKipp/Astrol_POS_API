using System.Text.Json.Serialization;

namespace AstrolPOSAPI.Application.Models.Mpesa
{
    /// <summary>
    /// OAuth token response from M-Pesa API
    /// </summary>
    public class MpesaAuthResponse
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; } = string.Empty;

        [JsonPropertyName("expires_in")]
        public string ExpiresIn { get; set; } = string.Empty;
    }

    /// <summary>
    /// STK Push request to M-Pesa API
    /// </summary>
    public class StkPushRequest
    {
        [JsonPropertyName("BusinessShortCode")]
        public string BusinessShortCode { get; set; } = string.Empty;

        [JsonPropertyName("Password")]
        public string Password { get; set; } = string.Empty;

        [JsonPropertyName("Timestamp")]
        public string Timestamp { get; set; } = string.Empty;

        [JsonPropertyName("TransactionType")]
        public string TransactionType { get; set; } = string.Empty;

        [JsonPropertyName("Amount")]
        public int Amount { get; set; }

        [JsonPropertyName("PartyA")]
        public string PartyA { get; set; } = string.Empty;

        [JsonPropertyName("PartyB")]
        public string PartyB { get; set; } = string.Empty;

        [JsonPropertyName("PhoneNumber")]
        public string PhoneNumber { get; set; } = string.Empty;

        [JsonPropertyName("CallBackURL")]
        public string CallBackURL { get; set; } = string.Empty;

        [JsonPropertyName("AccountReference")]
        public string AccountReference { get; set; } = string.Empty;

        [JsonPropertyName("TransactionDesc")]
        public string TransactionDesc { get; set; } = string.Empty;
    }

    /// <summary>
    /// STK Push response from M-Pesa API
    /// </summary>
    public class StkPushResponse
    {
        [JsonPropertyName("MerchantRequestID")]
        public string MerchantRequestID { get; set; } = string.Empty;

        [JsonPropertyName("CheckoutRequestID")]
        public string CheckoutRequestID { get; set; } = string.Empty;

        [JsonPropertyName("ResponseCode")]
        public string ResponseCode { get; set; } = string.Empty;

        [JsonPropertyName("ResponseDescription")]
        public string ResponseDescription { get; set; } = string.Empty;

        [JsonPropertyName("CustomerMessage")]
        public string CustomerMessage { get; set; } = string.Empty;

        // Error fields
        [JsonPropertyName("requestId")]
        public string? RequestId { get; set; }

        [JsonPropertyName("errorCode")]
        public string? ErrorCode { get; set; }

        [JsonPropertyName("errorMessage")]
        public string? ErrorMessage { get; set; }

        public bool IsSuccess => ResponseCode == "0" && string.IsNullOrEmpty(ErrorCode);
    }

    /// <summary>
    /// STK Push callback from M-Pesa
    /// </summary>
    public class StkPushCallback
    {
        [JsonPropertyName("Body")]
        public StkCallbackBody? Body { get; set; }
    }

    public class StkCallbackBody
    {
        [JsonPropertyName("stkCallback")]
        public StkCallbackContent? StkCallback { get; set; }
    }

    public class StkCallbackContent
    {
        [JsonPropertyName("MerchantRequestID")]
        public string MerchantRequestID { get; set; } = string.Empty;

        [JsonPropertyName("CheckoutRequestID")]
        public string CheckoutRequestID { get; set; } = string.Empty;

        [JsonPropertyName("ResultCode")]
        public int ResultCode { get; set; }

        [JsonPropertyName("ResultDesc")]
        public string ResultDesc { get; set; } = string.Empty;

        [JsonPropertyName("CallbackMetadata")]
        public CallbackMetadata? CallbackMetadata { get; set; }

        public bool IsSuccess => ResultCode == 0;
    }

    public class CallbackMetadata
    {
        [JsonPropertyName("Item")]
        public List<CallbackItem>? Item { get; set; }
    }

    public class CallbackItem
    {
        [JsonPropertyName("Name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("Value")]
        public object? Value { get; set; }
    }

    /// <summary>
    /// STK Query request
    /// </summary>
    public class StkQueryRequest
    {
        [JsonPropertyName("BusinessShortCode")]
        public string BusinessShortCode { get; set; } = string.Empty;

        [JsonPropertyName("Password")]
        public string Password { get; set; } = string.Empty;

        [JsonPropertyName("Timestamp")]
        public string Timestamp { get; set; } = string.Empty;

        [JsonPropertyName("CheckoutRequestID")]
        public string CheckoutRequestID { get; set; } = string.Empty;
    }

    /// <summary>
    /// STK Query response
    /// </summary>
    public class StkQueryResponse
    {
        [JsonPropertyName("MerchantRequestID")]
        public string MerchantRequestID { get; set; } = string.Empty;

        [JsonPropertyName("CheckoutRequestID")]
        public string CheckoutRequestID { get; set; } = string.Empty;

        [JsonPropertyName("ResponseCode")]
        public string ResponseCode { get; set; } = string.Empty;

        [JsonPropertyName("ResponseDescription")]
        public string ResponseDescription { get; set; } = string.Empty;

        [JsonPropertyName("ResultCode")]
        public string ResultCode { get; set; } = string.Empty;

        [JsonPropertyName("ResultDesc")]
        public string ResultDesc { get; set; } = string.Empty;

        public bool IsSuccess => ResultCode == "0";
    }

    /// <summary>
    /// Internal result for STK Push operations
    /// </summary>
    public class MpesaResult
    {
        public bool Success { get; set; }
        public string? CheckoutRequestId { get; set; }
        public string? MerchantRequestId { get; set; }
        public string? Message { get; set; }
        public string? ErrorCode { get; set; }
        public string? MpesaReceiptNumber { get; set; }
        public decimal? Amount { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime? TransactionDate { get; set; }
    }
}
