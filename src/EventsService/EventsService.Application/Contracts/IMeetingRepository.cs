namespace EventsService.Application.Contracts;

using System.Linq.Expressions;
using EventsService.Domain.Entities;

public interface IMeetingRepository : IRepository<Meeting>
{
    Task RejectMeetingAsync(Guid meetingId, Guid profileId, CancellationToken token);
}