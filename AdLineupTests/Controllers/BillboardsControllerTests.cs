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
    public class BillboardsControllerTests
    {
        AdLineupContext db = new AdLineupContext();

        // Billboards/BillboardAdsFlowchartDiagram/1
        [TestMethod]
        public void BillboardAdsFlowchartDiagram()
        {
            // Arrange
            BillboardsController controller = new BillboardsController();
            Billboard billboard = db.Billboards.FirstOrDefault();
            // Act
            ViewResult result = controller.BillboardAdsFlowchartDiagram(billboard.Id) as ViewResult;
            // Assert
            Assert.IsNotNull(result);
        }

        // GET: AddAdToBillboard
        [TestMethod]
        public void AddAdToBillboard()
        {
            // Arrange
            BillboardsController controller = new BillboardsController();
            Ad ad = db.Ads.FirstOrDefault();
            // Act
            ViewResult result = controller.AddAdToBillboard(ad.Id) as ViewResult;
            // Assert
            Assert.IsNotNull(result);
        }

        // POST: Billboards/AddAdToBillboard/5
        [TestMethod]
        public void AddAdToBillboardPost()
        {
            // Arrange
            BillboardsController controller = new BillboardsController();
            AdBillboardViewModel viewModel = new AdBillboardViewModel();
            Ad ad = db.Ads.FirstOrDefault();
            Billboard billboard = db.Billboards.FirstOrDefault();
            viewModel.BillboardId = billboard.Id;
            viewModel.AdId = ad.Id;
            // Act
            ViewResult result = controller.AddAdToBillboardPost(viewModel) as ViewResult;
            var method = typeof(BillboardsController).GetMethod("AddAdToBillboardPost");
            IEnumerable<HttpPostAttribute> postAttribute = method.GetCustomAttributes(typeof(HttpPostAttribute), false).Cast<HttpPostAttribute>();
            IEnumerable<ActionNameAttribute> actionNameAttribute = method.GetCustomAttributes(typeof(ActionNameAttribute), false).Cast<ActionNameAttribute>();
            // Assert
            Assert.IsNull(result);
            Assert.IsTrue(postAttribute.Count() > 0);
            Assert.IsTrue(actionNameAttribute.Count() > 0);
        }

        // GET: Billboards
        [TestMethod]
        public void Index()
        {
            // Arrange
            var controller = new BillboardsController();
            // Act
            ViewResult result = controller.Index() as ViewResult;
            // Assert
            Assert.IsNotNull(result);
        }

        // GET: Billboards/Details/5
        [TestMethod]
        public void Details()
        {
            // Arrange
            var controller = new BillboardsController();
            // Act
            ViewResult result = controller.Details(db.Billboards.FirstOrDefault().Id) as ViewResult;
            // Assert
            Assert.IsNotNull(result);
        }

        // GET: Billboards/Create
        [TestMethod]
        public void Create()
        {
            // Arrange
            var controller = new BillboardsController();
            // Act
            ViewResult result = controller.Create(-1, "") as ViewResult;
            // Assert
            Assert.IsNotNull(result);
        }

        // GET: Billboards/Edit/5
        [TestMethod]
        public void Edit()
        {
            // Arrange
            var controller = new BillboardsController();
            // Act
            ViewResult result = controller.Edit(db.Billboards.FirstOrDefault().Id) as ViewResult;
            // Assert
            Assert.IsNotNull(result);
        }

    }
}
