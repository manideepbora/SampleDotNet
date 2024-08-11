namespace ShortURLService.Domain
{
    
    using System.Security.Cryptography;
    
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
            var genURL = "";
            bool foundUnique = false;
            do{
                using (SHA256 sha256Hash = SHA256.Create())
                {
                    byte[] bytes = sha256Hash.ComputeHash(System.Text.Encoding.UTF8.GetBytes(originalUrl));
                    genURL = BitConverter.ToString(bytes).Replace("-", string.Empty).Substring(0, 5);
                }
                genURL = "www.example.com/" + genURL;
                try
                {
                    var data = await _storage.GetShortURLByIdAsync(genURL);
                    if(originalUrl == data.LongURL)
                    {
                        return data.ShortURL;
                    }
                }
                catch (System.Exception)
                {
                    foundUnique = true;
                }

            } while (!foundUnique);

            try
            {
                var shortURL = new ShortURLObject(){ShortURL = genURL, LongURL = originalUrl};
                await _storage.AddShortURLAsync(shortURL);
                return shortURL.ShortURL;
            }
            catch (System.InvalidOperationException)
            {
                throw new System.InvalidOperationException("Short URL already exists.");
            } 
        }

       public async Task<string> ShortenUrl(string originalUrl, string customShortenedUrl)
       {
        try
           {
               customShortenedUrl = "www.example.com/" + customShortenedUrl.Trim(); 
               var shortURL = new ShortURLObject(){ShortURL = customShortenedUrl, LongURL = originalUrl};
               await _storage.AddShortURLAsync(shortURL);
               return shortURL.ShortURL;
           }
           catch (System.Exception )
           {
               throw new System.InvalidOperationException("Short URL already exists.");
           }

           throw new NotImplementedException();
       }

       public async Task<string> ExpandUrl(string shortenedUrl)
       {
        try{
            var shortURL = await _storage.GetShortURLByIdAsync(shortenedUrl);
            shortURL.TimeAccess++;
            shortURL.LastAccessed = DateTime.Now;
            await _storage.UpdateShortURLAsync(shortURL);
            return shortURL.LongURL;
            }
        catch (System.Exception ) {
           throw new System.InvalidOperationException("Short URL Not exists.");
        }
       }

       public async Task DeleteUrl(string shortenedUrl)
       {
        try{
            await _storage.DeleteShortURLAsync(shortenedUrl);
        }
        catch (System.Exception ) {
           throw new System.InvalidOperationException("Short URL Not exists.");
        }
       }

       public async Task<ShortenUrlStatistics>  GetStatistics(string shortenedUrl)
       {
           try{
               var shortURL = await _storage.GetShortURLByIdAsync(shortenedUrl);
               return new ShortenUrlStatistics(){NumberOfTimesAccessed = shortURL.TimeAccess, LastAccessedOn = shortURL.LastAccessed};
           }
           catch(KeyNotFoundException){
               throw new System.Exception("Short URL Not exists.");
           }
       }
   }

    public class ShortenUrlStatistics
    {
         public int NumberOfTimesAccessed { get; set; }
         public DateTime LastAccessedOn { get; set; }
    }

}