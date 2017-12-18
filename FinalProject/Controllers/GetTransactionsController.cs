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
using Microsoft.AspNet.Identity;
using FinalProject.Helper;

namespace FinalProject.Controllers
{
    [ModifiedAuthorize(Roles = "Customer")]
    public class GetTransactionsController : ApiController
    {
        private DataEntities db = new DataEntities();

        // GET api/GetTransactions
        public IQueryable<Transaction> GetTransactions()
        {
            string userName = RequestContext.Principal.Identity.GetUserName();
            IQueryable<Transaction> allTransactions = db.Transactions.Where(c => c.Username == userName);
            return allTransactions;
        }
        
        // GET api/GetTransactions/5
        [ResponseType(typeof(Transaction))]
        public IHttpActionResult GetTransaction(string id)
        {
            Transaction transaction = db.Transactions.Find(id);
            if (transaction == null)
            {
                return NotFound();
            }
            return Ok(transaction);
        }

        public IQueryable<Transaction> GetTransactions(DateTime date)
        {
            string userName = RequestContext.Principal.Identity.GetUserName();
            IQueryable<Transaction> allTransactions = db.Transactions.Where(c => c.Username == userName);
            IQueryable<Transaction> allTransactionsAfterDate = allTransactions.Where(c => c.TimeStamp >= date);
            return allTransactionsAfterDate;
        }
        /*
        // PUT api/GetTransactions/5
        public IHttpActionResult PutTransaction(string id, Transaction transaction)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != transaction.TransactionID)
            {
                return BadRequest();
            }

            db.Entry(transaction).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TransactionExists(id))
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

        // POST api/GetTransactions
        [ResponseType(typeof(Transaction))]
        public IHttpActionResult PostTransaction(Transaction transaction)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Transactions.Add(transaction);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (TransactionExists(transaction.TransactionID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = transaction.TransactionID }, transaction);
        }

        // DELETE api/GetTransactions/5
        [ResponseType(typeof(Transaction))]
        public IHttpActionResult DeleteTransaction(string id)
        {
            Transaction transaction = db.Transactions.Find(id);
            if (transaction == null)
            {
                return NotFound();
            }

            db.Transactions.Remove(transaction);
            db.SaveChanges();

            return Ok(transaction);
        }
        */

        public class TransactionRequest
        {
            public DateTime date;
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TransactionExists(string id)
        {
            return db.Transactions.Count(e => e.TransactionID == id) > 0;
        }
    }
}