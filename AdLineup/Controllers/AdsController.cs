using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Web.Configuration;
using AdLineup.Models;

namespace AdLineup.Controllers
{
    public class AdsController : Controller
    {
        private LogManager logger = new LogManager();
        private AdLineupContext db = new AdLineupContext();

        // GET: AddBillboardToAd
        public ActionResult AddBillboardToAd(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ad ad = db.Ads.Find(id);
            var billboardsAvailable = db.Billboards.ToList().Except(ad.Billboards.ToList()).ToList();
            ViewBag.BillboardId = new SelectList(billboardsAvailable, "Id", "Name");
            if (ad == null)
            {
                return HttpNotFound();
            }
            AdBillboardViewModel viewModel = new AdBillboardViewModel();
            viewModel.AdId = ad.Id;
            viewModel.Ad_Name = ad.Name;
            return View(viewModel);
        }

        // POST: Ads/AddBillboardToAd/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("AddBillboardToAd")]
        [ValidateAntiForgeryToken]
        public ActionResult AddBillboardToAdPost([Bind(Include = "AdId,Ad_Name,BillboardId,Billboard_Name")] AdBillboardViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                Ad ad = db.Ads.Find(viewModel.AdId);
                Billboard billboard = db.Billboards.Find(viewModel.BillboardId);
                ad.Billboards.Add(billboard);
                db.Entry(ad).State = EntityState.Modified;
                db.SaveChanges();
                logger.Log("Ads/AddBillboardToAd/ - BillboardId:" + billboard.Id.ToString() + " to AdId: " + ad.Id.ToString());
                return RedirectToAction("Details", new { id = viewModel.AdId });
            }
            return View(viewModel);
        }

        // GET: RemoveBillboardFromAd
        public ActionResult RemoveBillboardFromAd(int? adId, int? billboardId)
        {
            if (adId == null || billboardId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ad ad = db.Ads.Find(adId);
            Billboard billboard = db.Billboards.Find(billboardId);
            if (ad == null || billboard == null)
            {
                return HttpNotFound();
            }
            ad.Billboards.Remove(billboard);
            db.Entry(ad).State = EntityState.Modified;
            db.SaveChanges();
            logger.Log("Ads/RemoveBillboardFromAd/ - BillboardId:" + billboard.Id.ToString() + " from AdId: " + ad.Id.ToString());
            return RedirectToAction("Details", new { id = adId });
        }

        // GET: Ads
        public ActionResult Index()
        {
            string printerFriendlyUrl = WebConfigurationManager.AppSettings["AppEngineUrl"];
            if (printerFriendlyUrl.EndsWith("/")) { printerFriendlyUrl = printerFriendlyUrl.TrimEnd('/'); }
            printerFriendlyUrl += ":" + WebConfigurationManager.AppSettings["AppEnginePort"];
            printerFriendlyUrl += "/api/reports/AdsIndexPrinterFriendly";
            ViewBag.PrinterFriendlyUrl = printerFriendlyUrl;
            ViewBag.AppEngineTimeout = WebConfigurationManager.AppSettings["AppEngineTimeout"];
            return View(db.Ads.ToList());
        }

        // GET: Ads/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ad ad = db.Ads.Find(id);
            if (ad == null)
            {
                return HttpNotFound();
            }
            return View(ad);
        }

        // GET: Ads/Create
        public ActionResult Create(int? id, string modelType)
        {
            if (modelType != null && modelType.Length > 0)
            {
            }
            else
            {
            }
            // modelType not always properly being passed as query string parameter so send it in ViewBag
            ViewBag.modelType = modelType;
            return View();
        }

        // POST: Ads/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Index,FlightStart,FlightEnd,ImageFilename")] Ad ad, int? id, string modelType)
        {
            string controllerName = "";
            if (ModelState.IsValid)
            {
                db.Ads.Add(ad);
                db.SaveChanges();
                logger.Log("Ads/Create - AdId:" + ad.Id.ToString());
                if (modelType != null && modelType.Length > 0)
                {
                    if (modelType.Contains("Billboard"))
                    {
                        db.Billboards.Find(id).Ads.Add(ad);
                        db.SaveChanges();
                        logger.Log("Ads/Create added - AdId:" + ad.Id.ToString() + " to BillboardId: " + id.ToString());
                        controllerName = "Billboards";
                    }
                    return RedirectToAction("Details", controllerName, new { Id = id });
                }
                return RedirectToAction("Index");
            }

            return View(ad);
        }

        // GET: Ads/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ad ad = db.Ads.Find(id);
            if (ad == null)
            {
                return HttpNotFound();
            }
            return View(ad);
        }

        // POST: Ads/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost([Bind(Include = "Id,Name,Index,FlightStart,FlightEnd,ImageFilename")] Ad ad)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ad).State = EntityState.Modified;
                db.SaveChanges();
                logger.Log("Ads/Edit/ - AdId:" + ad.Id.ToString());
                return RedirectToAction("Index");
            }
            return View(ad);
        }

        // GET: Ads/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ad ad = db.Ads.Find(id);
            if (ad == null)
            {
                return HttpNotFound();
            }
            return View(ad);
        }

        // POST: Ads/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeletePost(int id)
        {
            Ad ad = db.Ads.Find(id);
            db.Ads.Remove(ad);
            db.SaveChanges();
            logger.Log("Ads/Delete/ - AdId:" + ad.Id.ToString());
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
