using EquipmentManagementPlatform.Domain.Interfaces;
using Moq.AutoMock;
using Moq;
using EquipmentManagementPlatform.Domain.Models;
using EquipmentManagementPlatform.Domain.Models.Enums;
using EquipmentManagementPlatform.DomainServices;
using EquipmentManagementPlatform.Test.Unit.Helper;
using EquipmentManagementPlatform.Domain.Exceptions;

namespace EquipmentManagementPlatform.Test.Unit.DomainServices
{
    [TestFixture]
    internal class EquipmentServiceTest
    {
        private AutoMocker _autoMocker;
        private Mock<IEquipmentRepository> _equipmentRepositoryMock;
        private Mock<IEquipmentIntegrationService> _equipmentIntegrationServiceMock;
        private EquipmentService _equipmenService;

        [SetUp]
        public void Setup()
        {
            _autoMocker = new AutoMocker();
            _equipmentRepositoryMock = _autoMocker.GetMock<IEquipmentRepository>();
            _equipmentIntegrationServiceMock = _autoMocker.GetMock<IEquipmentIntegrationService>();
            _equipmenService = _autoMocker.CreateInstance<EquipmentService>();
        }

        [Test]
        public void UpdateEquipmentState_Given_OrderIdIsIncluded_And_OrderIsInAssignedOrders_And_NewStateIsValidTransitionState_When_UpdateEquipmentStateRequestIsReceived_Then_UpdatesDatabaseWithCorrectNewState_And_OrderId()
        {
            // Arrange
            int equipmentId = 1;
            var request = new UpdateEquipmentStateRequest(equipmentId, EquipmentState.YELLOW.ToString());
            var mockResponse = EquipmentHelper.CreateEquipment(equipmentId);

            _equipmentRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(Task.FromResult(mockResponse));
            _equipmentRepositoryMock.Setup(x => x.UpdateEquipmentState(It.IsAny<UpdateEquipmentStateRequest>())).Returns(Task.CompletedTask).Verifiable();
            _equipmentIntegrationServiceMock.Setup(x => x.UpdateEquipmentState(It.IsAny<int>(), It.IsAny<EquipmentState>())).Returns(Task.CompletedTask).Verifiable();

            // Act & Assert
            Assert.DoesNotThrowAsync(async () => await _equipmenService.UpdateEquipmentState(request));

            _equipmentRepositoryMock.Verify(x => x.GetById(It.Is<int>(id => id.Equals(request.EquipmentId))), Times.Once);
            _equipmentRepositoryMock.Verify(x => x.UpdateEquipmentState(It.Is<UpdateEquipmentStateRequest>(r => r.EquipmentId.Equals(request.EquipmentId) && r.NewState.Equals(request.NewState) && r.OrderId.Equals(request.OrderId))), Times.Once);
            _equipmentIntegrationServiceMock.Verify(x => x.UpdateEquipmentState(It.Is<int>(id => id.Equals(request.EquipmentId)), It.Is<EquipmentState>(s => s.Equals(request.NewState))), Times.Once);
        }

        [Test]
        public void UpdateEquipmentState_Given_EquipmentStateIsRed_And_OrderListIsEmpty_When_UpdateEquipmentStateRequestYellowTransitionIsReceived_Then_ThrowsEquipmentStartException()
        {
            // Arrange
            int equipmentId = 1;
            var request = new UpdateEquipmentStateRequest(equipmentId, EquipmentState.YELLOW.ToString());
            var mockResponse = EquipmentHelper.CreateEquipment(equipmentId, state: EquipmentState.RED, isOrderListEmpty: true);

            var expectedErrorMessage = $"Equipment with id {equipmentId} has no assigned orders to start on";

            _equipmentRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(Task.FromResult(mockResponse));
            _equipmentRepositoryMock.Setup(x => x.UpdateEquipmentState(It.IsAny<UpdateEquipmentStateRequest>())).Returns(Task.CompletedTask).Verifiable();
            _equipmentIntegrationServiceMock.Setup(x => x.UpdateEquipmentState(It.IsAny<int>(), It.IsAny<EquipmentState>())).Returns(Task.CompletedTask).Verifiable();

            // Act & Assert
            var receivedException = Assert.ThrowsAsync<EquipmentStartException>(async () => await _equipmenService.UpdateEquipmentState(request));

            Assert.That(receivedException.Message, Is.EqualTo(expectedErrorMessage));

            _equipmentRepositoryMock.Verify(x => x.GetById(It.Is<int>(id => id.Equals(request.EquipmentId))), Times.Once);
            _equipmentRepositoryMock.Verify(x => x.UpdateEquipmentState(It.Is<UpdateEquipmentStateRequest>(r => r.EquipmentId.Equals(request.EquipmentId) && r.NewState.Equals(request.NewState) && r.OrderId.Equals(request.OrderId))), Times.Never);
            _equipmentIntegrationServiceMock.Verify(x => x.UpdateEquipmentState(It.Is<int>(id => id.Equals(request.EquipmentId)), It.Is<EquipmentState>(s => s.Equals(request.NewState))), Times.Never);
        }

