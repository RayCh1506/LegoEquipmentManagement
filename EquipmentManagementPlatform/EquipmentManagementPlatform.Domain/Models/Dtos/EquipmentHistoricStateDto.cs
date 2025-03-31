namespace EquipmentManagementPlatform.Domain.Models.Dtos
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


        public string FromState { get; set; }
        public string ToState { get; set; }
        public DateTime TimeOfChange { get; set; }
        public int? OrderId { get; set; }
    }
}
