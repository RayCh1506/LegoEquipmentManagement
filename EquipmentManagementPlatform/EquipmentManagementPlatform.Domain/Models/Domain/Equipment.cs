namespace EquipmentManagementPlatform.Domain.Models
{
    public class Equipment
    {
        public Equipment(int id, string name, string location, EquipmentState state, bool isOperational, string faultMessage, IEnumerable<EquipmentHistoricState> historicStates,
            IEnumerable<int> assignedOrders, double overallEquipmentEffectiveness, string employee)
        {
            Id = id;
            Name = name;
            Location = location;
            State = state;
            IsOperational = isOperational;
            FaultMessage = faultMessage;
            HistoricStates = historicStates;
            AssignedOrders = assignedOrders;
            OverallEquipmentEffectiveness = overallEquipmentEffectiveness;
            Operator = employee;
        }

        public Equipment(int id, string name, string location, EquipmentState state, bool isOperational, string faultMessage, IEnumerable<EquipmentHistoricState> historicStates, int? currentOrder,
            IEnumerable<int> assignedOrders, double overallEquipmentEffectiveness, string employee)
            : this(id, name, location, state, isOperational, faultMessage, historicStates, assignedOrders, overallEquipmentEffectiveness, employee)
        {
            CurrentOrder = currentOrder;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public EquipmentState State { get; set; }
        public bool IsOperational { get; set; }
        public string FaultMessage {  get; set; }
        public IEnumerable<EquipmentHistoricState> HistoricStates { get; set; }
        public int? CurrentOrder { get; set; }
        public IEnumerable<int> AssignedOrders { get; set; }
        public double OverallEquipmentEffectiveness { get; set; }
        public string Operator { get; set; }
    }
}
