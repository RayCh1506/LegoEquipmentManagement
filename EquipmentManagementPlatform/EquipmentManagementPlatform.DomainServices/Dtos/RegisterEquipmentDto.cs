namespace EquipmentManagementPlatform.DomainServices.Dtos
{
    public class RegisterEquipmentDto
    {
        public RegisterEquipmentDto(string name, string location, string state, bool isOperational, string faultMessage)
        {
            Name = name;
            Location = location;
            State = state;
            FaultMessage = faultMessage;
        }

        public string Name { get; set; }
        public string Location { get; set; }
        public string State { get; set; }
        public bool IsOperational { get; set; }
        public string FaultMessage { get; set; }
    }
}
