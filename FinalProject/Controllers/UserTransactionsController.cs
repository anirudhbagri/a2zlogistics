using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FinalProject.Models;
using System.Globalization;

namespace FinalProject.Controllers
{
    [Authorize(Roles = "admin")]
    public class UserTransactionsController : Controller
    {
        private DataEntities db = new DataEntities();

        // GET: /UserTransactions/
        public ActionResult Index(string datestring, string username)
        {
            Response.AddHeader("Refresh", "180");
            DateTime? date = null;
            if (!string.IsNullOrEmpty(datestring))
            {
                try
                {
                    date = DateTime.ParseExact(datestring, "dd-MM-yyyy", CultureInfo.InvariantCulture);
                }
                catch (Exception) { ModelState.AddModelError("datestring", "Date incorrect"); }
            }
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(date.ToString()) && !string.IsNullOrEmpty(username))
                {
                    //IEnumerable<UserTransaction> userTransactionUsername = db.UserTransactions.Where(m => m.Username == username).Where(m => m.TimeStamp == date).ToList();
                    return View(db.UserTransactions.Where(m => m.Username.StartsWith(username)).Where(m => m.TimeStamp == date).ToList());
                }
                if (!string.IsNullOrEmpty(date.ToString()))
                {
                    return View(db.UserTransactions.Where(m => m.TimeStamp == date).OrderByDescending(m => m.TimeStamp).ThenBy(m => m.Username).ToList());
                }
                if (!string.IsNullOrEmpty(username))
                {
                    return View(db.UserTransactions.Where(m => m.Username.StartsWith(username)).OrderByDescending(m => m.TimeStamp).ThenBy(m => m.Username).ToList());
                }
            }
            return View(db.UserTransactions.OrderByDescending(m => m.TimeStamp).ThenBy(m => m.Username).ToList());
        }
        /*
        // GET: /UserTransactions/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserTransaction usertransaction = db.UserTransactions.Find(id);
            if (usertransaction == null)
            {
                return HttpNotFound();
            }
            return View(usertransaction);
        }
        */
        // GET: /UserTransactions/Create
        public ActionResult Create(UserRequest userrequest,string message)
        {
            ViewBag.message = message;
            UserTransaction usertransaction = new UserTransaction
            {
                Amount = userrequest.Amount,
                Mode = userrequest.Mode,
                Username = userrequest.Username,
                TimeStamp = DateTime.Now,
                Name = userrequest.Name,
                UserDescription = userrequest.UserDescription,
                AdminDescription = userrequest.AdminDescription
            };
            ViewBag.ID = userrequest.ID;
            return View(usertransaction);
        }
        

        // POST: /UserTransactions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TransactionID,Username,Amount,TimeStamp,Mode,UserDescription,AdminDescription")] UserTransaction usertransaction, string requestID)
        {
           if (String.IsNullOrEmpty(usertransaction.Username)) { ModelState.AddModelError("Username", "This field cannot be empty!"); }
           AspNetUser user = db.AspNetUsers.FirstOrDefault(m => m.UserName == usertransaction.Username);
           if(user == null){ModelState.AddModelError("Username", "Please check the username");}
           if (String.IsNullOrEmpty(usertransaction.TransactionID)) { ModelState.AddModelError("TransactionID", "This field cannot be empty!"); }
           if (String.IsNullOrEmpty(usertransaction.Amount.ToString())) { ModelState.AddModelError("Amount", "This field cannot be empty!"); }
           if (String.IsNullOrEmpty(usertransaction.Mode)) { ModelState.AddModelError("Mode", "Please enter mode of transaction"); }
           if (String.IsNullOrEmpty(usertransaction.UserDescription)) { ModelState.AddModelError("UserDescription", "Please add some description"); }
           if (String.IsNullOrEmpty(usertransaction.AdminDescription)) { ModelState.AddModelError("AdminDescription", "Please add some description"); }
           if (String.IsNullOrEmpty(usertransaction.TimeStamp.ToString())) { ModelState.AddModelError("TimeStamp", "This field cannot be empty"); }
           if (!String.IsNullOrEmpty(usertransaction.TransactionID))
           {
               UserTransaction tempTransaction = db.UserTransactions.Find(usertransaction.TransactionID);
               if (tempTransaction != null) { ModelState.AddModelError("TransactionID", "Duplicate transaction ID"); }
           }
           if (!ModelState.IsValid)
           {
               if (!String.IsNullOrEmpty(requestID)) ViewBag.ID = requestID;
               //ModelState.AddModelError("", "Please go back and re-create transaction");
           }
           if (ModelState.IsValid)
           {
               usertransaction.UserDescription = usertransaction.UserDescription.ToUpper();
               usertransaction.Name = user.Name;
               usertransaction.AdminDescription = usertransaction.AdminDescription.ToUpper();
               usertransaction.Mode = usertransaction.Mode.ToUpper();
                db.UserTransactions.Add(usertransaction);
                user.Balance += usertransaction.Amount;
                db.Entry(user).State = EntityState.Modified;
                if (!String.IsNullOrEmpty(requestID))
                {
                    UserRequest userrequest = db.UserRequests.Find(Convert.ToInt32(requestID));
                    if (userrequest != null)
                        db.UserRequests.Remove(userrequest);
                }
                try
                {
                    db.SaveChanges();
                    return RedirectToAction("Create", new { message = "Transaction recorded successfully" });
                }
                catch (Exception)
                {
                    ModelState.AddModelError("Error", "Something went wrong");
                }
            }
           return View(usertransaction);
        }
        /*
        // GET: /UserTransactions/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserTransaction usertransaction = db.UserTransactions.Find(id);
            if (usertransaction == null)
            {
                return HttpNotFound();
            }
            return View(usertransaction);
        }

        // POST: /UserTransactions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="TransactionID,Username,Amount,TimeStamp,Status,Mode")] UserTransaction usertransaction)
        {
            if (ModelState.IsValid)
            {
                db.Entry(usertransaction).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(usertransaction);
        }

        // GET: /UserTransactions/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserTransaction usertransaction = db.UserTransactions.Find(id);
            if (usertransaction == null)
            {
                return HttpNotFound();
            }
            return View(usertransaction);
        }

        // POST: /UserTransactions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            UserTransaction usertransaction = db.UserTransactions.Find(id);
            db.UserTransactions.Remove(usertransaction);
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
