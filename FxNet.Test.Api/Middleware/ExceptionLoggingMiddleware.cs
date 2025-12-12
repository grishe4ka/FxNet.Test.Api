using System.Text;
using FxNet.Test.Domain.Entities;
using FxNet.Test.Domain.Exceptions;
namespace FxNet.Test.Api.Middleware;

public class ExceptionLoggingMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionLoggingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context, AppDbContext db)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            var eventId = DateTime.UtcNow.Ticks;

            context.Request.EnableBuffering();
            string body = "";
            if (context.Request.ContentLength > 0)
            {
                context.Request.Body.Position = 0;
                using var reader = new StreamReader(
                    context.Request.Body,
                    Encoding.UTF8,
                    detectEncodingFromByteOrderMarks: false,
                    leaveOpen: true);
                body = await reader.ReadToEndAsync();
                context.Request.Body.Position = 0;
            }

            var journal = new JournalEntry
            {
                EventId = eventId,
                CreatedAt = DateTime.UtcNow,
                Text = ex.Message,
                RequestPath = context.Request.Path,
                HttpMethod = context.Request.Method,
                QueryString = context.Request.QueryString.ToString(),
                Body = body,
                ExceptionType = ex.GetType().Name,
                StackTrace = ex.ToString()
            };

            db.JournalEntries.Add(journal);
            await db.SaveChangesAsync();

            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/json";

            object response;

            if (ex is SecureException)
            {
                response = new
                {
                    type = ex.GetType().Name.Replace("Exception", string.Empty),
                    id = eventId.ToString(),
                    data = new { message = ex.Message }
                };
            }
            else
            {
                response = new
                {
                    type = "Exception",
                    id = eventId.ToString(),
                    data = new { message = $"Internal server error id = {eventId}" }
                };
            }

            await context.Response.WriteAsJsonAsync(response);
        }
    }
}

