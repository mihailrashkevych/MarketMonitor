using AutoMapper;
using MarketMonitor.API;
using MarketMonitor.DataStore.SQL;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddServicesConfiguration()
                .AddProvidersConfiguration()
                .AddClientsConfiguration();

var mapperConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new APIMappingProfile());
    mc.AddProfile(new CoreMappingProfile());
});

IMapper mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

builder.Services.AddDbContext<SQLDBContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("MonitorDatabaseConnectionString"),
        x => x.MigrationsAssembly("MarketMonitor.DataStore.SQL"));
});


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<SQLDBContext>();

    dbContext.Database.EnsureCreated();

}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();

