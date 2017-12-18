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
    public class GetCardController : ApiController
    {
        private DataEntities db = new DataEntities();

        // GET api/GetCard
        public IQueryable<CardDetail> GetCardDetails()
        {
            string userName = RequestContext.Principal.Identity.GetUserName();
            IQueryable<CardDetail> tempCards = db.CardDetails.Where(c => c.UserName == userName);
            return tempCards;
        }

        // GET api/GetCard/5
        /*[ResponseType(typeof(CardDetail))]
        public IHttpActionResult GetCardDetail(string id)
        {
            CardDetail carddetail = db.CardDetails.Find(id);
            if (carddetail == null)
            {
                return NotFound();
            }

            return Ok(carddetail);
        }*/

        // PUT api/GetCard/5
        /*public IHttpActionResult PutCardDetail(string id, CardDetail carddetail)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != carddetail.CardID)
            {
                return BadRequest();
            }

            db.Entry(carddetail).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CardDetailExists(id))
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

        // POST api/GetCard
        /*[ResponseType(typeof(CardDetail))]
        public IHttpActionResult PostCardDetail(CardDetail carddetail)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.CardDetails.Add(carddetail);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (CardDetailExists(carddetail.CardID))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = carddetail.CardID }, carddetail);
        }*/

        // DELETE api/GetCard/5
        /*[ResponseType(typeof(CardDetail))]
        public IHttpActionResult DeleteCardDetail(string id)
        {
            CardDetail carddetail = db.CardDetails.Find(id);
            if (carddetail == null)
            {
                return NotFound();
            }

            db.CardDetails.Remove(carddetail);
            db.SaveChanges();

            return Ok(carddetail);
        }*/

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CardDetailExists(string id)
        {
            return db.CardDetails.Count(e => e.CardID == id) > 0;
        }
    }
}