using Lanchonetes.Application.DTOs.Requests;
using Lanchonetes.Application.DTOs.Responses;
namespace Lanchonetes.Application.Interfaces;
public interface IPaymentService { Task<PaymentResponse> ProcessAsync(ProcessPaymentRequest request); Task<PaymentResponse> CallbackAsync(PaymentCallbackRequest request); Task<PaymentResponse> GetAsync(Guid id); }
