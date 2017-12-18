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
    public class GetDiscountTransactionsController : ApiController
    {
        private DataEntities db = new DataEntities();

        // GET: api/GetDiscountTransactions
        public IQueryable<DiscountTransaction> GetDiscountTransactions()
        {
            string userName = RequestContext.Principal.Identity.GetUserName();
            return db.DiscountTransactions.Where(p => p.Username == userName);
        }
        
        /*
        // GET: api/GetDiscountTransactions/5
        [ResponseType(typeof(DiscountTransaction))]
        public IHttpActionResult GetDiscountTransaction(string id)
        {
            DiscountTransaction discountTransaction = db.DiscountTransactions.Find(id);
            if (discountTransaction == null)
            {
                return NotFound();
            }

            return Ok(discountTransaction);
        }

        // PUT: api/GetDiscountTransactions/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutDiscountTransaction(string id, DiscountTransaction discountTransaction)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != discountTransaction.DiscountID)
            {
                return BadRequest();
            }

            db.Entry(discountTransaction).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DiscountTransactionExists(id))
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

        // POST: api/GetDiscountTransactions
        [ResponseType(typeof(DiscountTransaction))]
        public IHttpActionResult PostDiscountTransaction(DiscountTransaction discountTransaction)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.DiscountTransactions.Add(discountTransaction);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (DiscountTransactionExists(discountTransaction.DiscountID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = discountTransaction.DiscountID }, discountTransaction);
        }

        // DELETE: api/GetDiscountTransactions/5
        [ResponseType(typeof(DiscountTransaction))]
        public IHttpActionResult DeleteDiscountTransaction(string id)
        {
            DiscountTransaction discountTransaction = db.DiscountTransactions.Find(id);
            if (discountTransaction == null)
            {
                return NotFound();
            }

            db.DiscountTransactions.Remove(discountTransaction);
            db.SaveChanges();

            return Ok(discountTransaction);
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

        private bool DiscountTransactionExists(string id)
        {
            return db.DiscountTransactions.Count(e => e.DiscountID == id) > 0;
        }
    }
}