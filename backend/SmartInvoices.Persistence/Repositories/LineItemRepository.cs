using Microsoft.EntityFrameworkCore;
using SmartInvoices.Domain.Entities;
using SmartInvoices.Domain.Interfaces;

namespace SmartInvoices.Persistence.Repositories
{
    /// <summary>
    /// Implementacja repozytorium pozycji faktury
    /// </summary>
    public class LineItemRepository : Repository<LineItem>, ILineItemRepository
    {
        /// <summary>
        /// Konstruktor repozytorium pozycji faktury
        /// </summary>
        /// <param name="context">Kontekst bazy danych</param>
        public LineItemRepository(IDbContext context)
            : base(context) { }

        /// <inheritdoc />
        public async Task<IEnumerable<LineItem>> GetByInvoiceIdAsync(int invoiceId)
        {
            return await _dbSet.Where(li => li.InvoiceId == invoiceId).ToListAsync();
        }
    }
}
