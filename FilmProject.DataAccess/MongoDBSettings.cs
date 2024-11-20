namespace FilmProject.DataAccess
{
    public class MongoDBSettings
    {
        public string connectionURI { get; init; }
        public string databaseName { get; init; }
        public Collections collections { get; init; }
    }
    public class Collections
    {
        public string filmCollection { get; init; }
        public string filmCategoryCollection { get; init; }
    }
}
