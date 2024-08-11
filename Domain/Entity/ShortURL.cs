namespace ShortenUrl.Domain
{

public class ShortURLObject
{
    public ShortURLObject()
    {
        TimeAccess = 0;
        LastAccessed = DateTime.Now;
    }
    public string ShortURL { get; set; }
    public string LongURL { get; set; }
    public int TimeAccess { get; set; }
    public DateTime LastAccessed { get; set; }
}
}