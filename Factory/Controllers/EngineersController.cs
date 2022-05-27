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
    }
}