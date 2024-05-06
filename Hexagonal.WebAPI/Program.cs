using FluentValidation.AspNetCore;
using Hexagonal.Application.Common.Exceptions.Common;
using InvenTrack.Adapters.Rest.Extensions;
using InvenTrack.Adapters.SQL.Extensions;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddRestApplicationServices(); //Enable it when you want to use Rest Adapter
builder.Services.ConfigureRestServices(); //Enable it when you want to use Rest Adapter

// Configure Serilog
builder.Host.UseSerilog((context, services, configuration) => configuration
    .ReadFrom.Configuration(context.Configuration)
    .ReadFrom.Services(services)
    .Enrich.FromLogContext());
//Register Auto Mapper

//builder.Services.AddAutoMapper(typeof(IOrganizationService), typeof(AutoMapperResult));
builder.Services.AddAutoMapper(typeof(AutoMapperResult));

//Register Fluent Validation
builder.Services.AddFluentValidationAutoValidation();

//Register Validators
//builder.Services.AddValidatorsFromAssemblyContaining<OrganizationDtoValidator>();

//Register Database
builder.Services.InfrastructureServices(builder.Configuration);
builder.Services.AddSwaggerServices();
builder.Services.AddHttpContextAccessor();
//register serilog
Log.Logger = new LoggerConfiguration().CreateBootstrapLogger();
builder.Host.UseSerilog((ctx, lc) => lc.ReadFrom.Configuration(ctx.Configuration));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSwaggerServices();
app.UseSerilogRequestLogging();
app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
