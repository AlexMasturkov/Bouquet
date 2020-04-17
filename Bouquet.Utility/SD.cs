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

        public const string ssShoppingCart = "Shopping Cart Session";      
        public static string ConvertToRawHtml(string source)
        {
            char[] array = new char[source.Length];
            int arrayIndex = 0;
            bool inside = false;

            for (int i = 0; i < source.Length; i++)
            {
                char let = source[i];
                if (let == '<')
                {
                    inside = true;
                    continue;
                }
                if (let == '>')
                {
                    inside = false;
                    continue;
                }
                if (!inside)
                {
                    array[arrayIndex] = let;
                    arrayIndex++;
                }
            }
            return new string(array, 0, arrayIndex);
        }
    }
}
