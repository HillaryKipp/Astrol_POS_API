namespace AstrolPOSAPI.Application.Features.Purchasing.Invoice.DTOs
{
    public class PostedPurchInvHeaderDto
    {
        public string Id { get; set; } = string.Empty;
        public string No { get; set; } = string.Empty;
        public string BuyFromVendorNo { get; set; } = string.Empty;
        public string BuyFromVendorName { get; set; } = string.Empty;

        public DateTime PostingDate { get; set; }
        public DateTime DocumentDate { get; set; }
        public DateTime DueDate { get; set; }

        public string? VendorInvoiceNo { get; set; }
        public string? SourceNo { get; set; }
        public string? AttachmentUrl { get; set; }

        public decimal TotalAmount { get; set; }

        public List<PostedPurchInvLineDto> Lines { get; set; } = new();
    }

    public class PostedPurchInvLineDto
    {
        public string Id { get; set; } = string.Empty;
        public string DocumentNo { get; set; } = string.Empty;
        public string ItemNo { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Quantity { get; set; }
        public decimal DirectUnitCost { get; set; }
        public decimal LineAmount { get; set; }
    }
}
