namespace C3.Models;

public record TornErrorResponse(TornError Error);

public record TornError(int Code, string Error);
