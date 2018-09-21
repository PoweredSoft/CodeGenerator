using System;
using System.Collections.Generic;
using System.Text;
using PoweredSoft.CodeGenerator.Core;
using PoweredSoft.CodeGenerator.Models;

namespace PoweredSoft.CodeGenerator
{
    public class ParameterBuilder
    {
        public ParameterModel Model { get; protected set; } = new ParameterModel();

        public ParameterBuilder Type(string type)
        {
            Model.Type = type;
            return this;
        }

        public ParameterBuilder Name(string name)
        {
            Model.Name = name;
            return this;
        }

        public ParameterBuilder DefaultValue(string defaultValue)
        {
            Model.DefaultValue = defaultValue;
            return this;
        }

        public static ParameterBuilder Create() => new ParameterBuilder();
    }
}
