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
namespace FinalProject.Controllers
{
    public class GetUserTransactionsController : ApiController
    {
        private DataEntities db = new DataEntities();

        // GET api/GetUserTransactions
        public IQueryable<UserTransaction> GetUserTransactions()
        {
            string userName = RequestContext.Principal.Identity.GetUserName();
            IQueryable<UserTransaction> allTransactions = db.UserTransactions.Where(c => c.Username == userName);
            return allTransactions.OrderByDescending(m => m.TimeStamp);
        }

        // GET api/GetUserTransactions/5
        [ResponseType(typeof(UserTransaction))]
        public IHttpActionResult GetUserTransaction(string id)
        {
            UserTransaction usertransaction = db.UserTransactions.Find(id);
            if (usertransaction == null)
            {
                return NotFound();
            }

            return Ok(usertransaction);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UserTransactionExists(string id)
        {
            return db.UserTransactions.Count(e => e.TransactionID == id) > 0;
        }
    }
}