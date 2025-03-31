using EquipmentManagementPlatform.API.Controllers;
using EquipmentManagementPlatform.Domain.Exceptions;
using EquipmentManagementPlatform.Domain.Interfaces;
using EquipmentManagementPlatform.Test.Unit.Helper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Moq.AutoMock;

namespace EquipmentManagementPlatform.Test.Unit.API
{
    internal class EquipmentControllerTest
    {
        [TestFixture]
        public class EquipmentControllerTests
        {
            private AutoMocker _autoMocker;
            private Mock<IEquipmentService> _equipmentServiceMock;
            private EquipmentController _controller;

            [SetUp]
            public void Setup()
            {
                _autoMocker = new AutoMocker();
                _equipmentServiceMock = _autoMocker.GetMock<IEquipmentService>();
                _controller = _autoMocker.CreateInstance<EquipmentController>();
            }

            [Test]
            public async Task GetEquipment_When_DatabaseHasEquipment_Then_ReturnsOkResult_And_EquipmentList()
            {
                // Arrange
                var mockResponse = EquipmentHelper.CreateEquipmentCollection();
                var expectedResponse = EquipmentHelper.CreateEquipmentDtoCollection();

                _equipmentServiceMock.Setup(x => x.GetAllEquipment()).Returns(Task.FromResult(mockResponse))
                    .Verifiable();

                // Act
                var response = await _controller.GetEquipment();

                // Assert
                var result = response.Result as OkObjectResult;
                result.Value.Should().BeEquivalentTo(expectedResponse);

                _equipmentServiceMock.Verify(x => x.GetAllEquipment(), Times.Once);
            }

            [Test]
            public void GetEquipment_When_DatabaseHasNoEquipment_Then_ThrowsHttpNotFoundException()
            {
                // Arrange
                var expectedException = new EquipmentNotFoundException(1);
                var mockException = new EquipmentNotFoundException(1);

                _equipmentServiceMock.Setup(x => x.GetAllEquipment()).ThrowsAsync(mockException);

                // Act
                var thrownException = Assert.ThrowsAsync<EquipmentNotFoundException>(async () => await _controller.GetEquipment());

                // Assert
                Assert.That(thrownException.Message, Is.EqualTo(expectedException.Message));
                _equipmentServiceMock.Verify(x => x.GetAllEquipment(), Times.Once);
            }
        }
    }
}
