using SmartInvoices.Application.Interfaces;

namespace SmartInvoices.Infrastructure.Services;

internal class EmailService : IEmailService
{
    public async Task SendEmailAsync(string to, string subject, string body, bool isHtml = false)
    {
        await Task.CompletedTask; // Simulate async operation
    }

    public async Task SendEmailWithAttachmentsAsync(string to, string subject, string body, List<(string FilePath, string FileName)> attachments, bool isHtml = false)
    {
        await Task.CompletedTask;
    }
}
