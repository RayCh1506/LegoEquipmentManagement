using EquipmentManagementPlatform.Domain.Models.Dtos;

namespace EquipmentManagementPlatform.Domain.Models
{
    public class EquipmentDto
    {
        public EquipmentDto(int id, string name, string location, EquipmentState state, bool isOperational, string faultMessage, IEnumerable<EquipmentHistoricState> historicStates, int? currentOrder,
            IEnumerable<int> assignedOrders, double overallEquipmentEffectiveness, string employee)
        {
            Id = id;
            Name = name;
            Location = location;
            State = state.ToString();
            IsOperational = isOperational;
            FaultMessage = faultMessage;
            HistoricStates = historicStates.ToList().ConvertAll(h => new EquipmentHistoricStateDto(h.FromState, h.ToState, h.TimeOfChange, h.OrderId));
            CurrentOrder = currentOrder;
            AssignedOrders = assignedOrders;
            OverallEquipmentEffectiveness = overallEquipmentEffectiveness;
            Operator = employee;
        }

        public EquipmentDto(Equipment equipment) : this(equipment.Id, equipment.Name, equipment.Location, equipment.State, equipment.IsOperational, equipment.FaultMessage, equipment.HistoricStates,
            equipment.CurrentOrder, equipment.AssignedOrders, equipment.OverallEquipmentEffectiveness, equipment.Operator) { }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string State { get; set; }
        public bool IsOperational { get; set; }
        public string FaultMessage { get; set; }
        public IEnumerable<EquipmentHistoricStateDto> HistoricStates { get; set; }
        public int? CurrentOrder { get; set; }
        public IEnumerable<int> AssignedOrders { get; set; }
        public double OverallEquipmentEffectiveness { get; set; }
        public string Operator { get; set; }
    }
}
