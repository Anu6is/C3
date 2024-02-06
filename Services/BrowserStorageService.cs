using Blazored.SessionStorage;

namespace C3.Services;

public class BrowserStorageService(ISessionStorageService sessionStorageService)
{
    public async Task<WarSession> GetUserSessionAsync()
    {
        var warSession = await sessionStorageService.GetItemAsync<WarSession>(WarSession.Key);

        return warSession ?? new WarSession();
    }

    public async Task SaveSessionAsync(WarSession session)
    {
        await sessionStorageService.SetItemAsync(WarSession.Key, session);
    }
}
