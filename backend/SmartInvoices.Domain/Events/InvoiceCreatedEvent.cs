using System;
using SmartInvoices.Domain.Entities;

namespace SmartInvoices.Domain.Events
{
    /// <summary>
    /// Zdarzenie domenowe wywoływane po utworzeniu faktury
    /// </summary>
    public class InvoiceCreatedEvent
    {
        /// <summary>
        /// Faktura, która została utworzona
        /// </summary>
        public Invoice Invoice { get; }
        
        /// <summary>
        /// Data utworzenia zdarzenia
        /// </summary>
        public DateTime OccurredOn { get; }
        
        public InvoiceCreatedEvent(Invoice invoice)
        {
            Invoice = invoice;
            OccurredOn = DateTime.Now;
        }
    }
}
