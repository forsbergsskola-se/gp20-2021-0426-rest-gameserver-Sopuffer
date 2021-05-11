using System;
using System.Text.Json;
using System.Net.Http;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
namespace gp20_2021_0426_rest_gameserver_Sopuffer
{
    class Program
    {
        static async Task Main(string[] args)
        {
            ILameScooterRental rental = null;

            var count = await rental.GetScooterCountInStation(null);
            Console.WriteLine("Number of Scooters Available at this Station: ");
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
            var jsonObject = JsonConvert.DeserializeObject<LameScooterStationList>(stationName);
            return jsonObject.bikesAvailable;
        }

    }

    public class LameScooterStationList
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
