namespace EquipmentManagementPlatform.DomainServices.Exceptions
{
    public class EquipmentStopException : Exception
    {
        public EquipmentStopException(string errorMessage) : base(errorMessage) { }
    }
}
