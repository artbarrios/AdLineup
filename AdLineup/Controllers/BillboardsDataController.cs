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
using AdLineup.Models;

namespace AdLineup.Controllers
{
    public class BillboardsDataController : ApiController
    {
        private LogManager logger = new LogManager();
        private AdLineupContext db = new AdLineupContext();

        // GET: api/GetBillboardAds/?BillboardId=1
        [Route("api/GetBillboardAds/")]
        public List<Ad> GetBillboardAds(int BillboardId)
        {
            Billboard billboard = db.Billboards.Find(BillboardId);
            if (billboard == null)
            {
                return null;
            }
            return billboard.Ads;
        }

        // PUT: api/AddAdToBillboard/?BillboardId=1&AdId=1
        [HttpPut]
        [Route("api/AddAdToBillboard/")]
        [ResponseType(typeof(void))]
        public IHttpActionResult AddAdToBillboard(int BillboardId, int AdId)
        {
            Billboard billboard = db.Billboards.Find(BillboardId);
            Ad ad = db.Ads.Find(AdId);
            if (billboard != null && ad != null)
            {
                try
                {
                    billboard.Ads.Add(ad);
                    db.Entry(billboard).State = EntityState.Modified;
                    db.SaveChanges();
                    logger.Log("api/AddAdToBillboard - BillboardId:" + billboard.Id.ToString() + " AdId:" + ad.Id.ToString());
                }
                catch
                {
                    throw;
                }
            }
            else
            {
                return NotFound();
            }
            return StatusCode(HttpStatusCode.NoContent);
        } // AddAdToBillboard

        // PUT: api/RemoveAdFromBillboard/?BillboardId=1&AdId=1
        [HttpPut]
        [Route("api/RemoveAdFromBillboard/")]
        [ResponseType(typeof(void))]
        public IHttpActionResult RemoveAdFromBillboard(int BillboardId, int AdId)
        {
            Billboard billboard = db.Billboards.Find(BillboardId);
            Ad ad = db.Ads.Find(AdId);
            if (billboard != null && ad != null)
            {
                try
                {
                    billboard.Ads.Remove(ad);
                    db.Entry(billboard).State = EntityState.Modified;
                    db.SaveChanges();
                    logger.Log("api/RemoveAdToBillboard - BillboardId:" + billboard.Id.ToString() + " AdId:" + ad.Id.ToString());
                }
                catch
                {
                    throw;
                }
            }
            else
            {
                return NotFound();
            }
            return StatusCode(HttpStatusCode.NoContent);
        } // RemoveAdFromBillboard

        // GET: api/BillboardsData
        public IQueryable<Billboard> GetBillboards()
        {
            return db.Billboards;
        }

        // GET: api/BillboardsData/5
        [ResponseType(typeof(Billboard))]
        public IHttpActionResult GetBillboard(int id)
        {
            Billboard billboard = db.Billboards.Find(id);
            if (billboard == null)
            {
                return NotFound();
            }

            return Ok(billboard);
        }

        // PUT: api/BillboardsData/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutBillboard(int id, Billboard billboard)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != billboard.Id)
            {
                return BadRequest();
            }

            db.Entry(billboard).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
                logger.Log("PUT: api/BillboardsData/ - BillboardId:" + billboard.Id.ToString());
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BillboardExists(id))
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

        // POST: api/BillboardsData
        [ResponseType(typeof(Billboard))]
        public IHttpActionResult PostBillboard(Billboard billboard)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Billboards.Add(billboard);
            db.SaveChanges();
            logger.Log("POST: api/BillboardsData/ - BillboardId:" + billboard.Id.ToString());

            return CreatedAtRoute("DefaultApi", new { id = billboard.Id }, billboard);
        }

        // DELETE: api/BillboardsData/5
        [ResponseType(typeof(Billboard))]
        public IHttpActionResult DeleteBillboard(int id)
        {
            Billboard billboard = db.Billboards.Find(id);
            if (billboard == null)
            {
                return NotFound();
            }

            db.Billboards.Remove(billboard);
            db.SaveChanges();
            logger.Log("DELETE: api/BillboardsData/ - BillboardId:" + billboard.Id.ToString());

            return Ok(billboard);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool BillboardExists(int id)
        {
            return db.Billboards.Count(e => e.Id == id) > 0;
        }
    }
}
