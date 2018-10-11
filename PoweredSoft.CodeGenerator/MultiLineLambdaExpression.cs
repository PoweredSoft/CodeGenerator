using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PoweredSoft.CodeGenerator.Core;
using PoweredSoft.CodeGenerator.Extensions;

namespace PoweredSoft.CodeGenerator
{
    public class MultiLineLambdaExpression : IInlineGeneratable
    {
        public List<ParameterBuilder> Parameters { get; } = new List<ParameterBuilder>();
        public List<IGeneratable> Children { get; } = new List<IGeneratable>();

        public static MultiLineLambdaExpression Create() => new MultiLineLambdaExpression();

        public MultiLineLambdaExpression Parameter(Action<ParameterBuilder> action)
        {
            var p = ParameterBuilder.Create();
            Parameters.Add(p);
            action(p);
            return this;
        }

        public MultiLineLambdaExpression RawLine(string raw)
        {
            Children.Add(RawLineBuilder.Create(raw));
            return this;
        }

        public MultiLineLambdaExpression RawLine(Func<string> raw) => RawLine(raw());

        public MultiLineLambdaExpression Add(IGeneratable child)
        {
            Children.Add(child);
            return this;
        }

        public MultiLineLambdaExpression Add(Func<IGeneratable> child) => Add(child());

        public string GenerateInline()
        {
            string left;
            if (Parameters.Count == 1)
                left = Parameters.First().GetName();
            else
                left = $"({string.Join(", ", Parameters.Select(t => t.GetName()))})";

            var lines = new List<string>();
            lines.Add($"{left} => ");
            lines.Add("{");
            lines.AddRange(Children.IdentChildren());
            lines.Add("}");

            var ret = string.Join("\n", lines);
            return ret;
        }
    }
}
