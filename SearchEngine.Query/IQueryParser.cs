namespace SearchEngine.Query
{
    public interface IQueryParser
    {
        QueryRepresentation Parse(string query);
    }
} 