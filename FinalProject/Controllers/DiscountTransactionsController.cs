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
    public class DiscountTransactionsController : Controller
    {
        private DataEntities db = new DataEntities();

        // GET: DiscountTransactions
        public ActionResult Index()
        {
            return View(db.DiscountTransactions.OrderByDescending(m => m.TimeStamp).ToList());
        }

        // GET: DiscountTransactions/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DiscountTransaction discountTransaction = db.DiscountTransactions.Find(id);
            if (discountTransaction == null)
            {
                return HttpNotFound();
            }
            return View(discountTransaction);
        }
        /*
        // GET: DiscountTransactions/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DiscountTransactions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DiscountID,TimeStamp,CardID,Username,Amount,DiscountPercent,DiscountAmount")] DiscountTransaction discountTransaction)
        {
            if (ModelState.IsValid)
            {
                db.DiscountTransactions.Add(discountTransaction);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(discountTransaction);
        }

        // GET: DiscountTransactions/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DiscountTransaction discountTransaction = db.DiscountTransactions.Find(id);
            if (discountTransaction == null)
            {
                return HttpNotFound();
            }
            return View(discountTransaction);
        }

        // POST: DiscountTransactions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DiscountID,TimeStamp,CardID,Username,Amount,DiscountPercent,DiscountAmount")] DiscountTransaction discountTransaction)
        {
            if (ModelState.IsValid)
            {
                db.Entry(discountTransaction).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(discountTransaction);
        }

        // GET: DiscountTransactions/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DiscountTransaction discountTransaction = db.DiscountTransactions.Find(id);
            if (discountTransaction == null)
            {
                return HttpNotFound();
            }
            return View(discountTransaction);
        }

        // POST: DiscountTransactions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            DiscountTransaction discountTransaction = db.DiscountTransactions.Find(id);
            db.DiscountTransactions.Remove(discountTransaction);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        */
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
