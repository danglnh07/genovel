namespace Genovel.Models;

public class Chapter : Base
{
    public Guid StoryId { get; set; }
    public string ChapterNumber { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public List<Translation> Translations { get; set; } = [];
}
