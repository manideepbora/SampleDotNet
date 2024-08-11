namespace ShortURLService.Domain
{
    using ShortenUrl.Domain;
    using ShortURLService.Infrastucture;
    using System;

    public class UrlShortenerService : IUrlShortenerService
    {
        private readonly IShortURLStorage _storage;
        public UrlShortenerService(IShortURLStorage storage)    
        {
            _storage = storage;
        }

    public async Task<string> ShortenUrl(string originalUrl)
       {
           try
           {
               var shortURL = new ShortURLObject(){ShortURL = "shortURL", LongURL = originalUrl};
               await _storage.AddShortURLAsync(shortURL);
               return shortURL.ShortURL;
           }
           catch (System.InvalidOperationException ex)
           {
               throw new System.InvalidOperationException("Short URL already exists.");
           } 
           
       }

       public async Task<string> ShortenUrl(string originalUrl, string customShortenedUrl)
       {
        try
           {
               var shortURL = new ShortURLObject(){ShortURL = customShortenedUrl, LongURL = originalUrl};
               await _storage.AddShortURLAsync(shortURL);
               return shortURL.ShortURL;
           }
           catch (System.InvalidOperationException )
           {
               throw new System.InvalidOperationException("Short URL already exists.");
           }

           throw new NotImplementedException();
       }

       public async Task<string> ExpandUrl(string shortenedUrl)
       {
        try{
            var shortURL = await _storage.GetShortURLByIdAsync(shortenedUrl);
            return shortURL.LongURL;
            }
        catch (System.InvalidOperationException ){
           throw new System.InvalidOperationException("Short URL Not exists.");
        }
       }

       public async Task DeleteUrl(string shortenedUrl)
       {
        try{
            await _storage.DeleteShortURLAsync(shortenedUrl);
            }
        catch (System.InvalidOperationException ) {
           throw new System.InvalidOperationException("Short URL Not exists.");
        }
       }

       public async Task<ShortenUrlStatistics>  GetStatistics(string shortenedUrl)
       {
           try{
               var shortURL = await _storage.GetShortURLByIdAsync(shortenedUrl);
               return new ShortenUrlStatistics(){NumberOfTimesAccessed = shortURL.TimeAccess, LastAccessedOn = shortURL.LastAccessed};
           }
           catch (System.InvalidOperationException ){
               throw new System.InvalidOperationException("Short URL Not exists.");
           }
       }
   }

    public class ShortenUrlStatistics
    {
         public int NumberOfTimesAccessed { get; set; }
         public DateTime LastAccessedOn { get; set; }
    }

}