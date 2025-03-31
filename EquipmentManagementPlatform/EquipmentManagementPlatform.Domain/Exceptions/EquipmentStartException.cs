namespace EquipmentManagementPlatform.Domain.Exceptions
{
    public class EquipmentStartException : Exception
    {
        public EquipmentStartException(string errorMessage) : base(errorMessage) { }
    }
}
