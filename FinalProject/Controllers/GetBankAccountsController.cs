using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using FinalProject.Models;

namespace FinalProject.Controllers
{
    public class GetBankAccountsController : ApiController
    {
        private DataEntities db = new DataEntities();

        // GET api/GetBankAccounts
        public IQueryable<BankAccount> GetBankAccounts()
        {
            return db.BankAccounts;
        }
        /*
        // GET api/GetBankAccounts/5
        [ResponseType(typeof(BankAccount))]
        public IHttpActionResult GetBankAccount(int id)
        {
            BankAccount bankaccount = db.BankAccounts.Find(id);
            if (bankaccount == null)
            {
                return NotFound();
            }

            return Ok(bankaccount);
        }

        // PUT api/GetBankAccounts/5
        public IHttpActionResult PutBankAccount(int id, BankAccount bankaccount)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != bankaccount.ID)
            {
                return BadRequest();
            }

            db.Entry(bankaccount).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BankAccountExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST api/GetBankAccounts
        [ResponseType(typeof(BankAccount))]
        public IHttpActionResult PostBankAccount(BankAccount bankaccount)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.BankAccounts.Add(bankaccount);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (BankAccountExists(bankaccount.ID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = bankaccount.ID }, bankaccount);
        }

        // DELETE api/GetBankAccounts/5
        [ResponseType(typeof(BankAccount))]
        public IHttpActionResult DeleteBankAccount(int id)
        {
            BankAccount bankaccount = db.BankAccounts.Find(id);
            if (bankaccount == null)
            {
                return NotFound();
            }

            db.BankAccounts.Remove(bankaccount);
            db.SaveChanges();

            return Ok(bankaccount);
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

        private bool BankAccountExists(int id)
        {
            return db.BankAccounts.Count(e => e.ID == id) > 0;
        }
    }
}