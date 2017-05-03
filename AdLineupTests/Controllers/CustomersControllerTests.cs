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
    public class CustomersControllerTests
    {
        AdLineupContext db = new AdLineupContext();

        // GET: Customers
        [TestMethod]
        public void Index()
        {
            // Arrange
            var controller = new CustomersController();
            // Act
            ViewResult result = controller.Index() as ViewResult;
            // Assert
            Assert.IsNotNull(result);
        }

        // GET: Customers/Details/5
        [TestMethod]
        public void Details()
        {
            // Arrange
            var controller = new CustomersController();
            // Act
            ViewResult result = controller.Details(db.Customers.FirstOrDefault().Id) as ViewResult;
            // Assert
            Assert.IsNotNull(result);
        }

        // GET: Customers/Create
        [TestMethod]
        public void Create()
        {
            // Arrange
            var controller = new CustomersController();
            // Act
            ViewResult result = controller.Create() as ViewResult;
            // Assert
            Assert.IsNotNull(result);
        }

        // GET: Customers/Edit/5
        [TestMethod]
        public void Edit()
        {
            // Arrange
            var controller = new CustomersController();
            // Act
            ViewResult result = controller.Edit(db.Customers.FirstOrDefault().Id) as ViewResult;
            // Assert
            Assert.IsNotNull(result);
        }

    }
}
