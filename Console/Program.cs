using Microsoft.Extensions.DependencyInjection;
using System;
using ShortURLService.Domain;
using ShortURLService.Infrastucture;

namespace ConsoleAppWithDI
{
    class Program
    {
        static void Main(string[] args)
        {
            // Set up dependency injection
            var serviceProvider = new ServiceCollection()
                .AddSingleton<IUrlShortenerService, UrlShortenerService>()
                .AddSingleton<IShortURLStorage, ShortURLStorage>()
                .AddSingleton<IInvokeService, CommandLineService>()
                .BuildServiceProvider();

            // Get the service and run it
            var exampleService = serviceProvider.GetService<IInvokeService>();
            exampleService?.Run();
        }
    }
}