using System;
using System.Collections.Generic;
using System.Text;
using PoweredSoft.CodeGenerator.Core;

namespace PoweredSoft.CodeGenerator
{
    public class ParameterBuilder : IInlineGeneratable, IHasName
    {
        private string _type;
        private string _name;
        private string _defaultValue;

        public ParameterBuilder Type(string type)
        {
            _type = type;
            return this;
        }

        public ParameterBuilder Name(string name)
        {
            _name = name;
            return this;
        }

        public ParameterBuilder DefaultValue(string defaultValue)
        {
            _defaultValue = defaultValue;
            return this;
        }

        public static ParameterBuilder Create() => new ParameterBuilder();

        public string GenerateInline()
        {
            var ret = $"{_type} {_name}";
            if (!string.IsNullOrWhiteSpace(_defaultValue))
                ret += $" = {_defaultValue}";
            return ret;
        }

        public string GetName() => _name;
    }
}
