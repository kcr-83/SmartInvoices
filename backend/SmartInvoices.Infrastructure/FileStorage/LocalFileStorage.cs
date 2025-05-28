using Microsoft.Extensions.Logging;
using SmartInvoices.Application.Interfaces;

namespace SmartInvoices.Infrastructure.FileStorage;

public class LocalFileStorage : IFileStorage
{
    private readonly ILogger<LocalFileStorage> _logger;
    private readonly string _basePath;

    public LocalFileStorage(ILogger<LocalFileStorage> logger)
    {
        _logger = logger;
        _basePath = Path.Combine(Directory.GetCurrentDirectory(), "FileStorage");
        
        if (!Directory.Exists(_basePath))
        {
            Directory.CreateDirectory(_basePath);
        }
    }

    public async Task<string> SaveFileAsync(Stream fileStream, string fileName, string contentType, CancellationToken cancellationToken = default)
    {
        try
        {
            var uniqueFileName = $"{Guid.NewGuid()}_{fileName}";
            var filePath = Path.Combine(_basePath, uniqueFileName);
            
            using (var fileStreamWriter = new FileStream(filePath, FileMode.Create))
            {
                await fileStream.CopyToAsync(fileStreamWriter, cancellationToken);
            }
            
            _logger.LogInformation("Plik {FileName} zapisany w {FilePath}", fileName, filePath);
            return uniqueFileName;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Błąd podczas zapisywania pliku {FileName}", fileName);
            throw;
        }
    }

    public async Task<byte[]> GetFileAsync(string fileId, CancellationToken cancellationToken = default)
    {
        try
        {
            var filePath = Path.Combine(_basePath, fileId);
            if (!File.Exists(filePath))
            {
                _logger.LogWarning("Plik {FileId} nie istnieje", fileId);
                return Array.Empty<byte>();
            }
            
            return await File.ReadAllBytesAsync(filePath, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Błąd podczas odczytu pliku {FileId}", fileId);
            throw;
        }
    }

    public Task DeleteFileAsync(string fileId, CancellationToken cancellationToken = default)
    {
        try
        {
            var filePath = Path.Combine(_basePath, fileId);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                _logger.LogInformation("Usunięto plik {FileId}", fileId);
            }
            else
            {
                _logger.LogWarning("Próba usunięcia nieistniejącego pliku {FileId}", fileId);
            }
            
            return Task.CompletedTask;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Błąd podczas usuwania pliku {FileId}", fileId);
            throw;
        }
    }

    public Task<string> SaveFileAsync(string fileName, string contentType, byte[] content)
    {
        throw new NotImplementedException();
    }

    public Task<(byte[] Content, string ContentType)> GetFileAsync(string filePath)
    {
        throw new NotImplementedException();
    }

    public Task DeleteFileAsync(string filePath)
    {
        throw new NotImplementedException();
    }
}
