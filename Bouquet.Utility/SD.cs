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
        public const string ssShoppingAmount = "Shopping Amount Session";

        public const string StatusPending = "Pending";
        public const string StatusApproved = "Approved";
        public const string StatusInProcess = "Processing";
        public const string StatusShipped = "Shipped";
        public const string StatusCancelled = "Cancelled";
        public const string StatusRefunded = "Refunded";

        public const string PaymentStatusPending = "Pending";
        public const string PaymentStatusApproved = "Approved";
        public const string PaymentStatusDelayedPayment = "ApprovedForDelayedPayment";
        public const string PaymentStatusRejected = "Rejected";

        public const string ImageFolder = @"images\products";


        public static string ConvertToRawHtml(string source)
        {
            if(source == null)
            {
                return new string("");
            }

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
