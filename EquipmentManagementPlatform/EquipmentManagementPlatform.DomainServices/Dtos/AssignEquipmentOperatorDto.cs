﻿namespace EquipmentManagementPlatform.DomainServices.Dtos
{
    public class AssignEquipmentOperatorDto
    {
        public AssignEquipmentOperatorDto(string employee)
        {
            Employee = employee;
        }

        public string Employee { get; set; } 
    }
}
