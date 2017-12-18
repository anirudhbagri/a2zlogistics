using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FinalProject.Models;

namespace FinalProject.Controllers
{
    [Authorize(Roles = "admin")]
    public class UserRequestsController : Controller
    {
        private DataEntities db = new DataEntities();

        // GET: /UserRequests/
        public ActionResult Index()
        {
            Response.AddHeader("Refresh", "180");
            return View(db.UserRequests.OrderBy(m => m.Username).ToList());
        }
        /*
        // GET: /UserRequests/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserRequest userrequest = db.UserRequests.Find(id);
            if (userrequest == null)
            {
                return HttpNotFound();
            }
            return View(userrequest);
        }
        
        // GET: /UserRequests/Create
        public ActionResult Create()
        {
            return View();
        }
        
        // POST: /UserRequests/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="Username,Amount,Mode,ID,Name,Description")] UserRequest userrequest)
        {
            if (ModelState.IsValid)
            {
                db.UserRequests.Add(userrequest);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(userrequest);
        }
        */
        // GET: /UserRequests/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserRequest userrequest = db.UserRequests.Find(id);
            if (userrequest == null)
            {
                return HttpNotFound();
            }
            return RedirectToAction("Create","UserTransactions",userrequest);
        }
        /*
        // POST: /UserRequests/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="Username,Amount,Mode,ID,Name,Description")] UserRequest userrequest)
        {
            if (ModelState.IsValid)
            {
                db.Entry(userrequest).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(userrequest);
        }
        */
        // GET: /UserRequests/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserRequest userrequest = db.UserRequests.Find(id);
            if (userrequest == null)
            {
                return HttpNotFound();
            }
            return View(userrequest);
        }

        // POST: /UserRequests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            UserRequest userrequest = db.UserRequests.Find(id);
            db.UserRequests.Remove(userrequest);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
