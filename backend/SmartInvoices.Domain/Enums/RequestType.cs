namespace SmartInvoices.Domain.Enums
{
    /// <summary>
    /// Reprezentuje możliwe typy wniosków w systemie
    /// </summary>
    public enum RequestType
    {
        /// <summary>
        /// Wniosek o zmianę pozycji na fakturze
        /// </summary>
        Change = 0,
        
        /// <summary>
        /// Wniosek o zwrot całej faktury
        /// </summary>
        Refund = 1
    }
}
