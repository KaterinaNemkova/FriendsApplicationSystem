// <copyright file="Repository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace EventsService.Infrastructure.Repositories;

using EventsService.Domain.Contracts;
using EventsService.Domain.Entities;
using MongoDB.Driver;

public class Repository<T> : IRepository<T> where T : Entity
{
    protected readonly IMongoCollection<T> _collection;

    public Repository(IMongoCollection<T> collection)
    {
        _collection = collection;
    }

    public async Task CreateAsync(T entity, CancellationToken cancellationToken)
    {
        await this._collection.InsertOneAsync(entity, cancellationToken: cancellationToken);
    }

    public async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await this._collection.Find(x => x.Id == id).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task UpdateAsync(T entity, CancellationToken cancellationToken)
    {
        var filter = Builders<T>.Filter.Where(m => m.Id == entity.Id);
        await this._collection.ReplaceOneAsync(filter, entity, cancellationToken: cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        await this._collection.DeleteOneAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<List<T>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await this._collection.Find(_ => true).ToListAsync(cancellationToken);
    }

}