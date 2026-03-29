namespace AstrolPOSAPI.Shared.Configuration
{
    /// <summary>
    /// M-Pesa Daraja API configuration settings
    /// </summary>
    public class MpesaSettings
    {
        public const string SectionName = "Mpesa";

        /// <summary>
        /// Consumer Key from Safaricom Developer Portal
        /// </summary>
        public string ConsumerKey { get; set; } = string.Empty;

        /// <summary>
        /// Consumer Secret from Safaricom Developer Portal
        /// </summary>
        public string ConsumerSecret { get; set; } = string.Empty;

        /// <summary>
        /// Business Short Code (Paybill or Till Number)
        /// </summary>
        public string Shortcode { get; set; } = string.Empty;

        /// <summary>
        /// Lipa Na M-Pesa Online Passkey (provided by Safaricom)
        /// </summary>
        public string Passkey { get; set; } = string.Empty;

        /// <summary>
        /// Callback URL for payment confirmation
        /// </summary>
        public string CallbackUrl { get; set; } = string.Empty;

        /// <summary>
        /// Environment: "sandbox" or "production"
        /// </summary>
        public string Environment { get; set; } = "sandbox";

        /// <summary>
        /// Get the base URL based on environment
        /// </summary>
        public string BaseUrl => Environment.ToLower() == "production"
            ? "https://api.safaricom.co.ke"
            : "https://sandbox.safaricom.co.ke";

        /// <summary>
        /// Transaction type for STK Push
        /// CustomerPayBillOnline or CustomerBuyGoodsOnline
        /// </summary>
        public string TransactionType { get; set; } = "CustomerPayBillOnline";

        /// <summary>
        /// Account reference prefix (e.g., company name)
        /// </summary>
        public string AccountReferencePrefix { get; set; } = "SALE";
    }
}
