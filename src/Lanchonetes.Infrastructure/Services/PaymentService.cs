using Lanchonetes.Application.DTOs.Requests; using Lanchonetes.Application.DTOs.Responses; using Lanchonetes.Application.Interfaces;
namespace Lanchonetes.Infrastructure.Services;
public class PaymentService : IPaymentService { public Task<PaymentResponse> ProcessAsync(ProcessPaymentRequest request) => throw new NotImplementedException(); public Task<PaymentResponse> CallbackAsync(PaymentCallbackRequest request) => throw new NotImplementedException(); public Task<PaymentResponse> GetAsync(Guid id) => throw new NotImplementedException(); }
