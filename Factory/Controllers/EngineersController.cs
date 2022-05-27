using Microsoft.AspNetCore.Mvc;
using Factory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Factory.Controllers
{
    public class EngineersController : Controller
    {
      private readonly FactoryContext _db;

      public EngineersController(FactoryContext db)
      {
        _db = db;
      }

      public ActionResult Index()
      {
        return View(_db.Engineers.ToList());
      }

      public ActionResult Create()
      {
        return View();
      }
      [HttpPost]
      public ActionResult Create(Engineer engineer)
      {
        _db.Engineers.Add(engineer);
        _db.SaveChanges();
        return RedirectToAction("Index");
      }

      public ActionResult Details(int id)
      {
        var thisEngineer = _db.Engineers
            .Include(engineer => engineer.JoinEntities)
            .ThenInclude(join => join.Machine)
            .FirstOrDefault(engineer => engineer.EngineerId == id);
        return View(thisEngineer);
      }
      public ActionResult AddMachine(int id)
      {
        var thisEngineer = _db.Engineers.FirstOrDefault(engineer => engineer.EngineerId == id);
        ViewBag.MachineId = new SelectList(_db.Machines, "MachineId", "Name");
        return View(thisEngineer);
      }
      [HttpPost]
      public ActionResult AddMachine(Engineer engineer, int MachineId)
      {
        if (MachineId != 0)
        {
          _db.EngineerMachine.Add(new EngineerMachine() { EngineerId = engineer.EngineerId, MachineId = MachineId });
          _db.SaveChanges();
        }
        return Redirect($"/Engineers/Details/{engineer.EngineerId}");
      }
      [HttpPost]
      public ActionResult DeleteMachine(int joinId, int engineerId)
      {
        var joinEntry = _db.EngineerMachine.FirstOrDefault(entry => entry.EngineerMachineId == joinId);
        _db.EngineerMachine.Remove(joinEntry);
        _db.SaveChanges();
        return Redirect($"/Engineers/Details/{engineerId}");
      }
      public ActionResult Delete(int id)
      {
        var thisEngineer = _db.Engineers.FirstOrDefault(engineer => engineer.EngineerId == id);
        return View(thisEngineer);
      }
      [HttpPost, ActionName("Delete")]
      public ActionResult DeleteConfirmed(int id)
      {
        var thisEngineer = _db.Engineers.FirstOrDefault(engineer => engineer.EngineerId == id);
        _db.Engineers.Remove(thisEngineer);
        _db.SaveChanges();
        return RedirectToAction("Index");
      }
      public ActionResult Edit(int id)
      {
        var thisEngineer = _db.Engineers.FirstOrDefault(engineer => engineer.EngineerId == id);
        return View(thisEngineer);
      }
      [HttpPost]
      public ActionResult Edit(Engineer engineer)
      {
        _db.Entry(engineer).State = EntityState.Modified;
        _db.SaveChanges();
        return Redirect($"/Engineers/Details/{engineer.EngineerId}");
      }
    }
}