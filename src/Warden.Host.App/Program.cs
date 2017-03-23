using System;
using System.Net;
using System.Threading.Tasks;
using Warden.Core;
using Warden.Host;
using Warden.Watchers.Web;

namespace Warden.Host.App
{
    class Program
    {
        public static void Main(string[] args)
        {
            var wardenHost = new WardenHost();
            wardenHost.AddWarden(CreateWarden("Warden #1", 1));
            wardenHost.AddWarden(CreateWarden("Warden #2", 3));
            wardenHost.AddWarden(CreateWarden("Warden #3"));
            wardenHost.AddWarden(CreateWarden("Warden #4", 10));
            wardenHost.AddWarden(CreateWarden("Warden #5", 2));
            Task.WaitAll(wardenHost.StartAllWardensAsync());
        }

        private static IWarden CreateWarden(string name, int interval = 5)
        {
            var wardenConfiguration = WardenConfiguration
                .Create()
                .AddWebWatcher("http://httpstat.us/200", cfg =>
                {
                    cfg.EnsureThat(response => response.StatusCode == HttpStatusCode.Accepted);
                })  
                .SetGlobalWatcherHooks((hooks, integrations) =>
                {
                    Console.WriteLine($"Hello!");
                })
                .WithInterval(TimeSpan.FromSeconds(interval))
                .WithConsoleLogger()
                .Build();

            return WardenInstance.Create(name,wardenConfiguration);
        }
    }
}
