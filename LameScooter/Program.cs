using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text.Json;
using System.IO;
using Newtonsoft.Json;
    
namespace gp20_2021_0426_rest_gameserver_Sopuffer
{
    class Program
    {
        static async Task Main(string[] args)
        {
            OfflineLameScooterRental rental = new OfflineLameScooterRental();
            var count = await rental.GetScooterCountInStation("Linnanmäki");
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
               var file = await File.ReadAllTextAsync("Scooters.json", System.Text.Encoding.ASCII);

                var jsonObject = JsonConvert.DeserializeObject<LameScooterStationList>(file);

                foreach (var station in jsonObject.Stations)
                {
                    
                    if (station.name == stationName)
                    {
                         return station.bikesAvailable;
                    }
                   Console.WriteLine(station.name + " " + station.bikesAvailable);
                }
               return default;
            
        }

    public class LameScooterStationList
    {
        public List <LameScooterStation> Stations;
            
    }
    public class LameScooterStation
    {
        public string id { get; set; }
        public string name { get; set; }
        public float x { get; set; }
        public float y { get; set; }
        public int bikesAvailable { get; set; }
        public int spacesAvailable { get; set; }
        public int capacity { get; set; }
        public bool allowDropoff { get; set; }
        public bool allowOverloading { get; set; }
        public bool isFloatingBike { get; set; }
        public bool isCarStation { get; set; }
        public string state { get; set; }
        public string[] networks { get; set; }
        public bool realTimeData { get; set; }
    }
}
}
