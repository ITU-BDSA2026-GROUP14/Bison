using Bison.Razor;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var dbPath = Environment.GetEnvironmentVariable("BISONDBPATH") ?? Path.Combine(Path.GetTempPath(), "bison.db");

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddDbContext<BisonContext>(o => o.UseSqlite($"Data Source={dbPath}"));
builder.Services.AddScoped<IObservationService, ObservationService>();

var app = builder.Build();

// Create database and tables if they don't exist yet
using (var scope = app.Services.CreateScope())
{
    scope.ServiceProvider.GetRequiredService<BisonContext>().Database.EnsureCreated();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.MapRazorPages();

app.Run();

public partial class Program { };