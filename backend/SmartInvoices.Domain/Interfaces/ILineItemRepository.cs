using SmartInvoices.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartInvoices.Domain.Interfaces
{
    /// <summary>
    /// Interfejs repozytorium pozycji faktury
    /// </summary>
    public interface ILineItemRepository : IRepository<LineItem>
    {
        /// <summary>
        /// Pobiera pozycje dla danej faktury
        /// </summary>
        /// <param name="invoiceId">Identyfikator faktury</param>
        Task<IEnumerable<LineItem>> GetByInvoiceIdAsync(int invoiceId);
    }
}
