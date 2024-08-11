using ShortURLService.Domain;
public class CommandLineService : IInvokeService
{
    public CommandLineService(IUrlShortenerService _urlSvc)
    {
        _UrlSvc = _urlSvc;
    }

    private  IUrlShortenerService _UrlSvc;
    public void Run()
    {
        do 
        {
            Console.WriteLine("Add a new long URL (A):");
            Console.WriteLine("Delete a new long URL (D):");
            Console.WriteLine("Get a long URL (R):");
            Console.WriteLine("Get statistics of the short URL (S):");
            var response = Console.ReadLine();
            switch (response.ToLower())
            {
                case "a":
                    AddUrl();   
                    break;
                case "d":
                    DeleteUrl();
                    break;
                case "r":
                    GetUrl();
                    break;
                case "s":
                    GetStatistics();
                    break;
                default:
                    Console.WriteLine("Invalid option");
                    break;
            }

            Console.WriteLine("Do you want to continue? (y/n)");
        } while (Console.ReadLine().ToLower() == "y");
    }

    private void AddUrl()
    {
        try 
        {
            Console.WriteLine("Enter the long URL:");
            var longUrl = Console.ReadLine();
            string shortUrl = _UrlSvc.ShortenUrl(longUrl);
            Console.WriteLine($"Short URL: {shortUrl}");
        }
        catch (System.InvalidOperationException ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
    private void DeleteUrl()
    {
        Console.WriteLine("Enter the short URL:");
        var shortUrlToDelete = Console.ReadLine();
        _UrlSvc.DeleteUrl(shortUrlToDelete);
        Console.WriteLine("URL deleted");
    }   
    private void GetUrl()
    {
        Console.WriteLine("Enter the short URL:");
        var shortUrlToGet = Console.ReadLine();
        string longUrl = _UrlSvc.ExpandUrl(shortUrlToGet);
        Console.WriteLine($"Long URL: {longUrl}");
    }
    private void GetStatistics()
    {
        Console.WriteLine("Enter the short URL:");
        var shortUrlToGetStats = Console.ReadLine();
        var stats = _UrlSvc.GetStatistics(shortUrlToGetStats);
        Console.WriteLine($"Short URL: {shortUrlToGetStats}");
        Console.WriteLine($"Number of times accessed: {stats.NumberOfTimesAccessed}");
        Console.WriteLine($"Last accessed on: {stats.LastAccessedOn}");
    }
}