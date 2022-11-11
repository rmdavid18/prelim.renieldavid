using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using RestSharp;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using static RenielDavid.PrelimExam.Pages.IndexModel;

namespace RenielDavid.PrelimExam.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly object Restsharp;
        private object ViewModel;
        private WeatherData? viewModel;

        public string Message { get; private set; }

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public IndexModel(WeatherData? viewModel) => this.SetViewModel(viewModel);

        public WeatherData? GetViewModel()
        {
            return viewModel;
        }

        public void SetViewModel(WeatherData? value)
        {
            viewModel = value;
        }

        public Task<IActionResult> OnGet(
            IActionResult page, string? Weather, List<WeatherDetails> weatherData)


        {
            if (page is null)
            {
                throw new ArgumentNullException(nameof(page));
            }

            this.SetViewModel(new WeatherData
            {
                Weather = weatherData
            }
           
            ) ;
      
            {
                var client = new RestClient("https://api.darksky.net/forecast/64ee9d4e589bb2cb3788596fd477b0f7/14.8781,120.4546");

                var request = new RestRequest("", Method.Get);

                RestResponse response = client.Execute(request);

                var content = response.Content;

                var area = JsonConvert.DeserializeObject<WeatherData>(content);
            }
      
        }
        public void OnPostView(int id)
        {
            Message = $"View handler fired for {id}";
        }

        public class WeatherData
        {
            public WeatherMain? Main { get; set; }

            public List<WeatherDetails>? Weather { get; set; }
        }
        public class WeatherMain
        {
            public string? Temp { get; set; }

            [JsonPropertyName("feels-like")]
            public string? FeelsLike { get; set; }
            public string? Pressure { get; set; }
            public string? Humidity { get; set; }
        }

        public class WeatherDetails
        {
            public string? Temp { get; set; }
            public string? Icon { get; set; }

            public string? Description { get; set; }
        }

    }

}
