using Entities;
using RepositoryContracts;

namespace InMemoryRepositories;

public class CommentInMemoryRepository : ICommentRepository
{
    private readonly List<Comment> _comments = new();

    public Task<Comment> AddAsync(Comment comment)
    {
     
        comment.Id = _comments.Any() ? _comments.Max(c => c.Id) + 1 : 1;
        _comments.Add(comment);
        return Task.FromResult(comment);
    }

    public Task UpdateAsync(Comment comment)
    {
      
        Comment? existingComment = _comments.SingleOrDefault(c => c.Id == comment.Id);
        if (existingComment == null)
        {
            throw new InvalidOperationException($"Comment with ID '{comment.Id}' not found.");
        }


        _comments.Remove(existingComment);
        _comments.Add(comment);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(int id)
    {
      
        Comment? commentToRemove = _comments.SingleOrDefault(c => c.Id == id);
        if (commentToRemove == null)
        {
            throw new InvalidOperationException($"Comment with ID '{id}' not found.");
        }

   
        _comments.Remove(commentToRemove);
        return Task.CompletedTask;
    }

    public Task<Comment> GetSingleAsync(int id)
    {
  
        Comment? comment = _comments.SingleOrDefault(c => c.Id == id);
        if (comment == null)
        {
            throw new InvalidOperationException($"Comment with ID '{id}' not found.");
        }
        return Task.FromResult(comment);
    }

    public IQueryable<Comment> GetManyAsync()
    {
    
        return _comments.AsQueryable();
    }

 
    public CommentInMemoryRepository()
    {


        _comments.Add(new Comment { Id = 1, Body = "Great first post!", PostId = 1, UserId = 2 });
        _comments.Add(new Comment { Id = 2, Body = "Welcome to the forum!", PostId = 1, UserId = 3 });
        _comments.Add(new Comment { Id = 3, Body = "I agree, this is fun!", PostId = 2, UserId = 1 });
    }
}