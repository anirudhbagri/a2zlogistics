using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FinalProject.Models;
using System.Text;
using System.Security.Cryptography;

namespace FinalProject.Controllers
{
    public class BankAccountsController : Controller
    {
        private DataEntities db = new DataEntities();

        // GET: /BankAccounts/
        public ActionResult Index()
        {
            string decryptionKey = CreateKey(System.Convert.ToInt32(24));
            string validationKey = CreateKey(System.Convert.ToInt32(64));
            string output = "<machineKey validationKey=\"" + validationKey + "\" decryptionKey=\"" + decryptionKey + "\" validation=\"SHA1\"/>";
            //Console.WriteLine("<machineKey validationKey=\"{0}\" decryptionKey=\"{1}\" validation=\"SHA1\"/>", validationKey, decryptionKey);
            //ViewBag.message = SMSManager.SMSSender.sendSMS("8107117700");
            return View(db.BankAccounts.ToList());
        }

        // GET: /BankAccounts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BankAccount bankaccount = db.BankAccounts.Find(id);
            if (bankaccount == null)
            {
                return HttpNotFound();
            }
            return View(bankaccount);
        }

        // GET: /BankAccounts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /BankAccounts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="ID,BankNameAccountNo")] BankAccount bankaccount)
        {
            BankAccount account = db.BankAccounts.Find(bankaccount.ID);
            if (account != null) { ModelState.AddModelError("ID", "Duplicate ID, please give a unique ID"); }
            if (ModelState.IsValid)
            {
                db.BankAccounts.Add(bankaccount);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(bankaccount);
        }

        // GET: /BankAccounts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BankAccount bankaccount = db.BankAccounts.Find(id);
            if (bankaccount == null)
            {
                return HttpNotFound();
            }
            return View(bankaccount);
        }

        // POST: /BankAccounts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="ID,BankNameAccountNo")] BankAccount bankaccount)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bankaccount).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(bankaccount);
        }

        // GET: /BankAccounts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BankAccount bankaccount = db.BankAccounts.Find(id);
            if (bankaccount == null)
            {
                return HttpNotFound();
            }
            return View(bankaccount);
        }

        // POST: /BankAccounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BankAccount bankaccount = db.BankAccounts.Find(id);
            db.BankAccounts.Remove(bankaccount);
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

        static String CreateKey(int numBytes)
        {
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] buff = new byte[numBytes];

            rng.GetBytes(buff);
            return BytesToHexString(buff);
        }

        static String BytesToHexString(byte[] bytes)
        {
            StringBuilder hexString = new StringBuilder(64);

            for (int counter = 0; counter < bytes.Length; counter++)
            {
                hexString.Append(String.Format("{0:X2}", bytes[counter]));
            }
            return hexString.ToString();
        }
    }
}
