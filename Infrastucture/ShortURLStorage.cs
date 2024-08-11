namespace ShortURLService.Infrastucture
{
    using ShortenUrl.Domain;
    using System;
    using System.Threading.Tasks;
    using System.Collections.Concurrent;
    public class ShortURLStorage : IShortURLStorage
    {
        private ConcurrentDictionary<string, ShortURLObject> _shortURLs = new ConcurrentDictionary<string, ShortURLObject>();

        public Task<ShortURLObject> GetShortURLByIdAsync(string id)
        {
            if( !_shortURLs.TryGetValue(id, out ShortURLObject shortURL))
            {
                throw new KeyNotFoundException();
            }
            return Task.FromResult(shortURL);       
        }

        public Task AddShortURLAsync(ShortURLObject shortURL)
        {
            return Task.Run(() => { 
                if( !_shortURLs.TryAdd(shortURL.ShortURL, shortURL))
                { 
                    throw new InvalidOperationException("Short URL already exists");
                }
                });
        }

        public Task UpdateShortURLAsync(ShortURLObject shortURL)
        {
            return Task.Run(() => { 
                if( !_shortURLs.ContainsKey(shortURL.ShortURL))
                { 
                    throw new KeyNotFoundException();
                }
                _shortURLs[shortURL.ShortURL] =  shortURL;
                });

        }

        public Task DeleteShortURLAsync(string id)
        {
            return Task.Run(() => { 

                if( !_shortURLs.TryRemove(id, out ShortURLObject shortURL))
                { 
                    throw new KeyNotFoundException();
                }
            } );
        }
    }
}