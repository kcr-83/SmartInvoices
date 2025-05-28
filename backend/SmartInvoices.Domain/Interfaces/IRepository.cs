namespace SmartInvoices.Domain.Interfaces;

/// <summary>
/// Interfejs bazowy dla repozytoriów
/// </summary>
/// <typeparam name="T">Typ encji</typeparam>
public interface IRepository<T>
    where T : class
{
    /// <summary>
    /// Pobiera wszystkie encje danego typu
    /// </summary>
    Task<IEnumerable<T>> GetAllAsync();

    /// <summary>
    /// Pobiera encję po id
    /// </summary>
    /// <param name="id">Identyfikator encji</param>
    Task<T> GetByIdAsync(int id);

    /// <summary>
    /// Dodaje nową encję
    /// </summary>
    /// <param name="entity">Encja do dodania</param>
    Task AddAsync(T entity);

    /// <summary>
    /// Aktualizuje istniejącą encję
    /// </summary>
    /// <param name="entity">Encja do aktualizacji</param>
    void Update(T entity);

    /// <summary>
    /// Usuwa encję
    /// </summary>
    /// <param name="entity">Encja do usunięcia</param>
    void Delete(T entity);

    /// <summary>
    /// Usuwa encję po id
    /// </summary>
    /// <param name="id">Identyfikator encji do usunięcia</param>
    Task DeleteByIdAsync(int id);

    /// <summary>
    /// Zapisuje zmiany w repozytorium
    /// </summary>
    Task SaveChangesAsync();
}