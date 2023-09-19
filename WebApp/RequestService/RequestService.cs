using DTO;

namespace WebApp.RequestService;

public class RequestService : IRequestService
{
    public async Task<TRes> GetAsync<TRes>(string endpoint)
    {
        try
        {
            var httpClient = new HttpClient();
            using var response = await httpClient.GetAsync(endpoint);
            return await response.Content.ReadFromJsonAsync<TRes>();
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<ApiReponseDto> PutAsync(string endpoint, object body)
    {
        try
        {
            var httpClient = new HttpClient();
            var httpContent = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");
            using var response = await httpClient.PutAsync(endpoint, httpContent);
            return new ApiReponseDto
            {
                Content = await response.Content.ReadAsStringAsync(),
                IsSuccessStatusCode = response.IsSuccessStatusCode
            };
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<ApiReponseDto> PostAsync(string endpoint, object body)
    {
        try
        {
            var httpClient = new HttpClient();
            var httpContent = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");
            using var response = await httpClient.PostAsync(endpoint, httpContent);
            return new ApiReponseDto
            {
                Content = await response.Content.ReadAsStringAsync(),
                IsSuccessStatusCode = response.IsSuccessStatusCode
            };
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<ApiReponseDto> DeleteAsync(string endpoint)
    {
        try
        {
            var httpClient = new HttpClient();
            using var response = await httpClient.DeleteAsync(endpoint);
            return new ApiReponseDto
            {
                Content = await response.Content.ReadAsStringAsync(),
                IsSuccessStatusCode = response.IsSuccessStatusCode
            };
        }
        catch (Exception)
        {
            throw;
        }
    }
}
