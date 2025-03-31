using EquipmentManagementPlatform.Domain.Models;
using EquipmentManagementPlatform.Domain.Models.Enums;

namespace EquipmentManagementPlatform.DomainServices.Dtos
{
    public class EquipmentHistoricStateDto
    {
        public EquipmentHistoricStateDto(EquipmentState fromState, EquipmentState toState, DateTime timeOfChange, int? orderId)
        {
            FromState = fromState.ToString();
            ToState = toState.ToString();
            TimeOfChange = timeOfChange;
            OrderId = orderId;
        }

        public EquipmentHistoricStateDto(EquipmentHistoricState historicState)
        {
            FromState = historicState.FromState.ToString();
            ToState = historicState.ToState.ToString();
            TimeOfChange = historicState.TimeOfChange;
            OrderId = historicState.OrderId;
        }

        public string FromState { get; set; }
        public string ToState { get; set; }
        public DateTime TimeOfChange { get; set; }
        public int? OrderId { get; set; }
    }
}
