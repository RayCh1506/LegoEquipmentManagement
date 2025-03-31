using EquipmentManagementPlatform.Domain.Models;
using EquipmentManagementPlatform.Domain.Models.Enums;
using EquipmentManagementPlatform.DomainServices.Dtos;

namespace EquipmentManagementPlatform.Test.Unit.Helper
{
    public static class EquipmentHelper
    {
        public static Equipment CreateEquipment(int id = 1, EquipmentState state = EquipmentState.GREEN, bool isOrderListEmpty = false )
        {
            return new Equipment(
                id,
                new GeneralInformation("Name", "Location", state),
                new OperationalInformation(true, string.Empty, 22.2, "Employee"),
                isOrderListEmpty ? new OrderInformation(new List<int>(), null) : new OrderInformation(new List<int> { id }, null),
                new List<EquipmentHistoricState>
                {
                    new EquipmentHistoricState(EquipmentState.YELLOW, state, DateTime.Today, 555)
                });
        }

        public static EquipmentDto CreateEquipmentDto(int id = 1, EquipmentState state = EquipmentState.GREEN, bool isOrderListEmpty = false)
        {
            return new EquipmentDto(
                id,
                new GeneralInformationDto("Name", "Location", state.ToString()),
                new OperationalInformationDto(true, string.Empty, 22.2, "Employee"),
                isOrderListEmpty ? new OrderInformationDto(null, new List<int>()) : new OrderInformationDto(null, new List<int> { id }),
                new List<EquipmentHistoricStateDto>
                {
                    new EquipmentHistoricStateDto(EquipmentState.YELLOW, state, DateTime.Today, 555)
                });
        }

        public static IEnumerable<Equipment> CreateEquipmentCollection()
        {
            return new List<Equipment> 
            {
                CreateEquipment() 
            };
        }

        public static IEnumerable<EquipmentDto> CreateEquipmentDtoCollection()
        {
            return new List<EquipmentDto>
            {
                CreateEquipmentDto()
            };
        }
    }
}
