using EquipmentManagementPlatform.Domain.Exceptions;
using EquipmentManagementPlatform.Domain.Interfaces;
using EquipmentManagementPlatform.Domain.Models;
using EquipmentManagementPlatform.Domain.Models.Enums;
using EquipmentManagementPlatform.Repository.Context;
using EquipmentManagementPlatform.Repository.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EquipmentManagementPlatform.Repository
{
    public class EquipmentRepository : IEquipmentRepository
    {
        private readonly ILogger<EquipmentRepository> _logger;
        private readonly EquipmentContext _equipmentContext;

        public EquipmentRepository(ILogger<EquipmentRepository> logger, EquipmentContext equipmentContext)
        {
            _logger = logger;
            _equipmentContext = equipmentContext;    
        }

        public async Task<IEnumerable<Equipment>> GetAllEquipmentAsync()
        {
            _logger.LogInformation("Querying from db: all equipment fetch");

            var equipmentEntities = await _equipmentContext.Equipment.Include(e => e.HistoricStates).ToListAsync();

            return equipmentEntities.ConvertAll(ConvertEquipmentEntityToDomainModel);

        }

        public async Task<Equipment> GetById(int equipmentId)
        {
            _logger.LogInformation("Querying from db: equipment fetch for {equipmentId}", equipmentId);

            var entity = await _equipmentContext.Equipment.Include(e => e.HistoricStates).SingleOrDefaultAsync(e => e.Id.Equals(equipmentId)) ??
                throw new EquipmentNotFoundException(equipmentId);

            return (ConvertEquipmentEntityToDomainModel(entity));
        }
 
        public async Task UpdateEquipmentState(UpdateEquipmentStateRequest request)
        {
            _logger.LogInformation("Persisting equipment update for {id} to state {newState}", request.EquipmentId, request.NewState);
            
            var equipment = await _equipmentContext.Equipment.Include(e => e.HistoricStates).SingleOrDefaultAsync(e => e.Id.Equals(request.EquipmentId)) ?? 
                throw new EquipmentNotFoundException(request.EquipmentId);

            if (!equipment.CurrentOrder.Equals(request.OrderId))
                equipment.CurrentOrder = request.OrderId;

            var historicState = new EquipmentHistoricStateEntity(equipment.State, request.NewState.ToString(), DateTime.UtcNow, request.OrderId);

            equipment.HistoricStates.Add(historicState);
            equipment.State = request.NewState.ToString(); 

            await _equipmentContext.SaveChangesAsync();
        }

        public async Task StartEquipment(int equipmentId, int orderId)
        {
            _logger.LogInformation("Persisting start equipment action for {equipmentId}", equipmentId);

            var equipment = await _equipmentContext.Equipment.FindAsync(equipmentId) ??
                throw new EquipmentNotFoundException(equipmentId);
            equipment.CurrentOrder = orderId;

            await _equipmentContext.SaveChangesAsync();

            // Simulate equipment starting up
            await UpdateEquipmentState(new UpdateEquipmentStateRequest(equipmentId, EquipmentState.YELLOW.ToString(), orderId));
            await Task.Delay(5000);
            await UpdateEquipmentState(new UpdateEquipmentStateRequest(equipmentId, EquipmentState.GREEN.ToString(), orderId));
        }

        public async Task StopEquipment(int equipmentId)
        {
            _logger.LogInformation("Persisting stop equipment action for {equipmentId}", equipmentId);

            var equipment = await _equipmentContext.Equipment.FindAsync(equipmentId) ??
                throw new EquipmentNotFoundException(equipmentId);
            equipment.CurrentOrder = null;

            await _equipmentContext.SaveChangesAsync();

            // Simulate equipment stopping up
            await UpdateEquipmentState(new UpdateEquipmentStateRequest(equipmentId, EquipmentState.YELLOW.ToString(), equipment.CurrentOrder));
            await Task.Delay(5000);
            await UpdateEquipmentState(new UpdateEquipmentStateRequest(equipmentId, EquipmentState.RED.ToString(), equipment.CurrentOrder));
        }

        public async Task AddEquipmentOrders(int equipmentId, IEnumerable<int> equipmentOrders)
        {
            _logger.LogInformation("Persisting new orders to {equipmentId}: {orders}", equipmentId, string.Join(",", equipmentOrders));
            var equipment = await _equipmentContext.Equipment.FindAsync(equipmentId) ??
                throw new EquipmentNotFoundException(equipmentId);
            equipment.AssignedOrders = [..equipment.AssignedOrders, ..equipmentOrders];

            await _equipmentContext.SaveChangesAsync();
        }

        public async Task AssignOperator(int equipmentId, string employee)
        {
            _logger.LogInformation("Persisting assignment employee: {employee} to equipment with id: {id}", employee, equipmentId);

            var equipment = await _equipmentContext.Equipment.FindAsync(equipmentId) ??
                throw new EquipmentNotFoundException(equipmentId);

            equipment.Operator = employee;

            await _equipmentContext.SaveChangesAsync();
        }

        private static Equipment ConvertEquipmentEntityToDomainModel(EquipmentEntity entity)
        {
            var historicStates = entity.HistoricStates.ToList().ConvertAll(h => new EquipmentHistoricState(h.FromState, h.ToState, h.TimeOfChange, h.OrderId));

            EquipmentState equipmentState;
            if (!Enum.TryParse(entity.State, out equipmentState))
                throw new UnexpectedEnumValueException<EquipmentState>(entity.State);

            return new Equipment(
                entity.Id,
                new GeneralInformation(entity.Name, entity.Location, equipmentState),
                new OperationalInformation(entity.IsOperational, entity.FaultMessage, entity.OverallEquipmentEffectiveness, entity.Operator),
                new OrderInformation(entity.AssignedOrders, entity.CurrentOrder),
                historicStates
                );
        }
    }
}
