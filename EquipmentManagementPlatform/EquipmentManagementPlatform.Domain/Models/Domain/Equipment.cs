using EquipmentManagementPlatform.Domain.Models.Enums;

namespace EquipmentManagementPlatform.Domain.Models
{
    public class Equipment
    {
        public Equipment(int id, GeneralInformation generalInformation, OperationalInformation operationalInformation, OrderInformation orderInformation, IEnumerable<EquipmentHistoricState> historicStates)
        {
            Id = id;
            GeneralInformation = generalInformation;
            OperationalInformation = operationalInformation;
            OrderInformation = orderInformation;
            HistoricStates = historicStates;
        }
        public int Id { get; set; }
        public GeneralInformation GeneralInformation { get; set; }
        public OperationalInformation OperationalInformation { get; set; }
        public OrderInformation OrderInformation { get; set; }
        public IEnumerable<EquipmentHistoricState> HistoricStates { get; set; }
        
        
    }

    public class GeneralInformation
    {
        public GeneralInformation(string name, string location, EquipmentState state)
        {
            
            Name = name;
            Location = location;
            State = state;
        }

        public string Name { get; set; }
        public string Location { get; set; }
        public EquipmentState State { get; set; }
    }

    public class OperationalInformation
    {
        public OperationalInformation(bool isOperational, string faultMessage, double overallEquipmentEffectiveness, string employee)
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

    public class OrderInformation
    {
        public OrderInformation(IEnumerable<int> assignedOrders, int? currentOrder) :this(assignedOrders)
        {
            CurrentOrder = currentOrder;
        }

        public OrderInformation(IEnumerable<int> assignedOrders)
        {
            AssignedOrders = assignedOrders;
        }

        public int? CurrentOrder { get; set; }
        public IEnumerable<int> AssignedOrders { get; set; }
    }
}
