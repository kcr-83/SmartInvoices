using SmartInvoices.Domain.Enums;

namespace SmartInvoices.Domain.Entities
{
    /// <summary>
    /// Reprezentuje wniosek o zwrot całej faktury
    /// </summary>
    public class RefundRequest
    {
        /// <summary>
        /// Unikalny identyfikator wniosku o zwrot
        /// </summary>
        public int RefundRequestId { get; set; }

        /// <summary>
        /// Identyfikator użytkownika składającego wniosek
        /// </summary>
        public int RefUserId { get; set; }

        /// <summary>
        /// Identyfikator faktury, której dotyczy wniosek o zwrot
        /// </summary>
        public int InvoiceId { get; set; }

        /// <summary>
        /// Data złożenia wniosku o zwrot
        /// </summary>
        public DateTime RequestDate { get; set; }

        /// <summary>
        /// Powód złożenia wniosku o zwrot
        /// </summary>
        public string Reason { get; set; } = string.Empty;

        /// <summary>
        /// Status wniosku o zwrot
        /// </summary>
        public RefundRequestStatus Status { get; set; }

        /// <summary>
        /// Notatki administratora dotyczące wniosku
        /// </summary>
        public string AdminNotes { get; set; } = string.Empty;

        /// <summary>
        /// Identyfikator użytkownika (administratora) rozpatrującego wniosek
        /// </summary>
        public int? ReviewedBy { get; set; }

        /// <summary>
        /// Data rozpatrzenia wniosku
        /// </summary>
        public DateTime? ReviewDate { get; set; }

        /// <summary>
        /// Kwota zwrotu - może się różnić od oryginalnej kwoty faktury
        /// </summary>
        public decimal? RefundAmount { get; set; }

        /// <summary>
        /// Data realizacji zwrotu
        /// </summary>
        public DateTime? RefundDate { get; set; }

        /// <summary>
        /// Użytkownik składający wniosek o zwrot
        /// </summary>
        public User? User { get; set; }

        /// <summary>
        /// Faktura, której dotyczy wniosek o zwrot
        /// </summary>
        public Invoice? Invoice { get; set; }

        /// <summary>
        /// Kolekcja załączników dokumentów związanych z wnioskiem o zwrot
        /// </summary>
        public ICollection<DocumentAttachment> DocumentAttachments { get; set; } =
            new List<DocumentAttachment>();

        /// <summary>
        /// Zatwierdza wniosek o zwrot
        /// </summary>
        /// <param name="reviewedBy">Identyfikator użytkownika (administratora) zatwierdzającego wniosek</param>
        /// <param name="refundAmount">Kwota zwrotu</param>
        /// <param name="notes">Opcjonalne notatki</param>
        public void Approve(int reviewedBy, decimal refundAmount, string? notes = null)
        {
            Status = RefundRequestStatus.Approved;
            ReviewedBy = reviewedBy;
            ReviewDate = DateTime.Now;
            RefundAmount = refundAmount;

            if (!string.IsNullOrEmpty(notes))
            {
                AdminNotes = notes;
            }
        }

        /// <summary>
        /// Odrzuca wniosek o zwrot
        /// </summary>
        /// <param name="reviewedBy">Identyfikator użytkownika (administratora) odrzucającego wniosek</param>
        /// <param name="notes">Opcjonalne notatki uzasadniające odrzucenie</param>
        public void Reject(int reviewedBy, string? notes = null)
        {
            Status = RefundRequestStatus.Rejected;
            ReviewedBy = reviewedBy;
            ReviewDate = DateTime.Now;

            if (!string.IsNullOrEmpty(notes))
            {
                AdminNotes = notes;
            }
        }

        /// <summary>
        /// Przetwarza zwrot po zatwierdzeniu
        /// </summary>
        /// <param name="refundDate">Data realizacji zwrotu</param>
        public void ProcessRefund(DateTime refundDate)
        {
            if (Status != RefundRequestStatus.Approved)
            {
                throw new InvalidOperationException(
                    "Nie można przetworzyć zwrotu, który nie został zatwierdzony."
                );
            }

            Status = RefundRequestStatus.Refunded;
            RefundDate = refundDate;
        }
    }
}
