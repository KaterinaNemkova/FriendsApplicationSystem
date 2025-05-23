namespace EventsService.Infrastructure.Repositories;

using EventsService.Application.Contracts;
using EventsService.Domain.Entities;
using MongoDB.Driver;

public class DateRepository : Repository<Date>, IDateRepository
{
    public DateRepository(IMongoCollection<Date> collection)
        : base(collection)
    {
    }
}