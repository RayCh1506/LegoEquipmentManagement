using EquipmentManagementPlatform.Domain.Exceptions;
using EquipmentManagementPlatform.Domain.Interfaces;
using EquipmentManagementPlatform.Domain.Models;
using EquipmentManagementPlatform.Domain.Models.Enums;
using Microsoft.Extensions.Logging;

namespace EquipmentManagementPlatform.DomainServices
{
    public class EquipmentService : IEquipmentService
    {
        private readonly ILogger<EquipmentService> _logger;
        private readonly IEquipmentRepository _equipmentRepository;
        private readonly IEquipmentIntegrationService _integrationService;

        public EquipmentService(ILogger<EquipmentService> logger, IEquipmentRepository equipmentRepository, IEquipmentIntegrationService integrationService)
        {
            _logger = logger;   
            _equipmentRepository = equipmentRepository;
            _integrationService = integrationService;
        }

        public async Task<IEnumerable<Equipment>> GetAllEquipment()
        {
            return await _equipmentRepository.GetAllEquipmentAsync();
        }

        public async Task<Equipment> GetById(int equipmentId)
        {
            return await _equipmentRepository.GetById(equipmentId);
        }

        public async Task UpdateEquipmentState(UpdateEquipmentStateRequest request)
        {
            var equipment = await _equipmentRepository.GetById(request.EquipmentId);

            if (!IsValidTransition(equipment.GeneralInformation.State, request.NewState))
                throw new InvalidEquipmentStateTransitionException(equipment.GeneralInformation.State, request.NewState);

            if (request.NewState.Equals(EquipmentState.RED) && request.OrderId is not null)
            {
                request.OrderId = null;
            }
            else if (equipment.GeneralInformation.State.Equals(EquipmentState.RED))
            {
                if(equipment.OrderInformation.AssignedOrders is null || !equipment.OrderInformation.AssignedOrders.Any())
                    throw new EquipmentStartException($"Equipment with id {equipment.Id} has no assigned orders to start on");

                if(request.OrderId is null)
                {
                    request.OrderId = equipment.OrderInformation.AssignedOrders.First();
                }
                else
                {
                    if(!equipment.OrderInformation.AssignedOrders.Contains((int) request.OrderId))
                        throw new EquipmentStartException($"Equipment with id {equipment.Id} does not have the order with id {request.OrderId} assigned to it, please assign the order to the equipment first");
                }
            }

            await _integrationService.UpdateEquipmentState(request.EquipmentId, request.NewState);
            await _equipmentRepository.UpdateEquipmentState(request);
        }

        public async Task StartEquipment(int equipmentId, int? orderId)
        {
            var equipment = await _equipmentRepository.GetById(equipmentId);
            
            ValidateStartEquipmentConditions(equipment, orderId);

            // Added integration mock in the repository for simplicity
            await _equipmentRepository.StartEquipment(equipmentId, orderId ?? equipment.OrderInformation.AssignedOrders.First());
        }

        public async Task StopEquiment(int equipmentId)
        {
            var equipment = await _equipmentRepository.GetById(equipmentId);

            ValidateStopEquipmentConditions(equipment);

            // Added integration mock in the repository for simplicity
            await _equipmentRepository.StopEquipment(equipmentId);
        }

        public async Task AddEquipmentOrders(int equipmentId, IEnumerable<int> equipmentOrders)
        {
            await _integrationService.AddOrdersToEquipment(equipmentId, equipmentOrders);
            await _equipmentRepository.AddEquipmentOrders(equipmentId, equipmentOrders);
        }

        public async Task AssignOperator(int equipmentId, string employee)
        {
            await _integrationService.AssignEquipmentOperator(equipmentId, employee);
            await _equipmentRepository.AssignOperator(equipmentId, employee);
        }

        private static bool IsValidTransition(EquipmentState currentState, EquipmentState newState)
        {
            return currentState switch
            {
                EquipmentState.RED => newState == EquipmentState.YELLOW,
                EquipmentState.YELLOW => newState == EquipmentState.GREEN || newState == EquipmentState.RED,
                EquipmentState.GREEN => newState == EquipmentState.YELLOW,
                _ => false
            };
        }

        private static void ValidateStartEquipmentConditions(Equipment equipment, int? orderId)
        {
            if (equipment.GeneralInformation.State.Equals(EquipmentState.YELLOW))
                throw new EquipmentStartException($"Equipment with id {equipment.Id} is in the processing of starting/stopping, it is currently not possible to start the equipment");
            if (equipment.GeneralInformation.State.Equals(EquipmentState.GREEN))
                throw new EquipmentStartException($"Equipment with id {equipment.Id} is already running, please stop the equipment first before starting it");

            if(orderId is null)
            {
                if(equipment.OrderInformation.AssignedOrders is null || !equipment.OrderInformation.AssignedOrders.Any())
                    throw new EquipmentStartException($"Equipment with id {equipment.Id} has no assigned orders to start on");
            }
            else
            {
                if (!equipment.OrderInformation.AssignedOrders.ToList().Contains((int)orderId))
                    throw new EquipmentStartException($"Equipment with id {equipment.Id} does not have a scheduled order with id: {orderId} please schedule it first");
            }
            
        }

        private static void ValidateStopEquipmentConditions(Equipment equipment)
        {
            if (equipment.GeneralInformation.State.Equals(EquipmentState.YELLOW))
                throw new EquipmentStopException($"Equipment with id {equipment.Id} is in the processing of starting/stopping, it is currently not possible to stop the equipment");
            if (equipment.GeneralInformation.State.Equals(EquipmentState.RED))
                throw new EquipmentStopException($"Equipment with id {equipment.Id} is already stopped, please start the equipment first before stopping it");
        }
    }
}
