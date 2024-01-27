using Microsoft.AspNetCore.Mvc;

namespace NHExamples.EndPoint2.Controllers;

public class HomeController : Controller
{
    public HomeController()
    {

    }

    public IActionResult Index()
    {
        return Ok();
    }
}