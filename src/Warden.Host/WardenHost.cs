using System.Collections.Generic;
using System.Threading.Tasks;
using Warden;

namespace Warden.Host
{
    public class WardenHost
    {
        private readonly IDictionary<string, IWarden> _wardens = new Dictionary<string, IWarden>();

        public void AddWarden(IWarden warden)
        {
            _wardens[warden.Name] = warden;
        }

        public void RemoveWarden(string name)
        {
            _wardens.Remove(name);
        }

        public async Task StartWardenAsync(string name)
        {
            await _wardens[name].StartAsync();
        }

        public async Task StopWardenAsync(string name)
        {
            await _wardens[name].StopAsync();
        }

        public async Task PauseWardenAsync(string name)
        {
            await _wardens[name].PauseAsync();
        }

        public async Task StartAllWardensAsync()
        {
            var tasks = new List<Task>();
            foreach(var warden in _wardens)
            {
                tasks.Add(warden.Value.StartAsync());
            }
            await Task.WhenAll(tasks.ToArray());
        }
    }
}