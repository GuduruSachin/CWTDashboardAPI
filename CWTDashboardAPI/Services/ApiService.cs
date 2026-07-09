using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

public class ApiService
{
    private readonly HttpClient _httpClient;

    public ApiService()
    {
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri("https://gpsc.cwtwebpem.com/") // Replace with your API base URL
        };

        _httpClient.DefaultRequestHeaders.Add("apiKey", "dyK1DEQBmCUMnSLtmR4vULpcOlJR4ohSmbJlzWRbc0I"); // Replace with your API key
        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }

    public async Task<T> PostAsync<T>(string endpoint, object body)
    {
        var json = JsonConvert.SerializeObject(body);
        //var content = new StringContent(json);
        //content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        
        var response = await _httpClient.PostAsync(endpoint, content);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"API Error: {response.StatusCode} - {error}");
        }

        var responseJson = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<T>(responseJson);
    }
}