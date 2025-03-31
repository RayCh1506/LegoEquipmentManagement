using EquipmentManagementPlatform.Domain.Models;

namespace EquipmentManagementPlatform.Domain.Interfaces
{
    public interface IEquipmentRepository
    {
        Task<IEnumerable<Equipment>> GetAllEquipmentAsync();
        Task<Equipment> GetById(int equipmentId);
        Task UpdateEquipmentState(UpdateEquipmentStateRequest request);
        Task StartEquipment(int equipmentId, int orderId);
        Task StopEquipment(int equipmentId);
        Task AddEquipmentOrders(int equipmentId, IEnumerable<int> equipmentOrders);
        Task AssignOperator(int equipmentId, string employee);
    }
}
