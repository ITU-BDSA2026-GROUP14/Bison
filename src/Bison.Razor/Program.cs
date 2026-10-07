using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var dbPath = Environment.GetEnvironmentVariable("BISONDBPATH") ?? Path.Combine(Path.GetTempPath(), "bison.db");

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddDbContext<BisonDBContext>(options => options.UseSqlite($"Data Source={dbPath}"));
builder.Services.AddScoped<IPostRepository, PostRepository>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<BisonDBContext>();
    context.Database.EnsureCreated();
    DbInitializer.SeedDatabase(context);
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