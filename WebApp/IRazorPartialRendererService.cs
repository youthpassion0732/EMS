namespace WebApp;

public interface IRazorPartialRendererService
{
    Task<string> RenderPartialAsync<TModel>(string partialName, TModel model);
}
