using System;
using System.Collections.Generic;
using System.Text;

namespace Bouquet.Utility
{
    public static class SD
    {
        public const string ProcedureEventTypeCreate = "usp_CreateEventType";
        public const string ProcedureEventTypeGet = "usp_GetEventType";
        public const string ProcedureEventTypeGetAll = "usp_GetEventTypes";
        public const string ProcedureEventTypeUpdate = "usp_UpdateEventType";
        public const string ProcedureEventTypeDelete = "usp_DeleteEventType";

        public const string RoleIndividual = "Individual Customer";
        public const string RoleCompanyUser = "Company Customer";
        public const string RoleAdmin = "Admin";
        public const string RoleEmployee = "Employee";
    }
}
