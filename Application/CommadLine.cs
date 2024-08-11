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
            switch (response?.ToLower())
            {
                case "a":
                    AddUrl().GetAwaiter().GetResult();   
                    break;
                case "d":
                    DeleteUrl().GetAwaiter()
                        .GetResult();
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
        } while (Console.ReadLine()?.ToLower() == "y");
    }

    private async Task AddUrl()
    {

        Console.WriteLine("Enter the long URL:");
        var longUrl = Console.ReadLine();
        if (longUrl != null)
        {
            try{
                string shortUrl = await _UrlSvc.ShortenUrl(longUrl);
                Console.WriteLine($"Short URL: {shortUrl}");
            }   
            catch (UriFormatException)
            {
                Console.WriteLine("Invalid URL");
                return;
            }
        }
        else
        {
            Console.WriteLine("Invalid URL");
        }
    }
    private async Task DeleteUrl()
    {
        Console.WriteLine("Enter the short URL:");
        var shortUrlToDelete = Console.ReadLine();
        if(shortUrlToDelete == null)
        {
            Console.WriteLine("Invalid URL");
            return;
        }
        try {
            await _UrlSvc.DeleteUrl(shortUrlToDelete);
        }
        catch (System.InvalidOperationException)
        {
            Console.WriteLine("Short URL does not exist");
            return;
        }
    }   
    private async void GetUrl()
    {
        Console.WriteLine("Enter the short URL:");
        var shortUrlToGet = Console.ReadLine();
        if(shortUrlToGet == null)
        {
            Console.WriteLine("Invalid URL");
            return;
        }
        try {
            var longUrl = _UrlSvc.ExpandUrl(shortUrlToGet).GetAwaiter().GetResult();
            Console.WriteLine($"Long URL: {longUrl}");
        }
        catch (System.InvalidOperationException)
        {
            Console.WriteLine("Short URL does not exist");
            return;
        }
    }
    private void GetStatistics()
    {
        Console.WriteLine("Enter the short URL:");
        var shortUrlToGetStats = Console.ReadLine();
        if(shortUrlToGetStats == null)
        {
            Console.WriteLine("Invalid URL");
            return;
        }
        try {
            var stats = _UrlSvc.GetStatistics(shortUrlToGetStats).GetAwaiter().GetResult();
            Console.WriteLine($"Short URL: {shortUrlToGetStats}");
            Console.WriteLine($"Number of times accessed: {stats.NumberOfTimesAccessed}");
            Console.WriteLine($"Last accessed on: {stats.LastAccessedOn}");
        }
        catch (System.InvalidOperationException)
        {
            Console.WriteLine("Short URL does not exist");
            return;
        }
    }
}