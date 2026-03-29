using System.Text.Json.Serialization;

namespace AstrolPOSAPI.Application.Models.Mpesa
{
    public class C2BRegisterUrlRequest
    {
        public string ShortCode { get; set; } = string.Empty;
        public string ResponseType { get; set; } = "Completed";
        public string ConfirmationURL { get; set; } = string.Empty;
        public string ValidationURL { get; set; } = string.Empty;
    }

    public class C2BRegisterUrlResponse
    {
        [JsonPropertyName("OriginatorConversationID")]
        public string OriginatorConversationID { get; set; } = string.Empty;

        [JsonPropertyName("ConversationID")]
        public string ConversationID { get; set; } = string.Empty;

        [JsonPropertyName("ResponseDescription")]
        public string ResponseDescription { get; set; } = string.Empty;
    }

    public class C2BValidationRequest
    {
        public string TransactionType { get; set; } = string.Empty;
        public string TransID { get; set; } = string.Empty;
        public string TransTime { get; set; } = string.Empty;
        public string TransAmount { get; set; } = string.Empty;
        public string BusinessShortCode { get; set; } = string.Empty;
        public string BillRefNumber { get; set; } = string.Empty;
        public string InvoiceNumber { get; set; } = string.Empty;
        public string OrgAccountBalance { get; set; } = string.Empty;
        public string ThirdPartyTransID { get; set; } = string.Empty;
        public string MSISDN { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string MiddleName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
    }

    public class C2BValidationResponse
    {
        public string ResultCode { get; set; } = "0";
        public string ResultDesc { get; set; } = "Accepted";
    }

    public class C2BConfirmationRequest : C2BValidationRequest
    {
        // Identical structure to Validation
    }
}
