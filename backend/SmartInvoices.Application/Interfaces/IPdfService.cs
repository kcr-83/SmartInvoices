namespace SmartInvoices.Application.Interfaces;

/// <summary>
/// Interfejs dla usługi generowania plików PDF.
/// </summary>
public interface IPdfService
{
    /// <summary>
    /// Generuje plik PDF z faktury.
    /// </summary>
    /// <param name="invoiceId">Identyfikator faktury</param>
    /// <returns>Zadanie reprezentujące operację asynchroniczną, zwracające ścieżkę do wygenerowanego pliku PDF</returns>
    Task<string> GenerateInvoicePdfAsync(int invoiceId);
    
    /// <summary>
    /// Generuje plik PDF z raportu.
    /// </summary>
    /// <param name="reportData">Dane do raportu</param>
    /// <param name="reportTemplate">Szablon raportu</param>
    /// <returns>Zadanie reprezentujące operację asynchroniczną, zwracające ścieżkę do wygenerowanego pliku PDF</returns>
    Task<string> GenerateReportPdfAsync(object reportData, string reportTemplate);
}
