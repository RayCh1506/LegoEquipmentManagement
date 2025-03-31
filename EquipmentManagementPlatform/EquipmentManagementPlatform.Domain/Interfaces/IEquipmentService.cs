using EquipmentManagementPlatform.Domain.Models;

namespace EquipmentManagementPlatform.Domain.Interfaces
{
    public interface IEquipmentService
    {
        Task<IEnumerable<Equipment>> GetAllEquipment();
        Task<Equipment> GetById(int equipmentId);
        Task UpdateEquipmentState(UpdateEquipmentStateRequest request);
        Task StartEquipment(int equipmentId, int? orderId);
        Task StopEquiment(int equipmentId);
        Task AddEquipmentOrders(int equipmentId, IEnumerable<int> equipmentOrders);
        Task AssignOperator(int equipmentId, string employee);
    }
}
