namespace UserService.Application.Contracts;

using UserService.Domain.Entities;
using UserService.Domain.Enums;

public interface IProfileRepository
{
    Task CreateAsync(Profile profile, CancellationToken token);

    Task<Profile> GetByIdAsync(Guid id, CancellationToken token);

    Task<List<Profile>> GetAllAsync(CancellationToken token);

    Task EstablishStatus(Guid id, ActivityStatus status, CancellationToken token);

    Task UpdatePhotoAsync(Guid profileId, Photo photo, CancellationToken token);

    Task DeletePhotoAsync(Guid profileId, CancellationToken token);

}