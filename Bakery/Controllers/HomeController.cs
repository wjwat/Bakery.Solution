using Microsoft.AspNetCore.Mvc;
using System.Linq;

using Bakery.Models;
using Bakery.ViewModels;

namespace Bakery.Controllers
{
    public class HomeController : Controller
    {

      private readonly BakeryContext _db;

      public HomeController(BakeryContext db)
      {
        _db = db;
      }


      public ActionResult Index()
      {
        var HomeModel = new HomeViewModel {
          Flavors = _db.Flavors.ToList(),
          Treats = _db.Treats.ToList()
        };
        return View(HomeModel);
      }

    }
}
