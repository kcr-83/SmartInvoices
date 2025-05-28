namespace SmartInvoices.Application.Interfaces;

/// <summary>
/// Interfejs dla usługi wysyłania emaili.
/// </summary>
public interface IEmailService
{
    /// <summary>
    /// Wysyła wiadomość email.
    /// </summary>
    /// <param name="to">Adresat wiadomości</param>
    /// <param name="subject">Temat wiadomości</param>
    /// <param name="body">Treść wiadomości</param>
    /// <param name="isHtml">Czy wiadomość zawiera HTML</param>
    /// <returns>Zadanie reprezentujące operację asynchroniczną</returns>
    Task SendEmailAsync(string to, string subject, string body, bool isHtml = false);
    
    /// <summary>
    /// Wysyła wiadomość email z załącznikami.
    /// </summary>
    /// <param name="to">Adresat wiadomości</param>
    /// <param name="subject">Temat wiadomości</param>
    /// <param name="body">Treść wiadomości</param>
    /// <param name="attachments">Lista załączników (ścieżka pliku, nazwa pliku)</param>
    /// <param name="isHtml">Czy wiadomość zawiera HTML</param>
    /// <returns>Zadanie reprezentujące operację asynchroniczną</returns>
    Task SendEmailWithAttachmentsAsync(string to, string subject, string body, List<(string FilePath, string FileName)> attachments, bool isHtml = false);
}
