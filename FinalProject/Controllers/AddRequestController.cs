using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using FinalProject.Models;
using Microsoft.AspNet.Identity;
using FinalProject.Helper;
using FinalProject.ResponseClass;

namespace FinalProject.Controllers
{
    [ModifiedAuthorize(Roles = "Customer")]
    public class AddRequestController : ApiController
    {
        private DataEntities db = new DataEntities();

        // GET api/AddRequest
        /*
        public IQueryable<Request> GetRequests()
        {
            return db.Requests;
        }
        */

        // GET api/AddRequest/5
        /*
        [ResponseType(typeof(Request))]
        public IHttpActionResult GetRequest(string id)
        {
            Request request = db.Requests.Find(id);
            if (request == null)
            {
                return NotFound();
            }

            return Ok(request);
        }
         * 
         * 8/
        /*
        // PUT api/AddRequest/5
        public IHttpActionResult PutRequest(string id, Request request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != request.RequestID)
            {
                return BadRequest();
            }

            db.Entry(request).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RequestExists(id))
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
        */

        // POST api/AddRequest
        [ResponseType(typeof(RequestResponse))]
        public IHttpActionResult PostRequest(Request request)
        {
            string userName = RequestContext.Principal.Identity.GetUserName();
            AspNetUser user = db.AspNetUsers.FirstOrDefault(u => u.UserName == userName);
            CardDetail card = db.CardDetails.FirstOrDefault(c => c.CardID == request.CardID);
            if (user == null)
                return BadRequest("User doesnt exists, Please Logout and try again");
            if (card == null)
                return BadRequest("This card is not registered in the System, Please contact the Administrator");
            if (request.UserName != user.UserName)
                return BadRequest("This card does not belong to the user");
            if (user.Balance < request.Amount)
                return BadRequest("The user does not have sufficient balance!");
            request.UserName = userName;
            request.Name = user.Name;
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
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            user.Balance -= request.Amount;
            db.Requests.Add(request);
            db.Transactions.Add(transaction);
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                BadRequest("Could not process request..Please try again later");
            }
            RequestResponse response = new RequestResponse(request);
            response.Balance = user.Balance;
            return CreatedAtRoute("DefaultApi", new { id = request.RequestID }, response);
        }

        // DELETE api/AddRequest/5
        /*
        [ResponseType(typeof(Request))]
        public IHttpActionResult DeleteRequest(string id)
        {
            Request request = db.Requests.Find(id);
            if (request == null)
            {
                return NotFound();
            }

            db.Requests.Remove(request);
            db.SaveChanges();

            return Ok(request);
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

        private bool RequestExists(string id)
        {
            return db.Requests.Count(e => e.RequestID == id) > 0;
        }
    }
}