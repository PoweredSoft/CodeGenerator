using System.Collections.Generic;
using System.Linq;
using PoweredSoft.CodeGenerator.Core;

namespace PoweredSoft.CodeGenerator
{
    public class AttributeBuilder : IInlineGeneratable, IHasName
    {
        private string _name;

        public List<IInlineGeneratable> Values { get; protected set; } = new List<IInlineGeneratable>();

        protected virtual string GenerateValues()
        {
            if (!Values.Any())
                return "";

            return $"({string.Join(", ", Values.Select(t => t.GenerateInline()))})";
        }

        public string GenerateInline()
        {
            return $"[{_name}{GenerateValues()}]";
        }

        public AttributeBuilder Name(string name)
        {
            _name = name;
            return this;
        }

        public AttributeBuilder Value(params IInlineGeneratable[] values)
        {
            Values.AddRange(values);
            return this;
        }

        public AttributeBuilder Value(params string[] values)
        {
            Values.AddRange(values.Select(t => RawInlineBuilder.Create(t)));
            return this;
        }

        public string GetName() => this._name;
    }
}
