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
  public class TreatsController : Controller
  {
    private readonly BakeryContext _db;
    private readonly UserManager<ApplicationUser> _userManager;

    public TreatsController(UserManager<ApplicationUser> userManager, BakeryContext db)
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
      ViewBag.Flavors = _db.Flavors.ToList();
      return View();
    }

    [HttpPost]
    public async Task<ActionResult> Create(Treat treat, int[] FlavorId)
    {
      var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      var currentUser = await _userManager.FindByIdAsync(userId);
      treat.User = currentUser;

      _db.Treats.Add(treat);
      _db.SaveChanges();

      foreach (int f in FlavorId)
      {
        _db.FlavorTreat.Add(new FlavorTreat() {
          TreatId = treat.TreatId,
          FlavorId = f
        });
      }
      _db.SaveChanges();

      return RedirectToAction("Index");
    }

    [AllowAnonymous]
    public ActionResult Details(int id)
    {
      var thisTreat = _db.Treats
          .FirstOrDefault(treat => treat.TreatId == id);

      if (thisTreat == null)
      {
        RedirectToAction("Index");
      }

      return View(thisTreat);
    }

    public ActionResult Edit(int id)
    {
      var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      var thisTreat = _db.Treats.FirstOrDefault(treat => treat.TreatId == id);

      if (thisTreat == null || thisTreat.User.Id != userId)
      {
        return RedirectToAction("Index");
      }

      ViewBag.Flavors = _db.Flavors.ToList();
      ViewBag.ExistingFlavors = _db.FlavorTreat
          .Where(t => t.TreatId == id)
          .Select(f => f.FlavorId)
          .ToList();

      return View(thisTreat);
    }

    [HttpPost]
    public ActionResult Edit(Treat treat, int[] FlavorId)
    {
      // How would I validate that a given edit Post is authorized by the user
      // making the edit?

      _db.FlavorTreat
          .Where(t => t.TreatId == treat.TreatId
              && !FlavorId.Contains(t.FlavorId))
          .ToList()
          .ForEach(row => _db.FlavorTreat.Remove(row));

      foreach (int f in FlavorId)
      {
        if (_db.FlavorTreat.Any(ft => ft.TreatId == treat.TreatId && ft.FlavorId == f))
        {
          continue;
        }

        _db.FlavorTreat.Add(new FlavorTreat() {
          TreatId = treat.TreatId,
          FlavorId = f
        });
      }


      _db.Entry(treat).State = EntityState.Modified;
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult Delete(int id)
    {
      var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      var thisTreat = _db.Treats.FirstOrDefault(treat => treat.TreatId == id);

      if (thisTreat == null || thisTreat.User.Id != userId)
      {
        return RedirectToAction("Index");
      }

      return View(thisTreat);
    }

    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(int id)
    {
      var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
      var thisTreat = _db.Treats.FirstOrDefault(treat => treat.TreatId == id);

      if (thisTreat == null || thisTreat.User.Id != userId)
      {
        return RedirectToAction("Index");
      }

      _db.Treats.Remove(thisTreat);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

  }
}
