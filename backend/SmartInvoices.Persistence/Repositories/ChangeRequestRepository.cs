using Microsoft.EntityFrameworkCore;
using SmartInvoices.Application.DTOs.ChangeRequests;
using SmartInvoices.Domain.Entities;
using SmartInvoices.Domain.Interfaces;

namespace SmartInvoices.Persistence.Repositories;

public class ChangeRequestRepository : Repository<ChangeRequest>, SmartInvoices.Application.Interfaces.Repositories.IChangeRequestRepository
{
    public ChangeRequestRepository(IDbContext context) : base(context) { }

    public async Task<IEnumerable<ChangeRequest>> GetAllAsync()
    {
        return await _context.ChangeRequests.ToListAsync();
    }

    public async Task<ChangeRequest?> GetByIdAsync(int id)
    {
        return await _context.ChangeRequests.FindAsync(id);
    }

    public async Task<int> AddAsync(ChangeRequest changeRequest)
    {
        _context.ChangeRequests.Add(changeRequest);
        await _context.SaveChangesAsync();
        return changeRequest.ChangeRequestId;
    }

    public async Task UpdateAsync(ChangeRequest changeRequest)
    {
        ((DbContext)_context).Entry(changeRequest).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var changeRequest = await _context.ChangeRequests.FindAsync(id);
        if (changeRequest != null)
        {
            _context.ChangeRequests.Remove(changeRequest);
            await _context.SaveChangesAsync();
        }
    }

    public Task<IEnumerable<ChangeRequest>> GetByInvoiceIdAsync(int invoiceId)
    {
        throw new NotImplementedException();
    }

    public Task<ChangeRequest> AddAsync(ChangeRequest changeRequest, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<ChangeRequestListDto> GetChangeRequestsAsync(int userId, int page = 1, int pageSize = 10, string? status = null, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<ChangeRequestDetailDto?> GetChangeRequestByIdAsync(int changeRequestId, int userId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<ChangeRequest> CreateChangeRequestAsync(CreateChangeRequestDto changeRequest, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<ChangeRequest?> UpdateStatusAsync(UpdateChangeRequestStatusDto updateData, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
