namespace UserService.Application.Common.Exceptions;

public class PhotoNotFoundException : Exception
{
    public PhotoNotFoundException(string message) : base(message) { }

    public PhotoNotFoundException(Guid profileId) : base($"Profile {profileId} does not have a photo") { }
}