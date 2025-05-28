using System;
using System.Collections.Generic;
using SmartInvoices.Domain.Enums;

namespace SmartInvoices.Domain.Entities
{
    /// <summary>
    /// Reprezentuje wniosek o zmianę pozycji faktury
    /// </summary>
    public class ChangeRequest
    {
        /// <summary>
        /// Unikalny identyfikator wniosku o zmianę
        /// </summary>
        public int ChangeRequestId { get; set; }

        /// <summary>
        /// Identyfikator powiązanej faktury.
        /// </summary>
        public int InvoiceId { get; set; }

        /// <summary>
        /// Identyfikator użytkownika składającego wniosek
        /// </summary>
        public int RefUserId { get; set; }

        /// <summary>
        /// Identyfikator pozycji faktury, których dotyczy wniosek
        /// </summary>
        public List<int> LineItemIds { get; set; } = [];

        /// <summary>
        /// Data złożenia wniosku
        /// </summary>
        public DateTime RequestDate { get; set; }

        /// <summary>
        /// Żądana ilość w pozycji faktury
        /// </summary>
        public int? RequestedQuantity { get; set; }

        /// <summary>
        /// Żądana cena jednostkowa w pozycji faktury
        /// </summary>
        public decimal? RequestedUnitPrice { get; set; }

        /// <summary>
        /// Żądany opis pozycji faktury
        /// </summary>
        public string RequestedDescription { get; set; } = string.Empty;

        /// <summary>
        /// Powód zmiany
        /// </summary>
        public string Reason { get; set; } = string.Empty;

        /// <summary>
        /// Status wniosku o zmianę
        /// </summary>
        public RequestStatus Status { get; set; }

        /// <summary>
        /// Notatki administratora dotyczące tej zmiany
        /// </summary>
        public string? AdminNotes { get; set; }

        /// <summary>
        /// Identyfikator użytkownika (administratora) rozpatrującego wniosek
        /// </summary>
        public int? ReviewedBy { get; set; }

        /// <summary>
        /// Data rozpatrzenia wniosku
        /// </summary>
        public DateTime? ReviewDate { get; set; }

        /// <summary>
        /// Użytkownik, który złożył wniosek
        /// </summary>
        public User User { get; private set; } = null!;

        /// <summary>
        /// Pozycje faktury, których dotyczy wniosek
        /// </summary>
        public ICollection<LineItem> LineItems { get; private set; } = new List<LineItem>();

        /// <summary>
        /// Faktura, której dotyczy wniosek
        /// </summary>
        public Invoice Invoice { get; private set; } = null!;

        /// <summary>
        /// Zatwierdza wniosek o zmianę
        /// </summary>
        /// <param name="reviewedBy">Identyfikator użytkownika (administratora) zatwierdzającego wniosek</param>
        /// <param name="notes">Opcjonalne notatki</param>
        public void Approve(int reviewedBy, string notes = null)
        {
            Status = RequestStatus.Approved;
            ReviewedBy = reviewedBy;
            ReviewDate = DateTime.Now;

            if (!string.IsNullOrEmpty(notes))
            {
                AdminNotes = notes;
            }
        }

        /// <summary>
        /// Odrzuca wniosek o zmianę
        /// </summary>
        /// <param name="reviewedBy">Identyfikator użytkownika (administratora) odrzucającego wniosek</param>
        /// <param name="notes">Opcjonalne notatki uzasadniające odrzucenie</param>
        public void Reject(int reviewedBy, string notes = null)
        {
            Status = RequestStatus.Rejected;
            ReviewedBy = reviewedBy;
            ReviewDate = DateTime.Now;

            if (!string.IsNullOrEmpty(notes))
            {
                AdminNotes = notes;
            }
        }
    }
}
