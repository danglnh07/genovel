namespace Genovel.Models;

public class Story : Base
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<string> Genres { get; set; } = [];
    public string Language { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;
    public string Source { get; set; } = string.Empty;
    public bool IsOriginalCompleted { get; set; } = false;
}
