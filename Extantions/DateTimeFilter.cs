using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.OpenApi.Any;

namespace Book_Store.Extantions;

public class DateTimeFilter : IOperationFilter
{
  public void Apply(OpenApiOperation operation, OperationFilterContext context)
  {
    foreach (var parameter in operation.Parameters)
    {
      if (parameter.Schema.Type == "string" && parameter.Schema.Format == "date-time")
      {
        parameter.Description = "YYYY-MM-DDThh:mm:ssZ";
        parameter.Example = new OpenApiString(DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ"));
      }
    }
  }
}