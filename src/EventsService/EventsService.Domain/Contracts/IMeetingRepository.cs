namespace EventsService.Domain.Contracts;

using System.Linq.Expressions;
using EventsService.Domain.Entities;

public interface IMeetingRepository : IRepository<Meeting>
{
    Task<List<Meeting>> GetAllFutureAsync(Expression<Func<Meeting, bool>> predicate,
        CancellationToken cancellationToken);

    Task<List<Meeting>> GetAllPastAsync(Expression<Func<Meeting, bool>> predicate, CancellationToken cancellationToken);

}