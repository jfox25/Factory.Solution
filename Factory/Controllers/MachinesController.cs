using Microsoft.AspNetCore.Mvc;
using Factory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Factory.Controllers
{
    public class MachinesController : Controller
    {
      private readonly FactoryContext _db;

      public MachinesController(FactoryContext db)
      {
        _db = db;
      }

      public ActionResult Index()
      {
        return View(_db.Machines.ToList());
      }
      public ActionResult Create()
      {
        ViewBag.EngineerId = new SelectList(_db.Engineers, "EngineerId", "Name");
        return View();
      }
      [HttpPost]
      public ActionResult Create(Machine machine, int EngineerId)
      {
        _db.Machines.Add(machine);
        _db.SaveChanges();
        if (EngineerId != 0)
        {
          _db.EngineerMachine.Add(new EngineerMachine() { EngineerId = EngineerId, MachineId = machine.MachineId });
          _db.SaveChanges();
        }
        return RedirectToAction("Index");
      }
      public ActionResult Details(int id)
      {
        var thisMachine = _db.Machines
            .Include(machine => machine.JoinEntities)
            .ThenInclude(join => join.Engineer)
            .FirstOrDefault(machine => machine.MachineId == id);
        return View(thisMachine);
      }
      public ActionResult AddEngineer(int id)
      {
        var thisMachine = _db.Machines.FirstOrDefault(machine => machine.MachineId == id);
        ViewBag.EngineerId = new SelectList(_db.Engineers, "EngineerId", "Name");
        return View(thisMachine);
      }
      [HttpPost]
      public ActionResult AddEngineer(Machine machine, int EngineerId)
      {
        var engineerMachine = _db.EngineerMachine.FirstOrDefault(engineerMachine => engineerMachine.MachineId == machine.MachineId && engineerMachine.EngineerId == EngineerId);
        if (EngineerId != 0 && engineerMachine == null)
        {
          _db.EngineerMachine.Add(new EngineerMachine() { EngineerId = EngineerId, MachineId = machine.MachineId });
          _db.SaveChanges();
        }
        return Redirect($"/Machines/Details/{machine.MachineId}");
      }
      [HttpPost]
      public ActionResult DeleteEngineer(int joinId, int machineId)
      {
        var joinEntry = _db.EngineerMachine.FirstOrDefault(entry => entry.EngineerMachineId == joinId);
        _db.EngineerMachine.Remove(joinEntry);
        _db.SaveChanges();
        return Redirect($"/Machines/Details/{machineId}");
      }
      public ActionResult Delete(int id)
      {
        var thisMachine = _db.Machines.FirstOrDefault(machine => machine.MachineId == id);
        return View(thisMachine);
      }
      [HttpPost, ActionName("Delete")]
      public ActionResult DeleteConfirmed(int id)
      {
        var thisMachine = _db.Machines.FirstOrDefault(machine => machine.MachineId == id);
        _db.Machines.Remove(thisMachine);
        _db.SaveChanges();
        return RedirectToAction("Index");
      }
    }
}