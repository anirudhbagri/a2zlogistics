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
    public class DiscountController : Controller
    {
        private DataEntities db = new DataEntities();

        // GET: /Discount/
        public ActionResult Index(string message)
        {
            Response.AddHeader("Refresh", "180");
            ViewBag.message = message;
            return View(db.Discounts.OrderBy(x => x.Date).ThenBy(x => x.Username).ToList());
        }

        // GET: /Discount/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Discount discount = db.Discounts.Find(id);
            if (discount == null)
            {
                return HttpNotFound();
            }
            return View(discount);
        }
        /*
        // GET: /Discount/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Discount/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="DiscountID,Date,CardID,Username,TotalAmount")] Discount discount)
        {
            if (ModelState.IsValid)
            {
                db.Discounts.Add(discount);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(discount);
        }

        // GET: /Discount/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Discount discount = db.Discounts.Find(id);
            if (discount == null)
            {
                return HttpNotFound();
            }
            return View(discount);
        }

        // POST: /Discount/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="DiscountID,Date,CardID,Username,TotalAmount")] Discount discount)
        {
            if (ModelState.IsValid)
            {
                db.Entry(discount).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(discount);
        }
        */

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ApplyDiscount()
        {
            int id = Convert.ToInt32(Request.Form["id"]);
            decimal discountPercent = Convert.ToDecimal(Request.Form["discountPercent"+id]);
            if (discountPercent == null) {discountPercent = 0;}
            Discount discount = db.Discounts.Find(id);
            AspNetUser user = db.AspNetUsers.Where(m => m.UserName == discount.Username).FirstOrDefault();
            decimal discountAmount = (discountPercent * discount.TotalAmount) / 100;
            user.Discount += discountAmount;
            DiscountTransaction discountTransaction = new DiscountTransaction
            {
                Amount = discount.TotalAmount,
                CardID = discount.CardID,
                DiscountAmount = discountAmount,
                DiscountID = Guid.NewGuid().ToString(),
                DiscountPercent = discountPercent,
                TimeStamp = DateTime.Now,
                Username = discount.Username,
                CardType = db.CardDetails.FirstOrDefault(m => m.CardID == discount.CardID).CardType
            };
            db.DiscountTransactions.Add(discountTransaction);
            db.Entry(user).State = EntityState.Modified;
            db.Discounts.Remove(discount);
            try
            {
                db.SaveChanges();
            }
            catch (Exception)
            {
                RedirectToAction("Index", new { message = "Something went WRONG!! Discount was not applied!" });
            }
            return RedirectToAction("Index", new {message = "Discount applied" });
        }

        // GET: /Discount/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Discount discount = db.Discounts.Find(id);
            if (discount == null)
            {
                return HttpNotFound();
            }
            return View(discount);
        }

        // POST: /Discount/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Discount discount = db.Discounts.Find(id);
            db.Discounts.Remove(discount);
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
