using ePTS.Data;
using ePTS.Entities.Audit;
using ePTS.Entities.Identity;
using ePTS.Models.Models;
using ePTS.Web.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting.Json;
using Serilog.Sinks.MSSqlServer;
using Serilog.Sinks.SystemConsole.Themes;
using System.Configuration;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add configuration
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
                    .AddEnvironmentVariables();


// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services
    .AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<ApplicationRole>()
    .AddDefaultUI()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllersWithViews()
    // Add TempData provider
    .AddSessionStateTempDataProvider(); ;

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

// Session State - for storage of user data while the user browses the app
// Setup for in-memory session provider
builder.Services.AddDistributedMemoryCache();

// Sets Idle Session Timeout to 2 hours (before requesting user to login again)
builder.Services.AddSession(options =>
{
    options.Cookie.Name = "ePTSSession"; // Set the name of the session cookie to "ePTSSession"
    options.IdleTimeout = TimeSpan.FromHours(2); // Session timeout set to 24 hours
    options.Cookie.HttpOnly = true; // Cookie is inaccessible to JavaScript on the client side (this is good for security reasons)
    options.Cookie.IsEssential = true; // Cookie is marked as essential, so it won't be blocked if user disables non-essential cookies
});

builder.Services.ConfigureApplicationCookie(options =>
{
    //options.AccessDeniedPath = "/Identity/Account/AccessDenied"; // Uncomment if you want to set a custom access denied path
    options.Cookie.Name = "ePTSApp"; // Name of the authentication cookie
    options.Cookie.HttpOnly = true; // Cookie is inaccessible to JavaScript on the client side (this is good for security reasons)
    options.ExpireTimeSpan = TimeSpan.FromHours(24); // Auth cookie expiration time set to 24 hours
    //options.LoginPath = "/Identity/Account/Login"; // Uncomment if you want to set a custom login path
    // ReturnUrlParameter requires 
    // add using Microsoft.AspNetCore.Authentication.Cookies;
    options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter; // Default behavior, typically used to redirect users back to the page they tried accessing before logging in
    options.SlidingExpiration = true; // If true, the expiration time of the cookie will be reset on each request, effectively making it so the cookie only expires if the user is inactive for the duration of the ExpireTimeSpan.
});

// Using Role-based authorization to validate user roles access
// a member of a higher security role will inherit lower security policies.
// an Administrator will be able to Create, Update, Delete
// Roles are seeded through ApplicationDbInitializer.cs
builder.Services.AddAuthorization(options =>
{
    // Role-based policies
    // Allow create records only
    options.AddPolicy("RequireCreateRole",
        policy => policy.RequireRole("Create", "Edit", "Delete", "MELOfficer", "MEL", "Administrator"));

    // Allow create and update records
    options.AddPolicy("RequireEditRole",
        policy => policy.RequireRole("Edit", "Delete", "MELOfficer", "MEL", "Administrator"));

    // Allow create, update, delete records
    options.AddPolicy("RequireDeleteRole",
        policy => policy.RequireRole("Delete", "MELOfficer", "MEL", "Administrator"));

    // Allow access to M&E only areas for Monitoring, Evaluation and Learning (MEL) Officer access
    // Allow create, update, delete, and M&E Officer access
    options.AddPolicy("RequireMELOfficerRole",
        policy => policy.RequireRole("MELOfficer", "MEL", "Administrator"));

    // Allow Monitoring, Evaluation and Learning (MEL) admin access
    // Allow create, update, delete, and M&E Officer, and M&E Admin access
    options.AddPolicy("RequireMELRole",
        policy => policy.RequireRole("MEL", "Administrator"));

    // Administrator default role
    options.AddPolicy("RequireAdministratorRole",
        policy => policy.RequireRole("Administrator"));

    // Fallback authorization policy requires all users to be authenticated
    //options.FallbackPolicy = new AuthorizationPolicyBuilder()
    //.RequireAuthenticatedUser()
    //.Build();

});

builder.Services.Configure<IdentityOptions>(options =>
{
    // Lockout settings
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1);
    options.Lockout.MaxFailedAccessAttempts = 100;
    //options.Lockout.AllowedForNewUsers = false;
    options.User.RequireUniqueEmail = false;
    options.SignIn.RequireConfirmedEmail = false;
    //options.SignIn.RequireConfirmedAccount = false;
    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";

    // Password settings
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 0;
});

builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("SmtpSettings"));
builder.Services.AddTransient<IEmailSender, EmailService>();




#region Serilog Configuration
// Add Serilog
// Optional (remove when in production)
Serilog.Debugging.SelfLog.Enable(Console.Error);

// Configure Serilog
// columnOptions are used to remove the LogEvent column from the database
// and add the LogEvent column to the database
// This is to allow the use of Serilog's LogEventLevel to filter the log events
// in the database
var columnOptions = new ColumnOptions();
columnOptions.Store.Remove(StandardColumn.LogEvent);
columnOptions.Store.Add(StandardColumn.LogEvent);

// the log file path is set to the Logs folder in the application root
var logFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs\\log-.json");

// default log level is Error
var levelSwitch = new LoggingLevelSwitch(LogEventLevel.Error);

// 
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .MinimumLevel.ControlledBy(levelSwitch)
    .Enrich.FromLogContext()
    //.MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    //.MinimumLevel.Override("System", LogEventLevel.Warning)
    //.MinimumLevel.Override("Microsoft.AspNetCore.Authentication", LogEventLevel.Information)
    .WriteTo.Console(
        outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}",
        theme: AnsiConsoleTheme.Literate,
        applyThemeToRedirectedOutput: true
    )
    .WriteTo.File(
        new JsonFormatter(),
        logFilePath,
        rollingInterval: RollingInterval.Day,
        rollOnFileSizeLimit: true,
        fileSizeLimitBytes: 10_000_000, //10485760, 
        retainedFileCountLimit: 10
        //formatter: new JsonFormatter()
        //outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}"
    )
    .WriteTo.MSSqlServer(
        columnOptions: columnOptions,
        connectionString: connectionString,
        sinkOptions: new MSSqlServerSinkOptions
        {
            TableName = "ApplicationLogs",
            AutoCreateSqlTable = false,
        },
        restrictedToMinimumLevel: LogEventLevel.Information
    )
    .CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog(dispose: true);

// Add Serilog levelSwitch as a Singleton so it can be injected into the controller
builder.Services.AddSingleton(levelSwitch);

#endregion

// Authorization handlers
// Add CookieTempDataProvider for ViewData Messaging
builder.Services.AddSingleton<ITempDataProvider, CookieTempDataProvider>();


var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();



app.UseAuthentication();
app.UseAuthorization();
app.UseCookiePolicy();
app.UseSession();

app.MapAreaControllerRoute(
    name: "SettingsArea",
    areaName: "Settings",
    pattern: "Settings/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();
