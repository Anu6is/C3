using Blazor.SubtleCrypto;
using Blazored.LocalStorage;
using Blazored.SessionStorage;
using C3;
using C3.Domain.Models;
using C3.Application.State;
using C3.Application.Services;
using C3.Application.Services.Display;
using C3.Application.Mapping;
using C3.Presentation.Mapping;
using C3.Infrastructure.TornApi;
using C3.Infrastructure.Storage;
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
builder.Services.AddScoped<FilterStateContainer>();
builder.Services.AddScoped<ProtectedTokenStore>();
builder.Services.AddScoped<BrowserStorageService>();
builder.Services.AddScoped<FactionMemberService>();
builder.Services.AddSingleton<TimerService>();
builder.Services.AddSingleton<NumberFormattingService>();
builder.Services.AddSingleton<ColorMappingService>();
builder.Services.AddSingleton<FactionMemberViewModelMapper>();

builder.Services.AddSubtleCrypto(options => options.Key = ProtectedTokenStore.Key);

builder.Services.AddHttpClient<TornApiService>(client => client.BaseAddress = new Uri(TornApiAddress));
builder.Services.AddHttpClient<TornStatsApiService>(client => client.BaseAddress = new Uri(TornStatsApiAddress));

var host = builder.Build();
var timerService = host.Services.GetRequiredService<TimerService>();
timerService.Start();

await host.RunAsync();
