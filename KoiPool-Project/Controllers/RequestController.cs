using Microsoft.AspNetCore.Mvc;
using KoiPool_Project.Models; // Namespace thực tế của bạn
using System.Threading.Tasks;

public class RequestController : Controller
{
    private readonly DataContext _context;

    public RequestController(DataContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> SubmitRequest(RequestModel model)
    {
        if (ModelState.IsValid)
        {
            _context.Requests.Add(model);
            await _context.SaveChangesAsync();
            return RedirectToAction("Success"); // Redirect after successful save
        }

        return View("~/Views/Home/LienHe.cshtml", model); // Specify the exact path to avoid not found error
    }
}
