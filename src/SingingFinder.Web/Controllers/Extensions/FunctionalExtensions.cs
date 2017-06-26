using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SingingFinder.Web.Controllers.Extensions
{
    public static class FunctionalExtensions
    {
        public static TOut Map<TIn, TOut>(this TIn value, Func<TIn, TOut> func) => func(value);
    }
}