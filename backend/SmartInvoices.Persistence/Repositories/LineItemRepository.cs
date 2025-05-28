using Microsoft.EntityFrameworkCore;
using SmartInvoices.Application.DTOs.LineItems;
using SmartInvoices.Domain.Entities;
using SmartInvoices.Domain.Interfaces;

namespace SmartInvoices.Persistence.Repositories
{
    /// <summary>
    /// Implementacja repozytorium pozycji faktury
    /// </summary>
    public class LineItemRepository : Repository<LineItem>, SmartInvoices.Application.Interfaces.Repositories.ILineItemRepository
    {
        /// <summary>
        /// Konstruktor repozytorium pozycji faktury
        /// </summary>
        /// <param name="context">Kontekst bazy danych</param>
        public LineItemRepository(IDbContext context)
            : base(context) { }

        public async Task<LineItem?> GetByIdAsync(int lineItemId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Include(li => li.Invoice)
                .FirstOrDefaultAsync(li => li.LineItemId == lineItemId, cancellationToken);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<LineItem>> GetByInvoiceIdAsync(int invoiceId)
        {
            return await _dbSet.Where(li => li.InvoiceId == invoiceId).ToListAsync();
        }

        public async Task<Invoice?> GetInvoiceByLineItemIdAsync(int lineItemId, CancellationToken cancellationToken = default)
        {
            return await _context.Invoices
                .Include(i => i.LineItems)
                .FirstOrDefaultAsync(i => i.LineItems.Any(li => li.LineItemId == lineItemId), cancellationToken);
        }

        public async Task<LineItemDto?> GetLineItemDtoByIdAsync(int lineItemId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(li => li.LineItemId == lineItemId)
                .Select(li => new LineItemDto
                {
                    LineItemId = li.LineItemId,
                    Description = li.Description,
                    Quantity = li.Quantity,
                    UnitPrice = li.UnitPrice,
                    TaxRate = li.TaxRate,
                    TotalPrice = li.TotalPrice
                })
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<List<LineItemDto>> GetLineItemsByInvoiceIdAsync(int invoiceId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(li => li.InvoiceId == invoiceId)
                .Select(li => new LineItemDto
                {
                    LineItemId = li.LineItemId,
                    Description = li.Description,
                    Quantity = li.Quantity,
                    UnitPrice = li.UnitPrice,
                    TaxRate = li.TaxRate,
                    TotalPrice = li.TotalPrice
                })
                .ToListAsync(cancellationToken);
        }
    }
}
