using EventsService.Domain.Entities;

namespace EventsService.Domain.Contracts;

public interface IDateRepository
{
    Task CreateAsync(Date date, CancellationToken cancellationToken);

    Task<List<Date>> GetAllAsync(CancellationToken cancellationToken);

    Task<Date> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    Task UpdateAsync(Date date, CancellationToken cancellationToken);

    Task DeleteAsync(Guid id, CancellationToken cancellationToken);
}