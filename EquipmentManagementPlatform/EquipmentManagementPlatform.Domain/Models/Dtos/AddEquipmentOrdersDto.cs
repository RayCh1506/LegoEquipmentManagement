namespace EquipmentManagementPlatform.Domain.Models.Dtos
{
    public class AddEquipmentOrdersDto
    {
        public AddEquipmentOrdersDto(IEnumerable<int> equipmentOrders)
        {
            EquipmentOrders = equipmentOrders;
        }

        public IEnumerable<int> EquipmentOrders { get; set; }
    }
}
