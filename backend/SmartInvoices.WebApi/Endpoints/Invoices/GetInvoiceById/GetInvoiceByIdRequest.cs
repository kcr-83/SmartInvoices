namespace SmartInvoices.WebApi.Endpoints.Invoices.GetInvoiceById;

/// <summary>
/// Model żądania pobrania szczegółów faktury.
/// </summary>
public class GetInvoiceByIdRequest
{
    /// <summary>
    /// Identyfikator faktury.
    /// </summary>
    public int InvoiceId { get; set; }
}
