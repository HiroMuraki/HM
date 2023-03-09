namespace HM.Data;

public record class DbFieldInfo
{
    public DbFieldType Mode { get; set; }
    public string PropertyName { get; init; } = string.Empty;
    public string ColumnName { get; init; } = string.Empty;
}