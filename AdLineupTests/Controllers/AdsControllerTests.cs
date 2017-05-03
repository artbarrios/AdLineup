using System;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AdLineup.Controllers;
using AdLineup.Models;
using System.Linq;
using System.Collections.Generic;

namespace AdLineupTests.Controllers
{

    [TestClass]
    public class AdsControllerTests
    {
        AdLineupContext db = new AdLineupContext();

        // GET: AddBillboardToAd
        [TestMethod]
        public void AddBillboardToAd()
        {
            // Arrange
            AdsController controller = new AdsController();
            Billboard billboard = db.Billboards.FirstOrDefault();
            // Act
            ViewResult result = controller.AddBillboardToAd(billboard.Id) as ViewResult;
            // Assert
            Assert.IsNotNull(result);
        }

        // POST: Ads/AddBillboardToAd/5
        [TestMethod]
        public void AddBillboardToAdPost()
        {
            // Arrange
            AdsController controller = new AdsController();
            AdBillboardViewModel viewModel = new AdBillboardViewModel();
            Billboard billboard = db.Billboards.FirstOrDefault();
            Ad ad = db.Ads.FirstOrDefault();
            viewModel.BillboardId = billboard.Id;
            viewModel.AdId = ad.Id;
            // Act
            ViewResult result = controller.AddBillboardToAdPost(viewModel) as ViewResult;
            var method = typeof(AdsController).GetMethod("AddBillboardToAdPost");
            IEnumerable<HttpPostAttribute> postAttribute = method.GetCustomAttributes(typeof(HttpPostAttribute), false).Cast<HttpPostAttribute>();
            IEnumerable<ActionNameAttribute> actionNameAttribute = method.GetCustomAttributes(typeof(ActionNameAttribute), false).Cast<ActionNameAttribute>();
            // Assert
            Assert.IsNull(result);
            Assert.IsTrue(postAttribute.Count() > 0);
            Assert.IsTrue(actionNameAttribute.Count() > 0);
        }

        // GET: Ads
        [TestMethod]
        public void Index()
        {
            // Arrange
            var controller = new AdsController();
            // Act
            ViewResult result = controller.Index() as ViewResult;
            // Assert
            Assert.IsNotNull(result);
        }

        // GET: Ads/Details/5
        [TestMethod]
        public void Details()
        {
            // Arrange
            var controller = new AdsController();
            // Act
            ViewResult result = controller.Details(db.Ads.FirstOrDefault().Id) as ViewResult;
            // Assert
            Assert.IsNotNull(result);
        }

        // GET: Ads/Create
        [TestMethod]
        public void Create()
        {
            // Arrange
            var controller = new AdsController();
            // Act
            ViewResult result = controller.Create(-1, "") as ViewResult;
            // Assert
            Assert.IsNotNull(result);
        }

        // GET: Ads/Edit/5
        [TestMethod]
        public void Edit()
        {
            // Arrange
            var controller = new AdsController();
            // Act
            ViewResult result = controller.Edit(db.Ads.FirstOrDefault().Id) as ViewResult;
            // Assert
            Assert.IsNotNull(result);
        }

    }
}
