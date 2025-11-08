namespace FindFi.Ef.Domain.Exceptions;

public class NotFoundException(string message) : DomainException(message);
