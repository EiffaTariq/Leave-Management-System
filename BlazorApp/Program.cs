using BlazorApp;
using BlazorApp.Interface;
using BlazorApp.Services;
using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(/*builder.HostEnvironment.BaseAddress*/) });
builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri("http://localhost:5051/")
});

builder.Services.AddScoped<ILeaveRequestService, LeaveRequestService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ILocalStorageService, LocalStorageService>();
builder.Services.AddBlazoredSessionStorage();

await builder.Build().RunAsync();
