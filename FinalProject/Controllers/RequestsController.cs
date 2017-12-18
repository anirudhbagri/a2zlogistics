using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FinalProject.Models;
using Microsoft.AspNet.Identity;

namespace FinalProject.Controllers
{
    [Authorize(Roles = "admin")]
    public class RequestsController : Controller
    {
        private DataEntities db = new DataEntities();

        // GET: /Requests/
        public ActionResult Index(string acceptMessage, string declinedMessage)
        {
            Response.AddHeader("Refresh", "180");
            ViewBag.DecMessage = declinedMessage;
            ViewBag.AccMessage = acceptMessage;
            return View(db.Requests.OrderByDescending(m => m.TimeStamp).ThenBy(m => m.CardID).ToList());
        }
        public ActionResult Accept(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Request request = db.Requests.Find(id);
            if (request == null)
            {
                return HttpNotFound();
            }
            Transaction transaction = db.Transactions.Find(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }

            //AspNetUser user = db.AspNetUsers.FirstOrDefault(m => m.UserName == request.UserName);
            CardDetail card = db.CardDetails.FirstOrDefault(m => m.CardID == request.CardID);
            card.Balance += request.Amount;
            transaction.Status = 1;//Accepted
            transaction.Remarks = "Accepted";
            db.Entry(transaction).State = EntityState.Modified;
            db.Requests.Remove(request);
            db.SaveChanges();
            return RedirectToAction("Index", new { acceptMessage = "Request Accepted", declinedMessage="" });
        }

        [HttpPost]
        public ActionResult Decline()
        {
            string id = Request.Form["id"];
            string remarks = Request.Form["remarks"];
            if (string.IsNullOrEmpty(remarks))
                remarks = "Declined";
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Request request = db.Requests.Find(id);
            if (request == null)
            {
                return HttpNotFound();
            }
            Transaction transaction = db.Transactions.Find(id);
            CardDetail card = db.CardDetails.FirstOrDefault(m => m.CardID == request.CardID);
            AspNetUser user = db.AspNetUsers.FirstOrDefault(m => m.UserName == request.UserName);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            user.Balance += request.Amount;
            db.Requests.Remove(request);
            transaction.Status = 0;//Declined
            transaction.Remarks = remarks;
            db.Entry(transaction).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index", new { declinedMessage = "Request Declined", acceptMessage = "" });
        }

        // GET: /Requests/Details/5
        /*public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Request request = db.Requests.Find(id);
            if (request == null)
            {
                return HttpNotFound();
            }
            return View(request);
        }*/

        // GET: /Requests/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Requests/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="CardID,Amount,UserName")] Request request)
        {
            CardDetail card = null;
            AspNetUser user = null;
            if (String.IsNullOrEmpty(request.CardID)) { ModelState.AddModelError("CardID", "CardID cannot be empty"); }
            if (String.IsNullOrEmpty(request.Amount.ToString())) { ModelState.AddModelError("Amount", "Amount cannot be empty"); }
            if (String.IsNullOrEmpty(request.UserName)) { ModelState.AddModelError("UserName", "Mobile number cannot be empty"); }
            if (!String.IsNullOrEmpty(request.CardID))
            {
                card = db.CardDetails.FirstOrDefault(p => p.CardID == request.CardID);
                if (card == null) { ModelState.AddModelError("CardID", "Card doesnt exist"); }
            }
            if (!String.IsNullOrEmpty(request.UserName))
            {
                user = db.AspNetUsers.FirstOrDefault(u => u.UserName == request.UserName);
                if (user == null) { ModelState.AddModelError("UserName", "User doesnt exists"); }
                else if (user.Balance < request.Amount) { ModelState.AddModelError("Amount", "User does not have enough balance"); }
            }
            if(card!=null && user!=null)
            {
                if (card.UserName != user.UserName)
                {
                    ModelState.AddModelError("CardID", "This card doesnt belongs to this user!");
                }
            }
            if (ModelState.IsValid)
            {
                request.Name = user.Name;
                request.CardType = card.CardType;
                request.RequestID = Guid.NewGuid().ToString();
                request.TimeStamp = DateTime.Now;
                Transaction transaction = new Transaction()
                {
                    TransactionID = request.RequestID,
                    TimeStamp = request.TimeStamp,
                    Username = request.UserName,
                    CardID = request.CardID,
                    CardType = request.CardType,
                    Amount = request.Amount,
                    Status = 2,//Pending transaction
                    Remarks = "Pending",
                    Type = "Credit",
                    VehicleNumber = card.VehicleNumber
                };
                user.Balance -= request.Amount;
                db.Requests.Add(request);
                db.Transactions.Add(transaction);
                db.Requests.Add(request);
                try { db.SaveChanges(); }
                catch (Exception) { RedirectToAction("Index", new { declinedMessage = "Unable to save request, Something bad happened!", acceptMessage = "" }); }
                return RedirectToAction("Index",new { declinedMessage = "", acceptMessage = "Request Added" });
            }

            return View(request);
        }

        // GET: /Requests/Edit/5
        /*public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Request request = db.Requests.Find(id);
            if (request == null)
            {
                return HttpNotFound();
            }
            return View(request);
        }

        // POST: /Requests/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="CardID,CardType,Amount,UserName,TimeStamp,RequestID")] Request request)
        {
            if (ModelState.IsValid)
            {
                db.Entry(request).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(request);
        }
        */

        // GET: /Requests/Delete/5
        /*public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Request request = db.Requests.Find(id);
            if (request == null)
            {
                return HttpNotFound();
            }
            return View(request);
        }

        // POST: /Requests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Request request = db.Requests.Find(id);
            db.Requests.Remove(request);
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
