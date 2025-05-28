using System;

namespace SmartInvoices.Domain.Entities
{
    /// <summary>
    /// Reprezentuje załącznik dokumentu dla wniosku o zwrot
    /// </summary>
    public class DocumentAttachment
    {
        /// <summary>
        /// Unikalny identyfikator załącznika
        /// </summary>
        public int AttachmentId { get; set; }

        /// <summary>
        /// Identyfikator wniosku o zwrot, do którego należy załącznik
        /// </summary>
        public int RefRefundRequestId { get; set; }

        /// <summary>
        /// Nazwa pliku
        /// </summary>
        public string FileName { get; set; } = string.Empty;

        /// <summary>
        /// Rozmiar pliku w bajtach
        /// </summary>
        public int FileSize { get; set; }

        /// <summary>
        /// Typ pliku
        /// </summary>
        public string FileType { get; set; } = string.Empty;

        /// <summary>
        /// Ścieżka do pliku w systemie
        /// </summary>
        public string FilePath { get; set; } = string.Empty;

        /// <summary>
        /// Data przesłania pliku
        /// </summary>
        public DateTime UploadDate { get; set; }

        /// <summary>
        /// Powiązany wniosek o zwrot kosztów
        /// </summary>
        public RefundRequest? RefundRequest { get; set; } = null!;

        // Dodaj konstruktory i metody fabryczne z poprawnymi inicjalizacjami,
        // ponieważ w przekazanym fragmencie kodu ich nie widzę
    }
}
