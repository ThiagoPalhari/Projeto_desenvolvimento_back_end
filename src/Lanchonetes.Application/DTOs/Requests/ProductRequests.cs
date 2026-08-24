namespace Lanchonetes.Application.DTOs.Requests;
public record CreateProductRequest(string Name, string Description, decimal Price);
public record UpdateProductRequest(string Name, string Description, decimal Price, bool Active);
public record SetProductAvailabilityRequest(bool Available);
