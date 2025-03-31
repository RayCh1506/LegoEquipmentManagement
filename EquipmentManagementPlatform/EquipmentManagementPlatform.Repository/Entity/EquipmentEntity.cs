using EquipmentManagementPlatform.Domain.Models;
using EquipmentManagementPlatform.Domain.Models.Enums;

namespace EquipmentManagementPlatform.Repository.Entity
{
    public class EquipmentEntity
    {
        private EquipmentEntity() { }

        public EquipmentEntity(int id, string name, string location, EquipmentState state, bool isOperational, string faultMessage, IEnumerable<EquipmentHistoricState> historicStates, int currentOrder,
            IEnumerable<int> assignedOrders, double overallEquipmentEffectiveness, string employee)
            : this(id, name, location, state, isOperational, faultMessage, historicStates, assignedOrders, overallEquipmentEffectiveness, employee)
        {
            CurrentOrder = currentOrder;
        }

        public EquipmentEntity(int id, string name, string location, EquipmentState state, bool isOperational, string faultMessage, IEnumerable<EquipmentHistoricState> historicStates,
            IEnumerable<int> assignedOrders, double overallEquipmentEffectiveness, string employee)
        {
            Id = id;
            Name = name;
            Location = location;
            State = state.ToString();
            IsOperational = isOperational;
            FaultMessage = faultMessage;
            HistoricStates = historicStates.ToList().ConvertAll(historicState => new EquipmentHistoricStateEntity(historicState.FromState, historicState.ToState, historicState.TimeOfChange, historicState.OrderId));
            AssignedOrders = assignedOrders.ToList();
            OverallEquipmentEffectiveness = overallEquipmentEffectiveness;
            Operator = employee;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string State { get; set; }
        public bool IsOperational { get; set; }
        public string FaultMessage { get; set; }
        public virtual ICollection<EquipmentHistoricStateEntity> HistoricStates { get; set; }
        public int? CurrentOrder { get; set; }
        public virtual ICollection<int> AssignedOrders { get; set; }
        public double OverallEquipmentEffectiveness { get; set; }
        public string Operator { get; set; }
    }
}
