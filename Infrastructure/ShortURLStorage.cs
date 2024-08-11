namespace ShortURLService.Infrastucture
{
    using ShortURLService.Domain;
    using System;
    using System.Threading.Tasks;
    using System.Collections.Concurrent;
    public class ShortURLStorage : IShortURLStorage
    {
        private ConcurrentDictionary<string, ShortURLObject> _shortURLs = new ConcurrentDictionary<string, ShortURLObject>();
         private ConcurrentDictionary<string, string> _customURLL2S = new ConcurrentDictionary<string, string>();

        public Task<ShortURLObject> GetShortURLByIdAsync(string id)
        {
            if (!_shortURLs.TryGetValue(id, out ShortURLObject? shortURL))
            {
                throw new KeyNotFoundException();
            }
            return Task.FromResult(shortURL);       
        }

        public Task AddShortURLAsync(string id, ShortURLObject shortURL)
        {
            return Task.Run(() => { 
                if( !_shortURLs.TryAdd(id, shortURL))
                { 
                    throw new InvalidOperationException();
                }
                });
        }

        public Task UpdateShortURLAsync(string id, ShortURLObject shortURL)
        {
            return Task.Run(() => { 
                if( !_shortURLs.ContainsKey(id))
                { 
                    throw new KeyNotFoundException();
                }
                _shortURLs[id] =  shortURL;
                });

        }

        public Task DeleteShortURLAsync(string id)
        {
            return Task.Run(() => {
                if (!_shortURLs.TryRemove(id, out ShortURLObject ? shortURL))
                {
                    throw new KeyNotFoundException();
                }
            } );
        }
    
        public Task AddCustomURL(string longURL, string shortURL)
        {
            return Task.Run(() => {
                if (!_customURLL2S.TryAdd(longURL, shortURL))
                {
                    throw new InvalidOperationException();
                }
            });
        }   
        public Task <string> GetCustomURL(string longURL)
        {
            if (!_customURLL2S.TryGetValue(longURL, out string? shortURL))
            {
                throw new KeyNotFoundException();
            }
            return Task.FromResult(shortURL);
        }
        public Task DeleteCustomURL(string longURL)
        {
            return Task.Run(() => {
                if (!_customURLL2S.TryRemove(longURL, out string ? shortURL))
                {
                    throw new KeyNotFoundException();
                }
            });
        }

    
    
    }
}