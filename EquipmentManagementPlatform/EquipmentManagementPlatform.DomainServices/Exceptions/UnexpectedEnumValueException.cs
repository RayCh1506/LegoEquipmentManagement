﻿using System.Net;

namespace EquipmentManagementPlatform.DomainServices.Exceptions
{
    public class UnexpectedEnumValueException<T> : Exception
    {
        public UnexpectedEnumValueException(string value)
            : base($"Value { value} of enum {typeof(T).Name} is not supported") { }
    }
}
