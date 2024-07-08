namespace ExpandoPlayground;

public class FieldDescription
{
    public FieldDescription(string fieldName, string dbName, string? typeName)
    {
        (FieldName, DbName, TypeName) = (fieldName, dbName, typeName);
    }
    public string FieldName { get; set; }
    public string DbName { get; set; }
    public string? TypeName { get; set; }
}
