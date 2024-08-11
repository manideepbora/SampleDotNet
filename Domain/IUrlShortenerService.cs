namespace ShortURLService.Domain
{
    public interface IUrlShortenerService
    {
        string ShortenUrl(string originalUrl);
        string ShortenUrl(string originalUrl, string customShortenedUrl);
        string ExpandUrl(string shortenedUrl);
        void  DeleteUrl(string shortenedUrl);
        ShortenUrlStatistics GetStatistics(string shortenedUrl);
   };

 
}
