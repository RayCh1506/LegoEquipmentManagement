namespace EquipmentManagementPlatform.DomainServices.Exceptions
{
    public class EquipmentNotFoundException : Exception
    {
        public EquipmentNotFoundException(int equipmentId) : base($"Equipment with id {equipmentId} was not found") { }

    }
}
