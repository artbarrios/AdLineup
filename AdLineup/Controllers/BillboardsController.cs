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
    public class BillboardsController : Controller
    {
        private LogManager logger = new LogManager();
        private AdLineupContext db = new AdLineupContext();

        // Billboards/BillboardAdsFlowchartDiagram/1
        public ActionResult BillboardAdsFlowchartDiagram(int? id)
        {
            Billboard billboard = db.Billboards.Find(id);

            if (billboard.AdFlowchartDiagramData == null || billboard.AdFlowchartDiagramData.Length == 0)
            {
                Flowchart flowchart = BillboardAdsToFlowchart(billboard);
                billboard.AdFlowchartDiagramData = flowchart.ToJSON();
                db.Entry(billboard).State = EntityState.Modified;
                db.SaveChanges();
            }
            ViewBag.FlowchartTitle = "Ad Diagram for " + billboard.Name;
            ViewBag.FlowchartData = billboard.AdFlowchartDiagramData;
            return View(billboard);
        }

        // POST: Billboards/BillboardAdsFlowchartDiagram/1
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult BillboardAdsFlowchartDiagram([Bind(Include = "Id")] Billboard billboard, string flowchartData)
        {
            billboard = db.Billboards.Find(billboard.Id);
            if (flowchartData.Length > 0)
            {
                billboard.AdFlowchartDiagramData = flowchartData;
                db.SaveChanges();
            }
            return RedirectToAction("Details", new { id = billboard.Id });
        }

        // BillboardAdsToFlowchart
        public static Flowchart BillboardAdsToFlowchart(Billboard billboard)
        {
            // converts the specified objects into a Flowchart object and returns it
            Flowchart flowchart = new Flowchart();
            FlowchartOperator fcOperator = null;
            FlowchartOperator fcOperatorPrevious = null;
            FlowchartConnector fcInput = null;
            FlowchartConnector fcOutput = null;
            FlowchartLink fcLink = null;
            int top = 0;
            int left = 0;
            int opCount = 0;

            // check for valid input
            if (billboard != null)
            {
                flowchart.Id = billboard.Id.ToString();
                // add operators
                opCount = 1;
                foreach (Ad ad in billboard.Ads)
                {
                    fcOperator = new FlowchartOperator();
                    fcOperator.Id = "op" + billboard.Id.ToString() + ad.Id.ToString();
                    fcOperator.Title = ad.Name;
                    fcOperator.Top = top;
                    fcOperator.Left = left;
                    fcOperator.ImageSource = ad.ImageFilename;
                    top += 20;
                    left += 200;
                    // inputs
                    fcInput = new FlowchartConnector();
                    fcInput.Id = fcOperator.Id + "in1";
                    fcInput.Label = "";
                    fcOperator.Inputs.Add(fcInput);
                    // outputs
                    fcOutput = new FlowchartConnector();
                    fcOutput.Id = fcOperator.Id + "out1";
                    fcOutput.Label = "";
                    fcOperator.Outputs.Add(fcOutput);
                    // popup
                    fcOperator.Popup.header = "<h2>" + ad.Name + "</h2>";
                    fcOperator.Popup.body = @"";
                    fcOperator.Popup.body += @"<p>Name: " + ad.Name.ToString() + "</p>";
                    fcOperator.Popup.body += @"<p>Flight Start: " + ad.FlightStart.ToString() + "</p>";
                    fcOperator.Popup.body += @"<p>Flight End: " + ad.FlightEnd.ToString() + "</p>";
                    fcOperator.Popup.body += @"<img src='" + ad.ImageFilename + "' alt='Image'>";
                    // add the operator
                    flowchart.Operators.Add(fcOperator);
                    opCount += 1;
                }

                // add links
                foreach (FlowchartOperator myOperator in flowchart.Operators)
                {
                    if (fcOperatorPrevious != null)
                    {
                        fcLink = new FlowchartLink();
                        fcLink.Id = myOperator.Id + "lnk1";
                        fcLink.FromOperatorId = fcOperatorPrevious.Id;
                        fcLink.FromConnectorId = fcOperatorPrevious.Outputs.FirstOrDefault().Id;
                        fcLink.ToOperatorId = myOperator.Id;
                        fcLink.ToConnectorId = myOperator.Inputs.FirstOrDefault().Id;
                        flowchart.Links.Add(fcLink);
                    }
                    fcOperatorPrevious = myOperator;
                }
            }
            return flowchart;
        } // BillboardAdsToFlowchart ()

        // GET: AddAdToBillboard
        public ActionResult AddAdToBillboard(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Billboard billboard = db.Billboards.Find(id);
            var adsAvailable = db.Ads.ToList().Except(billboard.Ads.ToList()).ToList();
            ViewBag.AdId = new SelectList(adsAvailable, "Id", "Name");
            if (billboard == null)
            {
                return HttpNotFound();
            }
            AdBillboardViewModel viewModel = new AdBillboardViewModel();
            viewModel.BillboardId = billboard.Id;
            viewModel.Billboard_Name = billboard.Name;
            return View(viewModel);
        }

        // POST: Billboards/AddAdToBillboard/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("AddAdToBillboard")]
        [ValidateAntiForgeryToken]
        public ActionResult AddAdToBillboardPost([Bind(Include = "BillboardId,Billboard_Name,AdId,Ad_Name")] AdBillboardViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                Billboard billboard = db.Billboards.Find(viewModel.BillboardId);
                Ad ad = db.Ads.Find(viewModel.AdId);
                billboard.Ads.Add(ad);
                Flowchart flowchart = BillboardAdsToFlowchart(billboard);
                billboard.AdFlowchartDiagramData = flowchart.ToJSON();
                db.Entry(billboard).State = EntityState.Modified;
                db.SaveChanges();
                logger.Log("Billboards/AddAdToBillboard/ - AdId:" + ad.Id.ToString() + " to BillboardId: " + billboard.Id.ToString());
                return RedirectToAction("Details", new { id = viewModel.BillboardId });
            }
            return View(viewModel);
        }

        // GET: RemoveAdFromBillboard
        public ActionResult RemoveAdFromBillboard(int? billboardId, int? adId)
        {
            if (billboardId == null || adId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Billboard billboard = db.Billboards.Find(billboardId);
            Ad ad = db.Ads.Find(adId);
            if (billboard == null || ad == null)
            {
                return HttpNotFound();
            }
            billboard.Ads.Remove(ad);
                Flowchart flowchart = BillboardAdsToFlowchart(billboard);
                billboard.AdFlowchartDiagramData = flowchart.ToJSON();
            db.Entry(billboard).State = EntityState.Modified;
            db.SaveChanges();
            logger.Log("Billboards/RemoveAdFromBillboard/ - AdId:" + ad.Id.ToString() + " from BillboardId: " + billboard.Id.ToString());
            return RedirectToAction("Details", new { id = billboardId });
        }

        // GET: Billboards
        public ActionResult Index()
        {
            string printerFriendlyUrl = WebConfigurationManager.AppSettings["AppEngineUrl"];
            if (printerFriendlyUrl.EndsWith("/")) { printerFriendlyUrl = printerFriendlyUrl.TrimEnd('/'); }
            printerFriendlyUrl += ":" + WebConfigurationManager.AppSettings["AppEnginePort"];
            printerFriendlyUrl += "/api/reports/BillboardsIndexPrinterFriendly";
            ViewBag.PrinterFriendlyUrl = printerFriendlyUrl;
            ViewBag.AppEngineTimeout = WebConfigurationManager.AppSettings["AppEngineTimeout"];
            return View(db.Billboards.ToList());
        }

        // GET: Billboards/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Billboard billboard = db.Billboards.Find(id);
            if (billboard == null)
            {
                return HttpNotFound();
            }
            return View(billboard);
        }

        // GET: Billboards/Create
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

        // POST: Billboards/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Location,Latitude,Longitude,AdFlowchartDiagramData")] Billboard billboard, int? id, string modelType)
        {
            string controllerName = "";
            if (ModelState.IsValid)
            {
                db.Billboards.Add(billboard);
                db.SaveChanges();
                logger.Log("Billboards/Create - BillboardId:" + billboard.Id.ToString());
                if (modelType != null && modelType.Length > 0)
                {
                    if (modelType.Contains("Ad"))
                    {
                        db.Ads.Find(id).Billboards.Add(billboard);
                        db.SaveChanges();
                        logger.Log("Billboards/Create added - BillboardId:" + billboard.Id.ToString() + " to AdId: " + id.ToString());
                        controllerName = "Ads";
                    }
                    return RedirectToAction("Details", controllerName, new { Id = id });
                }
                return RedirectToAction("Index");
            }

            return View(billboard);
        }

        // GET: Billboards/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Billboard billboard = db.Billboards.Find(id);
            if (billboard == null)
            {
                return HttpNotFound();
            }
            return View(billboard);
        }

        // POST: Billboards/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost([Bind(Include = "Id,Name,Location,Latitude,Longitude,AdFlowchartDiagramData")] Billboard billboard)
        {
            if (ModelState.IsValid)
            {
                db.Entry(billboard).State = EntityState.Modified;
                db.SaveChanges();
                logger.Log("Billboards/Edit/ - BillboardId:" + billboard.Id.ToString());
                return RedirectToAction("Index");
            }
            return View(billboard);
        }

        // GET: Billboards/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Billboard billboard = db.Billboards.Find(id);
            if (billboard == null)
            {
                return HttpNotFound();
            }
            return View(billboard);
        }

        // POST: Billboards/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeletePost(int id)
        {
            Billboard billboard = db.Billboards.Find(id);
            db.Billboards.Remove(billboard);
            db.SaveChanges();
            logger.Log("Billboards/Delete/ - BillboardId:" + billboard.Id.ToString());
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
