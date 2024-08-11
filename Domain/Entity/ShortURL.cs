namespace ShortURLService.Domain
{

    public class ShortURLObject
    {
        public ShortURLObject()
        {
            TimeAccess = 0;
            LastAccessed = DateTime.Now;
        }
        public string ShortURL { get; set; } = string.Empty;
        public string LongURL { get; set; } = string.Empty;
        public int TimeAccess { get; set; }
        public DateTime LastAccessed { get; set; }
    }
}