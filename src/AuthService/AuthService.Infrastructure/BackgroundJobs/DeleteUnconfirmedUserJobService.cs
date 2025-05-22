namespace AuthService.Infrastructure.HangfireJobs;

using AuthService.Domain.Contracts;
using AuthService.Domain.Entities;
using Microsoft.AspNetCore.Identity;

public class DeleteUnconfirmedUserJobService : IDeleteUncorfimedUserService
{
    private readonly UserManager<ApplicationUser> _userManager;

    public DeleteUnconfirmedUserJobService(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task DeleteUnconfirmedUserAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user != null && !await _userManager.IsEmailConfirmedAsync(user))
        {
            await _userManager.DeleteAsync(user);
        }
    }

}