using Microsoft.EntityFrameworkCore;
using SmartInvoices.Application.DTOs.Invoices;
using SmartInvoices.Application.Mappings;
using SmartInvoices.Domain.Entities;
using SmartInvoices.Domain.Enums;
using SmartInvoices.Domain.Interfaces;

namespace SmartInvoices.Persistence.Repositories;

public class InvoiceRepository
    : Repository<Invoice>,
        SmartInvoices.Application.Interfaces.Repositories.IInvoiceRepository
{    public InvoiceRepository(IDbContext context)
        : base(context) { }

    #region Basic CRUD Operations

    public async Task<IEnumerable<Invoice>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Invoices
            .Include(i => i.LineItems)
            .Include(i => i.User)
            .ToListAsync(cancellationToken);
    }

    public async Task<Invoice?> GetByIdAsync(int invoiceId, CancellationToken cancellationToken = default)
    {
        return await _context.Invoices
            .Include(i => i.LineItems)
            .Include(i => i.User)
            .FirstOrDefaultAsync(i => i.InvoiceId == invoiceId, cancellationToken);
    }

    public async Task<Invoice> GetByInvoiceNumberAsync(
        string invoiceNumber,
        CancellationToken cancellationToken = default
    )
    {
        var invoice = await _context.Invoices
            .Include(i => i.LineItems)
            .Include(i => i.User)
            .FirstOrDefaultAsync(i => i.InvoiceNumber == invoiceNumber, cancellationToken);
        
        return invoice ?? throw new InvalidOperationException($"Faktura o numerze {invoiceNumber} nie została znaleziona.");
    }

    public async Task AddAsync(Invoice invoice, CancellationToken cancellationToken = default)
    {
        if (invoice == null)
            throw new ArgumentNullException(nameof(invoice));

        invoice.CreatedDate = DateTime.UtcNow;
        invoice.LastModifiedDate = DateTime.UtcNow;
        
        await _context.Invoices.AddAsync(invoice, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(Invoice invoice, CancellationToken cancellationToken = default)
    {
        if (invoice == null)
            throw new ArgumentNullException(nameof(invoice));

        invoice.LastModifiedDate = DateTime.UtcNow;
        
        _context.Invoices.Update(invoice);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Invoice invoice, CancellationToken cancellationToken = default)
    {
        if (invoice == null)
            throw new ArgumentNullException(nameof(invoice));

        _context.Invoices.Remove(invoice);
        await _context.SaveChangesAsync(cancellationToken);
    }

    #endregion

    #region Query Operations

    public async Task<IEnumerable<Invoice>> GetByUserIdAsync(
        int userId,
        CancellationToken cancellationToken = default
    )
    {
        return await _context.Invoices
            .Include(i => i.LineItems)
            .Where(i => i.RefUserId == userId)
            .OrderByDescending(i => i.CreatedDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Invoice>> GetFilteredAsync(
        int? userId = null,
        DateTime? fromDate = null,
        DateTime? toDate = null,
        InvoiceStatus? status = null,
        CancellationToken cancellationToken = default
    )
    {
        var query = _context.Invoices
            .Include(i => i.LineItems)
            .Include(i => i.User)
            .AsQueryable();

        if (userId.HasValue)
            query = query.Where(i => i.RefUserId == userId.Value);

        if (fromDate.HasValue)
            query = query.Where(i => i.IssueDate >= fromDate.Value);

        if (toDate.HasValue)
            query = query.Where(i => i.IssueDate <= toDate.Value);

        if (status.HasValue)
            query = query.Where(i => i.Status == status.Value);

        return await query
            .OrderByDescending(i => i.CreatedDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<Invoice>> GetInvoicesAsync(
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
        var query = _context.Invoices
            .Include(i => i.LineItems)
            .Include(i => i.User)
            .AsQueryable();

        // Apply search filter
        if (!string.IsNullOrWhiteSpace(searchString))
        {
            var searchLower = searchString.ToLower();
            query = query.Where(i => 
                i.InvoiceNumber.ToLower().Contains(searchLower) ||
                i.User.FirstName.ToLower().Contains(searchLower) ||
                i.User.LastName.ToLower().Contains(searchLower) ||
                i.Notes.ToLower().Contains(searchLower));
        }

        // Apply date filters
        if (startDate.HasValue)
            query = query.Where(i => i.IssueDate >= startDate.Value);

        if (endDate.HasValue)
            query = query.Where(i => i.IssueDate <= endDate.Value);

        // Apply status filter
        if (!string.IsNullOrWhiteSpace(status) && Enum.TryParse<InvoiceStatus>(status, true, out var statusEnum))
            query = query.Where(i => i.Status == statusEnum);

        // Apply amount filters
        if (minAmount.HasValue)
            query = query.Where(i => i.TotalAmount >= minAmount.Value);

        if (maxAmount.HasValue)
            query = query.Where(i => i.TotalAmount <= maxAmount.Value);

        // Apply sorting
        query = ApplySorting(query, sortBy, sortDirection);

        // Apply pagination
        if (pageNumber.HasValue && pageSize.HasValue)
        {
            var skip = (pageNumber.Value - 1) * pageSize.Value;
            query = query.Skip(skip).Take(pageSize.Value);
        }

        return await query.ToListAsync(cancellationToken);
    }

    #endregion

    #region DTO Operations

    public async Task<InvoiceListDto> GetInvoicesAsync(
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
        var query = _context.Invoices
            .Include(i => i.LineItems)
            .Where(i => i.RefUserId == userId)
            .AsQueryable();

        // Apply filters
        if (!string.IsNullOrWhiteSpace(status) && Enum.TryParse<InvoiceStatus>(status, true, out var statusEnum))
            query = query.Where(i => i.Status == statusEnum);

        if (startDate.HasValue)
            query = query.Where(i => i.IssueDate >= startDate.Value);

        if (endDate.HasValue)
            query = query.Where(i => i.IssueDate <= endDate.Value);

        // Apply sorting
        query = ApplySorting(query, sortBy, sortOrder);

        // Get total count before pagination
        var totalCount = await query.CountAsync(cancellationToken);

        // Apply pagination
        var skip = (page - 1) * pageSize;
        var invoices = await query
            .Skip(skip)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        // Map to DTOs
        var invoiceDtos = invoices.ToDto();

        return new InvoiceListDto
        {
            TotalCount = totalCount,
            PageSize = pageSize,
            CurrentPage = page,
            TotalPages = (int)Math.Ceiling((double)totalCount / pageSize),
            Invoices = invoiceDtos
        };
    }

    public async Task<InvoiceDetailDto?> GetInvoiceByIdAsync(
        int invoiceId,
        int userId,
        CancellationToken cancellationToken = default
    )
    {
        var invoice = await _context.Invoices
            .Include(i => i.LineItems)
            .Include(i => i.User)
            .FirstOrDefaultAsync(i => i.InvoiceId == invoiceId && i.RefUserId == userId, cancellationToken);

        return invoice?.ToDetailDto();
    }

    public async Task<byte[]?> GeneratePdfAsync(
        int invoiceId,
        int userId,
        CancellationToken cancellationToken = default
    )
    {
        // This would typically use a PDF generation service
        // For now, return null as placeholder
        var invoice = await GetInvoiceByIdAsync(invoiceId, userId, cancellationToken);
        
        if (invoice == null)
            return null;

        // TODO: Implement PDF generation using a service like iTextSharp or similar
        // This is a placeholder implementation
        throw new NotImplementedException("PDF generation not yet implemented. This requires a PDF generation service.");
    }

    #endregion

    #region LineItem Operations

    public async Task<LineItem> GetLineItemByIdAsync(
        int id,
        CancellationToken cancellationToken = default
    )
    {
        var lineItem = await _context.LineItems
            .Include(li => li.Invoice)
            .FirstOrDefaultAsync(li => li.LineItemId == id, cancellationToken);
            
        return lineItem ?? throw new InvalidOperationException($"Pozycja faktury o ID {id} nie została znaleziona.");
    }

    public async Task<IEnumerable<LineItem>> GetLineItemsByInvoiceIdAsync(
        int invoiceId,
        CancellationToken cancellationToken = default
    )
    {
        return await _context.LineItems
            .Include(li => li.Invoice)
            .Where(li => li.InvoiceId == invoiceId)
            .OrderBy(li => li.LineItemId)
            .ToListAsync(cancellationToken);
    }

    public async Task AddLineItemAsync(LineItem lineItem, CancellationToken cancellationToken = default)
    {
        if (lineItem == null)
            throw new ArgumentNullException(nameof(lineItem));

        await _context.LineItems.AddAsync(lineItem, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateLineItemAsync(
        LineItem lineItem,
        CancellationToken cancellationToken = default
    )
    {
        if (lineItem == null)
            throw new ArgumentNullException(nameof(lineItem));

        _context.LineItems.Update(lineItem);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteLineItemAsync(
        LineItem lineItem,
        CancellationToken cancellationToken = default
    )
    {
        if (lineItem == null)
            throw new ArgumentNullException(nameof(lineItem));

        _context.LineItems.Remove(lineItem);
        await _context.SaveChangesAsync(cancellationToken);
    }

    #endregion

    #region Legacy Methods (for backward compatibility)

    public async Task UpdateAsync(Invoice invoice)
    {
        await UpdateAsync(invoice, CancellationToken.None);
    }

    public async Task DeleteAsync(int id)
    {
        var invoice = await GetByIdAsync(id, CancellationToken.None);
        if (invoice != null)
        {
            await DeleteAsync(invoice, CancellationToken.None);
        }
    }

    #endregion

    #region Private Helper Methods

    private IQueryable<Invoice> ApplySorting(IQueryable<Invoice> query, string? sortBy, string? sortDirection)
    {
        var isDescending = !string.IsNullOrWhiteSpace(sortDirection) && 
                          sortDirection.ToLower() == "desc";

        return sortBy?.ToLower() switch
        {
            "invoicenumber" => isDescending ? query.OrderByDescending(i => i.InvoiceNumber) 
                                           : query.OrderBy(i => i.InvoiceNumber),
            "issuedate" => isDescending ? query.OrderByDescending(i => i.IssueDate) 
                                       : query.OrderBy(i => i.IssueDate),
            "duedate" => isDescending ? query.OrderByDescending(i => i.DueDate) 
                                     : query.OrderBy(i => i.DueDate),
            "totalamount" => isDescending ? query.OrderByDescending(i => i.TotalAmount) 
                                         : query.OrderBy(i => i.TotalAmount),
            "status" => isDescending ? query.OrderByDescending(i => i.Status) 
                                    : query.OrderBy(i => i.Status),
            _ => query.OrderByDescending(i => i.CreatedDate) // Default sorting
        };
    }

    #endregion

    }
