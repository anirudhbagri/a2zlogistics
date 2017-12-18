using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FinalProject.Models;
using FinalProject.Helper;
using System.Data.Entity.Infrastructure;

namespace FinalProject.Controllers
{
    [Authorize(Roles = "admin")]
    public class CardsController : Controller
    {
        private DataEntities db = new DataEntities();

        // GET: /Cards/
        public ActionResult Index(string username,string cardId)
        {
            Response.AddHeader("Refresh", "120"); ;
            if (!string.IsNullOrEmpty(cardId) && !string.IsNullOrEmpty(username))
            {
                return View(db.CardDetails.Where(m => m.CardID.StartsWith(cardId)).Where(m => m.UserName.StartsWith(username)).OrderBy(m => m.CardID).ToList());
            }
            if (!string.IsNullOrEmpty(cardId))
            {
                return View(db.CardDetails.Where(m => m.CardID.StartsWith(cardId)).OrderBy(m => m.CardID).ToList());
            }
            if (!string.IsNullOrEmpty(username))
            {
                return View(db.CardDetails.Where(m => m.UserName.StartsWith(username)).OrderBy(m => m.CardID).ToList());
            }
            return View(db.CardDetails.OrderBy(m => m.CardID).ToList());
        }

        // GET: /Cards/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CardDetail carddetail = db.CardDetails.Find(id);
            if (carddetail == null)
            {
                return HttpNotFound();
            }
            return View(carddetail);
        }

        // GET: /Cards/Create
        public ActionResult Create(string message)
        {
            ViewBag.message = message;
            return View();
        }

        // POST: /Cards/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CardDetail carddetail)
        {
            if (String.IsNullOrEmpty(carddetail.UserName)) {ModelState.AddModelError("UserName","User cannot be empty!"); }
            AspNetUser user = db.AspNetUsers.FirstOrDefault(u => u.UserName == carddetail.UserName);
            if (user == null) { ModelState.AddModelError("UserName","User not found!");}
            if (String.IsNullOrEmpty(carddetail.CardID)) {ModelState.AddModelError("CardID","This field cannot be empty!"); }
            CardDetail card = db.CardDetails.Find(carddetail.CardID);
            if (card != null) { ModelState.AddModelError("CardID", "Duplicate CardID, Please provide a unique cardID"); }
            if (String.IsNullOrEmpty(carddetail.Balance.ToString())) {ModelState.AddModelError("Balance","This field cannot be empty!"); }
            if (String.IsNullOrEmpty(carddetail.CardType)) {ModelState.AddModelError("CardType","This field cannot be empty!"); }
            if (String.IsNullOrEmpty(carddetail.ExpiryDate.ToString())) {ModelState.AddModelError("ExpiryDate","This field cannot be empty!"); }
            if (String.IsNullOrEmpty(carddetail.VehicleNumber)) {ModelState.AddModelError("VehicleNumber","This field cannot be empty!"); }
            
            if (ModelState.IsValid)
            {
                string message;
                carddetail.CardType = carddetail.CardType.ToUpper();
                
                try
                {
                    db.CardDetails.Add(carddetail);
                    db.SaveChanges();
                    message = "Card created succesfully!";
                }
                catch (Exception e)
                {
                    message = "Something went wrong..unable to connect to database";
                }
                return RedirectToAction("Create", new {message = message});
            }
            return View();
        }

        // GET: /Cards/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CardDetail carddetail = db.CardDetails.Find(id);
            if (carddetail == null)
            {
                return HttpNotFound();
            }
            return View(carddetail);
        }

        // POST: /Cards/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CardID,UserName,Balance,CardType,ExpiryDate,VehicleNumber")] CardDetail carddetail)
        {
            if (String.IsNullOrEmpty(carddetail.CardID)) {ModelState.AddModelError("UserName","User cannot be empty!"); }
            AspNetUser user = db.AspNetUsers.FirstOrDefault(u => u.UserName == carddetail.UserName);
            if (user == null){ModelState.AddModelError("UserName", "User not found!");}
            if (String.IsNullOrEmpty(carddetail.CardID)) {ModelState.AddModelError("CardID","This field cannot be empty!"); }
            if (String.IsNullOrEmpty(carddetail.Balance.ToString())) {ModelState.AddModelError("Balance","This field cannot be empty!"); }
            if (String.IsNullOrEmpty(carddetail.CardType)) {ModelState.AddModelError("CardType","This field cannot be empty!"); }
            if (String.IsNullOrEmpty(carddetail.ExpiryDate.ToString())) {ModelState.AddModelError("ExpiryDate","This field cannot be empty!"); }
            if (String.IsNullOrEmpty(carddetail.VehicleNumber)) {ModelState.AddModelError("VehicleNumber","This field cannot be empty!"); }
            if (ModelState.IsValid)
            {
                db.Entry(carddetail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", "Something went wrong, make sure all entries are in proper format");
            return View(carddetail);
        }

        // GET: /Cards/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CardDetail carddetail = db.CardDetails.Find(id);
            if (carddetail == null)
            {
                return HttpNotFound();
            }
            return View(carddetail);
        }

        // POST: /Cards/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            CardDetail carddetail = db.CardDetails.Find(id);
            db.CardDetails.Remove(carddetail);
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
