using System.Text.Json;
using Entities;
using RepositoryContracts;

namespace FileRepositories;

public class CommentFileRepository : ICommentRepository
{
    private readonly string _filePath = "comments.json";

    public CommentFileRepository()
    {
        // Create file with empty array if it doesn't exist
        if (!File.Exists(_filePath))
        {
            File.WriteAllText(_filePath, "[]");
        }
    }

    public async Task<Comment> AddAsync(Comment comment)
    {
        string commentsAsJson = await File.ReadAllTextAsync(_filePath);
        List<Comment> comments = JsonSerializer.Deserialize<List<Comment>>(commentsAsJson)!;

        // Generate new ID
        comment.Id = comments.Any() ? comments.Max(c => c.Id) + 1 : 1;
        comments.Add(comment);

        // Save back to file
        commentsAsJson = JsonSerializer.Serialize(comments, new JsonSerializerOptions { WriteIndented = true });
        await File.WriteAllTextAsync(_filePath, commentsAsJson);
        
        return comment;
    }

    public async Task UpdateAsync(Comment comment)
    {
        string commentsAsJson = await File.ReadAllTextAsync(_filePath);
        List<Comment> comments = JsonSerializer.Deserialize<List<Comment>>(commentsAsJson)!;

        Comment? existingComment = comments.SingleOrDefault(c => c.Id == comment.Id);
        if (existingComment == null)
        {
            throw new InvalidOperationException($"Comment with ID '{comment.Id}' not found.");
        }

        comments.Remove(existingComment);
        comments.Add(comment);

        commentsAsJson = JsonSerializer.Serialize(comments, new JsonSerializerOptions { WriteIndented = true });
        await File.WriteAllTextAsync(_filePath, commentsAsJson);
    }

    public async Task DeleteAsync(int id)
    {
        string commentsAsJson = await File.ReadAllTextAsync(_filePath);
        List<Comment> comments = JsonSerializer.Deserialize<List<Comment>>(commentsAsJson)!;

        Comment? commentToRemove = comments.SingleOrDefault(c => c.Id == id);
        if (commentToRemove == null)
        {
            throw new InvalidOperationException($"Comment with ID '{id}' not found.");
        }

        comments.Remove(commentToRemove);

        commentsAsJson = JsonSerializer.Serialize(comments, new JsonSerializerOptions { WriteIndented = true });
        await File.WriteAllTextAsync(_filePath, commentsAsJson);
    }

    public async Task<Comment> GetSingleAsync(int id)
    {
        string commentsAsJson = await File.ReadAllTextAsync(_filePath);
        List<Comment> comments = JsonSerializer.Deserialize<List<Comment>>(commentsAsJson)!;

        Comment? comment = comments.SingleOrDefault(c => c.Id == id);
        if (comment == null)
        {
            throw new InvalidOperationException($"Comment with ID '{id}' not found.");
        }
        
        return comment;
    }

    public IQueryable<Comment> GetManyAsync()
    {
        // Note: Using .Result because this method can't be async (interface requirement)
        string commentsAsJson = File.ReadAllTextAsync(_filePath).Result;
        List<Comment> comments = JsonSerializer.Deserialize<List<Comment>>(commentsAsJson)!;
        
        return comments.AsQueryable();
    }
}