        [Test]
        public void UpdateEquipmentState_Given_EquipmentStateIsRed_And_OrderListItemsDoNotMatchReceivedOrderId_When_UpdateEquipmentStateRequestYellowTransitionIsReceived_Then_ThrowsEquipmentStartException()
        {
            // Arrange
            int equipmentId = 1;
            var request = new UpdateEquipmentStateRequest(equipmentId, EquipmentState.YELLOW.ToString(), 9999999);
            var mockResponse = EquipmentHelper.CreateEquipment(equipmentId, state: EquipmentState.RED, isOrderListEmpty: false);

            var expectedErrorMessage = $"Equipment with id {equipmentId} does not have the order with id {request.OrderId} assigned to it, please assign the order to the equipment first";

            _equipmentRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(Task.FromResult(mockResponse));
            _equipmentRepositoryMock.Setup(x => x.UpdateEquipmentState(It.IsAny<UpdateEquipmentStateRequest>())).Returns(Task.CompletedTask).Verifiable();
            _equipmentIntegrationServiceMock.Setup(x => x.UpdateEquipmentState(It.IsAny<int>(), It.IsAny<EquipmentState>())).Returns(Task.CompletedTask).Verifiable();

            // Act & Assert
            var receivedException = Assert.ThrowsAsync<EquipmentStartException>(async () => await _equipmenService.UpdateEquipmentState(request));

            Assert.That(receivedException.Message, Is.EqualTo(expectedErrorMessage));

            _equipmentRepositoryMock.Verify(x => x.GetById(It.Is<int>(id => id.Equals(request.EquipmentId))), Times.Once);
            _equipmentRepositoryMock.Verify(x => x.UpdateEquipmentState(It.Is<UpdateEquipmentStateRequest>(r => r.EquipmentId.Equals(request.EquipmentId) && r.NewState.Equals(request.NewState) && r.OrderId.Equals(request.OrderId))), Times.Never);
            _equipmentIntegrationServiceMock.Verify(x => x.UpdateEquipmentState(It.Is<int>(id => id.Equals(request.EquipmentId)), It.Is<EquipmentState>(s => s.Equals(request.NewState))), Times.Never);
        }

        [Test]
        public void UpdateEquipmentState_Given_EquipmentStateIsRed_When_UpdateEquipmentStateRequestGreenTransitionIsReceived_Then_ThrowsInvalidEquipmentStateTransitionException()
        {
            // Arrange
            int equipmentId = 1;
            var request = new UpdateEquipmentStateRequest(equipmentId, EquipmentState.GREEN.ToString());
            var mockResponse = EquipmentHelper.CreateEquipment(equipmentId, state: EquipmentState.RED);

            var expectedErrorMessage = $"Equipment state cannot go from {EquipmentState.RED} to {EquipmentState.GREEN}";

            _equipmentRepositoryMock.Setup(x => x.GetById(It.IsAny<int>())).Returns(Task.FromResult(mockResponse));
            _equipmentRepositoryMock.Setup(x => x.UpdateEquipmentState(It.IsAny<UpdateEquipmentStateRequest>())).Returns(Task.CompletedTask).Verifiable();
            _equipmentIntegrationServiceMock.Setup(x => x.UpdateEquipmentState(It.IsAny<int>(), It.IsAny<EquipmentState>())).Returns(Task.CompletedTask).Verifiable();

            // Act & Assert
            var receivedException = Assert.ThrowsAsync<InvalidEquipmentStateTransitionException>(async () => await _equipmenService.UpdateEquipmentState(request));

            Assert.That(receivedException.Message, Is.EqualTo(expectedErrorMessage));

            _equipmentRepositoryMock.Verify(x => x.GetById(It.Is<int>(id => id.Equals(request.EquipmentId))), Times.Once);
            _equipmentRepositoryMock.Verify(x => x.UpdateEquipmentState(It.Is<UpdateEquipmentStateRequest>(r => r.EquipmentId.Equals(request.EquipmentId) && r.NewState.Equals(request.NewState) && r.OrderId.Equals(request.OrderId))), Times.Never);
            _equipmentIntegrationServiceMock.Verify(x => x.UpdateEquipmentState(It.Is<int>(id => id.Equals(request.EquipmentId)), It.Is<EquipmentState>(s => s.Equals(request.NewState))), Times.Never);
        }

    }
}
