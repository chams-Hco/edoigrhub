using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Mvc;
using CICSWebPortal.Controllers;
using CICSWebPortal.Services;
using CICSWebPortal.Models;

namespace CICSWebPortal.Tests.Controllers
{
    /// <summary>
    /// Summary description for RevenueControllerTest
    /// </summary>
    [TestClass]
    public class RevenueControllerTest
    {
        IDataService DataContext = null;
        RevenueController controller = null;

        [TestInitialize()]
        public void MyTestInitialize()
        {
            DataContext = new MockDataService();
            controller = new RevenueController(DataContext);
        }
        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 

        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void index()
        {
            var result = controller.Index() as ViewResult;

            var resultTask = result;
            var model = (List<Revenue>)result.ViewData.Model;

            Assert.IsNotNull(resultTask);

           // Assert.AreEqual(7, model.Count);
        }
    }
}
