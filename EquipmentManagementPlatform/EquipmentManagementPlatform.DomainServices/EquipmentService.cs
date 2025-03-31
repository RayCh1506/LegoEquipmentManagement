using EquipmentManagementPlatform.Domain.Exceptions;
using EquipmentManagementPlatform.Domain.Interfaces;
using EquipmentManagementPlatform.Domain.Models;
using Microsoft.Extensions.Logging;

namespace EquipmentManagementPlatform.DomainServices
{
    public class EquipmentService : IEquipmentService
    {
        private readonly ILogger<EquipmentService> _logger;
        private readonly IEquipmentRepository _equipmentRepository;

        public EquipmentService(ILogger<EquipmentService> logger, IEquipmentRepository equipmentRepository)
        {
            _logger = logger;   
            _equipmentRepository = equipmentRepository;
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

            if (!IsValidTransition(equipment.State, request.NewState))
                throw new InvalidEquipmentStateTransitionException(equipment.State, request.NewState);

            if (request.NewState.Equals(EquipmentState.RED) && request.OrderId is not null)
            {
                request.OrderId = null;
            }
            else if (equipment.State.Equals(EquipmentState.RED) && request.OrderId is null)
            {
                if(equipment.AssignedOrders is null || !equipment.AssignedOrders.Any())
                    throw new EquipmentStartException($"Equipment with id {equipment.Id} has no assigned orders to start on");

                request.OrderId = equipment.AssignedOrders.First();
            }

            await _equipmentRepository.UpdateEquipmentState(request);
        }

        public async Task StartEquipment(int equipmentId, int? orderId)
        {
            var equipment = await _equipmentRepository.GetById(equipmentId);
            
            ValidateStartEquipmentConditions(equipment, orderId);

            await _equipmentRepository.StartEquipment(equipmentId, equipment.AssignedOrders.First());
        }

        public async Task StopEquiment(int equipmentId)
        {
            var equipment = await _equipmentRepository.GetById(equipmentId);

            ValidateStopEquipmentConditions(equipment);

            await _equipmentRepository.StopEquipment(equipmentId);
        }

        public async Task AddEquipmentOrders(int equipmentId, IEnumerable<int> equipmentOrders)
        {
            await _equipmentRepository.AddEquipmentOrders(equipmentId, equipmentOrders);
        }

        public async Task AssignOperator(int equipmentId, string employee)
        {
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
            if (equipment.State.Equals(EquipmentState.YELLOW))
                throw new EquipmentStartException($"Equipment with id {equipment.Id} is in the processing of starting/stopping, it is currently not possible to start the equipment");
            if (equipment.State.Equals(EquipmentState.GREEN))
                throw new EquipmentStartException($"Equipment with id {equipment.Id} is already running, please stop the equipment first before starting it");

            if(orderId is null)
            {
                if(equipment.AssignedOrders is null || !equipment.AssignedOrders.Any())
                    throw new EquipmentStartException($"Equipment with id {equipment.Id} has no assigned orders to start on");
            }
            else
            {
                if (!equipment.AssignedOrders.ToList().Contains((int)orderId))
                    throw new EquipmentStartException($"Equipment with id {equipment.Id} does not have a scheduled order with id: {orderId} please schedule it first");
            }
            
        }

        private static void ValidateStopEquipmentConditions(Equipment equipment)
        {
            if (equipment.State.Equals(EquipmentState.YELLOW))
                throw new EquipmentStopException($"Equipment with id {equipment.Id} is in the processing of starting/stopping, it is currently not possible to stop the equipment");
            if (equipment.State.Equals(EquipmentState.RED))
                throw new EquipmentStopException($"Equipment with id {equipment.Id} is already stopped, please start the equipment first before stopping it");
        }
    }
}
