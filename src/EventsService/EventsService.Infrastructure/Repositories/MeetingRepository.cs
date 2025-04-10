namespace EventsService.Infrastructure.Repositories;

using System.Linq.Expressions;
using EventsService.Domain.Contracts;
using EventsService.Domain.Entities;
using MongoDB.Driver;

public class MeetingRepository : Repository<Meeting>, IMeetingRepository
{
    public MeetingRepository(IMongoCollection<Meeting> collection)
        : base(collection)
    {
    }
    
}