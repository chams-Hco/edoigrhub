using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CICSWebPortal.Services;
using CICSWebPortal.Controllers;
using System.Web.Mvc;
using System.Threading.Tasks;
using CICSWebPortal.Models;

namespace CICSWebPortal.Tests.Controllers
{
    /// <summary>
    /// Summary description for ClientControllerTest
    /// </summary>
    [TestClass]
    public class ClientControllerTest
    {
        IDataService DataContext=null;
        ClientController controller=null;
        // Use TestInitialize to run code before running each test 
        [TestInitialize()]
        public void MyTestInitialize() {

            DataContext = new MockDataService();
            controller = new ClientController(DataContext);
        
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

        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void Index()
        {
           var  result = controller.Index() as ViewResult;

           var resultTask = result;
           var model = (List<Client>)result.ViewData.Model;

           Assert.IsNotNull(resultTask);

            Assert.AreEqual(7, model.Count);


        }

        [TestMethod]
        public void  Create()
        {
            var result =  controller.Create();
       
            Assert.IsNotNull(result);

        }


        [TestMethod]
        public void CreateWithParam()
        {
            Client client = new Client
            {
                ClientId = 8,
                ClientName = "Olawale",
                Address = "21, Ileepo road",
                Email = "ade@gmail.com",
                PhoneNo1 = "097434343",
                PhoneNo2 = "0843434343",
                Status = true
            };
            controller.Create(client);

            List<Client> clients = (List<Client>)DataContext.GetAllClients();

            CollectionAssert.Contains(clients, client);


        }



        [TestMethod]
        public void Edit()
        {

            Client client = new Client
            {
                ClientId = 8,
                ClientName = "Olawale",
                Address = "21, Ileepo road",
                Email = "ade@gmail.com",
                PhoneNo1 = "097434343",
                PhoneNo2 = "0843434343",
                Status = true
            };


            controller.Edit(client);

            // get the list of books
            List<Client> Clients = (List<Client>)DataContext.GetAllClients();

            CollectionAssert.Contains(Clients, client);
        }


        
    }
}
