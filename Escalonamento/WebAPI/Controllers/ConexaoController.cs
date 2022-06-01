using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Escalonamento.Controllers
{
    public class ConexaoController : Controller
    {
        // GET: ConexaoController
        public ActionResult Index()
        {
            return View();
        }

        // GET: ConexaoController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ConexaoController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ConexaoController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ConexaoController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ConexaoController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ConexaoController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ConexaoController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
