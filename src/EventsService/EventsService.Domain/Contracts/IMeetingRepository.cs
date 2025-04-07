namespace EventsService.Domain.Contracts;

using System.Linq.Expressions;
using EventsService.Domain.Entities;

public interface IMeetingRepository : IRepository<Meeting>
{
    Task<List<Meeting>> GetAllFutureAsync(Guid id, CancellationToken cancellationToken);

    Task<List<Meeting>> GetAllPastAsync(Guid id, CancellationToken cancellationToken);

}