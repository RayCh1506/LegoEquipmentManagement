using EquipmentManagementPlatform.Domain.Models.Enums;

namespace EquipmentManagementPlatform.Domain.Interfaces
{
    public interface IEquipmentIntegrationService
    {
        Task UpdateEquipmentState(int id, EquipmentState equipmentState);
        Task AssignEquipmentOrders(int id, IEnumerable<int> orderIds);
        Task AssignEquipmentOperator(int id, string equipmentOperator);
        Task AddOrdersToEquipment(int id, IEnumerable<int> orderIds);
    }
}
