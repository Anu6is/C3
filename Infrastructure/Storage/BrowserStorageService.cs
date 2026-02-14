using Blazored.SessionStorage;
using Blazored.LocalStorage;
using C3.Domain.Models;
using C3.Application.DTOs;

namespace C3.Infrastructure.Storage;

public class BrowserStorageService(ISessionStorageService sessionStorageService)
{
    private const string FilterKey = "filters";
    public async Task<WarSession> GetUserSessionAsync()
    {
        var warSession = await sessionStorageService.GetItemAsync<WarSession>(WarSession.Key);

        return warSession ?? new WarSession();
    }

    public async Task SaveSessionAsync(WarSession session)
    {
        await sessionStorageService.SetItemAsync(WarSession.Key, session);
    }

    public async Task<CustomFilterOptions?> GetFilterOptionsAsync()
    {
        return await sessionStorageService.GetItemAsync<CustomFilterOptions>(FilterKey);
    }

    public async Task SaveFilterOptionsAsync(CustomFilterOptions filterOptions)
    {
        await sessionStorageService.SetItemAsync(FilterKey, filterOptions);
    }

    public async Task DeleteFilterOptionsAsync()
    {
        await sessionStorageService.RemoveItemAsync(FilterKey);
    }
}
