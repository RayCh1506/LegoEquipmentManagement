using EquipmentManagementPlatform.DomainServices.Dtos;

namespace EquipmentManagementPlatform.Domain.Models
{
    public class EquipmentDto
    {
        public EquipmentDto(Equipment equipment)
        {
            Id = equipment.Id;
            GeneralInformation = new GeneralInformationDto(equipment.GeneralInformation);
            OperationalInformation = new OperationalInformationDto(equipment.OperationalInformation);
            OrderInformation = new OrderInformationDto(equipment.OrderInformation);
            HistoricStates = equipment.HistoricStates.Select(hs => new EquipmentHistoricStateDto(hs)).ToList();
        }

        public EquipmentDto(int id, GeneralInformationDto generalInformation, OperationalInformationDto operationalInformation, OrderInformationDto orderInformation, IEnumerable<EquipmentHistoricStateDto> historicStates)
        {
            Id = id;
            GeneralInformation = generalInformation;
            OperationalInformation = operationalInformation;
            OrderInformation = orderInformation;
            HistoricStates = historicStates;
        }

        public int Id { get; set; }
        public GeneralInformationDto GeneralInformation { get; set; }
        public OperationalInformationDto OperationalInformation { get; set; }
        public OrderInformationDto OrderInformation { get; set; }
        public IEnumerable<EquipmentHistoricStateDto> HistoricStates { get; set; }
    }

    public class GeneralInformationDto
    {
        public GeneralInformationDto(GeneralInformation generalInformation)
        {
            Name = generalInformation.Name;
            Location = generalInformation.Location;
            State = generalInformation.State.ToString();
        }

        public GeneralInformationDto(string name, string location, string state)
        {
            Name = name;
            Location = location;
            State = state;
        }

        public string Name { get; set; }
        public string Location { get; set; }
        public string State { get; set; }
    }

    public class OperationalInformationDto
    {
        public OperationalInformationDto(OperationalInformation operationalInformation)
        {
            IsOperational = operationalInformation.IsOperational;
            FaultMessage = operationalInformation.FaultMessage;
            OverallEquipmentEffectiveness = operationalInformation.OverallEquipmentEffectiveness;
            Operator = operationalInformation.Operator;
        }

        public OperationalInformationDto(bool isOperational, string faultMessage, double overallEquipmentEffectiveness, string employee)
        {
            IsOperational = isOperational;
            FaultMessage = faultMessage;
            OverallEquipmentEffectiveness = overallEquipmentEffectiveness;
            Operator = employee;
        }

        public bool IsOperational { get; set; }
        public string FaultMessage { get; set; }
        public double OverallEquipmentEffectiveness { get; set; }
        public string Operator { get; set; }
    }

    public class OrderInformationDto
    {
        public OrderInformationDto(int? currentOrder, IEnumerable<int> assignedOrders)
        {
            CurrentOrder = currentOrder;
            AssignedOrders = assignedOrders;
        }

        public OrderInformationDto(OrderInformation orderInformation)
        {
            CurrentOrder = orderInformation.CurrentOrder;
            AssignedOrders = orderInformation.AssignedOrders.ToList();
        }

        public int? CurrentOrder { get; set; }
        public IEnumerable<int> AssignedOrders { get; set; }
    }
}
