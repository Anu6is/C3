using Blazor.SubtleCrypto;
using Blazored.LocalStorage;
using Blazored.SessionStorage;
using C3;
using C3.Models;
using C3.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;

const string TornApiAddress = "https://api.torn.com/";
const string TornStatsApiAddress = "https://www.tornstats.com/api/v2/";

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddMudServices();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddBlazoredSessionStorage();

builder.Services.AddScoped<WarSession>();
builder.Services.AddScoped<CustomFilterOptions>();
builder.Services.AddScoped<ProtectedTokenStore>();
builder.Services.AddScoped<BrowserStorageService>();

builder.Services.AddSubtleCrypto(options => options.Key = ProtectedTokenStore.Key);

builder.Services.AddHttpClient<TornApiService>(client => client.BaseAddress = new Uri(TornApiAddress));
builder.Services.AddHttpClient<TornStatsApiService>(client => client.BaseAddress = new Uri(TornStatsApiAddress));

await builder.Build().RunAsync();
