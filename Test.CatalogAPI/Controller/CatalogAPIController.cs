using KamilCataLogAPI.Controllers;
using KamilCataLogAPI.Model.Configurations;
using KamilCataLogAPI.Model.DTO;
using KamilCataLogAPI.QueryObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Test.CatalogAPI.Controller
{
    [TestClass]
    public class CatalogAPIController
    {
        private  Mock<ICatalogService> _mockCatalogService;
        private Mock<IOptions<CatalogAPISetting>> _mockAPIOptions;
        private Mock<IOptionsSnapshot<CatalogAPISetting>> _mockSnapShotOptions;
        private Mock<IOptionsMonitor<CatalogAPISetting>> _mockMonitorOptions;
        private Mock<IOptionsSnapshot<CatalogAPISwaggerConfigurationOptions>> _mockSwaggerConfiguration;
        private Mock<IOptionsSnapshot<MessageQueueConfiguration>> _mockMessageQueueConfiguration;

        /// <summary>
        /// Runs once per test case.
        /// </summary>
        [TestInitialize]
        public  void TestClassIntialize()
        {
            // mock other services.
            _mockAPIOptions = new Mock<IOptions<CatalogAPISetting>>();
            _mockMessageQueueConfiguration = new Mock<IOptionsSnapshot<MessageQueueConfiguration>>();
            _mockMonitorOptions = new Mock<IOptionsMonitor<CatalogAPISetting>>();
            _mockSnapShotOptions = new Mock<IOptionsSnapshot<CatalogAPISetting>>();
            _mockSwaggerConfiguration = new Mock<IOptionsSnapshot<CatalogAPISwaggerConfigurationOptions>>();
        }


        [TestMethod]
        [TestCategory("Success")]
        public void TestGetCatalogItemsByBrandReturnsValidData()
        {
            // arrange
            IEnumerable<CatalogItemDTO> catalogItem = new List<CatalogItemDTO>
            {
                new CatalogItemDTO{
                    Brand = "Test1Brand",
                    AvailableStock = 20,
                    Description ="Product one Description",
                    PictureUri="test_url",
                    Price = 10,
                    Title = "Product_title",
                    Type = "Product _type"
                }

            };

            this._mockCatalogService = new Mock<ICatalogService>();
            this._mockCatalogService
                .Setup(s => s.GetCatalogItemsByBrand(1))
                .Returns(catalogItem);

            

            // Act 

            var catalogController = new CatalogController(this._mockAPIOptions.Object,
                                            this._mockSnapShotOptions.Object,
                                            this._mockMonitorOptions.Object,
                                            this._mockSwaggerConfiguration.Object,
                                            this._mockMessageQueueConfiguration.Object,
                                            this._mockCatalogService.Object);

            var result = catalogController.GetCatalogItemsByBrand(1);

            // Assert

            _mockCatalogService.Verify(r => r.GetCatalogItemsByBrand(1));  // confirm that this mock method invoked by test case or nor.

            var objectResult = result.Result as OkObjectResult;
            Assert.AreEqual(200, objectResult.StatusCode);

            var viewModel = objectResult.Value as IEnumerable<CatalogItemDTO>; 
            Assert.AreEqual(10, viewModel.FirstOrDefault().Price);
        }

        [TestMethod]
        [TestCategory("BadRequest")]
        public void TestGetCatalogByBrandReturnBadRequest()
        {
            // Arrange
           this._mockCatalogService = new Mock<ICatalogService>();

            // Act
            var catalogController = new CatalogController(this._mockAPIOptions.Object,
                                      this._mockSnapShotOptions.Object,
                                      this._mockMonitorOptions.Object,
                                      this._mockSwaggerConfiguration.Object,
                                      this._mockMessageQueueConfiguration.Object,
                                      this._mockCatalogService.Object);

            var result = catalogController.GetCatalogItemsByBrand(0);

            // Assert 

            var objectResult = result.Result as BadRequestResult;

            Assert.AreEqual((int)HttpStatusCode.BadRequest, objectResult.StatusCode);
        }

        [TestMethod]
        [TestCategory("NotFound")]
        public void TestGetCatalogItemByBrandRetursNotFound()
        {
            // Arrange
            IEnumerable<CatalogItemDTO> catalogItem = new List<CatalogItemDTO>
            {
                // emty list
            };
            this._mockCatalogService = new Mock<ICatalogService>();
            this._mockCatalogService
                .Setup(s => s.GetCatalogItemsByBrand(1))
                .Returns(catalogItem);

            // Act
            var catalogController = new CatalogController(this._mockAPIOptions.Object,
                                      this._mockSnapShotOptions.Object,
                                      this._mockMonitorOptions.Object,
                                      this._mockSwaggerConfiguration.Object,
                                      this._mockMessageQueueConfiguration.Object,
                                      this._mockCatalogService.Object);

            var result = catalogController.GetCatalogItemsByBrand(1);

            // Assert 
            this._mockCatalogService.Verify(r => r.GetCatalogItemsByBrand(1));
            var objectResult = result.Result as NotFoundResult;
            Assert.AreEqual((int)HttpStatusCode.NotFound, objectResult.StatusCode);
        }
    }
}
