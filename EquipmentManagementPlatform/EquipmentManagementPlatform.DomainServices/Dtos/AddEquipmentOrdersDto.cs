namespace EquipmentManagementPlatform.DomainServices.Dtos
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
