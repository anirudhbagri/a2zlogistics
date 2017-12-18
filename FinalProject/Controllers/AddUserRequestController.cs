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
using FinalProject.Helper;
using System.Web;
using Microsoft.AspNet.Identity;

namespace FinalProject.Controllers
{
    [ModifiedAuthorize(Roles = "Customer")]
    public class AddUserRequestController : ApiController
    {
        private DataEntities db = new DataEntities();
        /*
        // GET api/AddUserRequest
        public IQueryable<UserRequest> GetUserRequests()
        {
            return db.UserRequests;
        }

        // GET api/AddUserRequest/5
        [ResponseType(typeof(UserRequest))]
        public IHttpActionResult GetUserRequest(string id)
        {
            UserRequest userrequest = db.UserRequests.Find(id);
            if (userrequest == null)
            {
                return NotFound();
            }

            return Ok(userrequest);
        }
        
        // PUT api/AddUserRequest/5
        public IHttpActionResult PutUserRequest(string id, UserRequest userrequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != userrequest.TransactionID)
            {
                return BadRequest();
            }

            db.Entry(userrequest).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserRequestExists(id))
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
        // POST api/AddUserRequest
        [ResponseType(typeof(UserRequest))]
        public IHttpActionResult PostUserRequest(UserRequest userrequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //string username = RequestContext.Principal.Identity.GetUserName();
            //AspNetUser user = db.AspNetUsers.FirstOrDefault(u => u.UserName == username);
            //if (user == null)
            //    return BadRequest("Something went wrong, Please Logout and try again");
            /*UserTransaction userTransaction = new UserTransaction
            {
                Amount = userrequest.Amount,
                Mode = userrequest.Mode,
                Username = userrequest.Username,
                Status = "Pending"
            };*/
            db.UserRequests.Add(userrequest);
            //db.UserTransactions.Add(userTransaction);
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (UserRequestExists(userrequest.ID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = userrequest.ID }, userrequest);
        }
        /*
        // DELETE api/AddUserRequest/5
        [ResponseType(typeof(UserRequest))]
        public IHttpActionResult DeleteUserRequest(string id)
        {
            UserRequest userrequest = db.UserRequests.Find(id);
            if (userrequest == null)
            {
                return NotFound();
            }

            db.UserRequests.Remove(userrequest);
            db.SaveChanges();

            return Ok(userrequest);
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

        private bool UserRequestExists(int id)
        {
            return db.UserRequests.Count(e => e.ID == id) > 0;
        }
    }
}