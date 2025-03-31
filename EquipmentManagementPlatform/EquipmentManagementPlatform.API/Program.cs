using EquipmentManagementPlatform.API.Middleware;
using EquipmentManagementPlatform.DomainServices.Bindings;
using EquipmentManagementPlatform.Integration.Equipment.Bindings;
using EquipmentManagementPlatform.Repository.Bindings;
using EquipmentManagementPlatform.Repository.Context;
using Microsoft.EntityFrameworkCore;


const string CUSTOM_CORS_POLICY_NAME = "CustomCorsPolicyName";

var builder = WebApplication.CreateBuilder(args);

EquipmentDomainServicesBindings.ConfigureBindings(builder.Services);
EquipmentRepositoryBindings.ConfigureBindings(builder.Services, builder.Configuration);
EquipmentIntegrationBindings.ConfigureBindings(builder.Services);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: CUSTOM_CORS_POLICY_NAME,
                      policy =>
                      {
                          policy.WithOrigins("http://localhost:3000");
                          policy.AllowAnyMethod();
                          policy.AllowAnyHeader();
                      });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var equipmentContext = scope.ServiceProvider.GetRequiredService<EquipmentContext>();
    equipmentContext.Database.Migrate();
}

app.UseMiddleware<ExceptionHandlerMiddleware>();

app.UseHttpsRedirection();

app.UseCors(CUSTOM_CORS_POLICY_NAME);

app.UseAuthorization();

app.MapControllers();

app.Run();
