using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ModelLib;
using RestItemServer.Controllers;

namespace UnitTestProjectRest
{
    [TestClass]
    public class UnitTest1
    {
        private ItemsController cntr = null;

        [TestInitialize]
        public void Init()
        {
            cntr = new ItemsController();
        }

        [TestMethod]
        public void TestMethod1()
        {
            //act
            List<Item> liste = new List<Item>(cntr.Get());

            //assert
            Assert.AreEqual(5, liste.Count);
        }
    }
}
