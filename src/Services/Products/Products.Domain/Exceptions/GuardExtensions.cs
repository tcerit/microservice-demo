using System;
using Ardalis.GuardClauses;

namespace Products.Domain.Exceptions
{
    public static class GuardExtensions
    {
        public static void InvalidUrl(this IGuardClause guardClause, string url)
        {
            if (!Uri.IsWellFormedUriString(url, UriKind.Absolute))
            {
                throw new InvalidUrlException();
            }
        }
    }
}

