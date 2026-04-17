namespace Genovel.Shared.Patterns;

public record Error(string Code, string Message, string? StackTrace = null)
{
    public static readonly Error None = new(string.Empty, string.Empty);

    public static Error Unexpected(string code, string description)
    {
        return new(code, description, Environment.StackTrace);
    }

    public static Error NotFound(string entity)
    {
        return new("404NotFound", $"{entity} not found with this ID");
    }

    public static bool IsNotFound(Error error)
    {
        return error.Code == "404NotFound";
    }

    public static Error InvalidRequest(string message)
    {
        return new("400BadRequest", $"Invalid request: {message}");
    }

    public static bool IsInvalidRequest(Error error)
    {
        return error.Code == "400BadRequest";
    }
}
