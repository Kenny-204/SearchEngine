namespace SearchEngine.Query.Core
{
    public interface IQueryParser
    {
        QueryRepresentation Parse(string query);
    }
}