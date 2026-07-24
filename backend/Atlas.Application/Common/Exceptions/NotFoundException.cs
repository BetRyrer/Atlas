namespace Atlas.Application.Common.Exceptions;

public sealed class NotFoundException(string message) : Exception(message)
{
    public static NotFoundException ForEntity(string entityName, object key) =>
        new($"{entityName} with id '{key}' was not found.");
}
