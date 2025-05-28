namespace SmartInvoices.Domain.Enums
{
    /// <summary>
    /// Reprezentuje możliwe statusy faktury w systemie
    /// </summary>
    public enum InvoiceStatus
    {
        /// <summary>
        /// Faktura utworzona, ale jeszcze niezatwierdzona
        /// </summary>
        Draft = 0,
        
        /// <summary>
        /// Faktura zatwierdzona, oczekująca na płatność
        /// </summary>
        Issued = 1,
        
        /// <summary>
        /// Faktura opłacona
        /// </summary>
        Paid = 2,
        
        /// <summary>
        /// Faktura przeterminowana (nieopłacona po terminie)
        /// </summary>
        Overdue = 3,
        
        /// <summary>
        /// Faktura anulowana
        /// </summary>
        Cancelled = 4,
        
        /// <summary>
        /// Faktura częściowo opłacona
        /// </summary>
        PartiallyPaid = 5,
        
        /// <summary>
        /// Faktura w trakcie przetwarzania zwrotu
        /// </summary>
        RefundInProgress = 6,
        
        /// <summary>
        /// Faktura zwrócona
        /// </summary>
        Refunded = 7,
        
        /// <summary>
        /// Faktura w trakcie przetwarzania zmian
        /// </summary>
        ChangeInProgress = 8
    }
}
