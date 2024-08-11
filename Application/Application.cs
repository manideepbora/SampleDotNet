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
            Console.WriteLine("<===== Welcome to URL Shortener Service =====>");
            Console.WriteLine("Add a new long URL               (A):");
            Console.WriteLine("Delete a new long URL            (D):");
            Console.WriteLine("Get a long URL                   (R):");
            Console.WriteLine("Get statistics of the short URL  (S):");
            Console.WriteLine("Please Enter your choice:            ");

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
                    GetUrl().GetAwaiter().GetResult();
                    break;
                case "s":
                    GetStatistics().GetAwaiter().GetResult();
                    break;
                default:
                    Console.WriteLine("Invalid option");
                    break;
            }
            Console.WriteLine("===========================================");
            Console.WriteLine("Do you want to continue? (y/n)");

        } while (Console.ReadLine()?.ToLower() == "y");
    }

    private async Task AddUrl()
    {

        Console.WriteLine("Enter the long URL:");
        var longUrl = Console.ReadLine();
        if (longUrl != null)
        {
            Console.WriteLine("Please provide the custom short url (optional):");
            var customUrl = Console.ReadLine();
            if(! string.IsNullOrEmpty(customUrl))
            {
                try{
                    string shortUrl = await _UrlSvc.ShortenUrl(longUrl, customUrl);
                    Console.WriteLine($"Short URL: {shortUrl}");
                }
                catch (Exception )
                {
                    Console.WriteLine("Short URL already exists.");
                    return;
                }
            }
            else
            {
                try{
                    string shortUrl = await _UrlSvc.ShortenUrl(longUrl);
                    Console.WriteLine($"Short URL: {shortUrl}");
                }   
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return;
                }
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
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return;
        }
    }   
    private async Task GetUrl()
    {
        Console.WriteLine("Enter the short URL:");
        var shortUrlToGet = Console.ReadLine();
        if(shortUrlToGet == null)
        {
            Console.WriteLine("Invalid URL");
            return;
        }
        try {
            var longUrl = await _UrlSvc.ExpandUrl(shortUrlToGet);
            Console.WriteLine($"Long URL: {longUrl}");
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return;
        }
    }
    private async Task GetStatistics()
    {
        Console.WriteLine("Enter the short URL:");
        var shortUrlToGetStats = Console.ReadLine();
        if(shortUrlToGetStats == null)
        {
            Console.WriteLine("Invalid URL");
            return;
        }
        try {
            var stats = await _UrlSvc.GetStatistics(shortUrlToGetStats);
            Console.WriteLine($"Short URL: {shortUrlToGetStats}");
            Console.WriteLine($"Number of times accessed: {stats.NumberOfTimesAccessed}");
            Console.WriteLine($"Last accessed on: {stats.LastAccessedOn}");
        }

        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            return;
        }
    }
}