using EquipmentManagementPlatform.Domain.Models;
using EquipmentManagementPlatform.Domain.Models.Enums;

namespace EquipmentManagementPlatform.Domain.Exceptions
{
    public class InvalidEquipmentStateTransitionException : Exception
    {
        public InvalidEquipmentStateTransitionException(EquipmentState fromState, EquipmentState toState) : base($"Equipment state cannot go from {fromState} to {toState}") { }
    }
}
