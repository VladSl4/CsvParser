namespace Data.Sql.Models.Configs;

public class DbConfig
{
    public string InitialCatalog {  get; set; }

    public string DataSource { get; set; }

    public bool IntegratedSecurity { get; set; }
}