namespace ShortURLService.Domain
{
    public interface IShortURLStorage
    {
        Task<ShortURLObject> GetShortURLByIdAsync(string id);
        Task AddShortURLAsync(ShortURLObject shortURL);
        
        Task UpdateShortURLAsync(ShortURLObject shortURL);
        
        Task DeleteShortURLAsync(string id);
    }
}