using EventsService.Infrastructure;
using EventsService.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDb(builder.Configuration);

builder.Services.AddDependencies();

builder.Services.AddRepresentation();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();