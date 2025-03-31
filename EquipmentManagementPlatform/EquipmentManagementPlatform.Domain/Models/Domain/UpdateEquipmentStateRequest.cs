using EquipmentManagementPlatform.Domain.Models.Enums;

namespace EquipmentManagementPlatform.Domain.Models
{
    public class UpdateEquipmentStateRequest
    {
        public UpdateEquipmentStateRequest(int equipmentId, string newState, int? orderId) : this(equipmentId, newState)
        {
            OrderId = orderId;
        }

        public UpdateEquipmentStateRequest(int equipmentId, string newState)
        {
            EquipmentId = equipmentId;

            if (!Enum.TryParse(newState, out EquipmentState newEnumState))
                throw new ArgumentException($"{newState} is not a valid equipment state");
            NewState = newEnumState;
        }

        public int EquipmentId { get; set; }
        public EquipmentState NewState { get; set; }
        public int? OrderId { get; set; }
    }
}
