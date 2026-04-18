namespace Genovel.Shared.Utils;

public static class Normalizer
{
    public static IList<string> NormalizeGenres(IList<string> genres)
    {
        genres = [.. genres.Select(genre => genre.Trim().ToLower())];
        return genres;
    }
}
