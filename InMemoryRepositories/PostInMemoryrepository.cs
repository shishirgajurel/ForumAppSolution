using Entities;
using RepositoryContracts;

namespace InMemoryRepositories;

public class PostInMemoryRepository : IPostRepository
{
    private readonly List<Post> _posts = new();

    public Task<Post> AddAsync(Post post)
    {
        // Generate a new ID
        post.Id = _posts.Any() ? _posts.Max(p => p.Id) + 1 : 1;
        _posts.Add(post);
        return Task.FromResult(post);
    }

    public Task UpdateAsync(Post post)
    {
        // Find the existing post
        Post? existingPost = _posts.SingleOrDefault(p => p.Id == post.Id);
        if (existingPost == null)
        {
            throw new InvalidOperationException($"Post with ID '{post.Id}' not found.");
        }

        // Replace it
        _posts.Remove(existingPost);
        _posts.Add(post);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(int id)
    {
        // Find the existing post
        Post? postToRemove = _posts.SingleOrDefault(p => p.Id == id);
        if (postToRemove == null)
        {
            throw new InvalidOperationException($"Post with ID '{id}' not found.");
        }

        // Remove it
        _posts.Remove(postToRemove);
        return Task.CompletedTask;
    }

    public Task<Post> GetSingleAsync(int id)
    {
        // Find the post
        Post? post = _posts.SingleOrDefault(p => p.Id == id);
        if (post == null)
        {
            throw new InvalidOperationException($"Post with ID '{id}' not found.");
        }
        return Task.FromResult(post);
    }

    public IQueryable<Post> GetManyAsync()
    {
        // Return the list as an IQueryable
        return _posts.AsQueryable();
    }
}