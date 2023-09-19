using DTO;

namespace WebApp.RequestService
{
    public interface IRequestService
    {
        Task<TRes> GetAsync<TRes>(string endpoint);
        Task<ApiReponseDto> PutAsync(string endpoint, object body);
        Task<ApiReponseDto> PostAsync(string endpoint, object body);
        Task<ApiReponseDto> DeleteAsync(string endpoint);
    }
}
