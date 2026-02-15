using C3.Domain;
using C3.Domain.Models;

namespace C3.Application.Interfaces;

public interface IUserRepository
{
    Task<Result<UserData>> GetCurrentUserAsync();
}
