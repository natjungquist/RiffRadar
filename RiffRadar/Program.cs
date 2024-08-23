using RiffRadar.Models.Services;
using RiffRadar.Models.Services.Interfaces;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("secrets.json", optional: true, reloadOnChange: true);

// Add services to the container.
builder.Services.AddControllersWithViews();

var config = builder.Configuration;

//Register
builder.Services.AddHttpClient<ISpotifyAccountService, SpotifyAccountService>(c =>
{
    c.BaseAddress = new Uri(config.GetValue<string>("SpotifyAPI"));
});
builder.Services.AddHttpClient <ISpotifyService, SpotifyService>();
builder.Services.AddTransient<IGenreManager, GenreManager>();


builder.Services.AddMemoryCache();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Login}/{id?}");

app.Run();
