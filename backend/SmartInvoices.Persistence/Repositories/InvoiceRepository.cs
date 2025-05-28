using Microsoft.EntityFrameworkCore;
using SmartInvoices.Domain.Entities;
using SmartInvoices.Domain.Enums;
using SmartInvoices.Domain.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace SmartInvoices.Persistence.Repositories
{
    /// <summary>
    /// Implementacja repozytorium faktur
    /// </summary>
    public class InvoiceRepository : Repository<Invoice>, IInvoiceRepository
    {
        /// <summary>
        /// Konstruktor repozytorium faktur
        /// </summary>
        /// <param name="context">Kontekst bazy danych</param>
        public InvoiceRepository(IDbContext context) : base(context)
        {
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Invoice>> GetByUserIdAsync(int userId)
        {
            return await _dbSet
                .Where(i => i.UserId == userId)
                .Include(i => i.LineItems)
                .ToListAsync();
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Invoice>> GetByStatusAsync(InvoiceStatus status)
        {
            return await _dbSet
                .Where(i => i.Status == status)
                .Include(i => i.LineItems)
                .ToListAsync();
        }

        /// <inheritdoc />
        public async Task<Invoice> GetByInvoiceNumberAsync(string invoiceNumber)
        {
            return await _dbSet
                .Where(i => i.InvoiceNumber == invoiceNumber)
                .Include(i => i.LineItems)
                .FirstOrDefaultAsync();
        }

        /// <inheritdoc />
        public override async Task<Invoice> GetByIdAsync(int id)
        {
            return await _dbSet
                .Where(i => i.InvoiceId == id)
                .Include(i => i.LineItems)
                .Include(i => i.RefundRequests)
                .FirstOrDefaultAsync();
        }

        /// <inheritdoc />
        public override async Task<IEnumerable<Invoice>> GetAllAsync()
        {
            return await _dbSet
                .Include(i => i.LineItems)
                .ToListAsync();
        }

        public Task<IEnumerable<Invoice>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Invoice>> GetByUserIdAsync(int userId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Invoice>> GetFilteredAsync(int? userId = null, DateTime? fromDate = null, DateTime? toDate = null, InvoiceStatus? status = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<List<Invoice>> GetInvoicesAsync(string? searchString, DateTime? startDate, DateTime? endDate, string? status, decimal? minAmount, decimal? maxAmount, string? sortBy, string? sortDirection, int? pageNumber, int? pageSize, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<Invoice> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<Invoice> GetByInvoiceNumberAsync(string invoiceNumber, CancellationToken cancellationToken = default)
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

        public Task<LineItem> GetLineItemByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<LineItem>> GetLineItemsByInvoiceIdAsync(int invoiceId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task AddLineItemAsync(LineItem lineItem, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task UpdateLineItemAsync(LineItem lineItem, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task DeleteLineItemAsync(LineItem lineItem, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
