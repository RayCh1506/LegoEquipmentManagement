using EquipmentManagementPlatform.Domain.Exceptions;

namespace EquipmentManagementPlatform.Domain.Models
{
    public class EquipmentHistoricState
    {
        public EquipmentHistoricState(EquipmentState fromState, EquipmentState toState, DateTime timeOfChange, int? orderId)
        {
            FromState = fromState;
            ToState = toState;
            TimeOfChange = timeOfChange;
            OrderId = orderId;
        }

        public EquipmentHistoricState(string fromState, string toState, DateTime timeOfChange, int? orderId)
        {
            FromState = ParseState(fromState);
            ToState = ParseState(toState);
            TimeOfChange = timeOfChange;
            OrderId = orderId;
        }

        public EquipmentState FromState { get; set; }
        public EquipmentState ToState { get; set; }
        public DateTime TimeOfChange { get; set; }
        public int? OrderId { get; set; }

        private static EquipmentState ParseState(string state)
        {
            if (!Enum.TryParse(state, out EquipmentState parsedState))
                throw new UnexpectedEnumValueException<EquipmentState>(state);
            return parsedState;
        }
    }
}