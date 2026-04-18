namespace Genovel.Models;

public enum Gender
{
    Male,
    Female,
    Unknown, // Character that gender didn't get reveal yet at the current progress, but should have a gender
    NotSpecified, // Character that hard to specify gender, or author purposely make unclear
    None // Non character (term, object, events, ...)
}


public class Glossary : Base
{
    public string Original { get; set; } = string.Empty;
    public string? Romanized { get; set; } = null;
    public List<Translation> Transaltions { get; set; } = [];
    public Guid StoryId { get; set; }
    public Guid FirstMention { get; set; } // Id of the chapter that this term first mention
    public Gender Gender { get; set; }
}
