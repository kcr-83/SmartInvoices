namespace SmartInvoices.Application.Interfaces;

/// <summary>
/// Interfejs dla usługi przechowywania plików.
/// </summary>
public interface IFileStorage
{
    /// <summary>
    /// Zapisuje plik.
    /// </summary>
    /// <param name="fileName">Nazwa pliku</param>
    /// <param name="contentType">Typ MIME pliku</param>
    /// <param name="content">Zawartość pliku</param>
    /// <returns>Zadanie reprezentujące operację asynchroniczną, zwracające unikalną ścieżkę do zapisanego pliku</returns>
    Task<string> SaveFileAsync(string fileName, string contentType, byte[] content);
    
    /// <summary>
    /// Pobiera plik.
    /// </summary>
    /// <param name="filePath">Ścieżka do pliku</param>
    /// <returns>Zadanie reprezentujące operację asynchroniczną, zwracające zawartość pliku</returns>
    Task<(byte[] Content, string ContentType)> GetFileAsync(string filePath);
    
    /// <summary>
    /// Usuwa plik.
    /// </summary>
    /// <param name="filePath">Ścieżka do pliku</param>
    /// <returns>Zadanie reprezentujące operację asynchroniczną</returns>
    Task DeleteFileAsync(string filePath);
}
