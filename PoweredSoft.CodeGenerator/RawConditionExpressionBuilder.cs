using System;
using System.Collections.Generic;
using System.Text;
using PoweredSoft.CodeGenerator.Core;

namespace PoweredSoft.CodeGenerator
{
    public abstract class ConditionExpressionBuilderBase<TBuilder> : IConditionExpressionBuilder
        where TBuilder : class, new()
    {
        protected bool _and;
        protected bool _wrap;

        public static TBuilder Create() => new TBuilder();

        public TBuilder And()
        {
            _and = true;
            return this as TBuilder;
        }

        public TBuilder Or()
        {
            _and = false;
            return this as TBuilder;
        }

        public TBuilder Wrap(bool wrap)
        {
            _wrap = wrap;
            return this as TBuilder;
        }

        public abstract string GenerateInline();

        public bool IsAnd() => _and;
        public bool ShouldWrap() => _wrap;
    }

    public class RawConditionExpressionBuilder : ConditionExpressionBuilderBase<RawConditionExpressionBuilder>
    {
        private string _rawCondition;

        public RawConditionExpressionBuilder Condition(string condition)
        {
            _rawCondition = condition;
            return this;
        }

        public override string GenerateInline() => _rawCondition;
    }
}
