using Entities;
using RepositoryContracts;

namespace InMemoryRepositories;

public class UserInMemoryRepository : IUserRepository
{
    private readonly List<User> _users = new();

    public Task<User> AddAsync(User user)
    {
        // Generate a new ID
        user.Id = _users.Any() ? _users.Max(u => u.Id) + 1 : 1;
        _users.Add(user);
        return Task.FromResult(user);
    }

    public Task UpdateAsync(User user)
    {
        // Find the existing user
        User? existingUser = _users.SingleOrDefault(u => u.Id == user.Id);
        if (existingUser == null)
        {
            throw new InvalidOperationException($"User with ID '{user.Id}' not found.");
        }

        // Replace it
        _users.Remove(existingUser);
        _users.Add(user);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(int id)
    {
        // Find the existing user
        User? userToRemove = _users.SingleOrDefault(u => u.Id == id);
        if (userToRemove == null)
        {
            throw new InvalidOperationException($"User with ID '{id}' not found.");
        }

        // Remove it
        _users.Remove(userToRemove);
        return Task.CompletedTask;
    }

    public Task<User> GetSingleAsync(int id)
    {
        // Find the user
        User? user = _users.SingleOrDefault(u => u.Id == id);
        if (user == null)
        {
            throw new InvalidOperationException($"User with ID '{id}' not found.");
        }
        return Task.FromResult(user);
    }

    public IQueryable<User> GetManyAsync()
    {
        // Return the list as an IQueryable
        return _users.AsQueryable();
    }

    // Optional: Constructor with dummy data
    public UserInMemoryRepository()
    {
        // Add some sample users
        _users.Add(new User { Id = 1, UserName = "john_doe", Password = "password123" });
        _users.Add(new User { Id = 2, UserName = "jane_smith", Password = "securepass" });
        _users.Add(new User { Id = 3, UserName = "bob_wilson", Password = "bobspass" });
    }
}