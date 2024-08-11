using System.Reflection.Metadata;

namespace ShortURLService.Domain
{
    public interface IShortURLStorage
    {
        Task<ShortURLObject> GetShortURLByIdAsync(string id);
        Task AddShortURLAsync(string id, ShortURLObject shortURL);
        
        Task UpdateShortURLAsync(string id, ShortURLObject shortURL);
        
        Task DeleteShortURLAsync(string id);
        Task AddCustomURL(string longURL, string shortURL);
        Task <string> GetCustomURL(string longURL);
        Task DeleteCustomURL(string longURL);
    }
}