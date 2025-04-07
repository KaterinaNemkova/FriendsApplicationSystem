// <copyright file="IRepository.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using EventsService.Domain.Entities;

namespace EventsService.Domain.Contracts;

public interface IRepository<T> where T : Entity
{
    Task CreateAsync(T entity, CancellationToken cancellationToken);

    Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    Task UpdateAsync(T entity, CancellationToken cancellationToken);

    Task DeleteAsync(Guid id, CancellationToken cancellationToken);

    Task<List<T>> GetAllAsync(CancellationToken cancellationToken);

    Task<List<T>> GetAllMyAsync(Guid id, CancellationToken cancellationToken);

}