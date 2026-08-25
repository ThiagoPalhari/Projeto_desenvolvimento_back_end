using Lanchonetes.Application.DTOs.Requests;
using Lanchonetes.Application.DTOs.Responses;
using Lanchonetes.Application.Interfaces;
using Lanchonetes.Domain.Entities;
using Lanchonetes.Domain.Enums;
using Lanchonetes.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Lanchonetes.Infrastructure.Services;

public class PaymentService(
    AppDbContext context,
    IAuditService auditService) : IPaymentService
{
    public async Task<PaymentResponse> ProcessAsync(
        ProcessPaymentRequest request)
    {
        var order = await context.Orders
            .FirstOrDefaultAsync(x => x.Id == request.OrderId);

        if (order is null)
            throw new KeyNotFoundException("Pedido não encontrado.");

        if (request.Amount <= 0)
            throw new ArgumentException(
                "O valor do pagamento deve ser maior que zero.");

        if (request.Amount != order.Total)
            throw new ArgumentException(
                "O valor do pagamento deve ser igual ao total do pedido.");

        var existingPayment = await context.Payments
            .FirstOrDefaultAsync(x => x.OrderId == request.OrderId);

        if (existingPayment is not null)
        {
            return new PaymentResponse(
                existingPayment.Id,
                existingPayment.Status.ToString(),
                existingPayment.ProviderTransactionId);
        }

        var payment = new Payment
        {
            Id = Guid.NewGuid(),
            OrderId = request.OrderId,
            Amount = request.Amount,
            ProviderTransactionId = $"SIM-{Guid.NewGuid():N}",
            Status = PaymentStatus.PENDING,
            CreatedAt = DateTime.UtcNow
        };

        context.Payments.Add(payment);
        order.Status = OrderStatus.PAYMENT_PENDING;

        await context.SaveChangesAsync();

        await auditService.RegisterAsync(
            null,
            "PROCESS_PAYMENT",
            nameof(Payment),
            payment.Id.ToString(),
            $"Solicitação de pagamento registrada para o pedido {request.OrderId}.");

        return new PaymentResponse(
            payment.Id,
            payment.Status.ToString(),
            payment.ProviderTransactionId);
    }

    public async Task<PaymentResponse> CallbackAsync(
        PaymentCallbackRequest request)
    {
        var payment = await context.Payments
            .FirstOrDefaultAsync(x => x.Id == request.PaymentId);

        if (payment is null)
            throw new KeyNotFoundException(
                "Pagamento não encontrado.");

        if (string.IsNullOrWhiteSpace(request.ProviderTransactionId))
            throw new ArgumentException(
                "ProviderTransactionId é obrigatório.");

        if (!Enum.TryParse<PaymentStatus>(
                request.Status,
                true,
                out var status))
        {
            throw new ArgumentException(
                "Status de pagamento inválido.");
        }

        payment.ProviderTransactionId =
            request.ProviderTransactionId;

        payment.Status = status;

        var order = await context.Orders
            .FirstOrDefaultAsync(x => x.Id == payment.OrderId);

        if (order is not null)
        {
            order.Status = status switch
            {
                PaymentStatus.APPROVED => OrderStatus.PAID,
                PaymentStatus.DECLINED => OrderStatus.CANCELLED,
                PaymentStatus.ERROR => OrderStatus.CANCELLED,
                PaymentStatus.CANCELLED => OrderStatus.CANCELLED,
                _ => OrderStatus.PAYMENT_PENDING
            };

            order.UpdatedAt = DateTime.UtcNow;
        }

        await context.SaveChangesAsync();

        await auditService.RegisterAsync(
            null,
            "PAYMENT_CALLBACK",
            nameof(Payment),
            payment.Id.ToString(),
            $"Pagamento do pedido {payment.OrderId} atualizado para {status}.");

        return new PaymentResponse(
            payment.Id,
            payment.Status.ToString(),
            payment.ProviderTransactionId);
    }

    public async Task<PaymentResponse> GetAsync(Guid id)
    {
        var payment = await context.Payments
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);

        if (payment is null)
            throw new KeyNotFoundException(
                "Pagamento não encontrado.");

        return new PaymentResponse(
            payment.Id,
            payment.Status.ToString(),
            payment.ProviderTransactionId);
    }
}