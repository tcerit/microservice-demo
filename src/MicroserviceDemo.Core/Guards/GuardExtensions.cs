using System;
using Ardalis.GuardClauses;

namespace Core.Guards
{
    public static class GuardExtensions
    {
        public static void EmptyGuid(this IGuardClause guardClause, Guid id, string propertyName)
        {

            if (id.Equals(Guid.Empty))
            {
                throw new ArgumentException($"{propertyName} can not be equalt to an empty guid");
            }
        }
    }
}

