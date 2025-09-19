using Entities;
using RepositoryContracts;

namespace CLI.UI;

public class CliApp
{
    private readonly IUserRepository _userRepository;
    private readonly IPostRepository _postRepository;
    private readonly ICommentRepository _commentRepository;

    public CliApp(IUserRepository userRepository, IPostRepository postRepository, ICommentRepository commentRepository)
    {
        _userRepository = userRepository;
        _postRepository = postRepository;
        _commentRepository = commentRepository;
    }

    public async Task StartAsync()
    {
        Console.WriteLine("=== Forum App CLI ===");
        
        while (true)
        {
            Console.WriteLine("\nMain Menu:");
            Console.WriteLine("1. User Management");
            Console.WriteLine("2. Post Management");
            Console.WriteLine("3. Comment Management");
            Console.WriteLine("4. View Posts Overview");
            Console.WriteLine("5. View Specific Post");
            Console.WriteLine("6. Exit");
            Console.Write("Select an option: ");

            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    await ShowUserManagementMenu();
                    break;
                case "2":
                    await ShowPostManagementMenu();
                    break;
                case "3":
                    await ShowCommentManagementMenu();
                    break;
                case "4":
                    await ViewPostsOverview();
                    break;
                case "5":
                    await ViewSpecificPost();
                    break;
                case "6":
                    Console.WriteLine("Goodbye!");
                    return;
                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    break;
            }
        }
    }

    // User Management
    private async Task ShowUserManagementMenu()
    {
        while (true)
        {
            Console.WriteLine("\nUser Management:");
            Console.WriteLine("1. Create New User");
            Console.WriteLine("2. View All Users");
            Console.WriteLine("3. Back to Main Menu");
            Console.Write("Select an option: ");

            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    await CreateUserAsync();
                    break;
                case "2":
                    await ViewAllUsersAsync();
                    break;
                case "3":
                    return;
                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    break;
            }
        }
    }

    private async Task CreateUserAsync()
    {
        Console.WriteLine("\n--- Create New User ---");
        Console.Write("Username: ");
        var username = Console.ReadLine();
        Console.Write("Password: ");
        var password = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            Console.WriteLine("Username and password cannot be empty.");
            return;
        }

        var user = new User { UserName = username, Password = password };
        var createdUser = await _userRepository.AddAsync(user);
        Console.WriteLine($"User created successfully! ID: {createdUser.Id}");
    }

    private async Task ViewAllUsersAsync()
    {
        Console.WriteLine("\n--- All Users ---");
        var users = _userRepository.GetManyAsync();
        
        foreach (var user in users)
        {
            Console.WriteLine($"ID: {user.Id}, Username: {user.UserName}");
        }
    }

    // Post Management
    private async Task ShowPostManagementMenu()
    {
        while (true)
        {
            Console.WriteLine("\nPost Management:");
            Console.WriteLine("1. Create New Post");
            Console.WriteLine("2. View All Posts");
            Console.WriteLine("3. Back to Main Menu");
            Console.Write("Select an option: ");

            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    await CreatePostAsync();
                    break;
                case "2":
                    await ViewAllPostsAsync();
                    break;
                case "3":
                    return;
                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    break;
            }
        }
    }

    private async Task CreatePostAsync()
    {
        Console.WriteLine("\n--- Create New Post ---");
        
        // Show available users
        await ViewAllUsersAsync();
        
        Console.Write("Select User ID: ");
        if (!int.TryParse(Console.ReadLine(), out int userId))
        {
            Console.WriteLine("Invalid User ID.");
            return;
        }

        // Verify user exists
        try
        {
            await _userRepository.GetSingleAsync(userId);
        }
        catch
        {
            Console.WriteLine("User not found.");
            return;
        }

        Console.Write("Title: ");
        var title = Console.ReadLine();
        Console.Write("Body: ");
        var body = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(body))
        {
            Console.WriteLine("Title and body cannot be empty.");
            return;
        }

        var post = new Post { Title = title, Body = body, UserId = userId };
        var createdPost = await _postRepository.AddAsync(post);
        Console.WriteLine($"Post created successfully! ID: {createdPost.Id}");
    }

    private async Task ViewAllPostsAsync()
    {
        Console.WriteLine("\n--- All Posts ---");
        var posts = _postRepository.GetManyAsync();
        
        foreach (var post in posts)
        {
            Console.WriteLine($"ID: {post.Id}, Title: {post.Title}");
        }
    }

    // Comment Management
    private async Task ShowCommentManagementMenu()
    {
        while (true)
        {
            Console.WriteLine("\nComment Management:");
            Console.WriteLine("1. Add Comment to Post");
            Console.WriteLine("2. View All Comments");
            Console.WriteLine("3. Back to Main Menu");
            Console.Write("Select an option: ");

            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    await AddCommentAsync();
                    break;
                case "2":
                    await ViewAllCommentsAsync();
                    break;
                case "3":
                    return;
                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    break;
            }
        }
    }

    private async Task AddCommentAsync()
    {
        Console.WriteLine("\n--- Add Comment ---");
        
        // Show available posts
        await ViewAllPostsAsync();
        
        Console.Write("Select Post ID: ");
        if (!int.TryParse(Console.ReadLine(), out int postId))
        {
            Console.WriteLine("Invalid Post ID.");
            return;
        }

        // Show available users
        await ViewAllUsersAsync();
        
        Console.Write("Select User ID: ");
        if (!int.TryParse(Console.ReadLine(), out int userId))
        {
            Console.WriteLine("Invalid User ID.");
            return;
        }

        // Verify post and user exist
        try
        {
            await _postRepository.GetSingleAsync(postId);
            await _userRepository.GetSingleAsync(userId);
        }
        catch
        {
            Console.WriteLine("Post or User not found.");
            return;
        }

        Console.Write("Comment Body: ");
        var body = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(body))
        {
            Console.WriteLine("Comment body cannot be empty.");
            return;
        }

        var comment = new Comment { Body = body, PostId = postId, UserId = userId };
        var createdComment = await _commentRepository.AddAsync(comment);
        Console.WriteLine($"Comment added successfully! ID: {createdComment.Id}");
    }

    private async Task ViewAllCommentsAsync()
    {
        Console.WriteLine("\n--- All Comments ---");
        var comments = _commentRepository.GetManyAsync();
        
        foreach (var comment in comments)
        {
            Console.WriteLine($"ID: {comment.Id}, Post ID: {comment.PostId}, User ID: {comment.UserId}, Body: {comment.Body}");
        }
    }

    // Required Features
    private async Task ViewPostsOverview()
    {
        Console.WriteLine("\n--- Posts Overview ---");
        var posts = _postRepository.GetManyAsync();
        
        foreach (var post in posts)
        {
            Console.WriteLine($"[{post.Id}] {post.Title}");
        }
    }

    private async Task ViewSpecificPost()
    {
        Console.WriteLine("\n--- View Specific Post ---");
        
        // Show available posts
        await ViewPostsOverview();
        
        Console.Write("Select Post ID to view: ");
        if (!int.TryParse(Console.ReadLine(), out int postId))
        {
            Console.WriteLine("Invalid Post ID.");
            return;
        }

        try
        {
            var post = await _postRepository.GetSingleAsync(postId);
            Console.WriteLine($"\n=== {post.Title} ===");
            Console.WriteLine(post.Body);
            Console.WriteLine($"\n--- Comments ---");

            // Get comments for this post
            var comments = _commentRepository.GetManyAsync().Where(c => c.PostId == postId);
            
            if (!comments.Any())
            {
                Console.WriteLine("No comments yet.");
            }
            else
            {
                foreach (var comment in comments)
                {
                    var user = await _userRepository.GetSingleAsync(comment.UserId);
                    Console.WriteLine($"{user.UserName}: {comment.Body}");
                }
            }
        }
        catch
        {
            Console.WriteLine("Post not found.");
        }
    }
}