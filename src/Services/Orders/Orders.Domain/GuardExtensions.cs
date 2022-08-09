using System;
using Ardalis.GuardClauses;

namespace Orders.Domain
{
    public static class GuardExtensions
    {
        public static void EmptyItemList(this IGuardClause guardClause, List<OrderItem> items)
        {
            if (items.Count() == 0)
            {
                throw new Exception("Can not place order with an empty item list");
            }
        }
    }
}

