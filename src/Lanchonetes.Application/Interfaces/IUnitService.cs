using Lanchonetes.Application.DTOs.Requests;
namespace Lanchonetes.Application.Interfaces;
public interface IUnitService { Task CreateAsync(CreateUnitRequest request); Task UpdateAsync(Guid id, UpdateUnitRequest request); Task DeleteAsync(Guid id); Task<object> GetAsync(Guid id); Task<IReadOnlyCollection<object>> GetAllAsync(); }
