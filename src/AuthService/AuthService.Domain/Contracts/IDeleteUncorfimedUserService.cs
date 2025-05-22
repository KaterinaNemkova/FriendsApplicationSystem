
namespace AuthService.Domain.Contracts;

public interface IDeleteUncorfimedUserService
{
    Task DeleteUnconfirmedUserAsync(string userId);
}