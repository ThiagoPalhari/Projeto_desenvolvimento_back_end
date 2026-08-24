namespace Lanchonetes.Application.DTOs.Requests;
public record CreateUnitRequest(string Name, string Cnpj, string Address);
public record UpdateUnitRequest(string Name, string Cnpj, string Address, bool Active);
