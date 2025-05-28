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
        public int RefundRequestId { get; set; }

        /// <summary>
        /// Nazwa pliku
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Rozmiar pliku w bajtach
        /// </summary>
        public int FileSize { get; set; }

        /// <summary>
        /// Typ pliku (MIME type)
        /// </summary>
        public string FileType { get; set; }

        /// <summary>
        /// Ścieżka do pliku w systemie przechowywania (może być relatywna)
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// Data przesłania pliku
        /// </summary>
        public DateTime UploadDate { get; set; }

        /// <summary>
        /// Wniosek o zwrot, do którego należy załącznik
        /// </summary>
        public RefundRequest RefundRequest { get; set; }
    }
}
