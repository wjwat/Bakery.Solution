using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;

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
        // Pull our UserId out first to use a basis for sorting the values in
        // our tables.
        var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        var HomeModel = new UserViewModel {
          Flavors = _db.Flavors.ToList()
              .OrderByDescending(f => f.User.Id == userId)
              .ToList(),
          Treats = _db.Treats
              .OrderByDescending(t => t.User.Id == userId)
              .ToList(),
          UserId = userId
        };

        return View(HomeModel);
      }

    }
}
