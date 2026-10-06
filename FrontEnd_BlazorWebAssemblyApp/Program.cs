using FrontEnd_BlazorWebAssemblyApp;
using FrontEnd_BlazorWebAssemblyApp.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using System.Runtime.Intrinsics.Arm;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Point the client HttpClient at the API base address (use the HTTPS API URL)
var apiBase = new Uri("https://localhost:7009/"); // <- replace with the API HTTPS URL printed when running the API


//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
//// if on different server, use address below
//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7109") });
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = apiBase });

builder.Services.AddScoped<IPassageService, PassageService>();



await builder.Build().RunAsync();
