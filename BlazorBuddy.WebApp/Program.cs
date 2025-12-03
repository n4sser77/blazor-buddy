using BlazorBuddy.Core.Interfaces;
using BlazorBuddy.WebApp.Components;
using BlazorBuddy.WebApp.Components.Account;
using BlazorBuddy.WebApp.Data;
using BlazorBuddy.WebApp.Repositories;
using BlazorBuddy.WebApp.Repositories.Interfaces;
using BlazorBuddy.WebApp.Services;
using BlazorBuddy.WebApp.Services.Interfaces;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = IdentityConstants.ApplicationScheme;
        options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
    })
    .AddIdentityCookies();

// Use in-memory database for development
//builder.Services.AddDbContext<ApplicationDbContext>(options =>
//    options.UseInMemoryDatabase("BlazorBuddyInMemory"));
//builder.Services.AddDatabaseDeveloperPageExceptionFilter();

if (!builder.Environment.IsEnvironment("IntegrationTest"))
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
        ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(connectionString));
    builder.Services.AddDatabaseDeveloperPageExceptionFilter();
}

builder.Services.AddIdentityCore<ApplicationUser>(options =>
    {
        options.SignIn.RequireConfirmedAccount = true;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();
builder.Services.AddScoped<INoteRepo, NoteRepo>();
builder.Services.AddScoped<IStudyPageRepo, StudyPageRepo>();
builder.Services.AddSingleton<StudyPageStateService>();
builder.Services.AddScoped<IImageRepo, ImageRepo>();
builder.Services.AddScoped<ICanvasRepo, CanvasRepo>();
builder.Services.AddScoped<IUserRepo, UserProfileRepo>();
builder.Services.AddScoped<IChatRepo, ChatRepo>();
builder.Services.AddScoped<IChatService, ChatService>();
builder.Services.AddSingleton<IChatEventBroker, ChatEventBroker>();
builder.Services.AddScoped<IFriendlistRepo, FriendListRepo>();
builder.Services.AddScoped<IFriendListService, FriendListService>();
builder.Services.AddScoped<IImageService, ImageService>();
builder.Services.AddScoped<ICanvasService, CanvasService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Add additional endpoints required by the Identity /Account Razor components.
app.MapAdditionalIdentityEndpoints();

app.Run();
