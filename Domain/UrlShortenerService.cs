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

       public string ShortenUrl(string originalUrl)
       {
           try
           {
               var shortURL = new ShortURLObject(){ShortURL = "shortURL", LongURL = originalUrl};
               _storage.AddShortURLAsync(shortURL);
               return shortURL.ShortURL;
           }
           catch (System.InvalidOperationException ex)
           {
               throw new System.InvalidOperationException("Short URL already exists.");
           } 
           
       }

       public string ShortenUrl(string originalUrl, string customShortenedUrl)
       {
           throw new NotImplementedException();
       }

       public string ExpandUrl(string shortenedUrl)
       {
           throw new NotImplementedException();
       }

       public void DeleteUrl(string shortenedUrl)
       {
           throw new NotImplementedException();
       }

       public ShortenUrlStatistics  GetStatistics(string shortenedUrl)
       {
           throw new NotImplementedException();
       }
   }

    public class ShortenUrlStatistics
    {
         public int NumberOfTimesAccessed { get; set; }
         public DateTime LastAccessedOn { get; set; }
    }

}