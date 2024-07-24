﻿using System.Linq.Expressions;

namespace Core.Utilities.Bugs
{
    public interface IFilter<T> where T : class
    {
        Expression<Func<T, bool>> ToExpression();
    }
}