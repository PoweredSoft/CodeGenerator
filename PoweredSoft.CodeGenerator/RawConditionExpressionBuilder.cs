using System;
using System.Collections.Generic;
using System.Text;
using PoweredSoft.CodeGenerator.Models;

namespace PoweredSoft.CodeGenerator
{
    public class ConditionExpressionBuilderBase<TModel, TBuilder>
        where TBuilder : class, new()
        where TModel : ConditionExpressionModel, new()
    {
        public TModel Model { get; protected set; } = new TModel();
        public static TBuilder Create() => new TBuilder();

        public TBuilder And()
        {
            Model.And = true;
            return this as TBuilder;
        }

        public TBuilder Or()
        {
            Model.And = false;
            return this as TBuilder;
        }
    }

    public class RawConditionExpressionBuilder : ConditionExpressionBuilderBase<RawConditionExpressionModel, RawConditionExpressionBuilder>
    {
        public RawConditionExpressionBuilder Condition(string condition)
        {
            Model.RawCondition = condition;
            return this;
        }
    }
}
