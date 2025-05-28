using System;
using System.Collections.Generic;

namespace SmartInvoices.Domain.Entities
{
    /// <summary>
    /// Reprezentuje pozycję na fakturze
    /// </summary>
    public class LineItem
    {
        /// <summary>
        /// Unikalny identyfikator pozycji
        /// </summary>
        public int LineItemId { get; set; }
        
        /// <summary>
        /// Identyfikator faktury, do której należy pozycja
        /// </summary>
        public int InvoiceId { get; set; }
        
        /// <summary>
        /// Opis pozycji faktury
        /// </summary>
        public string Description { get;  set; } = string.Empty;
        
        /// <summary>
        /// Ilość jednostek
        /// </summary>
        public int Quantity { get; set; }
        
        /// <summary>
        /// Cena jednostkowa
        /// </summary>
        public decimal UnitPrice { get; set; }
        
        /// <summary>
        /// Stawka podatku w procentach
        /// </summary>
        public decimal TaxRate { get; set; }
        
        /// <summary>
        /// Całkowita wartość pozycji (ilość * cena jednostkowa)
        /// </summary>
        public decimal TotalPrice { get; set; }
        
        /// <summary>
        /// Data utworzenia pozycji
        /// </summary>
        public DateTime CreatedDate { get; set; }
        
        /// <summary>
        /// Data ostatniej modyfikacji pozycji
        /// </summary>
        public DateTime LastModifiedDate { get; set; }
        
        /// <summary>
        /// Faktura, do której należy pozycja
        /// </summary>
        public Invoice? Invoice { get;  set; } = null!;
        
        /// <summary>
        /// Kolekcja wniosków o zmianę dotyczących tej pozycji faktury
        /// </summary>
        public ICollection<ChangeRequest> ChangeRequests { get; set; } = new List<ChangeRequest>();
        
        /// <summary>
        /// Oblicza całkowitą wartość pozycji
        /// </summary>
        public void CalculateTotalPrice()
        {
            TotalPrice = Quantity * UnitPrice;
        }
    }
}
