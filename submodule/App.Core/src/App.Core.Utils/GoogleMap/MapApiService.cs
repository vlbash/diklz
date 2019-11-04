using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Utils.GoogleMap
{
    public class MapApiService
    {
        private readonly string _apiKey;
        public MapApiService(IConfiguration config) {
            _apiKey = config.GetSection("GoogleMapsKey").Value;
        }

        public async Task<List<Location>> GetRoutePoints(string start, string end)
        {
            string result = await GetDirection(start, end);
            JObject json = JObject.Parse(result);
            if (json["status"].ToString() != "OK")  
                return await Task.FromResult<List<Location>>(null);
            List<Location> list = new List<Location>();
            list = DecodePolylinePoints(json["routes"][0]["overview_polyline"]["points"].ToString());
            return list;
        }

        private async Task<string> GetDirection(string start, string end) {
            Console.WriteLine("maps api");
            string page = $"https://maps.googleapis.com/maps/api/directions/json?origin={start}&destination={end}5&key=AIzaSyAxoH5KUN4JWHAqTMtSneU4okgyuqeX78E";
            using (HttpClient client = new HttpClient())
            using (HttpResponseMessage response = await client.GetAsync(page))
            {
                return await response.Content.ReadAsStringAsync();
            }
        }


        private List<Location> DecodePolylinePoints(string encodedPoints)
        {
            if (encodedPoints == null || encodedPoints == "") return null;
            List<Location> poly = new List<Location>();
            char[] polylinechars = encodedPoints.ToCharArray();
            int index = 0;

            int currentLat = 0;
            int currentLng = 0;
            int next5bits;
            int sum;
            int shifter;

            try
            {
                while (index < polylinechars.Length)
                {
                    // calculate next latitude
                    sum = 0;
                    shifter = 0;
                    do
                    {
                        next5bits = (int)polylinechars[index++] - 63;
                        sum |= (next5bits & 31) << shifter;
                        shifter += 5;
                    } while (next5bits >= 32 && index < polylinechars.Length);

                    if (index >= polylinechars.Length)
                        break;

                    currentLat += (sum & 1) == 1 ? ~(sum >> 1) : (sum >> 1);

                    //calculate next longitude
                    sum = 0;
                    shifter = 0;
                    do
                    {
                        next5bits = (int)polylinechars[index++] - 63;
                        sum |= (next5bits & 31) << shifter;
                        shifter += 5;
                    } while (next5bits >= 32 && index < polylinechars.Length);

                    if (index >= polylinechars.Length && next5bits >= 32)
                        break;

                    currentLng += (sum & 1) == 1 ? ~(sum >> 1) : (sum >> 1);
                    Location p = new Location();
                    p.Lat = Convert.ToDouble(currentLat) / 100000.0;
                    p.Lng = Convert.ToDouble(currentLng) / 100000.0;
                    poly.Add(p);
                }
            }
            catch
            {
                // logo it
            }
            return poly;
        }

    }
    public class  Location {
        public double Lat { get; set; }
        public double Lng { get; set; }
    }
}
