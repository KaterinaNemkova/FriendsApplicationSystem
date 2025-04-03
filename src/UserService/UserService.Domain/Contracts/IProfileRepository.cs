using System.Linq.Expressions;
using UserService.Domain.Entities;
using UserService.Domain.Enums;

namespace UserService.Domain.Contracts;

public interface IProfileRepository
{
    Task CreateAsync(Profile profile, CancellationToken token);
    Task<Profile> GetByIdAsync(Guid id, CancellationToken token);
    Task<List<Profile>> GetAllAsync(CancellationToken token);
    Task EstablishStatus(Guid Id, ActivityStatus status, CancellationToken token);
    Task UpdatePhotoAsync(Guid profileId, Photo photo, CancellationToken token);
    Task DeletePhotoAsync(Guid profileId, CancellationToken token);
    
}