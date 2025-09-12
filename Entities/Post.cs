namespace Entities;

public class Post
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }
    
    // Foreign key relationship to User
    public int UserId { get; set; }
}