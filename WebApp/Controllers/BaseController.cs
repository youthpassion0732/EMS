namespace WebApp.Controllers;

public class BaseController : Controller
{
    public IActionResult AddConfirmDelete(string method)
    {
        try
        {
            ViewBag.DeleteMethod = method;
            return PartialView("~/Views/Shared/_DeleteConfirmation.cshtml");
        }
        catch (Exception ex)
        {
            return HandleError(ex);
        }
    }

    protected JsonResult HandleError(Exception ex)
    {
        return Json(new { success = false, error = ex.Message });
    }

    protected PaginationViewModel SetPagination(int pageNo, int numberOfPages, string functionName)
    {
        try
        {
            PaginationViewModel pagination = new PaginationViewModel
            {
                CurrentPage = pageNo,
                TotalPages = numberOfPages
            };

            if (numberOfPages > 1)
            {
                if (pageNo < numberOfPages)
                    pagination.NextPageUrl = $"{functionName}({pageNo + 1})";

                if (pageNo > 1)
                    pagination.PreviousPageUrl = $"{functionName}({pageNo - 1})";
            }

            if ((pageNo) <= numberOfPages)
            {
                List<string> ranges = PaginationFun(pageNo, numberOfPages);
                PageUrl page = null;
                foreach (var i in ranges)
                {
                    if (i != "...")
                    {
                        page = new PageUrl
                        {
                            LinkValue = i,
                            Url = $"{functionName}({Convert.ToInt32(i)})"
                        };
                    }
                    else
                    {
                        page = new PageUrl
                        {
                            LinkValue = i,
                            Url = ""
                        };
                    }
                    pagination.Pages.Add(page);
                }
            }

            return pagination;
        }
        catch (Exception)
        {
            throw;
        }
    }

    private List<string> PaginationFun(int pageNo, int numberOfPages)
    {
        int left = pageNo - 4;
        int right = pageNo + 4;
        List<int> pageNumbers = new List<int>();
        List<string> rangeWithDots = new List<string>();
        int number = 0;

        for (int i = 1; i <= numberOfPages; i++)
        {
            if (i == 1 || i == numberOfPages || (i >= left && i < right))
            {
                pageNumbers.Add(i);
            }
        }

        foreach (int pageNumber in pageNumbers)
        {
            if (number > 0)
            {
                if (pageNumber - number == 2)
                {
                    rangeWithDots.Add((number + 1).ToString());
                }
                else if ((pageNumber - number) != 1)
                {
                    rangeWithDots.Add("...");
                }
            }
            rangeWithDots.Add(pageNumber.ToString());
            number = pageNumber;
        }

        return rangeWithDots;
    }
}