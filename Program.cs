using Book_Store.Data;
using Book_Store.Extantions;
using Book_Store.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
  // Swagger o'zgaruvchilari

  options.SwaggerDoc("v1", new OpenApiInfo
  {
    Version = "v1",
    Title = "API Title",
    Description = "API Description",
  });

  // DateTime turi uchun DatePicket ni o'rnatish
  options.MapType<DateTime>(() => new OpenApiSchema { Type = "string", Format = "date-time" });
  
  options.OperationFilter<DateTimeFilter>();
});

builder.Services.AddDbContext<AppDbContext>(options =>
{
  options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddScoped<IAllDatasService, AllDatasService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI(c =>
  {
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
        
    // Swagger UI-da DateTime turi uchun DatePicket ni o'rnatish
    c.ConfigObject.AdditionalItems["syntaxHighlight"] = new
    {
      activated = false,
      theme = "agate"
    };

    c.DefaultModelExpandDepth(-1);
    c.DefaultModelRendering(ModelRendering.Model);
    c.DocExpansion(DocExpansion.None);

    // Swagger UI Extension qo'shish
    c.EnableFilter();
    c.EnableDeepLinking();
    c.DisplayRequestDuration();
  });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();