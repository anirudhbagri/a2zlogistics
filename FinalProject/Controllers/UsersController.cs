using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FinalProject.Models;
using System.Web.Security;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace FinalProject.Controllers
{
    [Authorize(Roles = "admin")]
    public class UsersController : Controller
    {
        
        private DataEntities db = new DataEntities();
        public UserManager<ApplicationUser> UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
        // GET: /Users/
        public ActionResult Index(string message,string username)
        {
            Response.AddHeader("Refresh", "180");
            ViewBag.message = message;
            if (!string.IsNullOrEmpty(username))
            {
                return View(db.AspNetUsers.Where(m => m.UserName.StartsWith(username)).OrderByDescending(m => m.UserName).ToList());
            }
            return View(db.AspNetUsers.OrderBy(m => m.UserName).ToList());
        }

        // GET: /Users/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetUser aspnetuser = db.AspNetUsers.Find(id);
            if (aspnetuser == null)
            {
                return HttpNotFound();
            }
            List<CardDetail> cards = db.CardDetails.Where(m => m.UserName == aspnetuser.UserName).ToList();
            List<CardType> cardType = db.CardTypes.ToList();
            ViewBag.cards = cards;
            ViewBag.cardType = cardType;
            return View(aspnetuser);
        }

        // GET: /Users/Create
        /*
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="Id,UserName,PasswordHash,SecurityStamp,Discriminator,Name,Balance")] AspNetUser aspnetuser)
        {
            if (ModelState.IsValid)
            {
                db.AspNetUsers.Add(aspnetuser);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(aspnetuser);
        }
        */
        // GET: /Users/Edit/5
        public ActionResult Edit(string id, string smsMessage)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetUser aspnetuser = db.AspNetUsers.Find(id);
            if (aspnetuser == null)
            {
                return HttpNotFound();
            }
            return View(aspnetuser);
        }

        // POST: /Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="Id,UserName,PasswordHash,SecurityStamp,Discriminator,Name,Balance,Mobile_Number,Discount")] AspNetUser aspnetuser)
        {
            if (String.IsNullOrEmpty(aspnetuser.Name)) { ModelState.AddModelError("Name", "This field cannot be empty!"); }
            if (String.IsNullOrEmpty(aspnetuser.Balance.ToString())) { ModelState.AddModelError("Balance", "This field cannot be empty!"); }
            if (String.IsNullOrEmpty(aspnetuser.Mobile_Number)) { ModelState.AddModelError("Mobile_Number", "This field cannot be empty!"); }
            
            if (ModelState.IsValid)
            {
                db.Entry(aspnetuser).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new {message = "Changes Saved" });
            }
            return View(aspnetuser);
        }

        public ActionResult ResetPassword(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return RedirectToAction("ResetPassword", "Account", new { id = id });
        }
        // GET: /Users/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetUser aspnetuser = db.AspNetUsers.Find(id);
            if (aspnetuser == null)
            {
                return HttpNotFound();
            }
            if(UserManager.IsInRole(aspnetuser.Id,"admin")){
                ViewBag.warningMessage = "This is a ADMIN USER! Please do not delete it.";
            }
            return View(aspnetuser);
            }

        // POST: /Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            AspNetUser aspnetuser = db.AspNetUsers.Find(id);
            if (UserManager.IsInRole(aspnetuser.Id, "admin"))
            {
                ViewBag.warningMessage = "THIS IS A ADMIN USER! You CANNOT delete it";
                return View(aspnetuser);
            }
            else
            {
                db.AspNetUsers.Remove(aspnetuser);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public ActionResult TransferDiscount(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AspNetUser aspnetuser = db.AspNetUsers.Find(id);
            if (aspnetuser == null)
            {
                return HttpNotFound();
            }
            if (aspnetuser.Discount <= 0)
            {
                return RedirectToAction("Index", new { message = "Discount amount is 0" });
            }
            UserTransaction userTransaction = new UserTransaction
            {
                AdminDescription = "Reward Points",
                Amount = aspnetuser.Discount,
                Mode = "Reward Points",
                Name = aspnetuser.Name,
                TimeStamp = DateTime.Now.Date,
                TransactionID = Guid.NewGuid().ToString(),
                UserDescription = "Reward Points",
                Username = aspnetuser.UserName
            };
            aspnetuser.Balance += aspnetuser.Discount;
            aspnetuser.Discount = 0;
            string message;
            try
            {
                db.Entry(aspnetuser).State = EntityState.Modified;
                db.UserTransactions.Add(userTransaction);
                db.SaveChanges();
                message = "Discount transfered!";
            }
            catch (Exception e)
            {
                message = "Discount transfered failed...something went wrong";
            }
            return RedirectToAction("Index", new {message = message });
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
