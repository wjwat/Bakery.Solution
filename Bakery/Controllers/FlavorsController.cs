using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

using Bakery.Models;

namespace Bakery.Controllers
{
  public class FlavorsController : Controller
  {

    private readonly BakeryContext _db;

    public FlavorsController(BakeryContext db)
    {
      _db = db;
    }

    public ActionResult Index()
    {
      List<Flavor> flavors = _db.Flavors.ToList();
      return View(flavors);
    }

    public ActionResult Create()
    {
      ViewBag.Treats = _db.Treats.ToList();
      return View();
    }

    [HttpPost]
    public ActionResult Create(Flavor flavor, int[] TreatId)
    {
      // Save before adding treats or C# will throw an error because
      // it doesn't see the PK for our new Flavor
      _db.Flavors.Add(flavor);
      _db.SaveChanges();

      foreach (int m in TreatId)
      {
        _db.FlavorTreat.Add(new FlavorTreat() {
            FlavorId = flavor.FlavorId,
            TreatId = m
        });
      }
      _db.SaveChanges();

      return RedirectToAction("Index");
    }

    public ActionResult Details(int id)
    {
      var flavor = _db.Flavors
          .FirstOrDefault(e => e.FlavorId == id);
      return View(flavor);
    }

    public ActionResult Edit(int id)
    {
      var flavor = _db.Flavors.FirstOrDefault(e => e.FlavorId == id);
      ViewBag.Treats = _db.Treats.ToList();
      ViewBag.AuthorizedTreats = _db.FlavorTreat
          .Where(e => e.FlavorId == id)
          .Select(m => m.TreatId)
          .ToList();

      return View(flavor);
    }

    // Delete every relationship that is not passed back on submit, and
    // create any that are passed back that do not already exist.
    //
    // The values that are passed back in TreatId[] represent the total
    // state of relationships between our two entities.
    [HttpPost]
    public ActionResult Edit(Flavor flavor, int[] TreatId)
    {
      // Remove relationships not present in TreatId[]
      _db.FlavorTreat
          .Where(e => e.FlavorId == flavor.FlavorId
                    && !TreatId.Contains(e.TreatId))
          .ToList()
          .ForEach(row => _db.FlavorTreat.Remove(row));

      // Add relationships present in TreatId
      // Can this be done in a LINQ query?
      foreach (int m in TreatId)
      {
        // I'd prefer to attempt to Add the new key without the conditional
        // but in order to catch the exception I would have to pull in other
        // packages (which I'm unwilling to do), or have an overly broad
        // catch (which I am also unwilling to do).
        if (_db.FlavorTreat.Any(em => em.FlavorId == flavor.FlavorId && em.TreatId == m))
        {
          continue;
        }

        _db.FlavorTreat.Add(new FlavorTreat() {
            FlavorId = flavor.FlavorId,
            TreatId = m
        });
      }

      _db.Entry(flavor).State = EntityState.Modified;
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

    public ActionResult Delete(int id)
    {
      var flavor = _db.Flavors
          .FirstOrDefault(e => e.FlavorId == id);
      return View(flavor);
    }

    [HttpPost, ActionName("Delete")]
    public ActionResult DeleteConfirmed(int id)
    {
      var flavor = _db.Flavors.FirstOrDefault(e => e.FlavorId == id);
      _db.Flavors.Remove(flavor);
      _db.SaveChanges();
      return RedirectToAction("Index");
    }

  }
}
