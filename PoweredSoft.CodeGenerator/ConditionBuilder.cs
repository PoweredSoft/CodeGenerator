using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PoweredSoft.CodeGenerator.Core;
using PoweredSoft.CodeGenerator.Extensions;
using PoweredSoft.CodeGenerator.Models;

namespace PoweredSoft.CodeGenerator
{
    public abstract class ConditionBuilder<TModel, TBuilder> : IGeneratable
        where TBuilder : class, new()
        where TModel : ConditionModelBase, new()
    {
        protected TModel Model { get; set; } = new TModel();
        public static TBuilder Create() => new TBuilder();

        public TBuilder RawCondition(Action<RawConditionExpressionBuilder> action)
        {
            var builder = RawConditionExpressionBuilder.Create();
            var model = builder.Model;
            action(builder);
            Model.Expressions.Add(model);
            return this as TBuilder;
        }

        public TBuilder Add(IGeneratable child)
        {
            Model.Children.Add(child);
            return this as TBuilder;
        }

        public TBuilder Add(Func<IGeneratable> child)
        {
            Model.Children.Add(child());
            return this as TBuilder;
        }

        public List<string> GenerateLines()
        {
            if (!Model.Children.Any())
                throw new Exception("Must have lines, to create an condition");

            var ret = new List<string>();
            ret.Add(CreateConditionExpression());

            // single line condition will be generated.
            var singleLineConditionAlreadyAdded = false;
            if (Model.Children.Count() == 1)
            {
                var lines = Model.Children[0].GenerateLines();
                if (lines.Count == 1)
                {
                    ret.AddRange(lines.IdentLines());
                    singleLineConditionAlreadyAdded = true;
                }
            }

            if (!singleLineConditionAlreadyAdded)
            { 
                ret.Add("{");
                ret.AddRange(Model.Children.IdentChildren());
                ret.Add("}");
            }
           
            return ret;
        }

        private string CreateConditionExpression()
        {
            var ret = Model.ConditionType;
            if (ret == "else")
                return ret;

            var parts = Model.Expressions.Select((exp, index) =>
            {
                var expStr = "";
                if (index != 0)
                    expStr += (exp.And ? "&&" : "||") + " ";

                if (exp.Wrap)
                    expStr += "(";

                expStr += exp.Generate();

                if (exp.Wrap)
                    expStr += ")";

                return expStr;
            });

            ret += $" ({string.Join(" ", parts)})";
            return ret;
        }
    }
}
