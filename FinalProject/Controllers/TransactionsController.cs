using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FinalProject.Models;
using FinalProject.ResponseClass;
using System.Data.Entity.Infrastructure;
using System.Globalization;

namespace FinalProject.Controllers
{
    [Authorize(Roles = "admin")]
    public class TransactionsController : Controller
    {
        private DataEntities db = new DataEntities();

        // GET: /Transactions/
        /*public ActionResult Index()
        {
            return View(db.Transactions.OrderByDescending(m => m.TimeStamp).ThenBy(m => m.Username).ToList());
        }*/
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
                    //IEnumerable<Transaction> transactionUsername = db.Transactions.Where(m => m.Username == username).Where(m => m.TimeStamp == date).ToList();
                    return View(db.Transactions.Where(m => m.Username.StartsWith(username)).Where(m => m.TimeStamp == date).ToList());
                }
                if (!string.IsNullOrEmpty(date.ToString()))
                {
                    return View(db.Transactions.Where(m => m.TimeStamp == date).OrderByDescending(m => m.TimeStamp).ThenBy(m => m.Username).ToList());
                }
                if (!string.IsNullOrEmpty(username))
                {
                    return View(db.Transactions.Where(m => m.Username.StartsWith(username)).OrderByDescending(m => m.TimeStamp).ThenBy(m => m.Username).ToList());
                }
            }
            return View(db.Transactions.OrderByDescending(m => m.TimeStamp).ThenBy(m => m.Username).ToList());
        }
        [HttpGet]
        public ActionResult CardDebit(string message)
        {
            ViewBag.message = message;
            Transaction transaction = new Transaction();
            transaction.TimeStamp = DateTime.Now;
            transaction.TransactionID = Guid.NewGuid().ToString();
            return View(transaction);
        }
        [HttpPost]
        public ActionResult CardDebit(CardDebitRequest request)
        {
            string username = Request.Form["username"];
            if (String.IsNullOrEmpty(username)) { ModelState.AddModelError("username", "Username cannot be empty"); }
            if (String.IsNullOrEmpty(request.TransactionID)) { ModelState.AddModelError("TransactionID", "This field cannot be empty!"); }
            if (String.IsNullOrEmpty(request.TimeStamp.ToShortDateString())) { ModelState.AddModelError("TimeStamp", "This field cannot be empty!"); }
            if (String.IsNullOrEmpty(request.CardID)) { ModelState.AddModelError("CardID", "This field cannot be empty!"); }
            if (String.IsNullOrEmpty(request.Amount.ToString())) { ModelState.AddModelError("Amount", "This field cannot be empty!"); }
            if (String.IsNullOrEmpty(request.Remarks)) { ModelState.AddModelError("Remarks", "This field cannot be empty!"); }
            if (!String.IsNullOrEmpty(request.Remarks))
            {
                string pattern = "^[a-zA-Z]+,[0-9]+\\.[0-9]{2},[0-9]+\\.[0-9]{2},[a-zA-z]+$";
                if (!System.Text.RegularExpressions.Regex.IsMatch(request.Remarks, pattern))
                {
                    ModelState.AddModelError("Remarks", "Incorrect format (eg. Petrol,1.20,3.40,pump)");
                }
            }
            Transaction transaction = new Transaction{
                Amount = request.Amount,
                CardID = request.CardID,
                Remarks = request.Remarks,
                TimeStamp = request.TimeStamp,
                TransactionID = request.TransactionID
            };

            Transaction tempTransaction = db.Transactions.Find(request.TransactionID);
            if (tempTransaction != null) { ModelState.AddModelError("TransactionID", "Duplicate transaction ID"); }
            CardDetail card = db.CardDetails.FirstOrDefault(c => c.CardID == request.CardID);
            if (card == null) { ModelState.AddModelError("CardID", "Card not found"); }
            else
            {
                if (card.Balance < request.Amount) { ModelState.AddModelError("Amount", "Low balance in card!"); }
                if (card.UserName != username)
                {
                    ModelState.AddModelError("CardID", "Card doesnt belong to this user!");
                }
            }
            
            if (ModelState.IsValid)
            {
                transaction.Username = card.UserName;
                transaction.CardType = card.CardType;
                transaction.VehicleNumber = card.VehicleNumber;
                transaction.Status = 1;
                transaction.Type = "Debit";
                db.Transactions.Add(transaction);

                //Subtracting balance from card
                card.Balance -= transaction.Amount;

                //Discount transaction 
                IQueryable<Discount> discountsOnCard = db.Discounts.Where(m => m.CardID == request.CardID);
                if (discountsOnCard.Count() > 0)
                {
                    //Transaction on card exists
                    IQueryable<Discount> discountOnCardOnDate = discountsOnCard.Where(m => m.Date == request.TimeStamp);
                    if(discountOnCardOnDate.Count()>0){
                        // Transaction on same day exists
                        Discount discount  = discountOnCardOnDate.FirstOrDefault();
                        discount.TotalAmount += request.Amount;
                        db.Entry(discount).State = EntityState.Modified;
                    }
                    else {
                        //Transaction on same day doesnt exists
                        Discount discount = new Discount{
                        CardID = request.CardID,
                        Date = request.TimeStamp,
                        TotalAmount = request.Amount,
                        Username = card.UserName
                        };
                        db.Discounts.Add(discount);
                    }
                }
                else
                {
                    Discount discount = new Discount
                    {
                        CardID = request.CardID,
                        Date = request.TimeStamp,
                        TotalAmount = request.Amount,
                        Username = card.UserName
                    };
                    db.Discounts.Add(discount);
                }
                
                db.Entry(card).State = EntityState.Modified;
                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateException)
                {
                    ModelState.AddModelError("", "Something went wrong!");
                }
                return RedirectToAction("CardDebit", new {message = "Transaction recorded" });
            }
            return View();
        }
        /*
        // GET: /Transactions/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transaction transaction = db.Transactions.Find(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            return View(transaction);
        }

        // GET: /Transactions/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Transactions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="TransactionID,TimeStamp,Username,CardID,CardType,Amount,Status,Remarks")] Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                db.Transactions.Add(transaction);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(transaction);
        }
        */
        // GET: /Transactions/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transaction transaction = db.Transactions.Find(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            return View(transaction);
        }

        // POST: /Transactions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TransactionID,TimeStamp,Username,CardID,CardType,VehicleNumber,Type,Amount,Status,Remarks")] Transaction transaction)
        {
            if (String.IsNullOrEmpty(transaction.TimeStamp.ToShortDateString())) { ModelState.AddModelError("TimeStamp", "This field cannot be empty!"); }
            if (String.IsNullOrEmpty(transaction.Remarks)) { ModelState.AddModelError("Remarks", "This field cannot be empty!"); }
            if (!String.IsNullOrEmpty(transaction.Remarks))
            {
                string pattern = "^[a-zA-Z]+,[0-9]+\\.[0-9]{2},[0-9]+\\.[0-9]{2},[a-zA-z]+$";
                if (!System.Text.RegularExpressions.Regex.IsMatch(transaction.Remarks, pattern))
                {
                    ModelState.AddModelError("Remarks", "Incorrect format (eg. Petrol,1.20,3.40,pump)");
                }
            }
            if (ModelState.IsValid)
            {
                db.Entry(transaction).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(transaction);
        }

        // GET: /Transactions/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transaction transaction = db.Transactions.Find(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            return View(transaction);
        }

        // POST: /Transactions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Transaction transaction = db.Transactions.Find(id);
            db.Transactions.Remove(transaction);
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
