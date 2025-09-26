﻿using System.Text.Json;
using Entities;
using RepositoryContracts;

namespace FileRepositories;

public class UserFileRepository : IUserRepository
{
    private readonly string _filePath = "users.json";

    public UserFileRepository()
    {
        if (!File.Exists(_filePath))
        {
            File.WriteAllText(_filePath, "[]");
        }
    }

    public async Task<User> AddAsync(User user)
    {
        string usersAsJson = await File.ReadAllTextAsync(_filePath);
        List<User> users = JsonSerializer.Deserialize<List<User>>(usersAsJson)!;

        user.Id = users.Any() ? users.Max(u => u.Id) + 1 : 1;
        users.Add(user);

        usersAsJson = JsonSerializer.Serialize(users, new JsonSerializerOptions { WriteIndented = true });
        await File.WriteAllTextAsync(_filePath, usersAsJson);
        
        return user;
    }

    public async Task UpdateAsync(User user)
    {
        string usersAsJson = await File.ReadAllTextAsync(_filePath);
        List<User> users = JsonSerializer.Deserialize<List<User>>(usersAsJson)!;

        User? existingUser = users.SingleOrDefault(u => u.Id == user.Id);
        if (existingUser == null)
        {
            throw new InvalidOperationException($"User with ID '{user.Id}' not found.");
        }

        users.Remove(existingUser);
        users.Add(user);

        usersAsJson = JsonSerializer.Serialize(users, new JsonSerializerOptions { WriteIndented = true });
        await File.WriteAllTextAsync(_filePath, usersAsJson);
    }

    public async Task DeleteAsync(int id)
    {
        string usersAsJson = await File.ReadAllTextAsync(_filePath);
        List<User> users = JsonSerializer.Deserialize<List<User>>(usersAsJson)!;

        User? userToRemove = users.SingleOrDefault(u => u.Id == id);
        if (userToRemove == null)
        {
            throw new InvalidOperationException($"User with ID '{id}' not found.");
        }

        users.Remove(userToRemove);

        usersAsJson = JsonSerializer.Serialize(users, new JsonSerializerOptions { WriteIndented = true });
        await File.WriteAllTextAsync(_filePath, usersAsJson);
    }

    public async Task<User> GetSingleAsync(int id)
    {
        string usersAsJson = await File.ReadAllTextAsync(_filePath);
        List<User> users = JsonSerializer.Deserialize<List<User>>(usersAsJson)!;

        User? user = users.SingleOrDefault(u => u.Id == id);
        if (user == null)
        {
            throw new InvalidOperationException($"User with ID '{id}' not found.");
        }
        
        return user;
    }

    public IQueryable<User> GetManyAsync()
    {
        string usersAsJson = File.ReadAllTextAsync(_filePath).Result;
        List<User> users = JsonSerializer.Deserialize<List<User>>(usersAsJson)!;
        
        return users.AsQueryable();
    }
}