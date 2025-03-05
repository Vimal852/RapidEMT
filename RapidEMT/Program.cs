using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RapidEMT.Models;
using RapidEMT.Services;
using Blazored.Toast;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Register the DbContext
builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DBcon")));

// Register other services as scoped
builder.Services.AddScoped<IEmployeeService, EmployeeService>();

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddBlazoredToast(); 

// Add Identity services
builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 5;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.SignIn.RequireConfirmedEmail = false;
})
.AddEntityFrameworkStores<DataContext>();

Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console()  // Logs to Console
    .WriteTo.File("Logs/app.log", rollingInterval: RollingInterval.Day)  // Logs to File
    .WriteTo.Seq("http://localhost:5341") // Optional: Logs to Seq for centralized logging
    .MinimumLevel.Information()
    .CreateLogger();

builder.Host.UseSerilog();

var app = builder.Build();

// Create the database if it doesn't exist
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();
    dbContext.Database.EnsureCreated();
}

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage(); // Show detailed error pages in development
}
else
{
    app.UseExceptionHandler("/Error"); // Handle errors differently in production
}
app.UseSerilogRequestLogging();  // Enables automatic HTTP request logging

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
