namespace SmartInvoices.Domain.Enums
{
    /// <summary>
    /// Reprezentuje możliwe statusy wniosków w systemie
    /// </summary>
    public enum RequestStatus
    {
        /// <summary>
        /// Wniosek utworzony i oczekujący na przetworzenie
        /// </summary>
        Created = 0,
        
        /// <summary>
        /// Wniosek w trakcie rozpatrywania
        /// </summary>
        InProgress = 1,
        
        /// <summary>
        /// Wniosek zaakceptowany
        /// </summary>
        Approved = 2,
        
        /// <summary>
        /// Wniosek odrzucony
        /// </summary>
        Rejected = 3,
        
        /// <summary>
        /// Wniosek anulowany przez użytkownika
        /// </summary>
        Cancelled = 4,
        
        /// <summary>
        /// Wniosek wymaga dodatkowych informacji
        /// </summary>
        AdditionalInfoRequired = 5,
        
        /// <summary>
        /// Wniosek zrealizowany
        /// </summary>
        Completed = 6
    }
}
