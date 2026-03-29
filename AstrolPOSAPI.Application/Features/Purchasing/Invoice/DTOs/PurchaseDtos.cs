namespace AstrolPOSAPI.Application.Features.Purchasing.Invoice.DTOs
{
    public class PurchaseHeaderDto
    {
        public string Id { get; set; } = string.Empty;
        public string No { get; set; } = string.Empty;
        public string BuyFromVendorNo { get; set; } = string.Empty;
        public string BuyFromVendorName { get; set; } = string.Empty;

        public DateTime PostingDate { get; set; }
        public DateTime DocumentDate { get; set; }
        public DateTime DueDate { get; set; }

        public string? VendorInvoiceNo { get; set; }
        public string? AttachmentUrl { get; set; }
        public string Status { get; set; } = string.Empty;

        public List<PurchaseLineDto> Lines { get; set; } = new();
    }

    public class PurchaseLineDto
    {
        public string Id { get; set; } = string.Empty;
        public string ItemNo { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Quantity { get; set; }
        public decimal DirectUnitCost { get; set; }
        public decimal LineAmount { get; set; }
    }

    public class CreatePurchaseHeaderDto
    {
        public string BuyFromVendorNo { get; set; } = string.Empty;
        public DateTime PostingDate { get; set; }
        public DateTime DocumentDate { get; set; }
        public string? VendorInvoiceNo { get; set; }
        public string? AttachmentUrl { get; set; }
        public string CompanyId { get; set; } = string.Empty;

        public List<CreatePurchaseLineDto> Lines { get; set; } = new();
    }

    public class CreatePurchaseLineDto
    {
        public string ItemNo { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Quantity { get; set; }
        public decimal DirectUnitCost { get; set; }
    }
}
