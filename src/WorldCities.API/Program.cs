using Serilog;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;
using WorldCities.API.Middlewares;
using WorldCities.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Adds Serilog support
AddSerilogSupport(builder);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddPersistenceServices(builder.Configuration);
builder.Services.AddJwtBearerAuthentication(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSerilogRequestLogging();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

static void AddSerilogSupport(WebApplicationBuilder builder)
{
    builder.Host.UseSerilog((ctx, lc) => lc
        .ReadFrom.Configuration(ctx.Configuration)
        .WriteTo.MSSqlServer(connectionString:
                    ctx.Configuration.GetConnectionString("DefaultConnection"),
                restrictedToMinimumLevel: LogEventLevel.Information,
                sinkOptions: new MSSqlServerSinkOptions
                {
                    TableName = "LogEvents",
                    AutoCreateSqlTable = true
                }
                )
        .WriteTo.Console()
        );
}