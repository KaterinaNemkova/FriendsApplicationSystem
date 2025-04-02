namespace UserService.Application.Common.Exceptions;

public class EntityNotFoundException : Exception
{
    public EntityNotFoundException(string name, object key) : base($"Entity {name} with id {key} not found!") { }
}