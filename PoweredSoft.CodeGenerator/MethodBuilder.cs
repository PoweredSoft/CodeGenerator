using System;
using System.Collections.Generic;
using System.Text;
using PoweredSoft.CodeGenerator.Core;
using PoweredSoft.CodeGenerator.Models;

namespace PoweredSoft.CodeGenerator
{
    public class MethodBuilder : IGeneratable
    {
        public MethodModel Model { get; set; } = new MethodModel();

        public MethodBuilder Name(string name)
        {
            Model.Name = name;
            return this;
        }

        public MethodBuilder IsStatic(bool isStatic)
        {
            Model.IsStatic = isStatic;
            return this;
        }

        public MethodBuilder ReturnType(string type)
        {
            Model.ReturnType = type;
            return this;
        }

        public MethodBuilder Partial(bool partial)
        {
            Model.IsPartial = partial;
            return this;
        }

        public List<string> GenerateLines()
        {
            
        }
    }
}
