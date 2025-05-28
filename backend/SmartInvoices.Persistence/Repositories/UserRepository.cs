using Microsoft.EntityFrameworkCore;
using SmartInvoices.Domain.Entities;
using SmartInvoices.Domain.Interfaces;

namespace SmartInvoices.Persistence.Repositories
{
    /// <summary>
    /// Implementacja repozytorium użytkowników
    /// </summary>
    public class UserRepository : Repository<User>, Application.Interfaces.Repositories.IUserRepository
    {
        /// <summary>
        /// Konstruktor repozytorium użytkowników
        /// </summary>
        /// <param name="context">Kontekst bazy danych</param>
        public UserRepository(IDbContext context)
            : base(context) { }

        /// <inheritdoc />
        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _dbSet.Where(u => u.Email == email).FirstOrDefaultAsync();
        }

        /// <inheritdoc />
        public async Task<IEnumerable<User>> GetActiveUsersAsync()
        {
            return await _dbSet.Where(u => u.IsActive).ToListAsync();
        }

        public Task<IEnumerable<User>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<User> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<User> GetByEmailAsync(
            string email,
            CancellationToken cancellationToken = default
        )
        {
            throw new NotImplementedException();
        }

        public Task AddAsync(User user, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(User user, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(User user, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<NotificationSettings> GetNotificationSettingsAsync(
            int userId,
            CancellationToken cancellationToken = default
        )
        {
            throw new NotImplementedException();
        }

        public Task SaveNotificationSettingsAsync(
            NotificationSettings settings,
            CancellationToken cancellationToken = default
        )
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Notification>> GetNotificationsAsync(
            int userId,
            bool includeRead = false,
            CancellationToken cancellationToken = default
        )
        {
            throw new NotImplementedException();
        }

        public Task AddNotificationAsync(
            Notification notification,
            CancellationToken cancellationToken = default
        )
        {
            throw new NotImplementedException();
        }

        public Task UpdateNotificationAsync(
            Notification notification,
            CancellationToken cancellationToken = default
        )
        {
            throw new NotImplementedException();
        }

        public Task MarkAllNotificationsAsReadAsync(
            int userId,
            CancellationToken cancellationToken = default
        )
        {
            throw new NotImplementedException();
        }

        public Task AddAuditLogAsync(AuditLog log, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<AuditLog>> GetAuditLogsByUserIdAsync(
            int userId,
            CancellationToken cancellationToken = default
        )
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<AuditLog>> GetAuditLogsByEntityAsync(
            string entityType,
            int entityId,
            CancellationToken cancellationToken = default
        )
        {
            throw new NotImplementedException();
        }
    }
}
