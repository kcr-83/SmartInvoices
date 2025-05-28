namespace SmartInvoices.WebApi.Endpoints.Invoices.GetInvoicePdf;

/// <summary>
/// Model żądania pobrania faktury w formacie PDF.
/// </summary>
public class GetInvoicePdfRequest
{
    /// <summary>
    /// Identyfikator faktury.
    /// </summary>
    public int InvoiceId { get; set; }
}
