namespace ShortURLService.Domain
{
    
    using System.Security.Cryptography;
    
    using System;
    using System.Formats.Asn1;

    public class UrlShortenerService : IUrlShortenerService
    {
       private readonly IShortURLStorage _storage;
       private readonly string _prefix = "www.example.com/";
        
       public UrlShortenerService(IShortURLStorage storage)    
        {
            _storage = storage;
        }

       public async Task<string> ShortenUrl(string originalUrl)
        {
            int retry =0;
            do {
                //already shortened
                try {
                    var shortURL = await GetShortURLObjectFromLongURL(originalUrl, retry);
                    if(originalUrl == shortURL.LongURL)
                    {
                        return FullShortURLFromToken(shortURL.ShortURL);
                    }
                }
                catch (Exception)
                {
                }
            }
            while (retry++ < 5);

            retry = 0;
            do{
                var genURL = await GenerateShortUrl(originalUrl, retry);
                try
                {
                    var shortURL = new ShortURLObject(){ShortURL = genURL, LongURL = originalUrl};
                    await _storage.AddShortURLAsync(genURL, shortURL);
                    return FullShortURLFromToken( shortURL.ShortURL);
                }
                catch (System.InvalidOperationException)
                {   
                } 

            } while (retry++ < 5);
            throw new System.InvalidOperationException("Short URL already exists.");
        }
       public async Task<string> ShortenUrl(string originalUrl, string customShortenedUrl)
       {
        try
           {
               var newCustomShortenedUrl = customShortenedUrl.Trim(); 
               var shortURL = new ShortURLObject(){ShortURL = newCustomShortenedUrl, LongURL = originalUrl};
               await _storage.AddShortURLAsync(newCustomShortenedUrl, shortURL);
               await _storage.AddCustomURL(originalUrl, newCustomShortenedUrl);
               return FullShortURLFromToken( shortURL.ShortURL);
           }
           catch (System.Exception )
           {
               throw new System.InvalidOperationException("Short URL already exists.");
           }

       }
       public async Task<string> ExpandUrl(string shortenedUrl)
       {
        try{
            var token = TokenFromFullShortURL(shortenedUrl);
            var shortURL = await _storage.GetShortURLByIdAsync(token);
            shortURL.TimeAccess++;
            shortURL.LastAccessed = DateTime.Now;
            await _storage.UpdateShortURLAsync(shortURL.ShortURL, shortURL);
            return shortURL.LongURL;
            }
        catch (System.Exception ) {
           throw new System.InvalidOperationException("Short URL Not exists.");
        }
       }
       public async Task DeleteUrl(string longUrl)
       {
            int retry =0;
            do{
                try{
                    var shortenedUrl = await GetShortURLObjectFromLongURL(longUrl);
                    await _storage.DeleteShortURLAsync(shortenedUrl.ShortURL);
                    return;
                }
                catch (System.Exception ) 
                {
                }
        }
        while (retry++ < 5);
        throw new System.InvalidOperationException("Short URL Not exists.");
       }
       public async Task<ShortenUrlStatistics>  GetStatistics(string shortenedUrl)
       {
           try{
               var token = TokenFromFullShortURL(shortenedUrl); 
               var shortURL = await _storage.GetShortURLByIdAsync(token);
               return new ShortenUrlStatistics(){NumberOfTimesAccessed = shortURL.TimeAccess, LastAccessedOn = shortURL.LastAccessed};
           }
           catch(KeyNotFoundException){
               throw new System.Exception("Short URL Not exists.");
           }
       }

       private async Task<string> GenerateShortUrl(string originalUrl, int retry = 0)
       {
            byte[] bytes = await Task.Run(() => { 
                using SHA256 sha256Hash = SHA256.Create();
                return sha256Hash.ComputeHash(System.Text.Encoding.UTF8.GetBytes(originalUrl));
            });
            var token = BitConverter.ToString(bytes).Replace("-", string.Empty).Substring(0, 10); //max 32 characters
            if(retry > 0)
            {
                token = token + "_" + retry;
            }
            return token;
       }
       private async Task<ShortURLObject> GetShortURLObjectFromLongURL(string longUrl, int retry = 0)
       {
            var token = await  GenerateShortUrl(longUrl, retry);
           
            ShortURLObject? shortURL = null;
            try
            {
                shortURL = await _storage.GetShortURLByIdAsync(token);
            }
            catch (System.Exception)
            {
                if( retry > 0)
                {
                    throw new System.InvalidOperationException("Short URL Not exists.");
                }

                try
                {
                    var customToken = await _storage.GetCustomURL(longUrl);
                    shortURL = await _storage.GetShortURLByIdAsync(customToken);
                }
                catch (System.Exception)
                {
                    throw new System.InvalidOperationException("Short URL Not exists.");
                }
            }
            return shortURL;
       } 
       private string FullShortURLFromToken(string token)
       {
           return _prefix + token;
       }
       private string TokenFromFullShortURL(string fullShortURL)
       {
           return fullShortURL.Replace(_prefix, string.Empty);
       } 
   }

    public class ShortenUrlStatistics
    {
         public int NumberOfTimesAccessed { get; set; }
         public DateTime LastAccessedOn { get; set; }
    }

}