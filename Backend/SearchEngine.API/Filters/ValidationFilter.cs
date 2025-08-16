using MiniValidation;

namespace SearchEngine.Filters;

public class ValidationFilter<T> : IEndpointFilter
{
  public async ValueTask<object?> InvokeAsync(
    EndpointFilterInvocationContext context,
    EndpointFilterDelegate next
  )
  {
    var dto = context.Arguments.OfType<T>().FirstOrDefault();
    if (dto != null)
    {
      if (!MiniValidator.TryValidate(dto, out var errors))
        return Results.BadRequest(errors);
    }

    return await next(context);
  }
}
