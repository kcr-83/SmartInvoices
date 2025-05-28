using Microsoft.EntityFrameworkCore;
using SmartInvoices.Domain.Entities;
using SmartInvoices.Domain.Enums;
using SmartInvoices.Domain.Interfaces;

namespace SmartInvoices.Persistence.Repositories
{
    /// <summary>
    /// Implementacja repozytorium wniosków o zwrot
    /// </summary>
    public class RefundRequestRepository : Repository<RefundRequest>, IRefundRequestRepository
    {
        /// <summary>
        /// Konstruktor repozytorium wniosków o zwrot
        /// </summary>
        /// <param name="context">Kontekst bazy danych</param>
        public RefundRequestRepository(IDbContext context)
            : base(context) { }

        /// <inheritdoc />
        public async Task<IEnumerable<RefundRequest>> GetByUserIdAsync(int userId)
        {
            return await _dbSet.Where(r => r.RefUserId == userId).ToListAsync();
        }

        /// <inheritdoc />
        public async Task<IEnumerable<RefundRequest>> GetByInvoiceIdAsync(int invoiceId)
        {
            return await _dbSet.Where(r => r.InvoiceId == invoiceId).ToListAsync();
        }

        /// <inheritdoc />
        public async Task<IEnumerable<RefundRequest>> GetByStatusAsync(RefundRequestStatus status)
        {
            return await _dbSet.Where(r => r.Status == status).ToListAsync();
        }

        public Task<RefundRequest> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<RefundRequest>> GetByInvoiceIdAsync(
            int invoiceId,
            CancellationToken cancellationToken
        )
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<RefundRequest>> GetAllAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<RefundRequest> AddAsync(
            RefundRequest refundRequest,
            CancellationToken cancellationToken
        )
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(RefundRequest refundRequest, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(int id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
