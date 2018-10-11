using System;
using System.Collections.Generic;
using System.Text;
using PoweredSoft.CodeGenerator.Core;
using PoweredSoft.CodeGenerator.Extensions;

namespace PoweredSoft.CodeGenerator
{
    public class TernaryBuilder : IInlineGeneratable
    {
        private IInlineGeneratable _true;
        private IInlineGeneratable _false;
       
        public List<IConditionExpressionBuilder> Expressions { get; } = new List<IConditionExpressionBuilder>();

        public static TernaryBuilder Create() => new TernaryBuilder();

        public TernaryBuilder True(IInlineGeneratable ifTrue)
        {
            _true = ifTrue;
            return this;
        }

        public TernaryBuilder False(IInlineGeneratable ifFalse)
        {
            _false = ifFalse;
            return this;
        }

        public TernaryBuilder RawCondition(Action<RawConditionExpressionBuilder> action)
        {
            var builder = RawConditionExpressionBuilder.Create();
            Expressions.Add(builder);
            action(builder);
            return this as TernaryBuilder;
        }

        public string GenerateInline()
        {
            var line = $"{Expressions.GenerateConditionExpression()} ? {_true.GenerateInline()} : {_false.GenerateInline()}";
            return line;
        }
    }
}
