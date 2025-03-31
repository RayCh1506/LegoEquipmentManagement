using EquipmentManagementPlatform.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquipmentManagementPlatform.Domain.Interfaces
{
    public interface IEquipmentIntegrationService
    {
        Task UpdateEquipmentState(int id, EquipmentState equipmentState);
        Task AssignEquipmentOrders(int id, IEnumerable<int> orderIds);
        Task AssignEquipmentOperator(int id, string equipmentOperator);
    }
}
