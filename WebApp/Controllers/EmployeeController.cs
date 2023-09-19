using DTO;
using Newtonsoft.Json;

namespace WebApp.Controllers;

public class EmployeeController : BaseController
{
    private readonly Endpoints _endpoints;
    private readonly IRequestService _requestService;
    private readonly IRazorPartialRendererService _partialRendererService;
    private readonly ILogger<EmployeeController> _logger;

    public EmployeeController(IRequestService requestService, IOptions<Endpoints> endpoints, IRazorPartialRendererService partialRendererService, ILogger<EmployeeController> logger)
    {
        _requestService = requestService;
        _partialRendererService = partialRendererService;
        _endpoints = endpoints.Value;
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Pagination(PaginationRequestDto requestDto)
    {
        try
        {
            var apiReponseDto = await _requestService.PostAsync(_endpoints.AllRecords(), requestDto);
            var result = JsonConvert.DeserializeObject<PaginationResponseDto<EmployeeViewModel>>(apiReponseDto.Content);
            string html = await _partialRendererService.RenderPartialAsync("~/Views/Shared/_EmployeeTableRow.cshtml", result);
            string paginationHtml = string.Empty;
            if (result.NoOfPages > 1)
            {
                PaginationViewModel paginationModel = SetPagination(requestDto.PageNo, result.NoOfPages, "GetEmployeesList");
                paginationHtml = await _partialRendererService.RenderPartialAsync("_Pagination", paginationModel);
            }
            return Json(new { success = true, html, paginationHtml, totalRecords = result.TotalRecords, startValuesCount = result.StartCount, endValuesCount = result.EndCount });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return HandleError(ex);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Save(EmployeeViewModel model)
    {
        try
        {
            var apiReponseDto = new ApiReponseDto();
            var employee = new EmployeeViewModel();
            if (model.Id > 0)
                apiReponseDto = await _requestService.PutAsync(_endpoints.BaseUrl + _endpoints.Update, model);
            else
                apiReponseDto = await _requestService.PostAsync(_endpoints.BaseUrl + _endpoints.Add, model);

            if (apiReponseDto.IsSuccessStatusCode)
            {
                employee = JsonConvert.DeserializeObject<EmployeeViewModel>(apiReponseDto.Content);
                var response = new PaginationResponseDto<EmployeeViewModel>() { Data = new List<EmployeeViewModel> { employee } };
                string html = await _partialRendererService.RenderPartialAsync("~/Views/Shared/_EmployeeTableRow.cshtml", response);
                return Json(new { html, success = true });
            }
            else
            {
                return Json(new { success = false, error = apiReponseDto.Content });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return HandleError(ex);
        }
    }

    [HttpGet]
    public async Task<IActionResult> Add()
    {
        try
        {
            string html = await _partialRendererService.RenderPartialAsync("~/Views/Employee/_save.cshtml", new EmployeeViewModel());
            return Json(new { html, success = true });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return HandleError(ex);
        }
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        try
        {
            var response = await _requestService.GetAsync<EmployeeViewModel>(_endpoints.BaseUrl + _endpoints.GetById + "/" + id);
            string html = await _partialRendererService.RenderPartialAsync("~/Views/Employee/_save.cshtml", response);
            return Json(new { html, success = true });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return HandleError(ex);
        }
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteAsync([FromQuery] int id)
    {
        try
        {
            var response = await _requestService.DeleteAsync(_endpoints.BaseUrl + _endpoints.Delete + "/" + id);
            return Json(new { success = response.IsSuccessStatusCode, error = response.Content });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return HandleError(ex);
        }
    }
}