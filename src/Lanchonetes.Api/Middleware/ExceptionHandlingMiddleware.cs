using System.Net; using System.Text.Json;
namespace Lanchonetes.Api.Middleware;
public class ExceptionHandlingMiddleware(RequestDelegate next) { public async Task InvokeAsync(HttpContext context) { try { await next(context); } catch (Exception ex) { context.Response.StatusCode = (int)HttpStatusCode.InternalServerError; context.Response.ContentType = "application/json"; await context.Response.WriteAsync(JsonSerializer.Serialize(new { status = context.Response.StatusCode, message = ex.Message, traceId = context.TraceIdentifier })); } } }
