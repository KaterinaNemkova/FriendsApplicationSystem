using System.Linq.Expressions;
using UserService.Domain.Entities;
using UserService.Domain.Enums;

namespace UserService.Domain.Contracts;

public interface IProfileRepository
{
    Task CreateAsync(Profile profile, CancellationToken token);
    Task<Profile> GetByIdAsync(Guid id, CancellationToken token);
    Task<IEnumerable<Profile>> GetAllAsync(Expression<Func<Profile, bool>> filter, CancellationToken token);
    Task UpdateAsync(Profile profile, CancellationToken token);
    Task DeleteAsync(Profile profile, CancellationToken token);
    Task EstablishStatus(Guid Id, ActivityStatus status, CancellationToken token);
}