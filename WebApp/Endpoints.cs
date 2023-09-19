namespace WebApp;

public class Endpoints
{
    public string BaseUrl { get; set; }
    public string GetAll { get; set; }
    public string GetById { get; set; }
    public string Add { get; set; }
    public string Update { get; set; }
    public string Delete { get; set; }

    public string AllRecords() => string.Format(BaseUrl + GetAll);
}

