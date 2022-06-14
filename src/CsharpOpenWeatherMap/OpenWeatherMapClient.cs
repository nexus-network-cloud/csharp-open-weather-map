using CsharpOpenWeatherMap;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NexusNetworkCloud.CsharpOpenWeatherMap
{
    public class OpenWeatherMapClient
    {
        private static string address = "https://api.openweathermap.org/data/2.5";
        private readonly HttpClient client = new HttpClient();

        private readonly string _appId;

        public OpenWeatherMapClient(string appId)
        {
            _appId = appId;
        }

        ~OpenWeatherMapClient()
        {
            client.Dispose();
        }

        public async Task<WeatherReportResponse> GetCurrentWeatherByZipCodeAsync(int zip)
        {
            if (Math.Floor(Math.Log10(zip) + 1) != 5)
                throw new ArgumentException("Invalid ZIP Format Detected, Please Ensure That The Entered ZIP Code Is 5 Digits Long");

            HttpResponseMessage res = await client.GetAsync($"{address}/weather?zip={zip}&units=imperial&appid={_appId}");

            if (!res.IsSuccessStatusCode)
                return new WeatherReportResponse { RequestSuccessful = false, RequestError = await res.Content.ReadAsStringAsync() };
            else
                return new WeatherReportResponse { RequestSuccessful = true, WeatherReport = JObject.Parse(await res.Content.ReadAsStringAsync()).ToObject<WeatherReport>() };
        }
    }
}
