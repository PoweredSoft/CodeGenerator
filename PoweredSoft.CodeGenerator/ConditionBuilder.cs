using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PoweredSoft.CodeGenerator.Core;
using PoweredSoft.CodeGenerator.Extensions;

namespace PoweredSoft.CodeGenerator
{
    public abstract class ConditionBuilder<TBuilder> : IMultiLineGeneratable, IHasGeneratableChildren
        where TBuilder : class, new()
    {
        protected abstract string ConditionType { get; }
        public static TBuilder Create() => new TBuilder();
        public List<IConditionExpressionBuilder> Expressions { get; } = new List<IConditionExpressionBuilder>();
        public List<IGeneratable> Children { get; } = new List<IGeneratable>();

        public TBuilder RawCondition(Action<RawConditionExpressionBuilder> action)
        {
            var builder = RawConditionExpressionBuilder.Create();
            Expressions.Add(builder);
            action(builder);
            return this as TBuilder;
        }

        public TBuilder Add(IMultiLineGeneratable child)
        {
            Children.Add(child);
            return this as TBuilder;
        }

        public TBuilder Add(Func<IMultiLineGeneratable> child)
        {
            Children.Add(child());
            return this as TBuilder;
        }

        public List<string> GenerateLines()
        {
            if (!Children.Any())
                throw new Exception("Must have lines, to create an condition");

            var ret = new List<string>();
            ret.Add(CreateConditionExpression());

            // single line condition will be generated.
            var singleLineConditionAlreadyAdded = false;
            if (Children.Count() == 1)
            {
                var lines = Children[0].ToLines();
                if (lines.Count == 1)
                {
                    ret.AddRange(lines.IdentLines());
                    singleLineConditionAlreadyAdded = true;
                }
            }

            if (!singleLineConditionAlreadyAdded)
            { 
                ret.Add("{");
                ret.AddRange(Children.IdentChildren());
                ret.Add("}");
            }
           
            return ret;
        }

        private string CreateConditionExpression()
        {
            var ret = ConditionType;
            if (ret == "else")
                return ret;

            var parts = Expressions.Select((exp, index) =>
            {
                var expStr = "";
                if (index != 0)
                    expStr += (exp.IsAnd() ? "&&" : "||") + " ";

                if (exp.ShouldWrap())
                    expStr += "(";

                expStr += exp.GenerateInline();

                if (exp.ShouldWrap())
                    expStr += ")";

                return expStr;
            });

            ret += $" ({string.Join(" ", parts)})";
            return ret;
        }
    }
}
