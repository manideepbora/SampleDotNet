namespace ShortURLService.Domain
{
    public interface IUrlShortenerService
    {
        Task<string> ShortenUrl(string originalUrl);
        Task<string> ShortenUrl(string originalUrl, string customShortenedUrl);
        Task<string> ExpandUrl(string shortenedUrl);
        Task  DeleteUrl(string shortenedUrl);
        Task<ShortenUrlStatistics> GetStatistics(string shortenedUrl);
   };

 
}
