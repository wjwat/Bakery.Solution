using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

using Bakery.Models;
using Bakery.ViewModels;

namespace Bakery.Controllers
{
  [Authorize]
  public class FlavorsController : Controller
  {
    private readonly BakeryContext _db;
    private readonly UserManager<ApplicationUser> _userManager;

    public FlavorsController(UserManager<ApplicationUser> userManager, BakeryContext db)
    {
      _userManager = userManager;
      _db = db;
    }

    [AllowAnonymous]
    public ActionResult Index()
    {
      var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

      var Model = new UserViewModel {
        Flavors = _db.Flavors.ToList()
            .OrderByDescending(f => f.User.Id == userId)
            .ToList(),
        Treats = _db.Treats
            .OrderByDescending(t => t.User.Id == userId)
            .ToList(),
        UserId = userId
      };

      return View(Model);
    }

    public ActionResult Create()
    {
      ViewBag.Treats = _db.Treats.ToList();
      return View();
    }

    [HttpPost]
    public async Task<ActionResult> Create(Flavor flavor, int[] TreatId)
    {
      var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      var currentUser = await _userManager.FindByIdAsync(userId);
      flavor.User = currentUser;

      _db.Flavors.Add(flavor);
      _db.SaveChanges();

      foreach (int t in TreatId)
      {
        _db.FlavorTreat.Add(new FlavorTreat() {
          TreatId = t,
          FlavorId = flavor.FlavorId
        });
      }
      _db.SaveChanges();

      return RedirectToAction("Index");
    }

    [AllowAnonymous]
    public ActionResult Details(int id)
    {
      var thisFlavor = _db.Flavors
          .FirstOrDefault(flavor => flavor.FlavorId == id);

      if (thisFlavor == null)
      {
        RedirectToAction("Index");
      }

      return View(thisFlavor);
    }

    public ActionResult Edit(int id)
    {
      var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      var thisFlavor = _db.Flavors.FirstOrDefault(f => f.FlavorId == id);

      if (thisFlavor == null || thisFlavor.User.Id != userId)
      {
        return RedirectToAction("Index");
      }

      ViewBag.Treats = _db.Treats.ToList();
      ViewBag.ExistingTreats = _db.FlavorTreat
          .Where(f => f.FlavorId == id)
          .Select(t => t.TreatId)
          .ToList();

      return View(thisFlavor);
    }

    [HttpPost]
    public ActionResult Edit(Flavor flavor, int[] TreatId)
    {
      _db.FlavorTreat
          .Where(f => f.FlavorId == flavor.FlavorId
              && !TreatId.Contains(f.TreatId))
          .ToList()
          .ForEach(row => _db.FlavorTreat.Remove(row));

      foreach (int t in TreatId)
      {
        if (_db.FlavorTreat.Any(ft => ft.FlavorId == flavor.FlavorId && ft.TreatId == t))
        {
          continue;
        }

        _db.FlavorTreat.Add(new FlavorTreat() {
          TreatId = t,
          FlavorId = flavor.FlavorId
        });
      }


      _db.Entry(flavor).State = EntityState.Modified;
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult Delete(int id)
    {
      var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      var thisFlavor = _db.Flavors.FirstOrDefault(f => f.FlavorId == id);

      if (thisFlavor == null || thisFlavor.User.Id != userId)
      {
        return RedirectToAction("Index");
      }

      return View(thisFlavor);
    }

    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(int id)
    {
      var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      var thisFlavor = _db.Flavors.FirstOrDefault(f => f.FlavorId == id);

      if (thisFlavor == null || thisFlavor.User.Id != userId)
      {
        return RedirectToAction("Index");
      }

      _db.Flavors.Remove(thisFlavor);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

  }
}
