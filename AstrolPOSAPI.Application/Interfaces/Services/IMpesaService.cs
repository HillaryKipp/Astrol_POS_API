using AstrolPOSAPI.Application.Models.Mpesa;

namespace AstrolPOSAPI.Application.Interfaces.Services
{
    /// <summary>
    /// Interface for M-Pesa Daraja API operations
    /// </summary>
    public interface IMpesaService
    {
        /// <summary>
        /// Initiate STK Push to customer's phone
        /// </summary>
        /// <param name="phoneNumber">Customer phone number (254XXXXXXXXX format)</param>
        /// <param name="amount">Amount to charge</param>
        /// <param name="accountReference">Reference for the transaction (e.g., order number)</param>
        /// <param name="transactionDesc">Transaction description</param>
        /// <returns>STK Push result</returns>
        Task<MpesaResult> InitiateStkPushAsync(
            string phoneNumber,
            decimal amount,
            string accountReference,
            string transactionDesc);

        /// <summary>
        /// Query the status of an STK Push transaction
        /// </summary>
        /// <param name="checkoutRequestId">The CheckoutRequestID from STK Push response</param>
        /// <returns>Query result</returns>
        Task<MpesaResult> QueryStkStatusAsync(string checkoutRequestId);

        /// <summary>
        /// Process the callback from M-Pesa after payment
        /// </summary>
        /// <param name="callback">Callback payload from M-Pesa</param>
        /// <returns>Processed result</returns>
        Task<MpesaResult> ProcessCallbackAsync(StkPushCallback callback);

        /// <summary>
        /// Register C2B URLs with Safaricom
        /// </summary>
        Task<MpesaResult> RegisterC2BUrlsAsync(string shortCode, string confirmationUrl, string validationUrl);

        /// <summary>
        /// Validate C2B payment (optional callback)
        /// </summary>
        Task<C2BValidationResponse> ValidateC2BPaymentAsync(C2BValidationRequest request);

        /// <summary>
        /// Process C2B payment confirmation
        /// </summary>
        Task<MpesaResult> ProcessC2BConfirmationAsync(C2BConfirmationRequest request);

        /// <summary>
        /// Format phone number to M-Pesa format (254XXXXXXXXX)
        /// </summary>
        /// <param name="phoneNumber">Phone number in any format</param>
        /// <returns>Formatted phone number</returns>
        string FormatPhoneNumber(string phoneNumber);
    }
}
