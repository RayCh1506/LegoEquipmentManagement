namespace EquipmentManagementPlatform.DomainServices.Dtos
{
    public class UpdateStateDto
    {
        public UpdateStateDto(string newState, int? orderId)
        {
            NewState = newState;
            OrderId = orderId;
        }

        public string NewState { get; set; }
        public int? OrderId { get; set; }
    }
}
