namespace Genovel.Models;

public class Translation : Base
{
    public string Language { get; set; } = string.Empty;
    public string Version { get; set; } = string.Empty;
    public string TranslatedContent { get; set; } = string.Empty;
}
