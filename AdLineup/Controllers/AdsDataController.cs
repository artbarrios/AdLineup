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
    public class AdsDataController : ApiController
    {
        private LogManager logger = new LogManager();
        private AdLineupContext db = new AdLineupContext();

        // GET: api/GetAdBillboards/?AdId=1
        [Route("api/GetAdBillboards/")]
        public List<Billboard> GetAdBillboards(int AdId)
        {
            Ad ad = db.Ads.Find(AdId);
            if (ad == null)
            {
                return null;
            }
            return ad.Billboards;
        }

        // PUT: api/AddBillboardToAd/?AdId=1&BillboardId=1
        [HttpPut]
        [Route("api/AddBillboardToAd/")]
        [ResponseType(typeof(void))]
        public IHttpActionResult AddBillboardToAd(int AdId, int BillboardId)
        {
            Ad ad = db.Ads.Find(AdId);
            Billboard billboard = db.Billboards.Find(BillboardId);
            if (ad != null && billboard != null)
            {
                try
                {
                    ad.Billboards.Add(billboard);
                    db.Entry(ad).State = EntityState.Modified;
                    db.SaveChanges();
                    logger.Log("api/AddBillboardToAd - AdId:" + ad.Id.ToString() + " BillboardId:" + billboard.Id.ToString());
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
        } // AddBillboardToAd

        // PUT: api/RemoveBillboardFromAd/?AdId=1&BillboardId=1
        [HttpPut]
        [Route("api/RemoveBillboardFromAd/")]
        [ResponseType(typeof(void))]
        public IHttpActionResult RemoveBillboardFromAd(int AdId, int BillboardId)
        {
            Ad ad = db.Ads.Find(AdId);
            Billboard billboard = db.Billboards.Find(BillboardId);
            if (ad != null && billboard != null)
            {
                try
                {
                    ad.Billboards.Remove(billboard);
                    db.Entry(ad).State = EntityState.Modified;
                    db.SaveChanges();
                    logger.Log("api/RemoveBillboardToAd - AdId:" + ad.Id.ToString() + " BillboardId:" + billboard.Id.ToString());
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
        } // RemoveBillboardFromAd

        // GET: api/AdsData
        public IQueryable<Ad> GetAds()
        {
            return db.Ads;
        }

        // GET: api/AdsData/5
        [ResponseType(typeof(Ad))]
        public IHttpActionResult GetAd(int id)
        {
            Ad ad = db.Ads.Find(id);
            if (ad == null)
            {
                return NotFound();
            }

            return Ok(ad);
        }

        // PUT: api/AdsData/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutAd(int id, Ad ad)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != ad.Id)
            {
                return BadRequest();
            }

            db.Entry(ad).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
                logger.Log("PUT: api/AdsData/ - AdId:" + ad.Id.ToString());
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AdExists(id))
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

        // POST: api/AdsData
        [ResponseType(typeof(Ad))]
        public IHttpActionResult PostAd(Ad ad)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Ads.Add(ad);
            db.SaveChanges();
            logger.Log("POST: api/AdsData/ - AdId:" + ad.Id.ToString());

            return CreatedAtRoute("DefaultApi", new { id = ad.Id }, ad);
        }

        // DELETE: api/AdsData/5
        [ResponseType(typeof(Ad))]
        public IHttpActionResult DeleteAd(int id)
        {
            Ad ad = db.Ads.Find(id);
            if (ad == null)
            {
                return NotFound();
            }

            db.Ads.Remove(ad);
            db.SaveChanges();
            logger.Log("DELETE: api/AdsData/ - AdId:" + ad.Id.ToString());

            return Ok(ad);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AdExists(int id)
        {
            return db.Ads.Count(e => e.Id == id) > 0;
        }
    }
}
