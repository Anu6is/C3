namespace C3.Infrastructure.TornApi;

public record TornErrorResponse(TornError Error);

public record TornError(int Code, string Error);
