using Microsoft.EntityFrameworkCore;
using SmartInvoices.Domain.Interfaces;

namespace SmartInvoices.Persistence.Repositories
{
    /// <summary>
    /// Bazowa implementacja repozytorium
    /// </summary>
    /// <typeparam name="T">Typ encji</typeparam>
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly IDbContext _context;
        protected readonly DbSet<T> _dbSet;

        /// <summary>
        /// Konstruktor bazowego repozytorium
        /// </summary>
        /// <param name="context">Kontekst bazy danych</param>
        public Repository(IDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _dbSet = ((DbContext)context).Set<T>();
        }

        /// <inheritdoc />
        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        /// <inheritdoc />
        public virtual async Task<T> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        /// <inheritdoc />
        public virtual async Task AddAsync(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            await _dbSet.AddAsync(entity);
        }

        /// <inheritdoc />
        public virtual void Update(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            _dbSet.Attach(entity);
            ((DbContext)_context).Entry(entity).State = EntityState.Modified;
        }

        /// <inheritdoc />
        public virtual void Delete(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            if (((DbContext)_context).Entry(entity).State == EntityState.Detached)
                _dbSet.Attach(entity);

            _dbSet.Remove(entity);
        }

        /// <inheritdoc />
        public virtual async Task DeleteByIdAsync(int id)
        {
            T entity = await GetByIdAsync(id);
            if (entity != null)
                Delete(entity);
        }

        /// <inheritdoc />
        public virtual async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
