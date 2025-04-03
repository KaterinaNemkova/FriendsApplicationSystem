using EventsService.Domain.Contracts;
using EventsService.Domain.Entities;
using MongoDB.Driver;

namespace EventsService.Infrastructure.Repositories;

public class DateRepository : IDateRepository
{
    private readonly IMongoCollection<Date> _datesCollection;

    public DateRepository(IMongoDatabase database)
    {
        this._datesCollection = database.GetCollection<Date>("Dates");
    }

    public async Task CreateAsync(Date date, CancellationToken cancellationToken)
    {
        await this._datesCollection.InsertOneAsync(date, cancellationToken: cancellationToken);
    }

    public async Task<List<Date>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await this._datesCollection.Find(_ => true).ToListAsync(cancellationToken);
    }

    public async Task<Date> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await this._datesCollection.Find(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task UpdateAsync(Date date, CancellationToken cancellationToken)
    {
        await this._datesCollection.ReplaceOneAsync(x => x.Id == date.Id, date, cancellationToken: cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        await this._datesCollection.DeleteOneAsync(x => x.Id == id, cancellationToken);
    }
}