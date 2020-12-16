using AuditCheckList.Controllers;
using AuditCheckList.Provider;
using Castle.Core.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Net;

namespace AuditCheckListTests
{
    [TestFixture]
    public class Tests
    {
        private Mock<IAuditProvider> _provider;
        private AuditCheckListController controllerObj;
        [SetUp]
        public void Setup()
        {
            _provider = new Mock<IAuditProvider>();
            controllerObj = new AuditCheckListController(_provider.Object);
        }
        [Test]
        public void ReturnInternalList()
        {
            _provider.Setup(p => p.GetList("Internal")).Returns(new List<string>
            {
            "Have all Change requests followed SDLC before PROD move?",
            "Have all Change requests been approved by the application owner?",
            "Are all artifacts like CR document, Unit test cases available?",
            "Is the SIT and UAT sign-off available?",
            "Is data deletion from the system done with application owner approval?"
            });
            var result = controllerObj.Get("Internal");

            Assert.That(result, Is.InstanceOf<OkObjectResult>());
        }
        [Test]
        public void ReturnSOXList()
        {
            _provider.Setup(p => p.GetList("SOX")).Returns(new List<string>
            {
           "Have all Change requests followed SDLC before PROD move? ",
            "Have all Change requests been approved by the application owner?",
            "For a major change, was there a database backup taken before and after PROD move?",
            "Has the application owner approval obtained while adding a user to the system?",
            "Is data deletion from the system done with application owner approval"
            });
            var result = controllerObj.Get("SOX");

            Assert.That(result, Is.InstanceOf<OkObjectResult>());
        }
        [Test]
        public void ReturnNullList()
        {
            _provider.Setup(p => p.GetList(null));
            var result = controllerObj.Get(null) as ObjectResult;
            Assert.AreEqual(404, result.StatusCode);
            Assert.AreEqual("No questions are present", result.Value);
        }
        [Test]
        public void ReturnNullForinternal()
        {
            _provider.Setup(p => p.GetList("internal"));
            var result = controllerObj.Get("internal") as ObjectResult;
            Assert.AreEqual(404, result.StatusCode);
        }

    }
}