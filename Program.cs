using Blazor.SubtleCrypto;
using Blazored.SessionStorage;
using C3;
using C3.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;

const string TornApiAddress = "https://api.torn.com/";

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddMudServices();
builder.Services.AddBlazoredSessionStorage();

builder.Services.AddSingleton<WarSession>();
builder.Services.AddScoped<ProtectedTokenStore>();
builder.Services.AddScoped<BrowserStorageService>();

builder.Services.AddSubtleCrypto(options => options.Key = ProtectedTokenStore.Key);

builder.Services.AddHttpClient<TornApiService>(client => client.BaseAddress = new Uri(TornApiAddress));

await builder.Build().RunAsync();
