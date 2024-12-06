namespace FilmProject.Contracts.FilterErrors
{
    public class FilterErrorModel
    {
        public string fieldName { get; set; }
        public List<string> messages { get; set; } = new List<string>();
    }
}
