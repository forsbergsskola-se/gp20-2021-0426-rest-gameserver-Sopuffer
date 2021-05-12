using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
    
namespace gp20_2021_0426_rest_gameserver_Sopuffer
{
    class Program
    {
        static async Task Main(string[] args)
        {
            OfflineLameScooterRental rental = new OfflineLameScooterRental();
            var count = await rental.GetScooterCountInStation("Orionintie");
            Console.WriteLine("Number of Scooters Available at this Station: " + count);
        }
    }

    public interface ILameScooterRental
    {
        Task<int> GetScooterCountInStation(string stationName);
    }

    public class OfflineLameScooterRental : ILameScooterRental
    {
        public async Task<int> GetScooterCountInStation(string stationName)
        {
               var file = await File.ReadAllTextAsync("Scooters.json");

                var jsonObject = JsonConvert.DeserializeObject<LameScooterStationList>(file);

                foreach (var station in jsonObject.Stations)
                {
                    if (station.name == stationName)
                    {
                         return station.bikesAvailable;
                    }
                }
               return default;
            
        }

    public class LameScooterStationList
    {
        public List <LameScooterStation> Stations;
            
    }
    public class LameScooterStation
    {
        public string name { get; set; }
        public int bikesAvailable { get; set; }
     
    }
}
}
