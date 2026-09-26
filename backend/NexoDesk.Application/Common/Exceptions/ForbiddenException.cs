namespace NexoDesk.Application.Common.Exceptions;

public sealed class ForbiddenException(string message) : Exception(message);
