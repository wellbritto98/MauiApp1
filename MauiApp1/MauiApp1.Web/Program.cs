using MauiApp1.Web.Components;
using MauiApp1.Web.Data;
using MauiApp1.Web.Services;
using Microsoft.Fast.Components.FluentUI;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Add FluentUI
builder.Services.AddFluentUIComponents();

// Add database connection
builder.Services.AddSingleton<DatabaseConnection>();

// Add services
builder.Services.AddScoped<IInsumoService, InsumoService>();
builder.Services.AddScoped<IServicoService, ServicoService>();

// Add controllers
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddAdditionalAssemblies(typeof(MauiApp1.Shared._Imports).Assembly);

// Map controllers
app.MapControllers();

app.Run();
