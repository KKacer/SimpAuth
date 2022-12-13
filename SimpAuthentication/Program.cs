using Microsoft.AspNetCore.Components.Authorization;
using ProSimpAuth;
using SimpAuthentication.Pages;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

// Get the secrets from AppSettings
builder.Services.Configure<MySecrets>(builder.Configuration.GetSection("MySecrets"));

// Override the default AuthenticationStateProvider loaded in AddAuthentication with ours
builder.Services.AddScoped<AuthenticationStateProvider, VerySimpleAuthenticationStateProvider>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();