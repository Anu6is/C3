using Blazored.SessionStorage;
using Blazored.LocalStorage;
using C3.Models;

namespace C3.Services;

public class BrowserStorageService(ISessionStorageService sessionStorageService, ILocalStorageService localStorageService)
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
        return await localStorageService.GetItemAsync<CustomFilterOptions>(FilterKey);
    }

    public async Task SaveFilterOptionsAsync(CustomFilterOptions filterOptions)
    {
        await localStorageService.SetItemAsync(FilterKey, filterOptions);
    }

    public async Task DeleteFilterOptionsAsync()
    {
        await localStorageService.RemoveItemAsync(FilterKey);
    }
}
