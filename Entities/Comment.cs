namespace Entities;

public class Comment
{
    public int Id { get; set; }
    public string Body { get; set; }
    
    // Foreign key relationship to Post
    public int PostId { get; set; }
    // Foreign key relationship to User
    public int UserId { get; set; }
}