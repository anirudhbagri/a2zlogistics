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
using System.Web.Routing;
using FinalProject.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using FinalProject.Helper;
using FinalProject.ResponseClass;
using System.Threading.Tasks;

namespace FinalProject.Controllers
{
    [ModifiedAuthorize(Roles = "Customer")]
    public class GetUserController : ApiController
    {
        private DataEntities db = new DataEntities();
        public GetUserController()
            : this(new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext())))
        {
        }
        public GetUserController(UserManager<ApplicationUser> userManager)
        {
            UserManager = userManager;
        }
        public UserManager<ApplicationUser> UserManager { get; private set; }
        // GET api/GetUser
        /*
        public IQueryable<AspNetUser> GetAspNetUsers()
        {
            return db.AspNetUsers;
        }*/

        // GET api/GetUser/5
        [ResponseType(typeof(AspNetUser))]
        public IHttpActionResult GetAspNetUser()
        {
            string userId = RequestContext.Principal.Identity.GetUserId();
            AspNetUser aspnetuser = db.AspNetUsers.Find(userId);
            UserResponse userResponse = new UserResponse();
            if (aspnetuser == null)
            {
                return NotFound();
            }
            userResponse.Balance = aspnetuser.Balance;
            userResponse.Mobile_Number = aspnetuser.Mobile_Number;
            userResponse.Name = aspnetuser.Name;
            userResponse.UserName = aspnetuser.UserName;
            userResponse.Discount = aspnetuser.Discount;
            return Ok(userResponse);
        }

        // PUT api/GetUser/5
        /*public IHttpActionResult PutAspNetUser(string id, AspNetUser aspnetuser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != aspnetuser.Id)
            {
                return BadRequest();
            }

            db.Entry(aspnetuser).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AspNetUserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }*/

        // POST api/GetUser
        [ResponseType(typeof(AspNetUser))]
        public async Task<IHttpActionResult> PostAspNetUser(ManageUserViewModel model)
        {
            string userId = RequestContext.Principal.Identity.GetUserId();
            AspNetUser aspnetuser = db.AspNetUsers.Find(userId);
            
            if (ModelState.IsValid)
            {
                IdentityResult result = await UserManager.ChangePasswordAsync(aspnetuser.Id, model.OldPassword, model.NewPassword);

                if (result.Succeeded)
                {
                    Result resultResponse = new Result();
                    resultResponse.result = "Password changed!";
                    return Ok(resultResponse);
                }
                else
                {
                    return BadRequest("Something bad happened..we were unable to change password!");
                }
            }
            else
            {
                return BadRequest("Something bad happened..we were unable to change password!");
            }
        }
        class Result
        {
            public string result;
        }
        // POST api/GetUser
        /*[ResponseType(typeof(AspNetUser))]
        public IHttpActionResult PostAspNetUser(AspNetUser aspnetuser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            db.AspNetUsers.Add(aspnetuser);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (AspNetUserExists(aspnetuser.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = aspnetuser.Id }, aspnetuser);
        }
        */

        // DELETE api/GetUser/5
        /*[ResponseType(typeof(AspNetUser))]
        public IHttpActionResult DeleteAspNetUser(string id)
        {
            AspNetUser aspnetuser = db.AspNetUsers.Find(id);
            if (aspnetuser == null)
            {
                return NotFound();
            }

            db.AspNetUsers.Remove(aspnetuser);
            db.SaveChanges();

            return Ok(aspnetuser);
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

        private bool AspNetUserExists(string id)
        {
            return db.AspNetUsers.Count(e => e.Id == id) > 0;
        }
    }
}