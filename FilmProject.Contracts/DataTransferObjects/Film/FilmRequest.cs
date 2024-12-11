namespace FilmProject.Contracts.DataTransferObjects.Film
{
    public record FilmRequest(string FilmName, float Price, string? FilmDescription);
}
