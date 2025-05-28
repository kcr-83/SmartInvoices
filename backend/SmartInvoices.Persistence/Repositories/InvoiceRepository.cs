using Microsoft.EntityFrameworkCore;
using SmartInvoices.Application.DTOs.Invoices;
using SmartInvoices.Domain.Entities;
using SmartInvoices.Domain.Enums;
using SmartInvoices.Domain.Interfaces;

namespace SmartInvoices.Persistence.Repositories;

public class InvoiceRepository
    : Repository<Invoice>,
        SmartInvoices.Application.Interfaces.Repositories.IInvoiceRepository
{
    public InvoiceRepository(IDbContext context)
        : base(context) { }


    public async Task UpdateAsync(Invoice invoice)
    {
        ((DbContext)_context).Entry(invoice).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var invoice = await _context.Invoices.FindAsync(id);
        if (invoice != null)
        {
            _context.Invoices.Remove(invoice);
            await _context.SaveChangesAsync();
        }
    }

    public Task<IEnumerable<Invoice>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Invoice>> GetByUserIdAsync(
        int userId,
        CancellationToken cancellationToken = default
    )
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Invoice>> GetFilteredAsync(
        int? userId = null,
        DateTime? fromDate = null,
        DateTime? toDate = null,
        InvoiceStatus? status = null,
        CancellationToken cancellationToken = default
    )
    {
        throw new NotImplementedException();
    }

    public Task<List<Invoice>> GetInvoicesAsync(
        string? searchString,
        DateTime? startDate,
        DateTime? endDate,
        string? status,
        decimal? minAmount,
        decimal? maxAmount,
        string? sortBy,
        string? sortDirection,
        int? pageNumber,
        int? pageSize,
        CancellationToken cancellationToken = default
    )
    {
        throw new NotImplementedException();
    }

    public Task<Invoice> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<Invoice> GetByInvoiceNumberAsync(
        string invoiceNumber,
        CancellationToken cancellationToken = default
    )
    {
        throw new NotImplementedException();
    }

    public Task AddAsync(Invoice invoice, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(Invoice invoice, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(Invoice invoice, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<LineItem> GetLineItemByIdAsync(
        int id,
        CancellationToken cancellationToken = default
    )
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<LineItem>> GetLineItemsByInvoiceIdAsync(
        int invoiceId,
        CancellationToken cancellationToken = default
    )
    {
        throw new NotImplementedException();
    }

    public Task AddLineItemAsync(LineItem lineItem, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task UpdateLineItemAsync(
        LineItem lineItem,
        CancellationToken cancellationToken = default
    )
    {
        throw new NotImplementedException();
    }

    public Task DeleteLineItemAsync(
        LineItem lineItem,
        CancellationToken cancellationToken = default
    )
    {
        throw new NotImplementedException();
    }

    public Task<InvoiceListDto> GetInvoicesAsync(
        int userId,
        int page = 1,
        int pageSize = 10,
        string? status = null,
        DateTime? startDate = null,
        DateTime? endDate = null,
        string? sortBy = null,
        string? sortOrder = null,
        CancellationToken cancellationToken = default
    )
    {
        throw new NotImplementedException();
    }

    public Task<InvoiceDetailDto?> GetInvoiceByIdAsync(
        int invoiceId,
        int userId,
        CancellationToken cancellationToken = default
    )
    {
        throw new NotImplementedException();
    }

    public Task<byte[]?> GeneratePdfAsync(
        int invoiceId,
        int userId,
        CancellationToken cancellationToken = default
    )
    {
        throw new NotImplementedException();
    }
}
