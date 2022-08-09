using System;
using Core.Domain;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Linq.Expressions;

namespace Core.Data.Services
{
    public class EnumerationConverter<T> : ValueConverter<T, int> where T : Enumeration, new()
    {


        public EnumerationConverter(ConverterMappingHints mappingHints = default)
            : base(convertToProviderExpression, convertFromProviderExpression, mappingHints)
        { }

        private readonly static Expression<Func<T, int>> convertToProviderExpression = x => x.Value;
        private readonly static Expression<Func<int, T>> convertFromProviderExpression = x => Enumeration.FromValue<T>(x);



    }
}

