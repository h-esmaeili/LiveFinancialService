using MarketPulse.Api.Configs;
using MarketPulse.Api.Middleware;
using MarketPulse.Api.Service;
using MarketPulse.Api.ServiceAgent;
using MarketPulse.Api.ServiceWorker;

var builder = WebApplication.CreateBuilder(args);

// Configure logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

// Configure CORS to allow all origins (for development/testing purposes)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register WebSocket connection manager and price update service
builder.Services.Configure<TiingoSettings>(builder.Configuration.GetSection("TiingoSettings"));
builder.Services.AddSingleton<WebSocketConnectionManager>();
builder.Services.AddHostedService<MarketUpdateService>();
builder.Services.AddHttpClient<TiingoApiClient>(options => 
{
    var configuration = builder.Configuration;
    options.BaseAddress = new Uri(configuration["TiingoSettings:REST:ApiUrl"]); 
});

builder.Services.AddScoped<IInstrumentService, InstrumentService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Use the CORS policy in the app
app.UseCors("AllowAllOrigins");

app.UseHttpsRedirection();

app.MapControllers();

app.UseWebSockets();
app.UseMiddleware<WebSocketMiddleware>();
app.UseMiddleware<ExceptionMiddleware>();

app.Run();