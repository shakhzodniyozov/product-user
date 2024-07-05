namespace Domain;

public class User : BaseEntity
{
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public List<Product> Products { get; set; } = new();
}
