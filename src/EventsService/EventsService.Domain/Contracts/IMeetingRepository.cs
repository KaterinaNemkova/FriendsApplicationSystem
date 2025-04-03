namespace EventsService.Domain.Contracts;

using System.Linq.Expressions;
using EventsService.Domain.Entities;

public interface IMeetingRepository
{
    Task CreateAsync(Meeting meeting, CancellationToken cancellationToken);

    Task<Meeting?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<List<Meeting>> GetAllFutureAsync(Expression<Func<Meeting, bool>> predicate,
        CancellationToken cancellationToken);

    Task<List<Meeting>> GetAllPastAsync(Expression<Func<Meeting, bool>> predicate, CancellationToken cancellationToken);

    Task UpdateAsync(Meeting meeting, CancellationToken cancellationToken);

    Task DeleteAsync(Guid id, CancellationToken cancellationToken);
}