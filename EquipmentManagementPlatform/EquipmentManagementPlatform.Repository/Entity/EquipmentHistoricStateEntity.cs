using EquipmentManagementPlatform.Domain.Models;
using EquipmentManagementPlatform.Domain.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EquipmentManagementPlatform.Repository.Entity
{
    public class EquipmentHistoricStateEntity
    {
        private EquipmentHistoricStateEntity() { }

        public EquipmentHistoricStateEntity(EquipmentState fromState, EquipmentState toState, DateTime timeOfChange, int? orderId)
        {
            FromState = fromState.ToString();
            ToState = toState.ToString();
            TimeOfChange = timeOfChange;
            OrderId = orderId;
        }

        public EquipmentHistoricStateEntity(string fromState, string toState, DateTime timeOfChange, int? orderId)
        {
            FromState = fromState;
            ToState = toState;
            TimeOfChange = timeOfChange;
            OrderId = orderId;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string FromState { get; set; }
        public string ToState { get; set; }
        public DateTime TimeOfChange { get; set; }
        public int? OrderId { get; set; }
    }
}
