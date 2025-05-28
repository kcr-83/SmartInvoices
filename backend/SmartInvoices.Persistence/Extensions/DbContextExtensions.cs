using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SmartInvoices.Domain.Interfaces;

namespace SmartInvoices.Persistence.Extensions
{
    /// <summary>
    /// Rozszerzenia dla interfejsu IDbContext
    /// </summary>
    public static class DbContextExtensions
    {
        /// <summary>
        /// Rozszerzenie dające dostęp do DbSet dla dowolnego typu
        /// </summary>
        /// <typeparam name="TEntity">Typ encji</typeparam>
        /// <param name="context">Kontekst bazy danych</param>
        public static DbSet<TEntity> Set<TEntity>(this IDbContext context) where TEntity : class
        {
            return ((DbContext)context).Set<TEntity>();
        }

        /// <summary>
        /// Rozszerzenie dające dostęp do informacji o stanie encji
        /// </summary>
        /// <typeparam name="TEntity">Typ encji</typeparam>
        /// <param name="context">Kontekst bazy danych</param>
        /// <param name="entity">Encja</param>
        public static EntityEntry<TEntity> Entry<TEntity>(this IDbContext context, TEntity entity) where TEntity : class
        {
            return ((DbContext)context).Entry(entity);
        }
    }
}
